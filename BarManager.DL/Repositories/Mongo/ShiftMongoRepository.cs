using BarManager.DL.Interfaces;
using BarManager.Models.DTO;
using System.Collections.Generic;
using BarManager.Models.Common;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace BarManager.DL.Repositories.Mongo
{
    public class ShiftMongoRepository : IShiftRepository

    {
        private readonly IMongoCollection<Shift> _shiftCollection;
        
        public ShiftMongoRepository(IOptions<MongoDbConfiguration> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            var database = client.GetDatabase(config.Value.DatabaseName);

            _shiftCollection = database.GetCollection<Shift>("Shifts");
        }

        public async Task<Shift> Create(Shift shift)
        {
            await _shiftCollection.InsertOneAsync(shift);

            return shift;
        }

        public async Task<IEnumerable<Shift>> GetAll()
        {
            var result = await _shiftCollection.FindAsync(shift => true);
            return result.ToList();
        }

        public async Task<Shift> GetById(int id)
        {
            var result = await _shiftCollection.FindAsync(shift => shift.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<Shift> Update(Shift shift)
        {
            await _shiftCollection.ReplaceOneAsync(shiftToReplace => shiftToReplace.Id == shift.Id, shift);
            return shift;
        }

        public async Task Delete(int id)
        {
 
            await _shiftCollection.DeleteOneAsync(shift => shift.Id == id);

            
        }
    }
}
