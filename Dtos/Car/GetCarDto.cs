using System;
namespace backend.Dtos.Car
{
	public class GetCarDto
	{
        public int carId { get; set; }
        public string? model { get; set; }
        public double price { get; set; }
        public int year { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int userId { get; set; }
    }
}

