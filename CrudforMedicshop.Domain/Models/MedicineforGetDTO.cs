using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Domain.Models
{
    public class MedicineforGetDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int CountofSold { get; set; }
        public double Summa { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly ExpiredDate { get; set; }
    }
}
