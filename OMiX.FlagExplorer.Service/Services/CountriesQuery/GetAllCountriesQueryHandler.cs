using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using OMiX.FlagExplorer.Service.Infrastructure;
using OMiX.FlagExplorer.Service.Models.OpenApiCountry;
using OMiX.FlagExplorer.Service.Models.ViewModels;
using System.Text.Json;

namespace OMiX.FlagExplorer.Service.Services.CountriesQuery
{
    public class GetAllCountriesQueryHandler(IConfiguration configuration, IHttpClientProvider httpClientProvider, IMapper mapper) : IRequestHandler<GetAllCountriesQuery, List<Country>>
    {
        private readonly IConfiguration configuration = configuration;
        private readonly IHttpClientProvider httpClientProvider = httpClientProvider;
        private readonly IMapper mapper = mapper;

        public async Task<List<Country>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();
            var url = string.Concat(configuration["AppSettings:CountriesUrl"], "all");

            var response = await httpClientProvider.GetAsync(client, url);
            var responseStr = await response.Content.ReadAsStringAsync(cancellationToken);

            var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var restCountries = JsonSerializer.Deserialize<List<OpenApiCountry>>(responseStr, serializerOptions);

            var countries = mapper.Map<List<Country>>(restCountries);
            return [.. countries.OrderBy(x => x.Name)];
        }
    }
}
