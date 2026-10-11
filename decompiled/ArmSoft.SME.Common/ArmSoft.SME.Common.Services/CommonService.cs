using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common.Services;

public class CommonService
{
	public static string FormatTaxCodeForB2B(string taxCode, bool throwException = true)
	{
		if (string.IsNullOrWhiteSpace(taxCode))
		{
			if (throwException)
			{
				throw new RESTException(string.Format(Resources.FieldIsEmpty, Resources.OptionsTaxCode));
			}
			return string.Empty;
		}
		if (taxCode.Length != 8)
		{
			if (taxCode.Length == 10 || (taxCode.Length == 11 && taxCode.IndexOf('-') == 2))
			{
				taxCode = taxCode.Right(8);
			}
			else
			{
				if (throwException)
				{
					throw new RESTException(string.Format(Resources.FieldLengthMustBeEqualTo, Resources.OptionsTaxCode, (short)8) + ": " + taxCode);
				}
				taxCode = string.Empty;
			}
		}
		if (int.TryParse(taxCode, out var _))
		{
			return taxCode;
		}
		if (throwException)
		{
			throw new RESTException(string.Format(Resources.FieldContainInvalidSymbol, Resources.OptionsTaxCode));
		}
		return string.Empty;
	}
}
