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
        }
        
        private void LoadStates()
        {
            var states = _context.States.ToList();
            cmbState.ItemsSource = states;
            cmbState.DisplayMemberPath = "Name";
            cmbState.SelectedValuePath = "Id";

            lblCity.Visibility = Visibility.Collapsed;
            cmbCity.Visibility = Visibility.Collapsed;
            lblStream.Visibility = Visibility.Collapsed;
            cmbStream.Visibility = Visibility.Collapsed;
        }

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbState.SelectedValue == null) return;
            int selectedStateId = (int)cmbState.SelectedValue;

            cmbCity.ItemsSource = _context.Cities.Where(c => c.StateId == selectedStateId).ToList();
            cmbCity.DisplayMemberPath = "Name";
            cmbCity.SelectedValuePath = "Id";

            // Show City dropdown when a state is selected
            lblCity.Visibility = Visibility.Visible;
            cmbCity.Visibility = Visibility.Visible;
        }

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCity.SelectedValue == null) return;
            int selectedCityId = (int)cmbCity.SelectedValue;

            cmbSchool.ItemsSource = _context.Schools.Where(s => s.CityId == selectedCityId).ToList();
            cmbSchool.DisplayMemberPath = "Name";
            cmbSchool.SelectedValuePath = "Id";
        }

        private void cmbSchool_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSchool.SelectedValue == null) return;
            int selectedSchoolId = (int)cmbSchool.SelectedValue;

            cmbStream.ItemsSource = _context.Streams.Where(st => st.SchoolId == selectedSchoolId).ToList();
            cmbStream.DisplayMemberPath = "Name";
            cmbStream.SelectedValuePath = "Id";

            // Show Stream dropdown when a school is selected
            lblStream.Visibility = Visibility.Visible;
            cmbStream.Visibility = Visibility.Visible;
        }
        private string SaveImageToFolder(string sourceFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath))
                {
                    throw new FileNotFoundException("Selected image file not found.");
                }

                string appFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
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
            if (cmbState.SelectedValue == null || cmbCity.SelectedValue == null ||
                cmbSchool.SelectedValue == null || cmbStream.SelectedValue == null)
            {
                MessageBox.Show("Please select all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string selectedImagePath = txtImagePath.Text; // Get file path from TextBox
            string savedImagePath = SaveImageToFolder(selectedImagePath); // Save image and get new path

            var student = new Student
            {
                Name = txtName.Text,
                Gender = cmbGender.Text,
                StateId = (int)cmbState.SelectedValue,
                CityId = (int)cmbCity.SelectedValue,
                SchoolId = (int)cmbSchool.SelectedValue,
                StreamId = (int)cmbStream.SelectedValue,
                ImagePath = savedImagePath
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            MessageBox.Show("Student added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
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
                txtImagePath.Text = openFileDialog.FileName; // Display file path in TextBox
            }
        }
    }
}
