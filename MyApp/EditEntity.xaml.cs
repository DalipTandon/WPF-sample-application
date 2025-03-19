using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WebApp.Database;
using WebApp.Models;

namespace MyApp
{
    public partial class EditEntity : Window
    {
        private readonly DataContext _context = new DataContext();
        private Student _student=new Student();


        //private string _imagePath;
        //public string ImagePath
        //{
        //    get { return _imagePath; }
        //    set
        //    {
        //        _imagePath = value;
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            imgProfile.Source = new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute));
        //        }
        //    }
        //}
        public EditEntity(int studentId)
        {
            InitializeComponent();

            _student = _context.Students.FirstOrDefault(s => s.Id == studentId);

            if (_student == null)
            {
                MessageBox.Show("No student found in the database!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            LoadStates();
            LoadStudentData();
        }


        private void LoadStudentData()
        {
            // Pre-fill fields with student data
            txtName.Text = _student.Name;
            cmbGender.Text = _student.Gender;

            cmbState.SelectedValue = _student.StateId;
            cmbCity.SelectedValue = _student.CityId;
            cmbSchool.SelectedValue = _student.SchoolId;
            cmbStream.SelectedValue = _student.StreamId;
            txtAddress.Text = _student.Address;

            if (!string.IsNullOrEmpty(_student.ImagePath))
            {
                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", _student.ImagePath);

                // Ensure the file exists before setting the image source
                if (System.IO.File.Exists(fullPath))
                {
                    imgProfile.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                }
            }
        }



        private void LoadStates()
        {
            cmbState.ItemsSource = _context.States.ToList();
            cmbState.DisplayMemberPath = "Name";
            cmbState.SelectedValuePath = "Id";
        }

        private void cmbState_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbState.SelectedValue != null)
            {
                int stateId = (int)cmbState.SelectedValue;
                cmbCity.ItemsSource = _context.Cities.Where(c => c.StateId == stateId).ToList();
                cmbCity.DisplayMemberPath = "Name";
                cmbCity.SelectedValuePath = "Id";
            }
        }

        private void cmbCity_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbCity.SelectedValue != null)
            {
                int cityId = (int)cmbCity.SelectedValue;
                cmbSchool.ItemsSource = _context.Schools.Where(s => s.CityId == cityId).ToList();
                cmbSchool.DisplayMemberPath = "Name";
                cmbSchool.SelectedValuePath = "Id";
            }
        }

        private void cmbSchool_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbSchool.SelectedValue != null)
            {
                int schoolId = (int)cmbSchool.SelectedValue;
                cmbStream.ItemsSource = _context.Streams.Where(s => s.SchoolId == schoolId).ToList();
                cmbStream.DisplayMemberPath = "Name";
                cmbStream.SelectedValuePath = "Id";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update student details
                _student.Name = txtName.Text;
                _student.Gender = cmbGender.Text;
                _student.StateId = (int)cmbState.SelectedValue;
                _student.CityId = (int)cmbCity.SelectedValue;
                _student.SchoolId = (int)cmbSchool.SelectedValue;
                _student.StreamId = (int)cmbStream.SelectedValue;
                _student.Address = txtAddress.Text;

                string imagesFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Images"));

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // 🔹 Preserve existing image unless a new one is selected
                string existingImagePath = _student.ImagePath; 
                bool isNewImageSelected = false;

                if (imgProfile.Source is BitmapImage bitmapImage && bitmapImage.UriSource != null)
                {
                    string selectedImagePath = Path.GetFileName(bitmapImage.UriSource.LocalPath);
                    string destinationPath = Path.Combine(imagesFolder, selectedImagePath);

                    if (!File.Exists(destinationPath) || selectedImagePath != Path.GetFileName(existingImagePath))
                    {
                        File.Copy(bitmapImage.UriSource.LocalPath, destinationPath, true);
                        isNewImageSelected = true;
                    }

                    _student.ImagePath = Path.Combine("images", selectedImagePath);
                }

                // Ensure we do not overwrite ImagePath with null
                if (!isNewImageSelected)
                {
                    _student.ImagePath = existingImagePath;
                }

                // Save changes
                _context.SaveChanges();
               LoadStudentData();
                MessageBox.Show("Student updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
               // LoadStudentData();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                
        }






        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Define the absolute path of the Images folder
                    string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Images");
                    imagesFolder = Path.GetFullPath(imagesFolder); // Get absolute path

                    // Ensure the directory exists
                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    // Delete the previously stored image if it exists and is different
                    if (!string.IsNullOrEmpty(_student.ImagePath) && File.Exists(_student.ImagePath))
                    {
                        File.Delete(_student.ImagePath);
                    }

                    // Keep the original file name
                    string originalFileName = Path.GetFileName(openFileDialog.FileName);
                    string destinationPath = Path.Combine(imagesFolder, originalFileName);

                    // Check if the file already exists, then delete it before copying
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }

                    // Copy the new image
                    File.Copy(openFileDialog.FileName, destinationPath);

                    // Store the absolute path in the database
                    _student.ImagePath = destinationPath;

                    // Load and display the image
                    var image = new BitmapImage();
                    using (var stream = new FileStream(destinationPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                        image.Freeze();
                    }
                    imgProfile.Source = image;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error uploading image: " + ex.Message, "Image Upload Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }









        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
