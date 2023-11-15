using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Domain.Entities
{
    public class Medicine
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int CountsofSold { get; set; }
        public double Summa { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly ExpiredDate { get; set; }
    }
}
