using System.Text;
using Database.Library.Entity;
using LibraryAPI.Services;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<LibraryContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("LibraryDB")));
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IBooksService, BookService>();
        builder.Services.AddScoped<IBorrowedBooksByUserService, BorrowedBooksByUserService>();
        builder.Services.AddScoped<IAuthorUpdate, AuthorUpdateService>();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]))
                };
            });

        var app = builder.Build();
        app.Map("/", async (context) =>
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("OK");
        }); // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(); //TODO: How to add /swagger into URL
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}