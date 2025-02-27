using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp
{
    public class School
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [ForeignKey("City")]
        public int CityId { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<Stream> Streams { get; set; } = new HashSet<Stream>();
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
