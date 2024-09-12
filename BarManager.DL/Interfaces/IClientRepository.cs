using BarManager.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.DL.Interfaces
{
    public interface IClientRepository
    {
        Task <Client> Create(Client client);

        Task <Client> Update(Client client);

        Task Delete(int id);

        Task<Client> GetById(int id);

        Task<IEnumerable<Client>> GetAll();
    }
}
