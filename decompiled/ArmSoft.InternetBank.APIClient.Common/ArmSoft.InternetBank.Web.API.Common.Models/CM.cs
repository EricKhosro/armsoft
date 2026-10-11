using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CM : AccountPayment
{
	public string ReceiverId { get; set; }

	[JsonConverter(typeof(DateTimeConverter))]
	public DateTime? ExpirationDate { get; set; }

	public string WithdrawalCode { get; set; }

	public bool IsSMSSent { get; set; }

	public string ShareContent { get; set; }
}
