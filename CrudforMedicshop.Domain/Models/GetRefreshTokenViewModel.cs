using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Domain.Models
{
    public class GetRefreshTokenViewModel
    {
        public string RefreshToken { get; set;}
        public string AccessToken { get; set;}
    }
}
