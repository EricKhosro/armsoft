using System;

namespace ArmSoft.InternetBank.Web.API.Common;

public class MakePaymentParams
{
	public Guid OrderId { get; set; }

	public int AttachedCardId { get; set; }
}
