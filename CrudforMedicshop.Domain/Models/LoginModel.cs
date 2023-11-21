using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Domain.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="User name is Required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password name is Required")]
        public string? Password { get; set; }
    }
}
