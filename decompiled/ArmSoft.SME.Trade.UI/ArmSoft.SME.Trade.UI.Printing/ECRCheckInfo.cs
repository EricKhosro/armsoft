using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class ECRCheckInfo<T> : ECRReceiptResponseInfo
{
	[PrintDataMember("ECRDepWithVAT")]
	public ECRDepartmentInfo<T> ECRDepWithVAT { get; set; }

	[PrintDataMember("ECRDepWithOutVAT")]
	public ECRDepartmentInfo<T> ECRDepWithOutVAT { get; set; }
}
