using AutoMapper;
using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;

namespace FinalProject.Mappers
{
    public class UserMapping:Profile
    {
        public UserMapping()
        {
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UserResponse>()
                .ForMember(
                    dest => dest.PermissionNames,
                    opt => opt.MapFrom(
                            src => src.PermissionsForUsers
                            .Select(pfu => pfu.Permission.PermissionName)
                            .ToList()
                        )
                );
        }
    }
}
