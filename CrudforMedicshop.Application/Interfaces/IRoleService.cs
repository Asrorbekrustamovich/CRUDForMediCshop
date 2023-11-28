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
        public Task<Response<RoleGetDTO>> CreateRole(RoleCreateDTO role);
        public Task<bool> UpdateRole(RoleGetDTO role);
        public Task<bool> DeleteRole(int Roleid);
        public Task<RoleGetDTO> GetbyidRole(int Roleid);
        public Task<IEnumerable<RoleGetDTO>> GetAllRoles();

    }
}
