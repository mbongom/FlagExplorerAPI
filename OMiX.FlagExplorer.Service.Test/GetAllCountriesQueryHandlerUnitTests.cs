using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using OMiX.FlagExplorer.Service.Infrastructure;
using OMiX.FlagExplorer.Service.Models.OpenApiCountry;
using OMiX.FlagExplorer.Service.Models.ViewModels;
using OMiX.FlagExplorer.Service.Services.CountriesQuery;
using System.Net;
using System.Text.Json;

namespace OMiX.FlagExplorer.Service.Test
{
    public class GetAllCountriesQueryHandlerUnitTests
    {
        private readonly Mock<IConfiguration> configuration;
        private readonly Mock<IHttpClientProvider> httpClientProvider;
        private readonly Mock<IMapper> mapper;

        private readonly GetAllCountriesQueryHandler handler;

        public GetAllCountriesQueryHandlerUnitTests()
        {
            configuration = new Mock<IConfiguration>();
            httpClientProvider = new Mock<IHttpClientProvider>();
            mapper = new Mock<IMapper>();

            handler = new GetAllCountriesQueryHandler(configuration.Object, httpClientProvider.Object, mapper.Object);
        }

        [Fact]
        public async Task Handler_ShouldReturn_Countries()
        {
            //Arrange
            configuration.Setup(x => x["AppSettings:CountriesUrl"]).Returns("https://restcountries.com/v3.1/");
            httpClientProvider.Setup(x => x.GetAsync(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Returns(Task.FromResult(GetHttpResponseMessage()));
            mapper.Setup(x => x.Map<List<Country>>(It.IsAny<List<OpenApiCountry>>())).Returns(GetCountries());

            //Act
            var countries = await handler.Handle(new GetAllCountriesQuery(), CancellationToken.None);

            //Assert
            Assert.NotNull(countries);
            Assert.NotEmpty(countries);
        }

        private static List<OpenApiCountry> GetOpenApiCountries()
        {
            var countries = new List<OpenApiCountry>
            {
                new()
                {
                    Name = new OpenApiName { Common = "South Africa", Official = "Republic of South Africa" },
                    Flags = new OpenApilags { Png = "https://flagcdn.com/w320/za.png", Svg = "https://flagcdn.com/za.svg" },
                    Population = 59308690,
                    Capital = ["Pretoria", "Bloemfontein", "Cape Town"]
                },
                new()
                {
                    Name = new OpenApiName { Common = "Australia", Official = "Commonwealth of Australia" },
                    Flags = new OpenApilags { Png = "https://flagcdn.com/w320/au.png", Svg = "https://flagcdn.com/au.svg" },
                    Population = 25687041,
                    Capital = ["Canberra"]
                }
            };

            return countries;
        }

        private static List<Country> GetCountries()
        {
            var countries = new List<Country>
            {
                new() { Name = "South Africa", Flag = "https://flagcdn.com/w320/za.png" },
                new() { Name = "Australia", Flag = "https://flagcdn.com/w320/au.png" }
            };

            return countries;
        }

        private static HttpResponseMessage GetHttpResponseMessage()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(GetOpenApiCountries()))
            };
            return response;
        }
    }
}