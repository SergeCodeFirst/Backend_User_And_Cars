using System;
using backend.Dtos.Car;
using backend.Services.ServiceResponse;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services.CarService
{
	public interface ICarService
	{
		Task<ServiceResponse<List<GetCarDto>>> GetAllCars();
		Task<ServiceResponse<GetCarDto>> getCarById(int carId);
		Task<ServiceResponse<AddCarDto>> AddCar(AddCarDto newCar, string token);
		Task<ServiceResponse<GetCarDto>> updateCar([FromBody] UpdateCarDto updatedCar, int carId, string token);
        Task<ServiceResponse<GetCarDto>> DeleteCar(int carId, string token);

    }
}

