using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Domain.Models
{
    public class RegisteredModel1
    {
        [Required(ErrorMessage = "User name is Required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password name is Required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "email is Required")]
        public string Email { get; set; }
    }
}
