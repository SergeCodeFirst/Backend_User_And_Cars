using System;
namespace backend.Services.ServiceResponse
{
	public class ServiceResponse<T>
	{
		public T? data { get; set; }
		public bool success { get; set; } = true;
		public string? message { get; set; } = String.Empty;
		public string? auth { get; set; } = String.Empty;
    }
}

