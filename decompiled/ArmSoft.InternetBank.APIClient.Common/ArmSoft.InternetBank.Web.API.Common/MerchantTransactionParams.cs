using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common;

public class MerchantTransactionParams
{
	[JsonConverter(typeof(DateConverter))]
	public DateTime StartDate { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime EndDate { get; set; }

	public List<string> MerchantIds { get; set; }

	public List<string> TerminalIds { get; set; }

	public string AccountNumber { get; set; }

	public MerchantTransactionParams()
	{
		MerchantIds = null;
		TerminalIds = null;
		AccountNumber = null;
	}
}
