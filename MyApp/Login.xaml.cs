using System.Linq;
using System.Windows;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MyApp
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            using (var context = new Context())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);

                if (user != null && VerifyPassword(password, user.Password))
                {
                    //MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid email or password!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return HashPassword(enteredPassword) == storedHash;
        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Email and Password are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new Context())
            {
                if (db.Users.Any(u => u.Email == email))
                {
                    MessageBox.Show("Email already exists. Please use another email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string hashedPassword = HashPassword(password);

                var newUser = new UserAuth
                {
                    Email = email,
                    Password = hashedPassword
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                MessageBox.Show("Signup successful! Please log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Clear fields
                txtEmail.Clear();
                txtPassword.Clear();
            }
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
