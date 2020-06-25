using System.Linq;
using HotChocolate.Types;
using Portfolio.Core.Entities;
using Portfolio.Infrastructure.Services;

namespace Portfolio.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class TagQueries
    {
        private readonly ApplicationDataService _data;

        public TagQueries(ApplicationDataService data)
        {
            _data = data;
        }

        [UseFiltering]
        public IQueryable<Tag> Tags()
        {
            return _data.EfContext.Tags
                    .AsQueryable();
        }
    }
}
