using AutoMapper;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Portfolio.API.Mutations.InputTypes;
using Portfolio.Core.Entities;
using Portfolio.Infrastructure.Services;

namespace Portfolio.API.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class GeneralMutation
    {
        private readonly ApplicationDataService _data;
        private readonly ILogger<GeneralMutation> _logger;

        public GeneralMutation(ApplicationDataService data, ILogger<GeneralMutation> logger)
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
