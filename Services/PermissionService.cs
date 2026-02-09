using AutoMapper;
using Azure.Core;
using FinalProject.Exceptions;
using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services
{
    public class PermissionService(
        AlgoUniFinalProjectDbContext context,
        IMapper mapper
        ) : IPermissionService
    {
        public async Task AssignPermissionToUser(int userId, int permissionId)
        {
            var user = await context.Users.FindAsync(userId)
                ?? throw new ElementNotFoundException($"User with id {userId} was not found");

            var permission = await context.Permissions.FindAsync(permissionId)
                ?? throw new ElementNotFoundException($"Permission with id {permissionId} was not found");

            var exists = await context.PermissionsForUsers
                .AnyAsync(pfu => pfu.UserId == userId && pfu.PermissionId == permissionId);

            if (exists) throw new ConflictException($"User with id {userId} is already assigned permission with id {permissionId}");

            await context.PermissionsForUsers
                .AddAsync(new PermissionsForUser() { PermissionId = permissionId, UserId = userId });
            await context.SaveChangesAsync();
        }

        public async Task RemovePermissionForUser(int userId, int permissionId)
        {
            var user = await context.Users.FindAsync(userId)
                ?? throw new ElementNotFoundException($"User with id {userId} was not found");

            var permission = await context.Permissions.FindAsync(permissionId)
                ?? throw new ElementNotFoundException($"Permission with id {permissionId} was not found");

            var permForUser = await context.PermissionsForUsers
                .FirstOrDefaultAsync(pfu => pfu.UserId == userId && pfu.PermissionId == permissionId);
            if (permForUser == null)
                throw new ConflictException($"User with id {userId} does not have permission with id {permissionId}");

            context.PermissionsForUsers.Remove(permForUser);

            await context.SaveChangesAsync();
        }

        public async Task<PermissionResponse> CreatePermission(PermissionRequest request)
        {
            var permission = mapper.Map<Permission>(request);

            context.Permissions.Add(permission);
            await context.SaveChangesAsync();

            return mapper.Map<PermissionResponse>(permission);
        }

        public async Task DeletePermission(int id)
        {
            var permission = await context.Permissions
                .Include(p => p.PermissionsForUsers)
                .FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new ElementNotFoundException($"Permission with id {id} was not found");

            if (permission.PermissionsForUsers.Count != 0)
            {
                var userIds = permission.PermissionsForUsers.Select(pfu => pfu.UserId);
                throw new ConflictException($"This permission is curently assigned to users with ids: {string.Join(", ", userIds)}");
            }


            context.Permissions.Remove(permission);

            await context.SaveChangesAsync();
        }

        public async Task<List<PermissionResponse>> GetAllPermissions()
        {
            var permissions = await context.Permissions.ToListAsync();

            return mapper.Map<List<PermissionResponse>>(permissions);
        }

        public async Task<PermissionResponse> GetPermissionById(int id)
        {
            var permision = await context.Permissions
                .FirstOrDefaultAsync(u => u.Id == id) ?? null;

            return mapper.Map<PermissionResponse>(permision);
        }

        public async Task<PermissionResponse> UpdatePermission(int id, PermissionRequest request)
        {
            var permission = await context.Permissions.FindAsync(id);

            mapper.Map(request, permission);

            await context.SaveChangesAsync();

            return mapper.Map<PermissionResponse>(permission);
        }
    }
}
