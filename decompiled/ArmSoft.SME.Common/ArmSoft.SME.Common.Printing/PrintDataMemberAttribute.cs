using System;

namespace ArmSoft.SME.Common.Printing;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class PrintDataMemberAttribute : Attribute
{
	public string CaptionResourceId { get; }

	public Type ObjectType { get; }

	public DecimalPrecisionType DecimalPrecisionType { get; set; }

	public PrintDataMemberAttribute()
	{
	}

	public PrintDataMemberAttribute(string captionResourceId)
	{
		CaptionResourceId = captionResourceId;
	}

	public PrintDataMemberAttribute(string captionResourceId, DecimalPrecisionType decimalPrecisionType = DecimalPrecisionType.TwoDigits)
	{
		CaptionResourceId = captionResourceId;
		DecimalPrecisionType = decimalPrecisionType;
	}

	public PrintDataMemberAttribute(string captionResourceId, Type objectType)
	{
		CaptionResourceId = captionResourceId;
		ObjectType = objectType;
	}
}
