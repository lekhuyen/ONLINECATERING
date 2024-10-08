﻿using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RESTAURANT.API.DTOs;

namespace RESTAURANT.API.Models
{
	public class Lobby
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string LobbyName { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
        public string? Area { get; set; }
        public int? Type { get; set; }
        public decimal? Price { get; set; }
        public ICollection<LobbyImages>? LobbyImages { get; set; }

        public ICollection<Order>? Order { get; set; }


    }
}
