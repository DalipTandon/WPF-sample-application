using MyApp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation Property
        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
    }
}
