using System;
using Microsoft.AspNetCore.Mvc;
using backend.Dtos.Car;
using backend.Services.CarService;
using backend.Services.ServiceResponse;
using Microsoft.AspNetCore.JsonPatch;


namespace backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
    public class CarController : ControllerBase
	{
        // CONSTRUCTER
        private readonly ICarService _carService;

		public CarController(ICarService carService)
		{
			_carService = carService;
		}

        // ->->->->->->->
        //   ENDPOINTS
        // ->->->->->->->

        // TEST IF API IS WORKING
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("testapi", Name ="TestApi")]
		public string TestMyApi()
		{
			var res = "Api is working and ready to go!!";
			return Ok(res);
		}

        // GET ALL CARS
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("getallcars", Name ="GetAllCars")]
		public async Task<ActionResult<ServiceResponse<List<GetCarDto>>>> GetAllCars()
		{
			var res = await _carService.GetAllCars();
			return Ok(res);
		}

        // ADD A NEW CAR - ENDPOINT
        [ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("addnewcar")]
		public async Task<ActionResult<ServiceResponse<AddCarDto>>> AddNewCar([FromBody]AddCarDto newCar)
		{
			if (newCar == null)
			{
				return BadRequest("Car data is required");
			}
			// Validate the new car data
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			// get the token so we can get the id
			string token = HttpContext.Request.Cookies["Token"];

			if (token == null)
			{
				return BadRequest("User Must Login");
			}

			////decode token to get the logged in UserId, parse it to and integer and update newCar object
			//ServiceAuth authService = new ServiceAuth(_configuration);
			//int userId = int.Parse(authService.ValidateAndGetIdFromToken(token));

			//newCar.userId = userId;

			var res = await _carService.AddCar(newCar, token);
			return Ok(res);
		}

		// EDIT A CAR 1 -> Get car info to display on the edit form
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpGet("{carId:int}")]
        public async Task<ActionResult<ServiceResponse<GetCarDto>>> getCarById(int carId)
		{
			Console.WriteLine(carId);
			var res = await _carService.getCarById(carId);

			if (!res.success)
			{
				return BadRequest(res);
			}

			return Ok(res);
		}


		// EDIT A CAR 2 -> Update the car info with the new info provoded
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("updatecar/{carId:int}")]
        public async Task<ActionResult<ServiceResponse<GetCarDto>>> EditCarPocess([FromBody] UpdateCarDto updatedCar, int carId)
        {
            // Check for valide info -> Trigger validation errors
            if (!ModelState.IsValid)
            {
                // send validation error
                return BadRequest();
            }

            // Get login user token
            var token = HttpContext.Request.Cookies["Token"];
			if (token == null)
			{
				return BadRequest("User must Login");
			}

            var res = await _carService.updateCar(updatedCar, carId, token);

			if (!res.success)
			{
                // Car Not found
                return BadRequest(res.message);
			}

			return Ok(res);
        }

		// DELETE CAR
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete/{carId:int}")]
		public async Task<ActionResult<ServiceResponse<GetCarDto>>> Delete(int carId)
		{
            // Get login user token
            var token = HttpContext.Request.Cookies["Token"];
            if (token == null)
            {
                return BadRequest("User must Login");
            }

            var res = await _carService.DeleteCar(carId, token);

			if (!res.success)
			{
				return BadRequest(res.message);
			}

			return Ok(res);
		}
    }
}

