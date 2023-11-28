using AutoMapper;
using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using CrudforMedicshop.infrastructure.Dbcontext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly Mydbcontext _dbcontext;
        private readonly IConfiguration _configuration;

        public RoleService(Mydbcontext dbcontext, IConfiguration configuration, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async  Task<Response<RoleGetDTO>> CreateRole(RoleCreateDTO roleDTO)
        {
           var Role=_dbcontext.Roles.Where(x=>x.Name==roleDTO.Name).FirstOrDefault();
            if(Role!=null)
            {
                return new("Role is already Created");
            }
            var role=_mapper.Map<Role>(roleDTO);
            _dbcontext.Roles.Attach(role);
            var Rolegetbyid=_mapper.Map<RoleGetDTO>(role);
             var rows=_dbcontext.SaveChanges();
            if (rows > 0)
            {


                return new(Rolegetbyid);
            }
            return new("something went wrong");

        }

        public async Task<bool> DeleteRole(int Roleid)
        {var Role=_dbcontext.Roles.Where(x=>x.Id==Roleid).FirstOrDefault();
            _dbcontext.Roles.Remove(Role);
            var rows=_dbcontext.SaveChanges();
            if(rows>0)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<RoleGetDTO>> GetAllRoles()
        {
            var getall = _dbcontext.Roles.Include(x=>x.Permissions);
            var getalldto=_mapper.Map<IEnumerable<RoleGetDTO>>(getall);
            return getalldto;
        }

        public async Task<RoleGetDTO> GetbyidRole(int Roleid)
        {
          var s=_dbcontext.Roles.Select(x=>x).Include(x=>x.Permissions).Where(x=>x.Id==Roleid).First();
            var getbyid = _mapper.Map<RoleGetDTO>(s);
            return getbyid;
        }

        public async Task<bool> UpdateRole(RoleGetDTO roleDTO)
        {
            var Role1 = _dbcontext.Roles.Select(x => x).Include(x=>x.Permissions).Where(x => x.Name == roleDTO.Name).First();
            var Role=_mapper.Map<Role>(roleDTO);
            if(Role!=null)
            {
                _dbcontext.Roles.Update(Role);
                _dbcontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
