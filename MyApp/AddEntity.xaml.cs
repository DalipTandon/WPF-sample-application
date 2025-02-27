using System;
using System.Collections.Generic;
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
        }

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbState.SelectedValue == null) return;
            int selectedStateId = (int)cmbState.SelectedValue;

            cmbCity.ItemsSource = _context.Cities.Where(c => c.StateId == selectedStateId).ToList();
            cmbCity.DisplayMemberPath = "Name";
            cmbCity.SelectedValuePath = "Id";
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
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (cmbState.SelectedValue == null || cmbCity.SelectedValue == null ||
                cmbSchool.SelectedValue == null || cmbStream.SelectedValue == null)
            {
                MessageBox.Show("Please select all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var student = new Student
            {
                Name = txtName.Text,
                Gender = cmbGender.Text,
                StateId = (int)cmbState.SelectedValue,
                CityId = (int)cmbCity.SelectedValue,
                SchoolId = (int)cmbSchool.SelectedValue,
                StreamId = (int)cmbStream.SelectedValue
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
    }
}
