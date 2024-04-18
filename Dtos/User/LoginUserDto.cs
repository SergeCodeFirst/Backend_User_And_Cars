using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.User
{
	public class LoginUserDto
	{
        [Required(ErrorMessage = ("email is Required"))]
        [EmailAddress(ErrorMessage = "Not a valid Email")]
        public string? email { get; set; }
        [Required(ErrorMessage = ("password is Required"))]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password Need To be at least 8 Character")]
        public string? password { get; set; }
    }
}

