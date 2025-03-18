using MyApp;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Windows.Media.Imaging;

namespace WebApp.Models
{
    public class Student
    {
        [Key]
        public int Id {  get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; }

        // Foreign Keys
        [Required]
        [ForeignKey("State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }

        [Required]
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Required]
        [ForeignKey("School")]
        public int SchoolId { get; set; }
        public virtual School School { get; set; }

        [Required]
        [ForeignKey("Stream")]
        public int StreamId { get; set; }
        public virtual Stream Stream { get; set; }

        public string ImagePath { get; set; }
        public string Address { get; set; }

        // New Property: Image Source for WPF Binding (Not Stored in DB)
      
        [NotMapped] // Ensures it's not added to the database
        public BitmapImage StudentImageSource
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath)) return null;

                try
                {
                    // Ensure ImagePath is absolute
                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", ImagePath);

                    if (!System.IO.File.Exists(fullPath))
                        return null; // Prevent errors if the file doesn't exist

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Load without locking the file
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache; // Ignore caching
                    bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bitmap.EndInit();
                    bitmap.Freeze(); // Allows usage across threads without UI conflicts
                    return bitmap;
                }
                catch
                {
                    return null; // Return null if the image fails to load
                }
            }
        }

    }
}
