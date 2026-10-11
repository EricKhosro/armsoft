using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserListItem
{
	public int UserId { get; set; }

	public string Username { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Email { get; set; }

	public string Mobile { get; set; }

	public bool IsApproved { get; set; }

	public bool IsLockedOut { get; set; }

	public string TokenSerial { get; set; }

	public DateTime? UsageStartDate { get; set; }

	public DateTime? UsageEndDate { get; set; }

	public DateTime? LastLoginDate { get; set; }

	public DateTime? LastTransactionDate { get; set; }

	public DateTime? LastActivityDate { get; set; }
}
