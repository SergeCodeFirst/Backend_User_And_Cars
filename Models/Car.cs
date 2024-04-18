using System;
using System.ComponentModel.DataAnnotations;


namespace backend.Models
{
	public class Car
	{
		[Key]
		public int carId { get; set; }
		public string? model { get; set; }
        public double price { get; set; }
		public int year { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }

		public int userId { get; set; }
		public User? myUser { get; set; }
	}
}

