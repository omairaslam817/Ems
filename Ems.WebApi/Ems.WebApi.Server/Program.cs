using Ems.Application.Abstractions.Jwt;
using Ems.Application.AutoMapper;
using Ems.Application.Behaviors;
using Ems.Domain.Interfaces;
using Ems.Domain.Repositories;
using Ems.Infrastructure.Authentication;
using Ems.Persistence;
using Ems.Persistence.Interceptors;
using Ems.Persistence.Repositories;
using Ems.Persistence.Services;
using Ems.WebApi.Server.OptionsSetup;
using FluentValidation;
using Gatherly.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Runtime.ConstrainedExecution;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //.AddJwtBearer(o => o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    //{
    //     ValidateIssuer = true,
    //     ValidIssuer = "",

    //});
.AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>(); //handle Jwt authentication in our Api

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(
      "CorsPolicy",
      builder => builder.WithOrigins("https://localhost:4200")
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
});

builder
    .Services
    .Scan(
        selector => selector
            .FromAssemblies(
                Ems.Infrastructure.AssemblyReference.Assembly,
                Ems.Persistence.AssemblyReference.Assembly)
            .AddClasses(false)
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Ems.Application.AssemblyReference.Assembly));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder
    .Services
    .AddControllers()
    .AddApplicationPart(Ems.Presentation.AssemblyReference.Assembly);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddValidatorsFromAssembly(
    Ems.Application.AssemblyReference.Assembly,
    includeInternalTypes: true); 

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(Ems.Persistence.AssemblyReference.Assembly.ToString())));
builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

builder.Services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.ApplyMigration();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
