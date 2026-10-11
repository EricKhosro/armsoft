using System.Runtime.InteropServices;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("3C8E1C98-9686-462B-836D-FB9F2D10192E")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class TaxAuthority
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string DisplayName { get; set; }

	public string Type { get; set; }

	public bool IsActive { get; set; }
}
