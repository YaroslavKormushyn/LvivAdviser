using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace LvivAdviser.Domain.Entities
{
	public enum Type
	{
		Food= 1,
		Rest = 2,
		FreeTime = 3
	}

	[Table("Content")]
	public class Content
	{
		[Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

		[Required]
		public Type Type { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

		public object MainPhoto { get; set; }
		public decimal Rating { get; set; }

		public virtual IEnumerable<Rating> Ratings { get; set; }
	}
}
