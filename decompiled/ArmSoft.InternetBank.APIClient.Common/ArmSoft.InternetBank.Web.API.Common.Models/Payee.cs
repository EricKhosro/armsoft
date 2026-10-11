using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Payee
{
	public byte[] Version { get; set; }

	public Guid Id { get; set; }

	public string Name { get; set; }

	public string DisplayPayeeBank { get; set; }

	public string Note { get; set; }

	public PayeeType Type { get; set; }

	public CardTypeGlobal CardTypeGlobal { get; set; }

	public bool IsAvailableForClient { get; set; }

	public string ReceiverId { get; set; }

	public string DisplayReceiverId { get; set; }
}
