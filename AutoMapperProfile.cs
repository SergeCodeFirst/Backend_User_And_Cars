using System;
using AutoMapper;
using backend.Dtos.User;
using backend.Dtos.Car;
using backend.Models;

namespace backend
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<AddUserDto, User>();
			CreateMap<User, GetUserDto>();
			CreateMap<AddUserDto, GetUserDto>();
			CreateMap<AddCarDto, Car>();
			CreateMap<Car, GetCarDto>();
		}
	}
}

