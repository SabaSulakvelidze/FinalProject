using AutoMapper;
using FinalProject.Exceptions;
using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalProject.Services
{
    public class UserServices(
        AlgoUniFinalProjectDbContext context,
        IMapper mapper,
        IConfiguration config,
        IPasswordService passwordService) : IUserServices
    {
        public async Task<UserResponse> CreateUser(CreateUserRequest request)
        {
            if (request == null)
                throw new Exception(nameof(request));

            if (await context.Users.AnyAsync(u => u.UserName == request.Username))
                throw new ConflictException($"User with username '{request.Username}' already exists");

            var user = mapper.Map<User>(request);

            user.Password = passwordService.Hash(request.Password);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return mapper.Map<UserResponse>(user);
        }


        public async Task<List<UserResponse>> GetAllUsers()
        {
            var users = await context.Users
                .Include(u => u.PermissionsForUsers)
                    .ThenInclude(pfu => pfu.Permission)
                .ToListAsync();

            return mapper.Map<List<UserResponse>>(users);
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            var user = await context.Users
                .Include(u => u.PermissionsForUsers)
                    .ThenInclude(pfu => pfu.Permission)
                .FirstOrDefaultAsync(u => u.Id == id) ?? null;

            return mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateUser(int id, UpdateUserRequest request)
        {
            var user = await context.Users.FindAsync(id) 
                ?? throw new ElementNotFoundException($"User with id {id} not found");

            mapper.Map(request, user);

            await context.SaveChangesAsync();

            return mapper.Map<UserResponse>(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await context.Users
                .Include(u => u.PermissionsForUsers)
                .FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception();

            context.PermissionsForUsers.RemoveRange(user.PermissionsForUsers);
            context.Users.Remove(user);

            await context.SaveChangesAsync();
        }

        public async Task<string> SignIn(LogInRequest logInRequest)
        {
            var user = await context.Users
                .Include(u => u.PermissionsForUsers)
                    .ThenInclude(pfu => pfu.Permission)
                .FirstOrDefaultAsync(u => u.UserName == logInRequest.Username)
                ?? throw new Exception($"User with username: {logInRequest.Username} not found");

            if (!passwordService.Verify(logInRequest.Password, user.Password))
                throw new Exception("Password is incorect");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, logInRequest.Username),
                new Claim("UserID",user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, logInRequest.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var permissionNames = user.PermissionsForUsers
                                        .Where(pfu => pfu.Permission != null)
                                        .Select(pfu => pfu.Permission.PermissionName)
                                        .ToList();

            claims.AddRange(permissionNames.Select(p => new Claim("Permission", p)));

            var jwtKey = config["JwtSettings:Key"];
            if (string.IsNullOrEmpty(jwtKey)) throw new Exception("JWT key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: config["JwtSettings:Issuer"],
                    audience: config["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
