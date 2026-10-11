using System;

namespace ArmSoft.EMarkReceiptClient.Configuration;

public class EMarkOptions
{
	public string BaseUrl { get; set; } = "https://ecr.e-mark.am";

	public string Token { get; set; }

	public string RefreshToken { get; set; }

	public string CRN { get; set; }

	public Guid Datamatrix_file_id { get; set; }
}
