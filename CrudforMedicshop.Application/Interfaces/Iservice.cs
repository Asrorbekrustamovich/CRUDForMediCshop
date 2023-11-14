using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Application.Interfaces
{
    public interface Iservice<T>where T : class
    {
        public Task<T> Create(T entity);
        public Task<string> Update(T entity);
        public Task<string> Delete(int deletedid);
        public Task<IEnumerable<T>> Getall();
        public Task<T> GetById(int id);
        public Task<ThemostsoldMedicine> ThemostsoldMedicine();
        public Task<IEnumerable<Medicine>> NearlyExpiredMedicines();
        public Task<IEnumerable<Medicine>> SearchbyText(String MedicineName);
    }
}
