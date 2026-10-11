using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AttachedCardTransaction
{
	[JsonConverter(typeof(DateConverter))]
	public DateTime DocDate { get; set; }

	public string Type { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public string Details { get; set; }
}
