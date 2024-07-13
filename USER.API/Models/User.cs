﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USER.API.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; } = false;
        public string Role { get; set; } = "User";
        public string? RefeshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<FavoriteList>? FavoriteLists { get; set; }
        public int GradeId { get; set; }
        public Grade? Grade { get; set; }

    }
    
}
