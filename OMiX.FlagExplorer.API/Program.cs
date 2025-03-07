using AutoMapper;
using MediatR;
using Microsoft.OpenApi.Models;
using OMiX.FlagExplorer.Service.AutoMapper;
using OMiX.FlagExplorer.Service.Infrastructure;
using OMiX.FlagExplorer.Service.Models.ViewModels;
using OMiX.FlagExplorer.Service.Services.CountriesQuery;
using OMiX.FlagExplorer.Service.Services.CountryDetailQuery;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped(typeof(IRequestHandler<GetAllCountriesQuery, List<Country>>), typeof(GetAllCountriesQueryHandler));
builder.Services.AddScoped(typeof(IRequestHandler<GetCountryDetailsQuery, CountryDetails>), typeof(GetCountryDetailsQueryHandler));

var mapperConfig = new MapperConfiguration(config => config.AddProfile(typeof(OpenApiModelToViewModelProfile)));
builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddScoped<IHttpClientProvider, HttpClientProvider>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Flag Explorer App",
        Description = "Coding Challenge: Flag Explorer App by Mbongo Mpongwana",
        Contact = new OpenApiContact
        {
            Name = "Mbongo Mpongwana",
            Email = "mbongo.mpongwana@gmail.com",
        },
        License = new OpenApiLicense
        {
            Name = "OMiX",
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flag Explorer App");
    });
}

app.UseHttpsRedirection();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
