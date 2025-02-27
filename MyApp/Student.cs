using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp
{
    

    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public String Gender { get; set; } 

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
    }
}
