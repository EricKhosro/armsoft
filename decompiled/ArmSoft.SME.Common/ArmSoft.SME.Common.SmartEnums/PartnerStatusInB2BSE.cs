using System;

namespace ArmSoft.SME.Common.SmartEnums;

public class PartnerStatusInB2BSE : ASSmartEnum<PartnerStatusInB2BSE, byte>
{
	public static readonly PartnerStatusInB2BSE Active = new PartnerStatusInB2BSE("Active", 0, () => Resources.Partnerb2bStatusActive);

	public static readonly PartnerStatusInB2BSE NewRegistered = new PartnerStatusInB2BSE("NewRegistered", 1, () => Resources.Partnerb2bStatusNewRegistered);

	public static readonly PartnerStatusInB2BSE TmpInactive = new PartnerStatusInB2BSE("TmpInactive", 2, () => Resources.Partnerb2bStatusTmpInactive);

	public static readonly PartnerStatusInB2BSE Liquidated = new PartnerStatusInB2BSE("Liquidated", 3, () => Resources.Partnerb2bStatusLiquidated);

	public static readonly PartnerStatusInB2BSE Liquidating = new PartnerStatusInB2BSE("Liquidating", 4, () => Resources.Partnerb2bStatusLiquidating);

	public static readonly PartnerStatusInB2BSE LiquidatedAffiliated = new PartnerStatusInB2BSE("LiquidatedAffiliated", 5, () => Resources.Partnerb2bStatusLiquidatedAffiliated);

	public static readonly PartnerStatusInB2BSE LiquidatedMerged = new PartnerStatusInB2BSE("LiquidatedMerged", 6, () => Resources.Partnerb2bStatusLiquidatedMerged);

	public static readonly PartnerStatusInB2BSE Bankrupt = new PartnerStatusInB2BSE("Bankrupt", 7, () => Resources.Partnerb2bStatusBankrupt);

	public static readonly PartnerStatusInB2BSE Dead = new PartnerStatusInB2BSE("Dead", 8, () => Resources.Partnerb2bStatusDead);

	public static readonly PartnerStatusInB2BSE ProvidedByMistake = new PartnerStatusInB2BSE("ProvidedByMistake", 9, () => Resources.Partnerb2bStatusProvidedByMistake);

	public static readonly PartnerStatusInB2BSE ExclusionFromRegister = new PartnerStatusInB2BSE("ExclusionFromRegister", 10, () => Resources.Partnerb2bStatusExclusionFromRegister);

	public static readonly PartnerStatusInB2BSE Reorganized = new PartnerStatusInB2BSE("Reorganized", 11, () => Resources.Partnerb2bStatusReorganized);

	public static readonly PartnerStatusInB2BSE SpinOff = new PartnerStatusInB2BSE("SpinOff", 12, () => Resources.Partnerb2bStatusSpinOff);

	public static readonly PartnerStatusInB2BSE Bankrupting = new PartnerStatusInB2BSE("Bankrupting", 13, () => Resources.Partnerb2bStatusBankrupting);

	public static readonly PartnerStatusInB2BSE WithdrawnFromAccountingHMK = new PartnerStatusInB2BSE("WithdrawnFromAccountingHMK", 14, () => Resources.WithdrawnFromAccountingHMK);

	public static readonly PartnerStatusInB2BSE NotFound = new PartnerStatusInB2BSE("NotFound", 254, () => Resources.Partnerb2bStatusNotFound);

	public static readonly PartnerStatusInB2BSE NotExist = new PartnerStatusInB2BSE("NotExist", byte.MaxValue, () => "");

	protected PartnerStatusInB2BSE(string name, byte value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
