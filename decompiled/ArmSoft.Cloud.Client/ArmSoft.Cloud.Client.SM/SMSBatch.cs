using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.SM;

public class SMSBatch
{
	public string SenderName { get; set; }

	public List<SMSMessage> Messages { get; set; }
}
