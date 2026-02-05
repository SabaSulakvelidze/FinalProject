using AutoMapper;
using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;

namespace FinalProject.Mappers
{
    public class PermissionMapping:Profile
    {
        public PermissionMapping()
        {
            CreateMap<PermissionRequest, Permission>();
            CreateMap<Permission, PermissionResponse>();
        }
    }
}
