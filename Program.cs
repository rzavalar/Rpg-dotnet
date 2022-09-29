global using  Microsoft.AspNetCore.Mvc; 
global using dotnet__rpg.Models;
using dotnet__rpg.Services.CharacterService;
using dotnet__rpg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Also add DB HERE
builder.Services.AddDbContext<DataContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// oauth2
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("oauth2",new OpenApiSecurityScheme{
        Description="Standard authorization header using the bearer shceme\"bearer{token}",
        In = ParameterLocation.Header,
        Name="Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

//Agregamos automaper OJO 
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Inyectamos esto para la implementacion de servicios e interfaces
builder.Services.AddScoped<ICharacterService,CharacterService>();
builder.Services.AddScoped<IAuthRepository,AuthRepository>();

//Se agrego JWT BEARER
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
        .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Agregamos para hhtpaxesor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Agregar para auth
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
