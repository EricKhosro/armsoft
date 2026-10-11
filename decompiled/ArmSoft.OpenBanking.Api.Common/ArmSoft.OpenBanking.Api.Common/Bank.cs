using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace ArmSoft.OpenBanking.Api.Common;

public class Bank
{
	[Required(ErrorMessage = "Bank ID is required.")]
	[StringLength(3, MinimumLength = 3, ErrorMessage = "Bank ID must contain exactly 3 characters.")]
	[RegularExpression("^\\d{3}$", ErrorMessage = "Bank ID must contain exactly 3 digits.")]
	public required string Id { get; set; }

	[MaxLength(1024, ErrorMessage = "Armenian name cannot exceed 1024 characters.")]
	public string? ArmName { get; set; }

	[Required(ErrorMessage = "English name is required.")]
	[MaxLength(1024, ErrorMessage = "English name cannot exceed 1024 characters.")]
	public required string EngName { get; set; }

	[MaxLength(1024, ErrorMessage = "Russian name cannot exceed 1024 characters.")]
	public string? RusName { get; set; }

	[Url(ErrorMessage = "Logo URL must be a valid URL.")]
	public string? LogoUrl { get; set; }

	public BankStatus Status { get; set; }

	[MaxLength(1000, ErrorMessage = "Authentication page URL cannot exceed 1000 characters.")]
	public string? AuthenticationPageUrl { get; set; }

	[MaxLength(1000, ErrorMessage = "Authentication API URL cannot exceed 1000 characters.")]
	public string? AuthenticationApiUrl { get; set; }

	[MaxLength(200, ErrorMessage = "Client ID cannot exceed 200 characters.")]
	public string? ClientId { get; set; }

	public string? Secret { get; set; }

	[Required(ErrorMessage = "Available modules is required.")]
	public required List<string> AvailableModules { get; set; }

	[Required(ErrorMessage = "Service URL is required.")]
	[MaxLength(1000, ErrorMessage = "Service URL cannot exceed 1000 characters.")]
	[Url(ErrorMessage = "Service URL must be a valid URL.")]
	public required string ServiceUrl { get; set; }

	public string NameByCulture
	{
		get
		{
			string name = Thread.CurrentThread.CurrentCulture.Name;
			string text;
			if (name == "hy-AM")
			{
				text = ArmName;
			}
			else
			{
				text = ((!(name == "ru-RU")) ? EngName : RusName);
			}
			string text2 = text;
			if (string.IsNullOrWhiteSpace(text2))
			{
				text2 = EngName;
			}
			return text2;
		}
	}
}
