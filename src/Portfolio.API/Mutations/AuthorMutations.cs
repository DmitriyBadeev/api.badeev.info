using AutoMapper;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Portfolio.API.Mutations.InputTypes.Author;
using Portfolio.Core.Entities;
using Portfolio.Infrastructure.Services;

namespace Portfolio.API.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class AuthorMutations
    {
        private readonly ApplicationDataService _data;
        private readonly ILogger<AuthorMutations> _logger;

        public AuthorMutations(ApplicationDataService data, ILogger<AuthorMutations> logger)
        {
            _data = data;
            _logger = logger;
        }

        [Authorize]
        public Author CreateAuthor(CreateAuthorInput inputAuthor)
        {
            _logger.LogInformation($"Creating author {inputAuthor.Name}");
            var work = _data.EfContext.Works.Find(inputAuthor.WorkId);

            var author = new MapperConfiguration(cfg =>
                    cfg.CreateMap<CreateAuthorInput, Author>()
                        .ForMember("Work", opt => opt.MapFrom(c => work)))
                .CreateMapper()
                .Map<Author>(inputAuthor);

            var createdAuthor = _data.EfContext.Authors.Add(author);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Author {inputAuthor.Name} is created successfully (id = {createdAuthor.Entity.Id})");
            return createdAuthor.Entity;
        }

        [Authorize]
        public Author UpdateAuthor(UpdateAuthorInput inputAuthor)
        {
            _logger.LogInformation($"Updating author (id = {inputAuthor.Id})");

            var authorEntity = _data.EfContext.Authors.Find(inputAuthor.Id);

            authorEntity.Name = inputAuthor.Name ?? authorEntity.Name;
            authorEntity.Role = inputAuthor.Role ?? authorEntity.Role;
            authorEntity.Link = inputAuthor.Link ?? authorEntity.Link;
            authorEntity.WorkId = inputAuthor.WorkId ?? authorEntity.WorkId;

            _data.EfContext.Authors.Update(authorEntity);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Author (id = {inputAuthor.Id}) is updated successfully");
            return authorEntity;
        }

        [Authorize]
        public Author DeleteAuthor(int authorId)
        {
            _logger.LogInformation($"Deleting author with id = {authorId}");

            var authorEntity = _data.EfContext.Authors.Find(authorId);
            _data.EfContext.Remove(authorEntity);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Author with id = {authorId} is deleted successfully");
            return authorEntity;
        }
    }
}
