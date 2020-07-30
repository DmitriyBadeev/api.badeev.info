using System.Linq;
using HotChocolate.Types;
using Portfolio.Core.Entities;
using Portfolio.Core.Entities.Portfolio;
using Portfolio.Infrastructure.Services;

namespace Portfolio.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class AuthorQueries
    {
        private readonly ApplicationDataService _data;

        public AuthorQueries(ApplicationDataService data)
        {
            _data = data;
        }

        [UseFiltering]
        public IQueryable<Author> Authors()
        {
            return _data.EfContext.Authors
                .AsQueryable();
        }
    }
}
