using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation Property
        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }
}
