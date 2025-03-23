using AutoMapper;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.ShippingAddress.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.ShippingAddress.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.ShippingAddress.State))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ShippingAddress.ZipCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.ShippingAddress.Country))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount));
        }
    }
}
