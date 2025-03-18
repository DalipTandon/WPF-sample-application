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
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        if (result.Tables.Count > 0)
                        {
                            studentTable = result.Tables[0]; // Store data in memory
                            StudentDataGrid.ItemsSource = studentTable.DefaultView; // Display in DataGrid
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
                    student.Stream?.Name, // Get Stream Name instead of ID
                    student.Address,
                    student.ImagePath
                );
            }

            return table;
        }

    }
}