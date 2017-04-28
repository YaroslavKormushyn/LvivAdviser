using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LvivAdviser.Domain.Entities
{
	public enum Type
	{
		Food = 1,
		Rest = 2,
		FreeTime = 3
	}

	[Table("Content")]
	public class Content : EntityBase
	{
		public Content()
		{
			Ratings = new List<Rating>();	
		}

		[Required]
		public Type Type { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }
		
		public byte[] MainPhoto { get; set; }

		[NotMapped]
		public decimal Rating
		{
			get {
				return Ratings.Count() == 0 
					? 0 
					: Ratings.Sum(rating => rating.Rate) / Ratings.Count();
			}
		}

		public virtual IEnumerable<Rating> Ratings { get; set; }
	}
}
