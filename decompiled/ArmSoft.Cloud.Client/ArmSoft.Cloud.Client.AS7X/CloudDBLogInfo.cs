using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class CloudDBLogInfo
{
	public int LogId { get; set; }

	public int? DBId { get; set; }

	public string DBName { get; set; }

	public string DBCaption { get; set; }

	public int? KeyId { get; set; }

	public string KeyCode { get; set; }

	public string KeyName { get; set; }

	public int? UserId { get; set; }

	public string UserName { get; set; }

	public short Action { get; set; }

	public string Description { get; set; }

	public DateTime Date { get; set; }

	public int ProcessId { get; set; }

	public string Station { get; set; }

	public string IPAddress { get; set; }
}
