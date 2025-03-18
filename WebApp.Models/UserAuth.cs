using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class UserAuth
    {
        [Key]
        public int ID {  get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty ;
        
    }
}
