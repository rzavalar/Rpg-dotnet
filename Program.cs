global using  Microsoft.AspNetCore.Mvc; 
global using dotnet__rpg.Models;
using dotnet__rpg.Services.CharacterService;
using dotnet__rpg.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Also add DB HERE
builder.Services.AddDbContext<DataContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Agregamos automaper OJO 
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Inyectamos esto para la implementacion de servicios e interfaces
builder.Services.AddScoped<ICharacterService,CharacterService>();
builder.Services.AddScoped<IAuthRepository,AuthRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
