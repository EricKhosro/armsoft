using System;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "DirectoryType")]
[SmartEnumPartial(SmartEnumType.Base)]
public class DirectoryTypeSE : ASSmartEnum<DirectoryTypeSE, short>
{
	[JsonIgnore]
	public readonly Func<string> FullCaptionDelegate;

	public static readonly DirectoryTypeSE Bank = new DirectoryTypeSE("Bank", 231, () => Resources.Bank, () => Resources.Bank);

	public static readonly DirectoryTypeSE Currency = new DirectoryTypeSE("Currency", 232, () => Resources.Curr, () => Resources.Currency);

	public static readonly DirectoryTypeSE RatesOfExchange = new DirectoryTypeSE("RatesOfExchange", 233, () => Resources.ExchangeRate, () => Resources.ExchangeRate);

	public static readonly DirectoryTypeSE CPAClassifier = new DirectoryTypeSE("CPAClassifier", 234, () => Resources.CPAClassification, () => Resources.CPAClassification);

	public static readonly DirectoryTypeSE Database = new DirectoryTypeSE("Database", 235, () => Resources.Database, () => Resources.Database);

	public static readonly DirectoryTypeSE Users = new DirectoryTypeSE("Users", 236, () => Resources.User, () => Resources.User);

	public const short BankConst = 231;

	public const short CurrencyConst = 232;

	public const short RatesOfExchangeConst = 233;

	public const short CPAClassifierConst = 234;

	public const short DatabaseConst = 235;

	public const short UsersConst = 236;

	public string FullCaption => FullCaptionDelegate();

	protected DirectoryTypeSE(string name, short value, Func<string> caption, Func<string> fullCaption = null)
		: base(name, value, caption)
	{
		FullCaptionDelegate = ((fullCaption == null) ? caption : fullCaption);
	}
}
