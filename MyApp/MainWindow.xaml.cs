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
using System.IO;
using System.Diagnostics;
using System.Drawing;
using WebApp.Models;
using System.Xml.Linq;
using WebApp.Database;
using Microsoft.VisualBasic;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.Win32;
//using Microsoft.EntityFrameworkCore;

namespace MyApp
{
 
    public partial class MainWindow : Window
    {

        
        private readonly DataContext _context =new DataContext();
        private DataTable studentTable = new DataTable(); // Holds imported data


        public MainWindow()
        {
            InitializeComponent();
            LoadStudentData();
            LoadStudentsFromDatabase();
        }

        private void AddEntity_Click(object sender, RoutedEventArgs e)
        {
            AddEntity addEntityWindow = new AddEntity();
            addEntityWindow.Owner = this; 
            addEntityWindow.ShowDialog(); 
         
            LoadStudentData();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
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
                    //.Include(s => s.Address)
                    .ToList();

                StudentDataGrid.ItemsSource = students;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private bool DeleteImageFromFolder(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    File.Delete(imagePath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File deletion error: {ex.Message}", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
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
                    // Delete image if it exists
                    if (!string.IsNullOrEmpty(student.ImagePath))
                    {
                        bool isDeleted = DeleteImageFromFolder(student.ImagePath);
                        if (!isDeleted)
                        {
                            MessageBox.Show("Failed to delete student image, but the record will still be deleted.",
                                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }

                    // Remove student from database
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

        private void StudentDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Student selectedStudent)
            {
                OpenEditWindow(selectedStudent);
            }
        }
        private void OpenEditWindow(Student student)
        {
            int selectedStudentId = student.Id; // Assuming you have the selected student's ID
            var editWindow = new EditEntity(selectedStudentId);
            editWindow.ShowDialog();
                        LoadStudentData();

            //_context.SaveChanges();
            //LoadStudentData();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx",
                Title = "Select an Excel File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadExcelData(openFileDialog.FileName);
            }
        }

        private void LoadExcelData(string filePath)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); // Required for ExcelDataReader

                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true // Read the first row as column headers
                            }
                        });

                        if (result.Tables.Count > 0)
                        {
                            DataTable studentTable = result.Tables[0]; // Read the first sheet
                            List<Student> newStudents = ConvertDataTableToStudents(studentTable); // Convert to List

                            if (newStudents.Count > 0)
                            {
                                // Display data in DataGrid without saving to the database
                                StudentDataGrid.ItemsSource = null; // Reset binding
                                StudentDataGrid.ItemsSource = newStudents; // Show imported data

                                MessageBox.Show($"{newStudents.Count} students loaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("No valid student data found in the Excel file.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No data found in the Excel file.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading Excel file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LoadStudentsFromDatabase()
        {
            using (var dbContext = new DataContext()) // Replace with your actual DbContext
            {
                var students = dbContext.Students
                    .Include(s => s.State)
                    .Include(s => s.City)
                    .Include(s => s.School)
                    .Include(s => s.Stream)
                    .ToList();

                StudentDataGrid.ItemsSource = null;  // Reset binding
                StudentDataGrid.ItemsSource = students; // Load saved students
            }
        }



        // EXPORT: Generate Excel file from DataGrid
        private void Export_Click(object sender, RoutedEventArgs e)
         {
            if (StudentDataGrid.ItemsSource == null)
            {
                MessageBox.Show("No data to export.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Save as Excel File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                SaveDataGridToExcel(filePath);
            }
        }

        private void SaveDataGridToExcel(string filePath)
        {
            try
            {
                // Convert DataGrid ItemsSource (List<Student>) to DataTable
                if (StudentDataGrid.ItemsSource is List<Student> studentList)
                {
                    DataTable dt = ConvertToDataTable(studentList);

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Students");
                        wb.SaveAs(filePath);
                    }

                    MessageBox.Show("Data exported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No data to export.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private List<Student> ConvertDataTableToStudents(DataTable dt)
        {
            List<Student> students = new List<Student>();

            foreach (DataRow row in dt.Rows)
            {
                Student student = new Student()
                {
                    Id = row.Table.Columns.Contains("ID") && int.TryParse(row["ID"].ToString(), out int id) ? id : 0,
                    Name = row["Name"].ToString(),
                    Gender = row["Gender"].ToString(),
                    State = new State { Name = row["State"].ToString() },
                    City = new City { Name = row["City"].ToString() },
                    School = new School { Name = row["School"].ToString() },
                    Stream = new WebApp.Models.Stream { Name = row["Stream"].ToString() },
                    Address = row["Address"].ToString(),
                    ImagePath = row["ImagePath"].ToString()
                };

                students.Add(student);
            }

            return students;
        }


        private DataTable ConvertToDataTable(List<Student> students)
        {
            DataTable table = new DataTable("Students");

            // Define Columns
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Gender", typeof(string));
            table.Columns.Add("State", typeof(string));
            table.Columns.Add("City", typeof(string));
            table.Columns.Add("School", typeof(string));
            table.Columns.Add("Stream", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("ImagePath", typeof(string));

            // Fill Rows
            foreach (var student in students)
            {
                table.Rows.Add(
                    student.Id,
                    student.Name,
                    student.Gender,
                    student.State?.Name,  
                    student.City?.Name,  
                    student.School?.Name, 
                    student.Stream?.Name, 
                    student.Address,
                    student.ImagePath
                );
            }

            return table;
        }

        private void SaveToDatabase_Click(object sender, RoutedEventArgs e)
{
    if (StudentDataGrid.ItemsSource is List<Student> students && students.Count > 0)
    {
        using (var dbContext = new DataContext()) // Replace with your actual DbContext
        {
            foreach (var student in students)
            {
                // Ensure related entities exist in the database before adding new ones
                student.State = dbContext.States.FirstOrDefault(s => s.Name == student.State.Name) ?? student.State;
                student.City = dbContext.Cities.FirstOrDefault(c => c.Name == student.City.Name) ?? student.City;
                student.School = dbContext.Schools.FirstOrDefault(s => s.Name == student.School.Name) ?? student.School;
                student.Stream = dbContext.Streams.FirstOrDefault(st => st.Name == student.Stream.Name) ?? student.Stream;
            }

            dbContext.Students.AddRange(students);
            dbContext.SaveChanges();
        }

        MessageBox.Show("Students saved to the database successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    else
    {
        MessageBox.Show("No data to save!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}


    }
}