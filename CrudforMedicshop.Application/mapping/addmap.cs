using AutoMapper;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Application.mapping
{
    public class addmap : Profile
    {
        public addmap()
        {
            CreateMap<MedicineForCreateDTO,Medicine>();
            CreateMap<UserDTO,User>();
        }
    }
}
