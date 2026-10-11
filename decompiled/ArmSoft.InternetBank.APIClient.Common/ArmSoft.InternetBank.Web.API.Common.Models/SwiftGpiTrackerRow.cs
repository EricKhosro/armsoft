using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class SwiftGpiTrackerRow
{
	public DateTime Date { get; set; }

	public string Response { get; set; }

	public string Reason { get; set; }

	public string BICOriginator { get; set; }

	public string CountryOriginator { get; set; }

	public string BankOriginator { get; set; }

	public string BICForwardedTo { get; set; }

	public string CountryForwardedTo { get; set; }

	public string BankForwardedTo { get; set; }

	public string ParentMsgType { get; set; }
}
