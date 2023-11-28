using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using CrudforMedicshop.infrastructure.Dbcontext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CrudforMedicshop.infrastructure.Services;

namespace CrudforMedicshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly Mydbcontext _dbcontext;

        public RoleController(IRoleService roleService, Mydbcontext dbcontext)
        {
            _roleService = roleService;
            _dbcontext = dbcontext;
        }
        [HttpGet("GetallRole")]
        [AuthefrationAttributeFilter("GetallRole")]
        
        public async Task<IActionResult> GetallRole()
        {
            return Ok(await _roleService.GetAllRoles());
        }
        [HttpGet("GetbyidRoles")]
        [AuthefrationAttributeFilter("GetbyidRoles")]
        public async Task<IActionResult> GetbyidRoles(int Roleid)
        {
            return Ok(await _roleService.GetbyidRole(Roleid));
        }
        [HttpPost("CreateRole")]
        [AuthefrationAttributeFilter("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleCreateDTO roleCreateDTO)
        {
            var Role = _dbcontext.Roles.Where(x => x.Name == roleCreateDTO.Name).FirstOrDefault();
            if (Role == null)
            {
                await _roleService.CreateRole(roleCreateDTO);
            }
            return Ok("Role Already exist");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(int RoleId)
        {
            var Role = _dbcontext.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
            if (Role == null)
            {
                await _roleService.DeleteRole(RoleId);
            }
            return Ok("Role not Found");
        }
        [HttpPatch]
        public async Task<IActionResult> Update(RoleCreateDTO role)
        {
            var Role = _dbcontext.Roles.Where(x => x.Id ==role.Id).FirstOrDefault();
            if (Role == null)
            {
                await _roleService.UpdateRole(role);
            }
            return Ok("Role not Found");
        }
    }
}
