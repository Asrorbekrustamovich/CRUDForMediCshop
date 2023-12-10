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
            CreateMap<UserDTO,User>().ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RolesId.Select(roleId => new Role { Id = roleId })));
            CreateMap<RoleCreateDTO, Role>().ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.userids.Select(userid => new User { Id = userid })))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissionids.Select(permissionid => new permission { id = permissionid })));
            CreateMap<Role, RoleGetDTO>()
            .ForMember(dest => dest.userids, opt => opt.MapFrom(src => src.Users.Select(user => user.Id)))
            .ForMember(dest => dest.Permissionids, opt => opt.MapFrom(src => src.Permissions.Select(permission => permission.id)));
        }
    }
  }
 
