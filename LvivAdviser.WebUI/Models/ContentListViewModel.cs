using System.Collections.Generic;
using LvivAdviser.Domain.Entities;

namespace LvivAdviser.WebUI.Models
{
    public class ContentListViewModel
    {
        public IEnumerable<Content> Contents { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentType { get; set; }
    }
}