using MyApp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Models;

namespace WebApp.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [ForeignKey("State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }

        public virtual ICollection<School> Schools { get; set; } = new HashSet<School>();
    }
}
