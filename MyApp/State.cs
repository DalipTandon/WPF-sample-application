using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyApp
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
