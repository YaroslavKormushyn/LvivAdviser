using LvivAdviser.Domain.Entities;

using System.Collections.Generic;

namespace LvivAdviser.WebUI.Models
{
	public class ContentListViewModel
    {
        public IEnumerable<Content> Contents { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentType { get; set; }
    }
}