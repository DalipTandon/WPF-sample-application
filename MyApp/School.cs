using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class School
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Stream> Streams { get; set; } = new List<Stream>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
