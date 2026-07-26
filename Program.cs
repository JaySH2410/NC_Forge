using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Forge.Infrastructure;
using Forge.Infrastructure.Persistence;
using Forge.Infrastructure.Persistence.Seeds;
using Forge.Middleware;
using Forge.Shared.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
////.ConfigureApiBehaviorOptions(options =>
//// {
////     options.SuppressModelStateInvalidFilter = true;
//// })
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {


        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Forge APIs",
            Version = "v1"
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "jwt",
            In = ParameterLocation.Header,
            Description = "enter: bearer {your jwt token}"
        });
        options.AddSecurityRequirement(document => new() { [new OpenApiSecuritySchemeReference("Bearer", document)] = [] });
    }
);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();

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


await DatabaseSeeder.SeedAsync(app.Services);

app.Run();
