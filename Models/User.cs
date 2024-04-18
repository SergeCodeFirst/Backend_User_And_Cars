using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
	public class User
	{
		[Key]
		public int userId { get; set; }
		public string? firstName { get; set; }
		public string? lastName { get; set; }
		public string? email { get; set; }
		public string? password { get; set; }
        public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }


		List<Car>? myCars { get; set; }
	}
}

