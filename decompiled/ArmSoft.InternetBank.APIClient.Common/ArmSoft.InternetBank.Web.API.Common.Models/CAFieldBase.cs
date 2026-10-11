using System.Runtime.CompilerServices;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CAFieldBase
{
	public string Name { get; set; }

	public FieldType Type { get; set; }

	public string Label { get; set; }

	public bool ShowLabel { get; set; }

	public string Tooltip { get; set; }

	public object Value
	{
		get;
		[CompilerGenerated]
		set
		{
			field = RuntimeHelpers.GetObjectValue(value);
		}
	}
}
