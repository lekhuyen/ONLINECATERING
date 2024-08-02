using RESTAURANT.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTAURANT.API.Repositories
{
    public interface ILobbyRepository
    {
        Task<IEnumerable<Lobby>> GetAllLobbies();
        Task<Lobby> GetLobbyById(int id);
        Task<Lobby> CreateLobby(Lobby lobby, IEnumerable<LobbyImages> images);
        Task<Lobby> UpdateLobby(int id, Lobby lobby, IEnumerable<LobbyImages> images);
        Task DeleteLobby(int id);
    }
}
