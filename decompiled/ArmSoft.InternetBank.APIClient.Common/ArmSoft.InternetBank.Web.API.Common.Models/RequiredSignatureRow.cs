using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class RequiredSignatureRow
{
	public SignatureTypes SignatureType { get; set; }

	public int CurrentSignaturesCount { get; set; }

	public int RequiredSignaturesCount { get; set; }
}
