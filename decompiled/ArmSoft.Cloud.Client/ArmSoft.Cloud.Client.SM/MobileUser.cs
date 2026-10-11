using System;

namespace ArmSoft.Cloud.Client.SM;

public class MobileUser
{
	public int UserId { get; set; }

	public string Phone { get; set; } = "";

	public string FirstName { get; set; } = "";

	public string LastName { get; set; } = "";

	public string Email { get; set; } = "";

	public DateTime? LastSeen { get; set; }

	public int? TelegramId { get; set; }
}
