using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudforMedicshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly Iservice<Medicine> _iservice;

        public MedicineController(Iservice<Medicine> service)
        {
            _iservice = service;
        }

        [ HttpGet("GetallMedicine")]
        public async Task< IActionResult> Getall()
        {
            return Ok(await _iservice.Getall());
        }

        [HttpGet("Getbyid")]
        public async Task< IActionResult> Getbyid(int id)
        {
            var user = await _iservice.GetById(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User not found");
        }

        [HttpPost("Create")]
        public async Task< IActionResult> Create(MedicineForCreateDTO Medicine)
        {
            Medicine Medicine1 = new Medicine()
            {
                CountsofSold=Medicine.CountofSold,
                CreatedDate = Medicine.CreatedDate,
                Description = Medicine.Description,
                Name= Medicine.Name,
                Type= Medicine.Type,
                ExpiredDate=Medicine.ExpiredDate,   
            };
            var result =await _iservice.Create(Medicine1);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task< IActionResult> Delete(int id)
        {
            var result = await _iservice.Delete(id);
            return Ok(result);
        }

        [HttpPatch("Update")]
        public async Task< IActionResult> Update(Medicine Medicine)
        {
            var result = await _iservice.Update(Medicine);
            return Ok(result);
        }
        [HttpGet("NearlyExpiredMedicines")]
        public async Task<IActionResult> NearlyExpiredMedicines()
        {
           var result =await _iservice.NearlyExpiredMedicines();
            return Ok(result);
        }
        [HttpGet("SearchbyText")]
        public async Task<IActionResult> SearchbyText(string Mediciname)
        {
            var result = await _iservice.SearchbyText(Mediciname);
            return Ok(result);
        }
        [HttpGet("ThemostsoldMedicine")]
        public async Task<IActionResult> ThemostsoldMedicine()
        {
            var result = _iservice.ThemostsoldMedicine();
            return Ok(result);
        }
    }
}
