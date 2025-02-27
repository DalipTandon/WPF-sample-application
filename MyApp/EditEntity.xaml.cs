using System;
using System.Windows;

namespace MyApp
{
    public partial class EditEntity : Window
    {
        private Context _context = new Context();
        private Student _student;

        public EditEntity(Student student)
        {
            InitializeComponent();
            _student = student;

            // Pre-fill fields with student data
            txtName.Text = student.Name;
            cmbGender.SelectedItem = student.Gender;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Update student details
            _student.Name = txtName.Text;
            _student.Gender = cmbGender.Text;

            _context.SaveChanges();
            MessageBox.Show("Student updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
