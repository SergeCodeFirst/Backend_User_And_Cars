using System;
namespace backend.Dtos.User
{
	public class GetUserDto
	{
        public int userId { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}

