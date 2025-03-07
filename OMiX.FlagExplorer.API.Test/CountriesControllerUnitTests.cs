using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OMiX.FlagExplorer.API.Controllers;
using OMiX.FlagExplorer.Service.Models.ViewModels;
using OMiX.FlagExplorer.Service.Services.CountriesQuery;
using OMiX.FlagExplorer.Service.Services.CountryDetailQuery;

namespace OMiX.FlagExplorer.API.Test
{
    public class CountriesControllerUnitTests
    {
        private readonly Mock<IMediator> mediator;
        private readonly CountriesController countriesController;

        public CountriesControllerUnitTests()
        {
            mediator = new Mock<IMediator>();
            countriesController = new CountriesController(mediator.Object);
        }

        [Fact]
        public async Task Get_Countries_ShouldReturn_Countries()
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<GetAllCountriesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetCountries()));

            //Act
            var response = await countriesController.Get();
            var result = response as OkObjectResult;
            var countries = result?.Value as List<Country>;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(countries);
            Assert.NotEmpty(countries);
        }

        [Fact]
        public async Task Get_CountryDetails_ShouldReturn_CountryDetails()
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<GetCountryDetailsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetCountryDetails()));

            //Act
            var response = await countriesController.Get("South Africa");
            var result = response as OkObjectResult;
            var country = result?.Value as CountryDetails;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(country);
            Assert.Equal("Pretoria, Bloemfontein, Cape Town", country.Capital);
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

        private static CountryDetails GetCountryDetails()
        {
            return new CountryDetails
            {
                Name = "South Africa",
                Population = 59308690,
                Capital = "Pretoria, Bloemfontein, Cape Town",
                Flag = "https://flagcdn.com/w320/au.png"
            };
        }
    }
}