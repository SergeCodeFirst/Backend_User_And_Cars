using System;
using backend.Dtos.User;
using backend.Services.ServiceResponse;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services.UserService
{
	public interface IUserService
	{
		public Task<ServiceResponse<GetUserDto>> addUser(AddUserDto newUser);
		public Task<ServiceResponse<GetUserDto>> LoginProcess(LoginUserDto logUser);
        public string getAllUsers();
		public ServiceResponse<GetUserDto> getUserWithId(string token);

    }
}

