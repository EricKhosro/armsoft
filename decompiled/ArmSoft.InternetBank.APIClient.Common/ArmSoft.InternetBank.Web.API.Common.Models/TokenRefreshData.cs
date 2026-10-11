using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class TokenRefreshData
{
	public string Serial { get; set; }

	public string ManualEntryKey { get; set; }

	public Guid AutologinCode { get; set; }

	public Guid AuthenticatorKey1 { get; set; }

	public Guid AuthenticatorKey2 { get; set; }
}
