using System;
using ArmSoft.OnlineBanking.Common.Attributes;

namespace ArmSoft.OnlineBanking.Common.Enums;

public enum FieldType : byte
{
	NotSet,
	[CustomizationField(Type = typeof(string))]
	String,
	[CustomizationField(Type = typeof(string))]
	Text,
	[CustomizationField(Type = typeof(int?))]
	Integer,
	[CustomizationField(Type = typeof(decimal?))]
	Decimal,
	[CustomizationField(Type = typeof(DateTime?))]
	Date,
	[CustomizationField(Type = typeof(bool?))]
	CheckBox,
	[CustomizationField(Type = typeof(string))]
	Combo,
	[CustomizationField(Type = typeof(string))]
	Accounts,
	[CustomizationField(Type = typeof(string))]
	Cards,
	[CustomizationField(Type = typeof(string))]
	Deposits,
	[CustomizationField(Type = typeof(string))]
	Credits,
	[CustomizationField(Type = typeof(string))]
	Directory,
	[CustomizationField(Type = typeof(string))]
	HTML,
	[CustomizationField(Type = typeof(string))]
	Bonds,
	[CustomizationField(Type = typeof(string))]
	UrlDirectory,
	[CustomizationField(Type = typeof(string))]
	PhoneNumber,
	[CustomizationField(Type = typeof(string))]
	Email,
	[CustomizationField(Type = typeof(string))]
	Link,
	[CustomizationField(Type = typeof(string))]
	AccountNumber,
	[CustomizationField(Type = typeof(string))]
	CardNumber,
	File,
	[CustomizationField(Type = typeof(int?))]
	Vehicles,
	[CustomizationField(Type = typeof(string))]
	ClientDebts,
	[CustomizationField(Type = typeof(string))]
	PurchasedSecurities,
	[CustomizationField(Type = typeof(string))]
	MoneyInjunctions,
	[CustomizationField(Type = typeof(int?))]
	Payees,
	[CustomizationField(Type = typeof(string))]
	Image,
	[CustomizationField(Type = typeof(decimal))]
	Amount
}
