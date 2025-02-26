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

    }
}