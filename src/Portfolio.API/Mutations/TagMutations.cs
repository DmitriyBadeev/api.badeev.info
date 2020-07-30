using System.Linq;
using AutoMapper;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Portfolio.API.Mutations.InputTypes.Tag;
using Portfolio.Core.Entities;
using Portfolio.Core.Entities.Portfolio;
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

        [Authorize]
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

        [Authorize]
        public TagWork ConnectTagAndWork(ConnectTagWorkInput inputTagWork)
        {
            _logger.LogInformation($"Creating connect between tag (id = {inputTagWork.TagId}) and work (id = {inputTagWork.WorkId})");
            var connect = _data.EfContext.TagWorks.FirstOrDefault(tw =>
                tw.TagId == inputTagWork.TagId && tw.WorkId == inputTagWork.WorkId);

            if (connect != null)
            {
                _logger.LogInformation("Connect already exists");
                return null;
            }

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

        [Authorize]
        public TagWork DisconnectTagAndWork(ConnectTagWorkInput inputTagWork)
        {
            _logger.LogInformation($"Disconnecting between tag (id = {inputTagWork.TagId}) and work (id = {inputTagWork.WorkId})");
            var connect = _data.EfContext.TagWorks.FirstOrDefault(tw =>
                tw.TagId == inputTagWork.TagId && tw.WorkId == inputTagWork.WorkId);

            if (connect != null)
            {
                _data.EfContext.TagWorks.Remove(connect);
                _data.EfContext.SaveChanges();

                _logger.LogInformation($"Disconnecting between tag (id = {inputTagWork.TagId}) and work (id = {inputTagWork.WorkId}) is successfully");

                return connect;
            }

            _logger.LogInformation("Connect has not found");
            return null;
        }

        [Authorize]
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

        [Authorize]
        public Tag DeleteTag(int tagId)
        {
            _logger.LogInformation($"Deleting tag with id - {tagId}");

            var tagEntity = _data.EfContext.Tags.Find(tagId);
            _data.EfContext.Remove(tagEntity);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Tag with id - {tagId} is deleted successfully");
            return tagEntity;
        }

        [Authorize]
        public FrontendTag AddFrontendTag(int tagId)
        {
            _logger.LogInformation($"Adding frontend tag - {tagId}");

            var tag = _data.EfContext.Tags.Find(tagId);
            var frontendTag = new FrontendTag()
            {
                TagId = tagId,
                Tag = tag
            };

            var tagEntity = _data.EfContext.FrontendTags.Add(frontendTag);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Frontend tag {tagId} has added successfully");
            return tagEntity.Entity;
        }

        [Authorize]
        public BackendTag AddBackendTag(int tagId)
        {
            _logger.LogInformation($"Adding backend tag - {tagId}");

            var tag = _data.EfContext.Tags.Find(tagId);
            var backendTag = new BackendTag()
            {
                TagId = tagId,
                Tag = tag
            };

            var tagEntity = _data.EfContext.BackendTags.Add(backendTag);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Backend tag {tagId} has added successfully");
            return tagEntity.Entity;
        }

        [Authorize]
        public FrontendTag DeleteFrontendTag(int tagId)
        {
            _logger.LogInformation($"Deleting frontend tag - {tagId}");

            var frontendTag = _data.EfContext.FrontendTags.FirstOrDefault(t => t.TagId == tagId);

            if (frontendTag != null)
            {
                var tagEntity = _data.EfContext.FrontendTags.Remove(frontendTag);
                _data.EfContext.SaveChanges();
                _logger.LogInformation($"Backend tag {tagId} has deleted successfully");
                return tagEntity.Entity;
            }

            _logger.LogInformation($"Backend tag {tagId} has not found");
            return null;
        }

        [Authorize]
        public BackendTag DeleteBackendTag(int tagId)
        {
            _logger.LogInformation($"Deleting backend tag - {tagId}");

            var backendTag = _data.EfContext.BackendTags.FirstOrDefault(t => t.TagId == tagId);

            if (backendTag != null)
            {
                var tagEntity = _data.EfContext.BackendTags.Remove(backendTag);
                _data.EfContext.SaveChanges();
                _logger.LogInformation($"Backend tag {tagId} has deleted successfully");
                return tagEntity.Entity;
            }

            _logger.LogInformation($"Backend tag {tagId} has not found");
            return null;
        }
    }
}
