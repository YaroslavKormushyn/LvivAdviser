using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LvivAdviser.Domain.Entities;

namespace LvivAdviser.WebUI.Models
{
    //public class BlacklistViewModel
    //{
    //    public IEnumerable<Blacklist> Blacklist { get; set; }
    //}
    public class AddToBlacklistModel
    {
        [Required]
        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<User> NotInBlacklist { get; set; }
    }
}