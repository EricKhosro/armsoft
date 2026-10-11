using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "InvalidationTypeName")]
public class TaxInvoiceApprovalStateSE : ASSmartEnum<TaxInvoiceApprovalStateSE, string>
{
	public static readonly TaxInvoiceApprovalStateSE Approved = new TaxInvoiceApprovalStateSE("Approved", "1", () => Resources.Confirmed);

	public static readonly TaxInvoiceApprovalStateSE Pending = new TaxInvoiceApprovalStateSE("Pending", "2", () => Resources.EinvStatusForConfirm);

	public const string ApprovedConst = "1";

	public const string PendingConst = "2";

	public TaxInvoiceApprovalStateSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
