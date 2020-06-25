using AutoMapper;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Portfolio.API.Mutations.InputTypes;
using Portfolio.Core.Entities;
using Portfolio.Infrastructure.Services;

namespace Portfolio.API.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class WorkMutations
    {
        private readonly ApplicationDataService _data;
        private readonly ILogger<WorkMutations> _logger;

        public WorkMutations(ApplicationDataService data, ILogger<WorkMutations> logger)
        {
            _data = data;
            _logger = logger;
        }

        public Work CreateWork(CreateWorkInput inputWork)
        {
            _logger.LogInformation("Creating work");
            var work = new MapperConfiguration(cfg => cfg.CreateMap<CreateWorkInput, Work>())
                .CreateMapper()
                .Map<Work>(inputWork);

            var createdWork = _data.EfContext.Works.Add(work);
            _data.EfContext.SaveChanges();

            _logger.LogInformation("Work is created successfully");
            return createdWork.Entity;
        }
    }
}
