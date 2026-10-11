using System.Runtime.InteropServices;

namespace ArmSoft.SME.Common.Constants;

public static class SymbolsConstants
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct StringConstants
	{
		public const string ForwardSlashSymbol = "/";

		public const string PlusSymbol = "+";

		public const string MinusSymbol = "-";

		public const string DotSymbol = ".";

		public const string CommaSymbol = ",";

		public const string NumberSymbol = "N";

		public const string SpaceSymbol = " ";

		public const string ColonSymbol = ":";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CharConstants
	{
		public const char CommaSymbol = ',';
	}
}
