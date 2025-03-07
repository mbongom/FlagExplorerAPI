using AutoMapper;
using OMiX.FlagExplorer.Service.Models.OpenApiCountry;
using OMiX.FlagExplorer.Service.Models.ViewModels;

namespace OMiX.FlagExplorer.Service.AutoMapper
{
    public class OpenApiModelToViewModelProfile : Profile
    {
        public OpenApiModelToViewModelProfile()
        {
            CreateMap<OpenApiCountry, Country>()
                .ForMember(x => x.Name, dest => dest.MapFrom(opt => opt.Name.Common))
                .ForMember(x => x.Flag, dest => dest.MapFrom(opt => opt.Flags.Png));
            CreateMap<OpenApiCountry, CountryDetails>()
                .ForMember(x => x.Name, dest => dest.MapFrom(opt => opt.Name.Common))
                .ForMember(x => x.Flag, dest => dest.MapFrom(opt => opt.Flags.Png))
                .ForMember(x => x.Capital, dest => dest.MapFrom(opt => Capitals(opt.Capital)));
        }

        private static string Capitals(List<string> capitals)
        {
            var _capitals = string.Empty;
            capitals.ForEach(capital =>
            {
                if (!string.IsNullOrEmpty(_capitals)) _capitals += ", ";
                _capitals += capital;
            });
            
            return _capitals;
        }
    }
}
