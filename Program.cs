using FinalProject.Controllers;
using FinalProject.Exceptions;
using FinalProject.Hubs;
using FinalProject.Mappers;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameIdUserIdProvider>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddDbContext<AlgoUniFinalProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(UserMapping));
builder.Services.AddAutoMapper(typeof(PermissionMapping));
builder.Services.AddAutoMapper(typeof(ProjectMapping));
builder.Services.AddAutoMapper(typeof(ProjectTaskMapping));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/hubs/notifications"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
/*builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminPermission", policy => policy.RequireClaim("Permission", "ADMIN"))
    .AddPolicy("RequireManagerPermission", policy => policy.RequireClaim("Permission", "MANAGER"))
    .AddPolicy("RequireEmployeePermission", policy => policy.RequireClaim("Permission", "EMPLOYEE"));*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notifications");

app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerFeature>()?.Error;

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            ElementNotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsJsonAsync(new
        {
            error = exception?.Message
        });
    });
});

app.Run();
