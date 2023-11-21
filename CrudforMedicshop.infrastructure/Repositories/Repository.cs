using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using CrudforMedicshop.infrastructure.Dbcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Repositories
{
    public class Repository : Irepository<Medicine>
    {
        private readonly ApplicationDbcontext1 _context;

        public Repository(ApplicationDbcontext1 Mydbcontext)
        {
            _context = Mydbcontext;
        }

        public async Task<Medicine> create(Medicine entity)
        {
            _context.Medicines1.Add(entity);
           _context.SaveChanges();
            return entity;
        }
        public async Task<bool> delete(int deleteid)
        {
            var deletedobject =  await _context.Medicines1.FindAsync(deleteid);
            if (deletedobject != null)
            {
                _context.Medicines1.Remove(deletedobject);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<Medicine>> Getall()
        {
            var getall = _context.Medicines1;
            if (getall != null)
            {
                return getall;
            }
            else
            {
                return null;
            }
        }

        public  Task< Medicine> getbyid(int id)
        {
            var getbyid = _context.Medicines1.FirstAsync(x => x.id == id);
            if (getbyid != null)
            {
                return getbyid;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Medicine>> NearlyExpiredMedicines()
        {

            DateTime currentDate = DateTime.Now;
            DateTime previousMonth = currentDate.AddMonths(-1);
            DateOnly previous = DateOnly.FromDateTime(previousMonth);
           var nearlyExpiredMedicines =  _context.Medicines1.Where(x => x.ExpiredDate>=previous); 
            return nearlyExpiredMedicines;
   

        }

        public   async Task<IEnumerable<Medicine>> SearchbyText(string MedicineName)
        {
            var Searchbytext = _context.Medicines1.Select(x => x).Where(x => x.Name.Contains(MedicineName));
            return Searchbytext;
            
        }

        public async Task<ThemostsoldMedicine> ThemostsoldMedicine()
        {
            var ThemostsoldMedicine = _context.Medicines1.OrderByDescending(x => x.CountsofSold).Select(x => x);
             ThemostsoldMedicine Mostsold = new ThemostsoldMedicine()
            {   id= ThemostsoldMedicine.First().id,
                Description = ThemostsoldMedicine.First().Description,
                Name = ThemostsoldMedicine.First().Name,
                Type = ThemostsoldMedicine.First().Type,
                 Summa = ThemostsoldMedicine.First().Summa


             };
            return Mostsold;
        }

        public async Task<bool> update(Medicine entity)
        {
            var updatedobject =  await _context.Medicines1.FindAsync(entity.id);
            if (updatedobject != null)
            {
              updatedobject.id = entity.id;
                updatedobject.Name = entity.Name;
                updatedobject.Type = entity.Type;
                updatedobject.Summa = entity.Summa;

                _context.Medicines1.Update(updatedobject);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
