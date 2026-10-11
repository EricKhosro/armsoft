using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CE : AccountPayment
{
	public string IdentificationDocumentNumber { get; set; }

	public string DebtorName { get; set; }

	public string PlaintiffName { get; set; }

	public string InquestId { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime InquestDate { get; set; }

	public decimal RequestedAmount { get; set; }

	public decimal ChargeAmount { get; set; }

	public string ReceiverAcc { get; set; }
}
