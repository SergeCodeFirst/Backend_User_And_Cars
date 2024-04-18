using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;


namespace backend.Dtos.User
{

    public class AddUserDto
	{
        [Required(ErrorMessage =("First Name is Require"))]
        [MinLength(2, ErrorMessage ="First Name Need to be at least 2 Characters")]
        public string? firstName { get; set; }
        [Required(ErrorMessage = ("Last Name is Required"))]
        [MinLength(2, ErrorMessage = "Last Name Need to be at least 2 Characters")]
        public string? lastName { get; set; }
        [Required(ErrorMessage = ("email is Required"))]
        [EmailAddress(ErrorMessage ="Not a valid Email")]
        public string? email { get; set; }
        [Required(ErrorMessage = ("password is Required"))]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage ="Password Need To be at least 8 Character")]
        public string? password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage ="Password do not match")]
        public string? confirmpassword { get; set; }
    }
}

