namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class ECRCheckInfo<T> : ECRReceiptResponseInfo
{
	public ECRDepartmentInfo<T> ECRDepWithVAT { get; set; } = new ECRDepartmentInfo<T>();

	public ECRDepartmentInfo<T> ECRDepWithOutVAT { get; set; } = new ECRDepartmentInfo<T>();
}
