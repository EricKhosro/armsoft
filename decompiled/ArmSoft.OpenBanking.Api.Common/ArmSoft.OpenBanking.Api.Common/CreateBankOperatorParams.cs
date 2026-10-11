using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace ArmSoft.OpenBanking.Api.Common;

public class CreateBankOperatorParams : IValidatableObject
{
	[Required]
	[MaxLength(200)]
	public required string Username { get; set; }

	[Required]
	[MaxLength(45)]
	public required string Ip { get; set; }

	[Required]
	[MaxLength(3)]
	public required string BankCode { get; set; }

	public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
	{
		if (!IPAddress.TryParse(Ip, out IPAddress _))
		{
			yield return new System.ComponentModel.DataAnnotations.ValidationResult("Ip must be a valid IPv4 or IPv6 address.", new _003C_003Ez__ReadOnlySingleElementList<string>("Ip"));
		}
		if (!Regex.IsMatch(BankCode, "^[0-9]{3}$"))
		{
			yield return new System.ComponentModel.DataAnnotations.ValidationResult("BankCode must consist of 3 digits.", new _003C_003Ez__ReadOnlySingleElementList<string>("BankCode"));
		}
	}
}
