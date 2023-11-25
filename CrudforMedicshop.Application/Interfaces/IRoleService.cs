using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Application.Interfaces
{
    public interface IRoleService
    {
        public Task<Response<Role>> CreateRole(RoleCreateDTO role);
        public Task<bool> UpdateRole(RoleCreateDTO role);
        public Task<bool> DeleteRole(int Roleid);
        public Task<Role> GetbyidRole(int Roleid);
        public Task<IEnumerable<Role>> GetAllRoles();

    }
}
