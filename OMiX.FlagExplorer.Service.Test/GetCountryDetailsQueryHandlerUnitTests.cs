using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using OMiX.FlagExplorer.Service.Infrastructure;
using OMiX.FlagExplorer.Service.Models.OpenApiCountry;
using OMiX.FlagExplorer.Service.Models.ViewModels;
using OMiX.FlagExplorer.Service.Services.CountryDetailQuery;
using System.Net;
using System.Text.Json;

namespace OMiX.FlagExplorer.Service.Test
{
    public class GetCountryDetailsQueryHandlerUnitTests
    {
        private readonly Mock<IConfiguration> configuration;
        private readonly Mock<IHttpClientProvider> httpClientProvider;
        private readonly Mock<IMapper> mapper;

        private readonly GetCountryDetailsQueryHandler handler;

        public GetCountryDetailsQueryHandlerUnitTests()
        {
            configuration = new Mock<IConfiguration>();
            httpClientProvider = new Mock<IHttpClientProvider>();
            mapper = new Mock<IMapper>();

            handler = new GetCountryDetailsQueryHandler(configuration.Object, httpClientProvider.Object, mapper.Object);
        }

        [Fact]
        public async Task Handler_ShouldReturn_CountryDetails()
        {
            //Arrange
            configuration.Setup(x => x["AppSettings:CountriesUrl"]).Returns("https://restcountries.com/v3.1/");
            httpClientProvider.Setup(x => x.GetAsync(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Returns(Task.FromResult(GetHttpResponseMessage()));
            mapper.Setup(x => x.Map<List<CountryDetails>>(It.IsAny<List<OpenApiCountry>>())).Returns(GetCountryDetails());

            //Act
            var country = await handler.Handle(new GetCountryDetailsQuery("South Africa"), CancellationToken.None);

            //Assert
            Assert.NotNull(country);
            Assert.Equal("Pretoria, Bloemfontein, Cape Town", country.Capital);
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
                }
            };

            return countries;
        }

        private static List<CountryDetails> GetCountryDetails()
        {
            var countries = new List<CountryDetails>
            {
                new()
                {
                    Name = "South Africa",
                    Population = 59308690,
                    Capital = "Pretoria, Bloemfontein, Cape Town",
                    Flag = "https://flagcdn.com/w320/au.png"
                }
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
