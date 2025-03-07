using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using OMiX.FlagExplorer.Service.Infrastructure;
using OMiX.FlagExplorer.Service.Models.OpenApiCountry;
using OMiX.FlagExplorer.Service.Models.ViewModels;
using System.Net;
using System.Text.Json;

namespace OMiX.FlagExplorer.Service.Services.CountryDetailQuery
{
    public class GetCountryDetailsQueryHandler(IConfiguration configuration, IHttpClientProvider httpClientProvider, IMapper mapper) : IRequestHandler<GetCountryDetailsQuery, CountryDetails>
    {
        private readonly IConfiguration configuration = configuration;
        private readonly IHttpClientProvider httpClientProvider = httpClientProvider;
        private readonly IMapper mapper = mapper;

        public async Task<CountryDetails> Handle(GetCountryDetailsQuery request, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();
            var url = string.Concat(configuration["AppSettings:CountriesUrl"], "name/", request.Name);

            var response = await httpClientProvider.GetAsync(client, url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseStr = await response.Content.ReadAsStringAsync(cancellationToken);
                var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var restCountries = JsonSerializer.Deserialize<List<OpenApiCountry>>(responseStr, serializerOptions);

                var countryDetails = mapper.Map<List<CountryDetails>>(restCountries);
                return countryDetails.FirstOrDefault();
            }

            return null;
        }
    }
}
