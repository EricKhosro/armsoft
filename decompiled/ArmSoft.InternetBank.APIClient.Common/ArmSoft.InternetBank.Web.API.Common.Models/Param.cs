using System.Runtime.CompilerServices;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Param
{
	public string Key { get; set; }

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
