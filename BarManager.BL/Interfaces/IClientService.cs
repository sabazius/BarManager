using BarManager.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarManager.BL.Interfaces
{
    public interface IClientService
    {
        Task<Client> Create(Client client);

        Task<Client> Update(Client client);

        Task Delete(int id);

        Task<Client> GetById(int id);

        Task<IEnumerable<Client>> GetAll();
    }
}
