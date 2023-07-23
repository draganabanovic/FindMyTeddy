using FindMyTeddy.API.JwtMenager;
using FindMyTeddy.Data;
using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain.Interfaces;
using FindMyTeddy.Domain.Services;
using FindMyTeddy.Repositories;
using FindMyTeddy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FindMyTeddyContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("FindMyTeddyConnection"))
);

builder.Services.AddIdentity<User,AppRole>().AddEntityFrameworkStores<FindMyTeddyContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(x => {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["AuthSettings:Issuer"],
        ValidAudience = builder.Configuration["AuthSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:JwtSecret"]))
    };
});

builder.Services.AddTransient<IJwtAuthManager, JwtAuthManager>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPetRepository, PetRepository>();
builder.Services.AddTransient<ICharacteristicRepository, CharacteristicRepository>();
builder.Services.AddTransient<IPetLastLocationRepository, PetLastLocationRepository>();


builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPetService, PetService>();
builder.Services.AddTransient<IPetLastLocationService, PetLastLocationService>();
builder.Services.AddTransient<ICharacteristicService, CharacteristicService>();


builder.Services.AddCors(p => p.AddPolicy("CorsPolicy", corsBuilder =>
{
    corsBuilder.WithOrigins(builder.Configuration["ReactAppURL"]).AllowAnyMethod().AllowAnyHeader();
    
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
