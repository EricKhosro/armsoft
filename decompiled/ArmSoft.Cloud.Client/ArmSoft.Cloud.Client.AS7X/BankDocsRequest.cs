using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.AS7X;

public class BankDocsRequest
{
	public string TaxCode { get; set; }

	[JsonConverter(typeof(OnlyDateConverter))]
	public DateTime Date { get; set; }

	public string BankCode { get; set; }

	public short DocType { get; set; }

	public int Count { get; set; }
}
