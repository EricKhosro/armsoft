using System.Runtime.InteropServices;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("DB3551E9-7788-4CBF-B937-B752B8D04309")]
public enum ErrorType : byte
{
	None,
	Error,
	Warning
}
