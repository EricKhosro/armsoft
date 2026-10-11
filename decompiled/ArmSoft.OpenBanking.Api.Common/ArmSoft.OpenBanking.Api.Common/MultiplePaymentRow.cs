using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class MultiplePaymentRow
{
	public Guid Id { get; set; }

	public Guid DocumentId { get; set; }

	public int Row { get; set; }

	public string Receiver { get; set; } = string.Empty;

	public string ReceiverAcc { get; set; } = string.Empty;

	public ReceiverLegalStatus ReceiverLegalStatus { get; set; }

	public decimal Amount { get; set; }
}
