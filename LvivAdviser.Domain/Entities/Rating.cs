using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LvivAdviser.Domain.Entities
{
	[Table("Rating")]
	public class Rating : EntityBase
	{
		[Required]
		[Range(1, 5)]
		public int Rate { get; set; }

		public string Comment { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }

		public User User { get; set; }

		[ForeignKey("Content")]
		public int ContentId { get; set; }
		
		public Content Content { get; set; }
	}
}
