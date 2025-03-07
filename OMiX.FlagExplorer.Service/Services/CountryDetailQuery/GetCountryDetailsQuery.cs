using MediatR;
using OMiX.FlagExplorer.Service.Models.ViewModels;

namespace OMiX.FlagExplorer.Service.Services.CountryDetailQuery
{
    public class GetCountryDetailsQuery(string name) : IRequest<CountryDetails>
    {
        public string Name { get; set; } = name;
    }
}
