using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MyApp
{
    public partial class EditEntity : Window
    {
        private Context _context = new Context();
        private Student _student;

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                if (!string.IsNullOrEmpty(value))
                {
                    imgProfile.Source = new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute));
                }
            }
        }


        public EditEntity(Student student)
        {
            InitializeComponent();
            _student = student;

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
                ImagePath = fullPath;
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
                _student.ImagePath = _imagePath;
                _student.Address= txtAddress.Text;
              //  _context.Students.Update(_student);
                _context.SaveChanges();

                MessageBox.Show("Student updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select a Profile Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName; // Update the ImagePath property
            }
        }


    }
}
