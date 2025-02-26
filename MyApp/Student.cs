using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; }

        // Foreign Keys
        [ForeignKey("State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [ForeignKey("School")]
        public int SchoolId { get; set; }
        public virtual School School { get; set; }

        [ForeignKey("Stream")]
        public int StreamId { get; set; }
        public virtual Stream Stream { get; set; }
    }
}
