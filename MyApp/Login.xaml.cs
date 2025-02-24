using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of MainWindow
            MainWindow mainWindow = new MainWindow();

            // Preserve size and state of Login Window
            if (this.WindowState == WindowState.Maximized)
            {
                mainWindow.WindowState = WindowState.Maximized; // Open MainWindow maximized
            }
            else
            {
                mainWindow.Width = this.Width;
                mainWindow.Height = this.Height;
            }

            // Show the main window and close the login window
            mainWindow.Show();
            this.Close();
        }

    }
}
