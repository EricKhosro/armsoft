using System.Runtime.InteropServices;

namespace ArmSoft.PossoftPayClient;

public class Constants
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct FinancialOperationType
	{
		public const string sale = "sale";

		public const string cash = "cash";

		public const string saleCash = "saleCash";

		public const string @void = "void";

		public const string refund = "refund";

		public const string settlement = "settlement";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct AdministrativeOperationType
	{
		public const string getMerchantList = "getMerchantList";

		public const string getRecord = "getRecord";

		public const string report = "report";

		public const string test = "test";

		public const string ping = "ping";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct ManagingOperationType
	{
		public const string wait = "wait";
	}

	public enum FunctionCodes : long
	{
		Brief = 1L,
		Detailed,
		CopyLastReport
	}

	public const int amdISONumeric = 51;
}
