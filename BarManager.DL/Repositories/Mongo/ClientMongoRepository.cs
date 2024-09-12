using BarManager.DL.Interfaces;
using BarManager.Models.Common;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client = BarManager.Models.DTO.Client;

namespace BarManager.DL.Repositories.Mongo
{
    public class ClientMongoRepository : IClientRepository
    {
        private readonly IMongoCollection<Client> _clientCollection;

        public ClientMongoRepository(IOptions<MongoDbConfiguration> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            var database = client.GetDatabase(config.Value.DatabaseName);

            _clientCollection = database.GetCollection<Client>("Clients");
        }

        public async Task<Client> Create(Client userPosition)
        {
            await _clientCollection.InsertOneAsync(userPosition);

            return userPosition;
        }

        public async Task <Client> Update(Client client)
        {
            await _clientCollection.ReplaceOneAsync(clientToReplace => clientToReplace.Id == client.Id, client);
            return client;
        }

        public async Task Delete(int id)
        {
            await _clientCollection.DeleteOneAsync(client => client.Id == id);

        }

        public async Task<Client> GetById(int id)
        {
            var result = await _clientCollection.FindAsync(userPosition => userPosition.Id == id);

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            var result = await _clientCollection.FindAsync(client => true);

            return result.ToEnumerable();
        }
    }
}
