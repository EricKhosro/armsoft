using System;

namespace ArmSoft.SME.Common.SmartEnums;

public class PartnerStatusInTaxProgramSE : ASSmartEnum<PartnerStatusInTaxProgramSE, string>
{
	public static readonly PartnerStatusInTaxProgramSE NewRegistered = new PartnerStatusInTaxProgramSE("NewRegistered", "00", () => Resources.NewRegistered);

	public static readonly PartnerStatusInTaxProgramSE Active = new PartnerStatusInTaxProgramSE("Active", "01", () => Resources.Active);

	public static readonly PartnerStatusInTaxProgramSE Bankrupt = new PartnerStatusInTaxProgramSE("Bankrupt", "02", () => Resources.Bankrupt);

	public static readonly PartnerStatusInTaxProgramSE Dissolved = new PartnerStatusInTaxProgramSE("Dissolved", "10", () => Resources.Dissolved);

	public static readonly PartnerStatusInTaxProgramSE Died = new PartnerStatusInTaxProgramSE("Died", "12", () => Resources.Died);

	public static readonly PartnerStatusInTaxProgramSE IncorrectlyProvided = new PartnerStatusInTaxProgramSE("IncorrectlyProvided", "13", () => Resources.IncorrectlyProvided);

	public static readonly PartnerStatusInTaxProgramSE DissolvedMerged = new PartnerStatusInTaxProgramSE("DissolvedMerged", "14", () => Resources.DissolvedMerged);

	public static readonly PartnerStatusInTaxProgramSE DissolvedUnited = new PartnerStatusInTaxProgramSE("DissolvedUnited", "15", () => Resources.DissolvedUnited);

	public static readonly PartnerStatusInTaxProgramSE TimedSuspended = new PartnerStatusInTaxProgramSE("TimedSuspended", "18", () => Resources.TimedSuspended);

	public static readonly PartnerStatusInTaxProgramSE DissolvedProcess = new PartnerStatusInTaxProgramSE("DissolvedProcess", "19", () => Resources.DissolvedProcess);

	public static readonly PartnerStatusInTaxProgramSE ExtractedFromAccounting = new PartnerStatusInTaxProgramSE("ExtractedFromAccounting", "20", () => Resources.ExtractedFromAccounting);

	public static readonly PartnerStatusInTaxProgramSE InRiskOfBankruptcy = new PartnerStatusInTaxProgramSE("InRiskOfBankruptcy", "21", () => Resources.InRiskOfBankruptcy);

	protected PartnerStatusInTaxProgramSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
