using System.Text.RegularExpressions;

namespace ArmSoft.OpenBanking.Api.Common;

public static class BankCodeValidator
{
	private static readonly Regex BankCodePattern = new Regex("^[0-9]{3}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	public static bool TryNormalize(string? bankCode, out string normalizedBankCode)
	{
		normalizedBankCode = (bankCode ?? string.Empty).Trim();
		return BankCodePattern.IsMatch(normalizedBankCode);
	}
}
