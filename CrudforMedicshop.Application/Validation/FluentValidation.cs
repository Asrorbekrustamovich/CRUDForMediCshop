using CrudforMedicshop.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Application.Validation
{
    public class FluentValidation:AbstractValidator<MedicineForCreateDTO>
    {
        public FluentValidation()
        {
            RuleFor(x => x.Name).Must(Uppercase).WithMessage("birinchi harf katta harf bilan yozilishi kerak");
            RuleFor(x => x.CreatedDate).Must(CreatedDate).WithMessage("Yaratilgan sanasi ertangi yoki kelajakdagi kun bo`la olmaydi");
            RuleFor(x => x.ExpiredDate).Must(ExpiredDate).WithMessage("yaroqlilik muddati tugagan yoki bugun bo`lgan dorilarni kiritaolmaysiz");
        }
        private static bool Uppercase(string name)
        {
            if (char.IsUpper(name[0]))
            {
                return true;
            }
            return false;
        }
        private static bool CreatedDate(DateOnly date)
        {
            if(date>DateOnly.FromDateTime(DateTime.Now))
            {
                return false;
            }
            return true;
        }
        private static bool ExpiredDate(DateOnly date)
        {
            if(date<=DateOnly.FromDateTime(DateTime.Now))
            {
                return false;
            }
            return true;
        }
    }
    
}
