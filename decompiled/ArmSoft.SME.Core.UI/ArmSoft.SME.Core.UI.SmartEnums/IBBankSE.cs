using System;
using ArmSoft.InternetBank.APIClient;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.SmartEnums;

[SmartEnumProviderSupport(3, "Banks")]
public class IBBankSE : ASSmartEnum<IBBankSE, string>
{
	public static readonly IBBankSE Ameria = new IBBankSE("Ameria", "157", Bank.Ameria, "ameria.svg", () => Resources.Bank_Ameria, showInList: true, isNewAdded: false, 0);

	public static readonly IBBankSE Amio = new IBBankSE("Amio", "115", Bank.Amio, "amio.svg", () => Resources.Bank_Amio, showInList: true, isNewAdded: false, 1);

	public static readonly IBBankSE Ararat = new IBBankSE("Ararat", "151", Bank.Ararat, "ararat.svg", () => Resources.Bank_AraratBank, showInList: true, isNewAdded: false, 2);

	public static readonly IBBankSE ArmSwiss = new IBBankSE("ArmSwiss", "250", Bank.ArmSwiss, "armswiss.svg", () => Resources.Bank_ArmSwissBank, showInList: true, isNewAdded: false, 3);

	public static readonly IBBankSE Byblos = new IBBankSE("Byblos", "214", Bank.Byblos, "byblos.svg", () => Resources.Bank_Byblos, showInList: true, isNewAdded: false, 4);

	public static readonly IBBankSE Evoca = new IBBankSE("Evoca", "166", Bank.Evoca, "evoca.svg", () => Resources.Bank_Evoca, showInList: true, isNewAdded: false, 5);

	public static readonly IBBankSE Converse = new IBBankSE("Converse", "193", Bank.Converse, "converse.svg", () => Resources.Bank_Converse, showInList: true, isNewAdded: false, 6);

	public static readonly IBBankSE AEB = new IBBankSE("AEB", "163", (Bank)21, "aeb.svg", () => Resources.Bank_ArmEconomBank, showInList: true, isNewAdded: true, 9);

	public static readonly IBBankSE ArmSoftDemo777 = new IBBankSE("ArmSoftDemo777", "777", Bank.Demo, "default.svg", () => Resources.Bank_Demo777, showInList: true, isNewAdded: false, 10);

	public static readonly IBBankSE ArmSoftDemo888 = new IBBankSE("ArmSoftDemo888", "888", Bank.Demo, "default.svg", () => Resources.Bank_Demo888, showInList: true, isNewAdded: false, 11);

	public string Icon { get; }

	public Bank IBBankId { get; }

	public bool ShowInList { get; }

	public bool IsNewAdded { get; }

	public short OrderID { get; }

	private IBBankSE(string name, string value, Bank ibBankId, string icon, Func<string> caption, bool showInList, bool isNewAdded, short orderId)
		: base(name, value, caption)
	{
		Icon = icon;
		IBBankId = ibBankId;
		ShowInList = showInList;
		OrderID = orderId;
		IsNewAdded = isNewAdded;
	}
}
