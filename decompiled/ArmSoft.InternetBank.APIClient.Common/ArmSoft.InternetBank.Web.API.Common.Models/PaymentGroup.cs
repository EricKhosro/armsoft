using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PaymentGroup
{
	public int Id { get; set; }

	public LocalizableString Title { get; set; }

	public string IconUrl { get; set; }

	public List<PaymentGroupItem> Items { get; set; }

	public bool IsSubgroup { get; set; }
}
