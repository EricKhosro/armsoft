using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Core.UI;

public class ValidationService
{
	public static void ValidateWrongPeriod(DateTime startDate, DateTime endDate, bool isWordPeriod = true)
	{
		if (endDate < startDate)
		{
			throw new ASException(isWordPeriod ? Resources.WrongDatePeriod : Resources.ValidateWrongPeriod);
		}
	}

	public static void ValidateWrongPeriod(DateTime? startDate, DateTime? endDate, bool isWordPeriod = true)
	{
		if (startDate.HasValue && endDate.HasValue)
		{
			ValidateWrongPeriod(startDate.Value, endDate.Value);
		}
	}

	public static void ValidateWrongPeriod(DateOnly startDate, DateOnly endDate, bool isWordPeriod = true)
	{
		if (endDate < startDate)
		{
			throw new ASException(isWordPeriod ? Resources.WrongDatePeriod : Resources.ValidateWrongPeriod);
		}
	}

	public static void ValidateWrongPeriod(DateOnly? startDate, DateOnly? endDate, bool isWordPeriod = true)
	{
		if (startDate.HasValue && endDate.HasValue)
		{
			ValidateWrongPeriod(startDate.Value, endDate.Value, isWordPeriod);
		}
	}

	public static void OnlyOneNotEmpty(IList<(string, string)> list)
	{
		IEnumerable<(string, string)> source = list.Where(((string, string) p) => !string.IsNullOrEmpty(p.Item2));
		if (source.Count() > 1)
		{
			string arg = string.Join("\", \"", new ReadOnlySpan<string>(source.Select(((string, string) p) => p.Item1).ToArray()));
			throw new ASException(string.Format(Resources.OnlyOneFieldFromListCanBeNotEmpty, arg));
		}
	}

	public static void ValidatePercent(string name, string caption, decimal value, bool isRequired = false)
	{
		ValidateNumber(name, caption, value, 6, 2, isRequired, isNonNegative: true);
		if (value > 100m)
		{
			throw new ASException(Resources.PercentLessHundred, name);
		}
	}

	public static void ValidateNumber(string name, string caption, decimal value, short length, short precision, bool isRequired = false, bool isNonNegative = false)
	{
		if (isRequired)
		{
			NonZeroValidate(name, caption, value);
		}
		if (isNonNegative)
		{
			NonNegativeValidate(name, caption, value);
		}
		if (value - value.RoundAs(precision) != 0m)
		{
			throw new ASException(string.Format(string.Format(Resources.InvalidNumberPrecision, caption, 0, precision), caption), name);
		}
		if (Convert.ToString(Math.Truncate(value)).Length + ((precision > 0) ? (precision + 1) : 0) > length)
		{
			throw new ASException(string.Format(Resources.InvalidNumberLength, caption, 0, length), name);
		}
	}

	protected static void NonNegativeValidate(string name, string caption, decimal value)
	{
		if (value < 0m)
		{
			throw new ASException(string.Format(Resources.ValueIsNegativeWithName, caption), name);
		}
	}

	protected static void NonZeroValidate(string name, string caption, decimal value)
	{
		if (value == 0m)
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, caption), name);
		}
	}

	public static void ValidateString(string name, string caption, string value, int length, bool isRequired = false)
	{
		if (isRequired)
		{
			EmptyStringValidate(name, caption, value);
		}
		if (!string.IsNullOrWhiteSpace(value) && value.Length > length)
		{
			throw new ASException(string.Format(Resources.FieldLengthExceeded, caption, value, length), name);
		}
	}

	protected static void EmptyStringValidate(string name, string caption, string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, caption), name);
		}
	}
}
