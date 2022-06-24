using AutoMapper;
using CatalogApi.Commands;
using CatalogApi.Models;

namespace CatalogApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCatalogItemCommand, CatalogItem>();
        }
    }
}
