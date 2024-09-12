using System;
using BarManager.BL.Interfaces;
using BarManager.DL.Interfaces;
using BarManager.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;


namespace BarManager.BL.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger _logger;

        public ClientService(IClientRepository clientRepository, ILogger logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<Client> Create(Client client)
        {
            var result = await _clientRepository.GetAll();

            var index = result.OrderByDescending(x => x.Id).FirstOrDefault()?.Id;
            
            client.Id = (int) (index != null ? index + 1 : 1);
            return await _clientRepository.Create(client);
        }

        public async Task <Client> Update(Client client)
        {
            return await _clientRepository.Update(client);
        }

        public async Task Delete(int id)
        {
            await _clientRepository.Delete(id);
        }

        public Task<Client> GetById(int id)
        {
            return _clientRepository.GetById(id);
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _clientRepository.GetAll();
        }
    }
}
