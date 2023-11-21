using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Domain.Models
{
    public class TokenViewModel
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
