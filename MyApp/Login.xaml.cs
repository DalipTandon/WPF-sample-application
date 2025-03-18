using System.Linq;
using System.Windows;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebApp.Database;

namespace MyApp
{
    public partial class Login : Window
    {
        private readonly DataContext _context;

        public Login()
        {
            InitializeComponent();
            _context = new DataContext();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                MessageBox.Show("User not found. Please check your email and try again.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Proceed to the next window
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return HashPassword(enteredPassword) == storedHash;
        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                MessageBox.Show("Email already registered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string hashedPassword = HashPassword(password);
            _context.Users.Add(new UserAuth{ Email = email, Password = hashedPassword });
            _context.SaveChanges();

            MessageBox.Show("Signup successful! Please log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
