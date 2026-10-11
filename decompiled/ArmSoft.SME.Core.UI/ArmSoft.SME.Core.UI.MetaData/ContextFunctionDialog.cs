using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.MetaData;

public class ContextFunctionDialog : ContextFunctionDialogLayoutGroup
{
	public class BooleanControl
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public bool Value { get; set; }
	}

	public class StringControl
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public short Length { get; set; }

		public bool IsRequired { get; set; }

		public string Value { get; set; }
	}

	public class DateControl
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public bool UseLongDate { get; set; }

		public bool IsRequired { get; set; }

		public DateOnly? Value { get; set; }
	}

	public class NumericControl
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public short Length { get; set; }

		public short Precision { get; set; }

		public bool IsRequired { get; set; }

		public bool AllowNegativeNumbers { get; set; }

		public bool UseCurrencyFormat { get; set; }

		public decimal Value { get; set; }
	}

	public string Title { get; set; }

	public object this[string name]
	{
		get
		{
			object obj = Controls[name];
			if (obj is BooleanControl booleanControl)
			{
				return booleanControl.Value;
			}
			if (obj is StringControl stringControl)
			{
				return stringControl.Value;
			}
			if (obj is DateControl dateControl)
			{
				return dateControl.Value;
			}
			if (obj is NumericControl numericControl)
			{
				return numericControl.Value;
			}
			throw new InvalidOperationException("Invalid control type");
		}
		set
		{
			object obj = Controls[name];
			if (obj is BooleanControl booleanControl)
			{
				booleanControl.Value = (bool)value;
				return;
			}
			if (obj is StringControl stringControl)
			{
				stringControl.Value = (string)value;
				return;
			}
			if (obj is DateControl dateControl)
			{
				dateControl.Value = (DateOnly?)value;
				return;
			}
			if (obj is NumericControl numericControl)
			{
				numericControl.Value = (decimal)value;
				return;
			}
			throw new InvalidOperationException("Invalid control type");
		}
	}

	internal ContextFunctionDialog()
		: base(new Dictionary<string, object>())
	{
		Title = "";
	}

	public bool Show()
	{
		return new ContextFunctionDialogWindow().Invoke(this);
	}
}
