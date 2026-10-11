namespace ArmSoft.SME.Trade.Common.CustomTypes;

public class PartnerContractGroup
{
	public string PartnerCode { get; set; } = string.Empty;

	public string ContractCode { get; set; } = string.Empty;

	public decimal SummaPart { get; set; }

	public PartnerContractGroup(string partnerCode, string contractCode, decimal summaPart = 0m)
	{
		PartnerCode = partnerCode;
		ContractCode = contractCode;
		SummaPart = summaPart;
	}

	public PartnerContractGroup()
	{
	}
}
