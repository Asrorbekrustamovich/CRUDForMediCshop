using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Domain.Entities
{
    public class permission
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<Role> roles { get; set; }
    }
}
