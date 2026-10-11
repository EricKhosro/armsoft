using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TypeWord")]
public class TaxDocReceivedOrSubmittedSE : ASSmartEnum<TaxDocReceivedOrSubmittedSE, string>
{
	public static readonly TaxDocReceivedOrSubmittedSE Received = new TaxDocReceivedOrSubmittedSE("Received", "1", () => Resources.EInvoicingReceived);

	public static readonly TaxDocReceivedOrSubmittedSE Submitted = new TaxDocReceivedOrSubmittedSE("Submitted", "2", () => Resources.EInvoicingSubmitted);

	public TaxDocReceivedOrSubmittedSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
