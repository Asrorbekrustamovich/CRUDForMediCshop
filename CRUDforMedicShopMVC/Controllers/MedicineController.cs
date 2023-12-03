using AutoMapper;
using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using CrudforMedicshop.infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CRUDforMedicShopMVC.Controllers
{
    public class MedicineController : Controller
    {

        private readonly Iservice<Medicine> _iservice;
        private readonly IMapper _mapper;

        public MedicineController(Iservice<Medicine> service, IMapper mapper)
        {
            _iservice = service;
            _mapper = mapper;
        }
        [HttpGet("GetallMedicine")]
        [EnableRateLimiting("Tokenbox")]
        //[AuthefrationAttributeFilter("GetallMedicine")]
        public async Task<IActionResult> GetallMedicine()
        {
            return Ok(await _iservice.Getall());
        }

        [HttpGet("GetbyidMedicine")]
        [AuthefrationAttributeFilter("GetbyidMedicine")]
        public async Task<IActionResult> GetbyidMedicine(int id)
        {
            var user = await _iservice.GetById(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User not found");
        }

        [HttpPost("CreateMedicine")]
        [AuthefrationAttributeFilter("CreateMedicine")]
        public async Task<IActionResult> CreateMedicine(MedicineForCreateDTO Medicine)
        {
            Medicine Medicine1 = _mapper.Map<Medicine>(Medicine);

            var result = await _iservice.Create(Medicine1);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        [AuthefrationAttributeFilter("DeleteMedicine")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var result = await _iservice.Delete(id);
            return Ok(result);
        }

        [HttpPatch("Update")]
        [AuthefrationAttributeFilter("UpdateMedicine")]
        public async Task<IActionResult> UpdateMedicine(Medicine Medicine)
        {
            var result = await _iservice.Update(Medicine);
            return Ok(result);
        }
        [HttpGet("NearlyExpiredMedicines")]
        [AuthefrationAttributeFilter("NearlyExpiredMedicines")]
        public async Task<IActionResult> NearlyExpiredMedicines()
        {
            var result = await _iservice.NearlyExpiredMedicines();
            return Ok(result);
        }
        [HttpGet("SearchbyText")]
        [AuthefrationAttributeFilter("SearchbyText")]
        public async Task<IActionResult> SearchbyText(string Mediciname)
        {
            var result = await _iservice.SearchbyText(Mediciname);
            return Ok(result);
        }
        [HttpGet("ThemostsoldMedicine")]
        [AuthefrationAttributeFilter("ThemostsoldMedicine")]
        public async Task<IActionResult> ThemostsoldMedicine()
        {
            var result = _iservice.ThemostsoldMedicine();
            return Ok(result);
        }
    }
}
