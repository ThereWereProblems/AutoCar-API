using AutoCar.Entities;
using AutoCar.Models.ViewModels;
using AutoMapper;

namespace AutoCar.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<User, UserView>().ReverseMap();
            //CreateMap<User, UserDto>().ReverseMap();
            //CreateMap<User, MyAccountDetailsView>().ReverseMap();
            //CreateMap<Product, CreateProductDto>().ReverseMap();
            //CreateMap<Product, ProductDto>().ReverseMap();
            //CreateMap<OrderView, Order>().ReverseMap().ForMember(o => o.Status, s => s.MapFrom(x => x.OrderStatus.Name));
            //CreateMap<OpinionView, Opinion>().ReverseMap().ForMember(o => o.UserName, s => s.MapFrom(x => x.User.FirstName));
            //CreateMap<User, UserDetailsView>();
            //CreateMap<Product, ProductsView>().ReverseMap();
            //CreateMap<Post, PostDto>().ReverseMap();


        }
    }
}
