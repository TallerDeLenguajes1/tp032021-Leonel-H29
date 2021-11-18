using AutoMapper;
using TP3_HerreraLeonel.Entities;
using TP3_HerreraLeonel.Models;
using TP3_HerreraLeonel.ViewModels;

namespace TP3_HerreraLeonel
{
    public class MapProfile : Profile{
        public MapProfile()
        {
            CreateMap<Cadete, CadeteIndexViewModel>().ReverseMap();
            CreateMap<Cadete, CadeteAltaViewModel>().ReverseMap();
            CreateMap<Cadete, CadeteModificarViewModel>().ReverseMap();
        }
    }
}