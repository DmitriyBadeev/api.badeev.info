using AutoMapper;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Portfolio.API.Mutations.InputTypes;
using Portfolio.API.Mutations.InputTypes.Tag;
using Portfolio.Core.Entities;
using Portfolio.Infrastructure.Services;

namespace Portfolio.API.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class TagMutations
    {
        private readonly ApplicationDataService _data;
        private readonly ILogger<TagMutations> _logger;

        public TagMutations(ApplicationDataService data, ILogger<TagMutations> logger)
        {
            _data = data;
            _logger = logger;
        }

        public Tag CreateTag(CreateTagInput inputTag)
        {
            _logger.LogInformation($"Creating tag {inputTag.Title}");
            var tag = new MapperConfiguration(cfg => cfg.CreateMap<CreateTagInput, Tag>())
                .CreateMapper()
                .Map<Tag>(inputTag);

            var createdTag = _data.EfContext.Tags.Add(tag);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Tag {inputTag.Title} is created successfully");
            return createdTag.Entity;
        }

        public TagWork ConnectTagAndWork(ConnectTagWorkInput inputTagWork)
        {
            _logger.LogInformation($"Creating connect between tag (id = {inputTagWork.TagId}) and work (id = {inputTagWork.WorkId})");
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

            _logger.LogInformation($"Connect between tag (id = {inputTagWork.TagId}) and work (id = {inputTagWork.WorkId}) is created successfully");
            return createdConnection.Entity;
        }

        public Tag UpdateTag(UpdateTagInput inputTag)
        {
            _logger.LogInformation($"Updating tag (id = {inputTag.Id})");

            var tagEntity = _data.EfContext.Tags.Find(inputTag.Id);

            tagEntity.Title = inputTag.Title ?? tagEntity.Title;
            tagEntity.Description = inputTag.Description ?? tagEntity.Description;

            _data.EfContext.Tags.Update(tagEntity);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Tag (id = {inputTag.Id}) is updated successfully");
            return tagEntity;
        }

        public Tag DeleteTag(int tagId)
        {
            _logger.LogInformation($"Deleting tag with id - {tagId}");

            var tagEntity = _data.EfContext.Tags.Find(tagId);
            _data.EfContext.Remove(tagEntity);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Tag with id - {tagId} is deleted successfully");
            return tagEntity;
        }
    }
}
