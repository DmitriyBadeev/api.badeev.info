using AutoMapper;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Portfolio.API.Mutations.InputTypes;
using Portfolio.API.Mutations.InputTypes.Work;
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
            _logger.LogInformation($"Creating work {inputWork.Title}");
            var work = new MapperConfiguration(cfg => cfg.CreateMap<CreateWorkInput, Work>())
                .CreateMapper()
                .Map<Work>(inputWork);

            var createdWork = _data.EfContext.Works.Add(work);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Work {inputWork.Title} is created successfully (id = ${createdWork.Entity.Id})");
            return createdWork.Entity;
        }

        public Work UpdateWork(UpdateWorkInput inputWork)
        {
            _logger.LogInformation($"Updating work (id = {inputWork.Id})");

            var workEntity = _data.EfContext.Works.Find(inputWork.Id);

            workEntity.Title = inputWork.Title ?? workEntity.Title;
            workEntity.ShortDescription = inputWork.ShortDescription ?? workEntity.ShortDescription;
            workEntity.Date = inputWork.Date ?? workEntity.Date;
            workEntity.Html = inputWork.Html ?? workEntity.Html;
            workEntity.Link = inputWork.Link ?? workEntity.Link;
            workEntity.ImgPath = inputWork.ImgPath ?? workEntity.ImgPath;
            workEntity.Task = inputWork.Task ?? workEntity.Task;

            _data.EfContext.Works.Update(workEntity);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Work (id = {inputWork.Id}) is updated successfully");
            return workEntity;
        }

        public Work DeleteWork(int workId)
        {
            _logger.LogInformation($"Deleting work (id = {workId})");

            var workEntity = _data.EfContext.Works.Find(workId);
            _data.EfContext.Remove(workEntity);
            _data.EfContext.SaveChanges();

            _logger.LogInformation($"Work (id = {workId}) is deleted successfully");
            return workEntity;
        }
    }
}
