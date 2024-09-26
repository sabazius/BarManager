using AutoMapper;
using BarManager.BL.Interfaces;
using BarManager.Models.DTO;
using BarManager.Models.Requests;
using BarManager.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BarManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        private readonly IMapper _mapper;

        public ShiftController(IShiftService shiftService, IMapper mapper)
        {
            _shiftService = shiftService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() 
        {
            var result = await _shiftService.GetAll();

            return Ok(result);
            
        }

        [HttpGet("GetByID")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (Id <= 0) return BadRequest();

            var result = await _shiftService.GetById(Id);

            if (result == null) return NotFound(Id);

            var response =  _mapper.Map<ShiftResponse>(result);

            return Ok(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateShift([FromBody] ShiftRequest shiftRequest)
        {
            if (shiftRequest == null) return BadRequest();

            var shift = _mapper.Map<Shift>(shiftRequest);

            var result = await _shiftService.Create(shift);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest(id);

             await _shiftService.Delete(id);

            return Ok();
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Shift shift)
        {
            if (shift == null) return BadRequest();

            var searchShift = _shiftService.GetById(shift.Id);

            if (searchShift == null ) return NotFound();

            var result = await _shiftService.Update(shift);

            return Ok(result);

        }
    }
}
