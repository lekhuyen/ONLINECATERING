using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INFORMATIONAPI.Services
{
    public class ContactService : IContactRepositories
    {
        private readonly IMongoCollection<Contact> _contacts;

        public ContactService(DatabaseContext context)
        {
            _contacts = context.Contact; // Assuming DatabaseContext has a Contact property of type IMongoCollection<Contact>
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            return await _contacts.Find(_ => true).ToListAsync();
        }

        public async Task<Contact> GetContactById(string id)
        {
            return await _contacts.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateContact(Contact contact)
        {
            await _contacts.InsertOneAsync(contact);
        }

        public async Task<bool> UpdateContact(string id, Contact contact)
        {
            var result = await _contacts.ReplaceOneAsync(c => c.Id == id, contact);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteContact(string id)
        {
            var result = await _contacts.DeleteOneAsync(c => c.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> RespondToMessage(string id, string response)
        {
            var filter = Builders<Contact>.Filter.Eq(c => c.Id, id);
            var update = Builders<Contact>.Update
                .Set(c => c.IsAdminResponse, true)
                .Set(c => c.ResponseMessage, response)
                .Set(c => c.ResponseDate, DateTime.UtcNow);

            var result = await _contacts.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
