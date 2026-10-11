using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class TaxPropertyCriteria
{
	public string Id { get; set; }

	public TaxPropertyType Type { get; set; }

	public string TransportPlateNo { get; set; }

	public string TransportTechPassportNo { get; set; }

	public string PassportNo { get; set; }

	public string SocCard { get; set; }

	public string TaxCode { get; set; }

	public TaxPropertyCriteria()
	{
		Id = string.Empty;
		Type = TaxPropertyType.NotSet;
		TransportPlateNo = string.Empty;
		TransportTechPassportNo = string.Empty;
		PassportNo = string.Empty;
		SocCard = string.Empty;
		TaxCode = string.Empty;
	}
}
