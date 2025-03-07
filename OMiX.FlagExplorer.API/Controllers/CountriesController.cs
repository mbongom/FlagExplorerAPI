using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMiX.FlagExplorer.Service.Services.CountriesQuery;
using OMiX.FlagExplorer.Service.Services.CountryDetailQuery;

namespace OMiX.FlagExplorer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet()]
        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var countries = await _mediator.Send(new GetAllCountriesQuery());
            return Ok(countries);
        }

        [HttpGet("{name}")]
        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string name)
        {
            var countryDetails = await _mediator.Send(new GetCountryDetailsQuery(name));
            return Ok(countryDetails);
        }
    }
}
