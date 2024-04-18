using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Car
{
	public class AddCarDto
	{
        [Required(ErrorMessage ="Model Name is required")]
        [MinLength(2, ErrorMessage = "Model Name must be at least 2 characters")]
        public string? model { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public double? price { get; set; }
        [Required(ErrorMessage ="Year is required")]
        public int? year { get; set; }
        public int userId { get; set; }
    }
}

