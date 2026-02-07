using AutoMapper;
using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;

namespace FinalProject.Mappers
{
    public class ProjectTaskMapping : Profile
    {
        public ProjectTaskMapping()
        {
            CreateMap<CreateProjectTaskRequest, ProjectTask>();
            CreateMap<UpdateProjectTaskRequest, ProjectTask>();
            CreateMap<ProjectTask, ProjectTaskResponse>();
        }
    }
}
