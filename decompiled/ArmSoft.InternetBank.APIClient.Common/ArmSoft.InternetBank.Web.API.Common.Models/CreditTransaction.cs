using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CreditTransaction
{
	[JsonConverter(typeof(DateConverter))]
	public DateTime OperationDate { get; set; }

	public decimal Amount { get; set; }

	public string Description { get; set; }

	public string Currency { get; set; }
}
