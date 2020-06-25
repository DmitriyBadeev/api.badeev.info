using AutoMapper;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Portfolio.API.Mutations.InputTypes;
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

        public Author CreateAuthor(CreateAuthorInput inputAuthor)
        {
            _logger.LogInformation("Creating author");
            var work = _data.EfContext.Works.Find(inputAuthor.WorkId);

            var author = new MapperConfiguration(cfg =>
                    cfg.CreateMap<CreateAuthorInput, Author>()
                        .ForMember("Work", opt => opt.MapFrom(c => work)))
                .CreateMapper()
                .Map<Author>(inputAuthor);

            var createdAuthor = _data.EfContext.Authors.Add(author);
            _data.EfContext.SaveChanges();

            _logger.LogInformation("Author is created successfully");
            return createdAuthor.Entity;
        }
    }
}
