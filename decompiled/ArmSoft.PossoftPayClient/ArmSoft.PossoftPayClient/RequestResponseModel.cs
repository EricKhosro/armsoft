namespace ArmSoft.PossoftPayClient;

public class RequestResponseModel
{
	public string operation { get; set; }

	public long amount { get; set; }

	public long amountCashback { get; set; }

	public long currency { get; set; }

	public long CRN { get; set; }

	public long functionCode { get; set; }

	public string rrn { get; set; }

	public string refNum { get; set; }

	public bool result { get; set; }

	public string responseCode { get; set; }

	public string responseText { get; set; }

	public string merchantReceipt { get; set; }

	public string cardholderReceipt { get; set; }

	public string headerImageString { get; set; }

	public long receiptLineLength { get; set; }

	public long timeout { get; set; }

	public string merchantName { get; set; }

	public long merchantId { get; set; }

	public string[] merchantInfo { get; set; }

	public long acquirerId { get; set; }

	public long discount { get; set; }

	public long voidReason { get; set; }
}
