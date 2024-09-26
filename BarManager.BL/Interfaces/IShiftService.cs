using BarManager.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarManager.BL.Interfaces
{
    public interface IShiftService
    {
        Task<Shift> Create(Shift shift);

        Task<Shift> Update(Shift shift);

        Task Delete(int id);

        Task<Shift> GetById(int id);

        Task<IEnumerable<Shift>> GetAll();
    }
}
