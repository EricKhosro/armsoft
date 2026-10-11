using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
[Guid("61C4D637-FE47-48CE-BBE1-19EFCF503EE2")]
public class AccountTransactionOldModel
{
	[JsonConverter(typeof(DateConverter))]
	public DateTime CreationDate { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime OperationDate { get; set; }

	public decimal DebitAmount { get; set; }

	public decimal DebitAmountAMD { get; set; }

	public decimal CreditAmount { get; set; }

	public decimal CreditAmountAMD { get; set; }

	public string Currency { get; set; }

	public string OperationType { get; set; }

	public string Comment { get; set; }

	public string CorrespondentAccount { get; set; }

	public string CorrespondentName { get; set; }

	public string DocumentNumber { get; set; }

	public string TransactionID { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public Guid? DocumentID { get; set; }

	public string DocumentIDString
	{
		get
		{
			if (DocumentID.HasValue)
			{
				return DocumentID.ToString();
			}
			return string.Empty;
		}
	}
}
