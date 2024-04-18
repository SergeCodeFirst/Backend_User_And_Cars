using System;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using backend.Data;
using backend.Models;
using backend.Dtos.Car;
using backend.Services.ServiceResponse;
using backend.Services.AuthService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Linq;


namespace backend.Services.CarService
{
	public class CarService : ICarService
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public CarService(DataContext context, IMapper mapper, IConfiguration configuration)
		{
			_context = context;
			_mapper = mapper;
			_configuration = configuration;
        }

		// GET ALL CARS
		public async Task<ServiceResponse<List<GetCarDto>>> GetAllCars()
		{
			ServiceResponse<List<GetCarDto>> serviceResponse = new ServiceResponse<List<GetCarDto>>();

			var allCars = await _context.cars.Select(c => _mapper.Map<GetCarDto>(c)).ToListAsync();

			serviceResponse.data = allCars;
			serviceResponse.success = true;
			serviceResponse.message = "Here is all your cars";

			return serviceResponse;
		}

		// ADD NEW CAR
		public async Task<ServiceResponse<AddCarDto>> AddCar([FromBody]AddCarDto newCar, string token)
		{
            //decode token to get the logged in UserId, parse it to and integer and update newCar object
            ServiceAuth authService = new ServiceAuth(_configuration);
            int userId = int.Parse(authService.ValidateAndGetIdFromToken(token));
			// update newCar object user login in user id 
            newCar.userId = userId;

			Car carToAdd = _mapper.Map<Car>(newCar);
            //GetUserDto existingUser = _mapper.Map<GetUserDto>(newUser);
            // add ito db
            _context.cars.Add(carToAdd);
			await _context.SaveChangesAsync();

            var ServiceResponse = new ServiceResponse<AddCarDto>();

			ServiceResponse.data = newCar;
			ServiceResponse.success = true;
			ServiceResponse.message = "Car added successfully";
			return ServiceResponse;
		}

        // GET A CAR
        public async Task<ServiceResponse<GetCarDto>> getCarById(int carId)
		{
			ServiceResponse<GetCarDto> serviceResponse = new ServiceResponse<GetCarDto>();
			var carDb = await _context.cars.FirstOrDefaultAsync(c => c.carId == carId);

			if (carDb == null)
			{
				serviceResponse.success = false;
				serviceResponse.message = "Car Not found";

				return serviceResponse;
            }

			var userCar = _mapper.Map<GetCarDto>(carDb);

			serviceResponse.data = userCar;
			serviceResponse.success = true;
			serviceResponse.message = "Here is your Car";
			return serviceResponse;
		}

        // UPDATE CAR
        public async Task<ServiceResponse<GetCarDto>> updateCar([FromBody] UpdateCarDto updatedCar, int carId, string token)
		{
			ServiceResponse<GetCarDto> serviceResponse = new ServiceResponse<GetCarDto>();

			// Get car in db 
			var carDb = await _context.cars.FirstOrDefaultAsync(c => c.carId == carId);
			if (carDb == null)
			{
				serviceResponse.success = false;
				serviceResponse.message = "Car not found";
				return serviceResponse;
			};

            //decode token to get the logged in UserId, parse it to and integer 
            ServiceAuth authService = new ServiceAuth(_configuration);
            int userId = int.Parse(authService.ValidateAndGetIdFromToken(token));

            // check if the car userid is the same as the one that's login
			if (userId != carDb.userId)
			{
                serviceResponse.success = false;
                serviceResponse.message = "Not your Car";
                return serviceResponse;
            }

            carDb.model = updatedCar.model;
			carDb.price = Convert.ToDouble(updatedCar.price); // double? -> double
			carDb.year = updatedCar.year ?? 0; // if updatedCar.year is null it will reolace it with 0 if not it will use updatedCar.year value 
            carDb.updatedAt = DateTime.Now;

			await _context.SaveChangesAsync();

			var finalCarUpdated = _mapper.Map<GetCarDto>(carDb);

			serviceResponse.data = finalCarUpdated;
			serviceResponse.success = true;
			serviceResponse.message = "Car updated Successfully";

            return serviceResponse;
		}

        // DELETE A CAR
        public async Task<ServiceResponse<GetCarDto>> DeleteCar(int carId, string token)
		{
			ServiceResponse<GetCarDto> serviceResponse = new ServiceResponse<GetCarDto>();

			// get existing car
			var carDb = await _context.cars.FirstOrDefaultAsync(c => c.carId == carId);

			// if car does not exist sent success failed
			if (carDb == null)
			{
				serviceResponse.success = false;
				serviceResponse.message = "Car Not Found";
				return serviceResponse;
			}

			// I could make 133 - 143 as a method in the AuthService
			//decode token to get the logged in UserId, parse it to and integer 
            ServiceAuth authService = new ServiceAuth(_configuration);
            int userId = int.Parse(authService.ValidateAndGetIdFromToken(token));

            // check if the car userid is the same as the one that's login
            if (userId != carDb.userId)
            {
                serviceResponse.success = false;
                serviceResponse.message = "Not your Car";
                return serviceResponse;
            }

            // remove car and save db changes
            _context.cars.Remove(carDb);
			await _context.SaveChangesAsync();

			serviceResponse.success = true;
			serviceResponse.message = "Car deleted Successfully";
			return serviceResponse;
		}

    }
}

