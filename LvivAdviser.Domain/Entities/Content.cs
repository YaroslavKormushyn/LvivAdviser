using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
		[Required(ErrorMessage = "Please specify a type")]
		public Type Type { get; set; }

		[Required(ErrorMessage = "Please enter a content name")]
		public string Name { get; set; }

		[DataType(DataType.MultilineText)]
		[Required(ErrorMessage = "Please enter a description")]
		public string Description { get; set; }

		public object MainPhoto { get; set; }

		public decimal Rating { get; set; }

		public virtual IEnumerable<Rating> Ratings { get; set; }
	}
}
