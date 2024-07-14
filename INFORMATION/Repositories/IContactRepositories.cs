using INFORMATIONAPI.Models;

namespace INFORMATIONAPI.Repositories
{
    public interface IContactRepositories
    {
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<Contact> GetContactById(string id);
        Task CreateContact(Contact contact);
        Task<bool> UpdateContact(string id, Contact contact);
        Task<bool> DeleteContact(string id);
        Task<bool> RespondToMessage(string id, string response);
    }
}
