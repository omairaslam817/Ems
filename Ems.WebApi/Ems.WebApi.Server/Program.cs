using Ems.Application.AutoMapper;
using Ems.Domain.Interfaces;
using Ems.Infrastructure.DbContexts;
using Ems.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
            .AsImplementedInterfaces()
            .WithScopedLifetime());
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Ems.Application.AssemblyReference.Assembly));
builder
    .Services
    .AddControllers()
    .AddApplicationPart(Ems.Presentation.AssemblyReference.Assembly);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(Ems.Infrastructure.AssemblyReference.Assembly.ToString())));

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

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
