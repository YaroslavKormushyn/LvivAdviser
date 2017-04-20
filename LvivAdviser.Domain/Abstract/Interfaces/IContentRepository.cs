using LvivAdviser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LvivAdviser.Domain.Abstract.Interfaces
{
	public interface IContentRepository : IRepository<Content>
	{
        IEnumerable<Content> Contents { get; }
        void SaveContent(Content content);
    }
}
