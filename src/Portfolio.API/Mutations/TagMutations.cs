using AutoMapper;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Portfolio.API.Mutations.InputTypes;
using Portfolio.Core.Entities;
using Portfolio.Infrastructure.Services;

namespace Portfolio.API.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class TagMutations
    {
        private readonly ApplicationDataService _data;
        private readonly ILogger<GeneralMutation> _logger;

        public TagMutations(ApplicationDataService data, ILogger<GeneralMutation> logger)
        {
            _data = data;
            _logger = logger;
        }

        public Tag CreateTag(CreateTagInput inputTag)
        {
            _logger.LogInformation("Creating tag");
            var tag = new MapperConfiguration(cfg => cfg.CreateMap<CreateTagInput, Tag>())
                .CreateMapper()
                .Map<Tag>(inputTag);

            var createdTag = _data.EfContext.Tags.Add(tag);
            _data.EfContext.SaveChanges();

            _logger.LogInformation("Tag is created successfully");
            return createdTag.Entity;
        }

        public TagWork ConnectTagAndWork(ConnectTagWorkInput inputTagWork)
        {
            _logger.LogInformation("Creating connect between tag and work");
            var work = _data.EfContext.Works.Find(inputTagWork.WorkId);
            var tag = _data.EfContext.Tags.Find(inputTagWork.TagId);

            var tagWork = new TagWork()
            {
                Tag = tag,
                TagId = inputTagWork.TagId,
                Work = work,
                WorkId = inputTagWork.WorkId
            };

            var createdConnection = _data.EfContext.TagWorks.Add(tagWork);
            _data.EfContext.SaveChanges();

            _logger.LogInformation("Connect between tag and work is created successfully");
            return createdConnection.Entity;
        }
    }
}
