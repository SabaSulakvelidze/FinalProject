using AutoMapper;
using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;

namespace FinalProject.Mappers
{
    public class ProjectMapping:Profile
    {
        public ProjectMapping()
        {
            CreateMap<CreateProjectRequest, Project>();
            CreateMap<UpdateProjectRequest, Project>();
            CreateMap<Project, ProjectResponse>();
        }
    }
}
