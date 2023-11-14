using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Application.Interfaces
{
    public interface Irepository<T>where T : class
    {
        public Task<T> create(T entity);
        public Task<bool> update(T entity);
        public Task< bool> delete(int deleteid);
        public Task< IEnumerable<T>> Getall();
        public  Task<T> getbyid(int id);
        public Task<ThemostsoldMedicine> ThemostsoldMedicine();
        public Task<IEnumerable<Medicine>>NearlyExpiredMedicines();
        public Task<IEnumerable<Medicine>> SearchbyText(String MedicineName);
    }
}
