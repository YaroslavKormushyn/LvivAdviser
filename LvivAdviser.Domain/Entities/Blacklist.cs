using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LvivAdviser.Domain.Entities
{
    public class Blacklist : EntityBase
    {
        [Required]
        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        [Required]
        public string Reason { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        virtual public User User { get; set; }
    }
}
