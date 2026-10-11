using System;

namespace ArmSoft.SME.Core.UI;

public class ASException : Exception
{
	public string FieldName { get; set; }

	public int RowNum { get; set; } = -1;

	public int Icon { get; set; } = -1;

	public int? ErrorCode { get; set; }

	public string Title { get; set; } = string.Empty;

	public ASException()
	{
	}

	public ASException(string message)
		: base(message)
	{
	}

	public ASException(string message, int icon)
		: base(message)
	{
		Icon = icon;
	}

	public ASException(string message, string fieldName)
		: base(message)
	{
		FieldName = fieldName;
	}

	public ASException(string message, string list, int rowNum)
		: base(message)
	{
		FieldName = list;
		RowNum = rowNum;
	}

	public ASException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
