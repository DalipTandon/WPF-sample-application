using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyApp
{
    public partial class AddEntity : Window
    {
        private Context _context = new Context();

        public AddEntity()
        {
            InitializeComponent();
            LoadStates();
            LoadGender(); // Load gender placeholder
        }

        private void LoadGender()
        {
            cmbGender.Items.Clear();
            List<string> genders = new List<string>
            {
                "Select Gender", // Placeholder
                "Male",
                "Female",
                "Other"
            };

            cmbGender.ItemsSource = genders;
            cmbGender.SelectedIndex = 0; // Default to placeholder
        }

        private void LoadStates()
        {
            var states = _context.States.ToList();
            states.Insert(0, new State { Id = 0, Name = "Select State" });

            cmbState.ItemsSource = states;
            cmbState.DisplayMemberPath = "Name";
            cmbState.SelectedValuePath = "Id";
            cmbState.SelectedIndex = 0;

            // Set up empty placeholders for dependent dropdowns
            LoadCities(0);
            LoadSchools(0);
            LoadStreams(0);
        }

        private void LoadCities(int stateId)
        {
            var cities = _context.Cities.Where(c => c.StateId == stateId).ToList();
            cities.Insert(0, new City { Id = 0, Name = "Select City" });

            cmbCity.ItemsSource = cities;
            cmbCity.DisplayMemberPath = "Name";
            cmbCity.SelectedValuePath = "Id";
            cmbCity.SelectedIndex = 0;

            lblCity.Visibility = stateId > 0 ? Visibility.Visible : Visibility.Collapsed;
            cmbCity.Visibility = stateId > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadSchools(int cityId)
        {
            var schools = _context.Schools.Where(s => s.CityId == cityId).ToList();
            schools.Insert(0, new School { Id = 0, Name = "Select School" });

            cmbSchool.ItemsSource = schools;
            cmbSchool.DisplayMemberPath = "Name";
            cmbSchool.SelectedValuePath = "Id";
            cmbSchool.SelectedIndex = 0;
        }

        private void LoadStreams(int schoolId)
        {
            var streams = _context.Streams.Where(st => st.SchoolId == schoolId).ToList();
            streams.Insert(0, new Stream { Id = 0, Name = "Select Stream" });

            cmbStream.ItemsSource = streams;
            cmbStream.DisplayMemberPath = "Name";
            cmbStream.SelectedValuePath = "Id";
            cmbStream.SelectedIndex = 0;

            lblStream.Visibility = schoolId > 0 ? Visibility.Visible : Visibility.Collapsed;
            cmbStream.Visibility = schoolId > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedStateId = cmbState.SelectedValue is int id ? id : 0;
            LoadCities(selectedStateId);
            LoadSchools(0); // Reset schools
            LoadStreams(0); // Reset streams
        }

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedCityId = cmbCity.SelectedValue is int id ? id : 0;
            LoadSchools(selectedCityId);
            LoadStreams(0); // Reset streams
        }

        private void cmbSchool_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedSchoolId = cmbSchool.SelectedValue is int id ? id : 0;
            LoadStreams(selectedSchoolId);
        }

        private string SaveImageToFolder(string sourceFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath))
                {
                    throw new FileNotFoundException("Selected image file not found.");
                }

                string projectRootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                string appFolderPath = Path.Combine(projectRootPath, "Images");

                if (!Directory.Exists(appFolderPath))
                {
                    Directory.CreateDirectory(appFolderPath);
                }

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(sourceFilePath)}";
                string destinationPath = Path.Combine(appFolderPath, fileName);

                File.Copy(sourceFilePath, destinationPath, true);

                return destinationPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving image: {ex.Message}", "Image Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
     
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (cmbState.SelectedValue is int stateId && stateId == 0 ||
                cmbCity.SelectedValue is int cityId && cityId == 0 ||
                cmbSchool.SelectedValue is int schoolId && schoolId == 0 ||
                cmbStream.SelectedValue is int streamId && streamId == 0 ||
                cmbGender.SelectedIndex == 0)
            {
                MessageBox.Show("Please select all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string selectedImagePath = txtImagePath.Text;
            string savedImagePath = SaveImageToFolder(selectedImagePath);

            var student = new Student
            {
                Name = txtName.Text,
                Gender = cmbGender.Text,
                StateId = (int)cmbState.SelectedValue,
                CityId = (int)cmbCity.SelectedValue,
                SchoolId = (int)cmbSchool.SelectedValue,
                StreamId = (int)cmbStream.SelectedValue,
                Address= txtAddress.Text,
                ImagePath = savedImagePath
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            MessageBox.Show("Student added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                txtImagePath.Text = openFileDialog.FileName;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
