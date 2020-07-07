﻿using System.Linq;
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
        public IQueryable<Work> Works()
        {
            return _data.EfContext.Works
                .OrderByDescending(w => w.Date)
                .AsQueryable();
        }

        [UsePaging]
        public IQueryable<Work> WorksByTagIds(int[] tagIds, bool isFilter)
        {
            if (isFilter)
                return _data.EfContext.Works
                    .Include(w => w.Tags)
                    .Where(w => tagIds.All(id => w.Tags.Any(t => t.TagId == id)))
                    .OrderByDescending(w => w.Date)
                    .AsQueryable();

            return _data.EfContext.Works
                .OrderByDescending(w => w.Date)
                .AsQueryable();
        }

        public Work WorkById(int id)
        {
            return _data.EfContext.Works
                .Include(w => w.Authors)
                .FirstOrDefault(w => w.Id == id);
        }
    }
}
