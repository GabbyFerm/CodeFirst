using CodeFirst.Database;
using CodeFirst.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ThursdayDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure DbContext to use SQL Server
            builder.Services.AddDbContext<CodeFirstDB>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CodeFirstDbConnection")));

            builder.Services.AddFluentValidationAutoValidation()
                       .AddFluentValidationClientsideAdapters()
                       .AddValidatorsFromAssemblyContaining<UserToUpdateDtoValidator>();

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Handle circular references
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                });

            builder.Services.AddAutoMapper(typeof(Program));

            // Swagger/OpenAPI 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Ensure DB is created and seed data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CodeFirstDB>();
                var seeder = new DatabaseSeeder(context);
                await seeder.SeedAsync(); // Seed the database
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
