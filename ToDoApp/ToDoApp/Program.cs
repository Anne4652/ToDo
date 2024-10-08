using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Security.Principal;
using System.Text;
using ToDoApp.DataAccess;
using ToDoApp.DataAccess.Entities;
using ToDoApp.DataAccess.Repositories;
using ToDoApp.Interfaces.DataAccess;
using ToDoApp.Interfaces.Services;
using ToDoApp.Services;
using ToDoApp.Services.Mapping;

var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

try
{
    logger.Debug("Initializing application...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddAutoMapper(typeof(MappingProfile));

    builder.Services.AddControllers();


    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
        };
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoApp", Version = "v1" });
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    });

    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddNLog();
    });

    builder.Services.AddDbContext<ToDoDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<ITaskRepository, TaskRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ITaskService, TaskService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IUserContextService, UserContextService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IAccountService, AccountService>();

    builder.Services.AddIdentityCore<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 4;
    })
        .AddEntityFrameworkStores<ToDoDbContext>()
        .AddSignInManager<SignInManager<IdentityUser>>()
        .AddUserManager<UserManager<IdentityUser>>()
        .AddDefaultTokenProviders();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularApp",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
    });

    var app = builder.Build();

    app.UseCors("AllowAngularApp");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application stopped due to an exception.");
    throw;
}
finally
{
    LogManager.Shutdown();
}