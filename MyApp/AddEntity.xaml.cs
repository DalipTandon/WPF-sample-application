using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MyApp
{
    public partial class AddEntity : Window
    {
        public AddEntity()
        {
            InitializeComponent();
        }

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbState.SelectedItem == null)
                return;

            string selectedState = (cmbState.SelectedItem as ComboBoxItem).Content.ToString();

            // Clear previous cities
            cmbCity.Items.Clear();

            // Dictionary for state-wise cities
            Dictionary<string, List<string>> stateCities = new Dictionary<string, List<string>>()
            {
                { "Punjab", new List<string> { "Amritsar", "Ludhiana", "Jalandhar", "Patiala" } },
                { "Haryana", new List<string> { "Gurgaon", "Faridabad", "Panipat", "Ambala" } },
                { "Delhi", new List<string> { "New Delhi", "Dwarka", "Karol Bagh", "Saket" } }
            };

            if (stateCities.ContainsKey(selectedState))
            {
                foreach (var city in stateCities[selectedState])
                {
                    cmbCity.Items.Add(new ComboBoxItem { Content = city });
                }

                // Show city field when a state is selected
                lblCity.Visibility = Visibility.Visible;
                cmbCity.Visibility = Visibility.Visible;
            }
            else
            {
                lblCity.Visibility = Visibility.Collapsed;
                cmbCity.Visibility = Visibility.Collapsed;
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string gender = cmbGender.Text;
            string state = cmbState.Text; // Fixed: Use cmbState instead of txtState
            string city = cmbCity.Text;   // Capture selected city
            //string stream = txtStream.Text;
            string school = cmbSchool.Text; // Fixed: Use cmbSchool (ComboBox) instead of txtSchool

            MessageBox.Show($"Name: {name}\nGender: {gender}\nState: {state}\nCity: {city}\nSchool: {school}", "Submitted Data");

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
