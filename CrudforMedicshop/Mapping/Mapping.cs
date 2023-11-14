using AutoMapper;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;

namespace CrudforMedicshop.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Medicine,MedicineforGetDTO>();
        }
    }
}
