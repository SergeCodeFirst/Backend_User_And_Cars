using System;
using AutoMapper;
using backend.Data;
using backend.Models;
using backend.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using backend.Services.AuthService;
using Microsoft.EntityFrameworkCore;
using backend.Services.ServiceResponse;

namespace backend.Services.UserService
{
	public class UserService : IUserService
    {
		private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(DataContext context, IMapper mapper, IConfiguration configuration)
		{
			_context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        // ADD USER - REGISTRATION
        public async Task<ServiceResponse<GetUserDto>> addUser([FromBody] AddUserDto newUser)
        {
            var ServiceResponse = new ServiceResponse<GetUserDto>();

            // Check if user Existed
            User user_db = await _context.users.FirstOrDefaultAsync(u => u.email.ToLower() == newUser.email.ToLower());

            // If user already existe send validation message
            if (user_db != null)
            {
                GetUserDto existingUser = _mapper.Map<GetUserDto>(newUser);
                
                ServiceResponse.data = existingUser;
                ServiceResponse.success = false;
                ServiceResponse.message = "User Already existe!";

                return ServiceResponse;
            }

            // Get User info ready for database
            User addUser = _mapper.Map<User>(newUser);

            // Hash pwd: Install the nuget package Microsoft.BCrypt.Next
            addUser.password = BCrypt.Net.BCrypt.HashPassword(addUser.password);

            // Add user to Db
            _context.Add(addUser);
            await _context.SaveChangesAsync();

            // Get the user id an info to store in session
            User? newUserDb = await _context.users.FirstOrDefaultAsync(u => u.email!.ToLower() == addUser.email!.ToLower());
            GetUserDto resUser = _mapper.Map<GetUserDto>(newUserDb);

            // Set my token
            ServiceAuth resAuth = new ServiceAuth(_configuration);
            string token = resAuth.CreateToken(newUserDb!);

            // Adding my token into a cookies
            

            // Update Response
            ServiceResponse.data = resUser;
            ServiceResponse.success = true;
            ServiceResponse.message = "Added User Successfully!";
            ServiceResponse.auth = token;

            return ServiceResponse;
        }

        // LOGIN PROCESS
        public async Task<ServiceResponse<GetUserDto>> LoginProcess(LoginUserDto logUser)
        {
            ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();

            // get User with email - If Null redirect else continue
            User userDbRes = await _context.users.FirstOrDefaultAsync(u => u.email.ToLower() == logUser.email.ToLower());
            // check if email does not existe -> send validation error
            if (userDbRes == null)
            {
                serviceResponse.success = false;
                serviceResponse.message = "Invalid Login Attempt!";
                return serviceResponse;
            }

            // compare added password and user in db password
            bool passwordMatches = BCrypt.Net.BCrypt.Verify(logUser.password, userDbRes.password);
            // If password does not match -> send validation error
            if (!passwordMatches)
            {
                serviceResponse.success = false;
                serviceResponse.message = "Invalid Login Attempt!";
                return serviceResponse;
            }

            // add user in sesions
            ServiceAuth resAuth = new ServiceAuth(_configuration);
            string token = resAuth.CreateToken(userDbRes!);

            GetUserDto userDb = _mapper.Map<GetUserDto>(userDbRes);
            serviceResponse.data = userDb;
            serviceResponse.success = true;
            serviceResponse.message = "Login Successfully!";
            serviceResponse.auth = token;

            return serviceResponse;
        }
        // GET USER WITH ID
        public ServiceResponse<GetUserDto> getUserWithId(string token)
        {
            // Decode cookies and get user
            ServiceAuth resAuth = new ServiceAuth(_configuration);
            string userId = resAuth.ValidateAndGetIdFromToken(token);

            // get user with user id
            User userDb = _context.users.FirstOrDefault(u => u.userId == int.Parse(userId));

            GetUserDto logUser = _mapper.Map<GetUserDto>(userDb);

            var ServiceResponse = new ServiceResponse<GetUserDto>();

            ServiceResponse.data = logUser;
            ServiceResponse.success = true;
            ServiceResponse.message = "Here is Logged user";

            return ServiceResponse;
        }


        public string getAllUsers()
        {
            return "Ok";
        }

    }
}

