using AutoMapper;
using ExperimentalApp.Core.DTOs;
using ExperimentalApp.Core.Models;

namespace ExperimentalApp.BusinessLogic.Mappers
{
    /// <summary>
    /// Configuration class for adjusting mapper settings.
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Represents constructor in which adjusts mapper settings
        /// </summary>
        public MapperProfile() 
        {
            CreateMap<Store, StoreResponseDTO>();
            CreateMap<Film, FilmResponseDTO>()
            .ForMember(dest => dest.FilmId, opt => opt.MapFrom(src => src.FilmId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear.GetValueOrDefault())) 
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.Name)) 
            .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length.GetValueOrDefault()))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));
        }
    }
}
