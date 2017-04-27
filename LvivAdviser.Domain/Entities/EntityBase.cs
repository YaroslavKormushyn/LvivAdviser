using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace LvivAdviser.Domain.Entities
{
	public abstract class EntityBase
	{
		[Key]
		[HiddenInput(DisplayValue = false)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
	}
}
