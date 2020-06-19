using System.Linq;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities;
using Portfolio.Infrastructure;

namespace Portfolio.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class GeneralQuery
    {
        private readonly AppDbContext _data;

        public GeneralQuery(AppDbContext data)
        {
            _data = data;
        }

        [UsePaging]
        [UseFiltering]
        public IQueryable<Work> Works => _data.Works
            .Include(w => w.Authors)
            .Include(w => w.Tags)
            .AsQueryable();

        [UseFiltering]
        public IQueryable<Tag> Tags => _data.Tags
            .AsQueryable();

        [UseFiltering]
        public IQueryable<Author> Authors => _data.Authors
            .AsQueryable();
    }
}
