using Microsoft.EntityFrameworkCore;
using WebApi.Configuration;
using WebApi.Data;
using WebApi.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
      app.UseSwagger();
    app.UseSwaggerUI();
  
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
