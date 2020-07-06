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
                .OrderByDescending(w => w.Date)
                .AsQueryable();
        }

        public IQueryable<Work> WorksByTagIds(int[] tagIds)
        {
            var workIds = _data.EfContext.TagWorks
                .Where(tw => tagIds.Contains(tw.TagId))
                .Select(tw => tw.WorkId);

            return _data.EfContext.Works
                .Where(w => workIds.Contains(w.Id));
        }

        public Work WorkById(int id)
        {
            return _data.EfContext.Works
                .Include(w => w.Authors)
                .FirstOrDefault(w => w.Id == id);
        }
    }
}
