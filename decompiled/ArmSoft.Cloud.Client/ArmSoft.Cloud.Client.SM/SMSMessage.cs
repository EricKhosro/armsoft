using System;
using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.SM;

public class SMSMessage
{
	public Guid ObjectId { get; set; }

	public string Message { get; set; }

	public List<string> PhoneNumbers { get; set; }
}
