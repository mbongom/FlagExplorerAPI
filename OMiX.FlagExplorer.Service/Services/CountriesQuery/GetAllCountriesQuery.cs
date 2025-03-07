using MediatR;
using OMiX.FlagExplorer.Service.Models.ViewModels;

namespace OMiX.FlagExplorer.Service.Services.CountriesQuery
{
    public class GetAllCountriesQuery : IRequest<List<Country>>
    {
    }
}
