using AutoMapper;
using eShop.Areas.Admin.ViewModels.Category;
using eShop.Database.Entities;

namespace eShop.WebConfigs
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			// Cấu hình các class được phép map
			CreateMap<ProductCategory, AddOrUpdateCategoryVM>().ReverseMap();
		}
		public static MapperConfiguration CategoryConfig = new MapperConfiguration(opt =>
		{
			opt.CreateMap<ProductCategory, AddOrUpdateCategoryVM>();
			opt.CreateMap<ProductCategory, ListItemCategoryVM>();

		});
	}
}
