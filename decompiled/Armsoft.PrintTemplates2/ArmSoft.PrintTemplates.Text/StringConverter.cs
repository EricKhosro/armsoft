using System;
using System.Text.RegularExpressions;

namespace ArmSoft.PrintTemplates.Text;

internal sealed class StringConverter
{
	private static readonly Regex RegExpFilter = new Regex("[\\x01-\\x08]|[\\x0B-\\x0C]|[\\x0E-\\x1F]|[\\x7F-\\x84]|[\\x86-\\x9F]");

	private StringConverter()
	{
	}

	public static string RemoveXMLRestrictedCharacters<T>(T source) where T : IConvertible
	{
		if (source != null)
		{
			return RegExpFilter.Replace(Convert.ChangeType(source, typeof(T)).ToString(), "");
		}
		return string.Empty;
	}
}
