using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Car
{
	public class UpdateCarDto
	{
        [Required(ErrorMessage = "Model Name is required")]
        [MinLength(2, ErrorMessage = "Model Name must be at least 2 characters")]
        public string? model { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be at least 0")]
        public double? price { get; set; }
        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2024, ErrorMessage = "year must be at least 1900")]
        public int? year { get; set; }
    }
}

