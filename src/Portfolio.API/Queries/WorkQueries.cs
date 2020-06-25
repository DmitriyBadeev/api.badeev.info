using System.Linq;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities;
using Portfolio.Infrastructure.Services;

namespace Portfolio.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class WorkQueries
    {
        private readonly ApplicationDataService _data;

        public WorkQueries(ApplicationDataService data)
        {
            _data = data;
        }

        [UsePaging]
        [UseFiltering]
        public IQueryable<Work> Works()
        {
            return _data.EfContext.Works
                .Include(w => w.Authors)
                .Include(w => w.Tags)
                .OrderByDescending(w => w.Date)
                .AsQueryable();
        }

        public Work WorksById(int id)
        {
            return _data.EfContext.Works
                .Include(w => w.Authors)
                .Include(w => w.Tags)
                .FirstOrDefault(w => w.Id == id);
        }
    }
}
