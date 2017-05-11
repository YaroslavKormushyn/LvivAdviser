using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

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
		[Required(ErrorMessage = "Enter type")]
		public Type Type { get; set; }

		[Required(ErrorMessage = "Enter name")] 
		public string Name { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }
		
		public byte[] MainPhoto { get; set; }

		[NotMapped]
		[HiddenInput(DisplayValue = false)]
		public decimal Rating
		{
			get {
				return !Ratings.Any() 
					? 0 
					: Ratings.Sum(rating => rating.Rate) / Ratings.Count();
			}
		}

		[HiddenInput(DisplayValue = false)]
		public virtual IEnumerable<Rating> Ratings { get; } = new List<Rating>();
		
		[HiddenInput(DisplayValue = false)]
		public ICollection<User> FavouritedByUsers { get; set; } = new List<User>();
	}
}
