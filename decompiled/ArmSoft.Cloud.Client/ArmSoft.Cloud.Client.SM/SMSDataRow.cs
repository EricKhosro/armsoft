using System;

namespace ArmSoft.Cloud.Client.SM;

public class SMSDataRow
{
	public int SenderId { get; set; }

	public string SenderName { get; set; }

	public DateTime Date { get; set; }

	public Guid ObjectId { get; set; }

	public string Message { get; set; }

	public string PhoneNumber { get; set; }

	public int Id { get; set; }

	public string State { get; set; }

	public string RefId { get; set; }

	public string Description { get; set; }
}
