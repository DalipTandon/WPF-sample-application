using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }

        public virtual ICollection<School> Schools { get; set; } = new List<School>();
    }
}
