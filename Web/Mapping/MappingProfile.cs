using AutoMapper;
using Data.Entities.Shop;
using Web.Areas.Admin.Models.Shop;

namespace Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Admin Area

                CreateMap<CreateProductViewModel, Product>();
                CreateMap<Product, CreateProductViewModel>();

            #endregion
        }
    }
}
