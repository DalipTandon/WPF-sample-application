using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;

namespace MyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Context _context = new Context();
        public MainWindow()
        {
            InitializeComponent();
            LoadStudentData();

        }
       
        private void AddEntity_Click(object sender, RoutedEventArgs e)
        {
            AddEntity addEntityWindow = new AddEntity();
            addEntityWindow.Owner = this; // Set MainWindow as owner
            addEntityWindow.ShowDialog(); // Open as modal
            //addEntityWindow.Show();
            //addEntityWindow.Topmost = true;
            //addEntityWindow.WindowStyle=
            LoadStudentData();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                //this.Close();
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }
        private void LoadStudentData()
        {
            try
            {
                var students = _context.Students
                    .Include(s => s.State)
                    .Include(s => s.City)
                    .Include(s => s.School)
                    .Include(s => s.Stream)
                    .ToList();

                StudentDataGrid.ItemsSource = students;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
           
            var button = sender as Button;
            if (button == null) return;

            var student = (button.DataContext as Student);
            if (student == null) return;

            // Open Edit Window
            EditEntity editWindow = new EditEntity(student);
            editWindow.Owner = this; // Set MainWindow as owner
            editWindow.ShowDialog(); // Open as modal dialog

            // Refresh DataGrid after editing
            _context.SaveChanges();
            LoadStudentData();
        }


        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                MessageBox.Show("Unexpected error: Button reference is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (button.DataContext is not Student student)
            {
                MessageBox.Show("No student selected for deletion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {student.Name}?",
                                                      "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Students.Remove(student);
                    _context.SaveChanges();

                    MessageBox.Show("Student deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Refresh DataGrid after deletion
                    LoadStudentData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


    }
}