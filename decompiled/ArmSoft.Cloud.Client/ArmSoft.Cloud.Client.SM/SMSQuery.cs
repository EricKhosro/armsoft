using System;

namespace ArmSoft.Cloud.Client.SM;

public class SMSQuery
{
	public string DateBegin { get; set; }

	public string DateEnd { get; set; }

	public Guid? ObjectId { get; set; }

	public string PhoneNumber { get; set; }

	public string State { get; set; }
}
