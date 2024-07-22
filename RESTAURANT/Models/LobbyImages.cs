using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
	public class LobbyImages
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? ImagesUrl { get; set; }
		public int LobbyId { get; set; }
		public Lobby? Lobby { get; set; }
	}
}
