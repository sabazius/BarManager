using BarManager.BL.Interfaces;
using BarManager.DL.Interfaces;
using BarManager.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManager.BL.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        public ShiftService(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        public async Task<Shift> Create(Shift shift)
        {
            var result = await _shiftRepository.GetAll();

            var index = result.OrderByDescending(x => x.Id).FirstOrDefault()?.Id;

            shift.Id = (int)(index != null ? index + 1 : 1);

            return await _shiftRepository.Create(shift);
        } 
        public async Task<Shift> Update(Shift shift)
        {
            return await _shiftRepository.Update(shift);
        }
        public async Task<Shift> GetById(int id)
        {
            return await _shiftRepository.GetById(id);
        }

        public async Task Delete(int id)
        {
            await _shiftRepository.Delete(id);
        }

        public async Task<IEnumerable<Shift>> GetAll()
        {
            return await _shiftRepository.GetAll();
        }

        
    }
}
