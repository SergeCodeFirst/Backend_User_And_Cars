using System;
using backend.Data;


using System.ComponentModel.DataAnnotations;

namespace backend.Models.Validators
{
	public class ExistingEmail : ValidationAttribute
	{
        private readonly DataContext _context;

        public ExistingEmail(DataContext context)
        {
            _context = context;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? userEmail = (string?)value;

            User? user_db = _context.users.FirstOrDefault(u => u.email.ToLower() == userEmail.ToLower());

            if (user_db != null)
            {
                return new ValidationResult("User Already existe!");
            }

            return ValidationResult.Success;


        }
    }
}

