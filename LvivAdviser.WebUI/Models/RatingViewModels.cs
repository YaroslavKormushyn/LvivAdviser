using System.ComponentModel.DataAnnotations;

namespace LvivAdviser.WebUI.Models
{
	public class RatingEditModel
	{
		public int RatingId { get; set; }

		[Required]
		public string UserId { get; set; }
		[Required]
		public int ContentId { get; set; }

		[Required]
		public int Rating { get; set; }

		public string Comment { get; set; }
	}

	public class CommentEditModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public int CommentId { get; set; }

		public string Comment { get; set; }
	}
}