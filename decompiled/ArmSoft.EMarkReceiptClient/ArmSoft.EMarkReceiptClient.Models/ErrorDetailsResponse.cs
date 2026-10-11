using System.Collections.Generic;

namespace ArmSoft.EMarkReceiptClient.Models;

public class ErrorDetailsResponse
{
	public class ErrorDetails
	{
		public List<string> invalid_codes { get; set; } = new List<string>();
	}

	public string error_code { get; set; } = string.Empty;

	public string error_message { get; set; }

	public ErrorDetails error_details { get; set; }
}
