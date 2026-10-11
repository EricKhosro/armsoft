using System.Runtime.InteropServices;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("31BA7A63-9AEA-4ACA-8E4D-7ABDDA6481D2")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MPRow
{
	private decimal mAmount;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Receiver { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverAccount { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public ReceiverLegalStatus ReceiverLegalStatus { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal Amount
	{
		[return: MarshalAs(UnmanagedType.Currency)]
		get
		{
			return mAmount;
		}
		[param: MarshalAs(UnmanagedType.Currency)]
		set
		{
			mAmount = value;
		}
	}
}
