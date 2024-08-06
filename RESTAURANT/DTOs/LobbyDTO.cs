using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.DTOs
{
    public class LobbyDTO
    {
        public string LobbyName { get; set; }
        public decimal? Price { get; set; }
    }
}
