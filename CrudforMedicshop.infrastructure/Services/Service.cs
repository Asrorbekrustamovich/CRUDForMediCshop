using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Services
{
    public class Service : Iservice<Medicine>
    {
        private readonly Irepository<Medicine> _repostory;
        public Service(Irepository<Medicine> repostory)
        {
            _repostory = repostory;
        }

        public async Task<Medicine> Create(Medicine entity)
        {
            await _repostory.create(entity);
            return entity;
        }

        public async Task< string> Delete(int deletedid)
        {
            if (await _repostory.delete(deletedid) == true)
            {
                return "object is deleted";
            }
            return "object is not deleted";
        }

        public async Task<IEnumerable<Medicine>> Getall()
        {
            IEnumerable<Medicine> getall = await _repostory.Getall();
            return getall;
        }


        public async Task< Medicine> GetById(int id)
        {
            return await _repostory.getbyid(id);
        }

        public async Task<IEnumerable<Medicine>> NearlyExpiredMedicines()
        {
            return await _repostory.NearlyExpiredMedicines();
        }

        public async Task<IEnumerable<Medicine>> SearchbyText(string MedicineName)
        {
           return await _repostory.SearchbyText(MedicineName);
        }

        public  async Task<ThemostsoldMedicine> ThemostsoldMedicine()
        {
            return await _repostory.ThemostsoldMedicine();
        }

        public async Task<string> Update(Medicine entity)
        {
            if (await _repostory.update(entity))
            {
                return "updated";
            }
            return "error";
        }
    }
}
