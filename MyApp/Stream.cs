using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class Stream
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("School")]
        public int SchoolId { get; set; }
        public virtual School School { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
