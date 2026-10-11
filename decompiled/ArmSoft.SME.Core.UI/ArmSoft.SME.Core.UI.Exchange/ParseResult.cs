using System;
using System.Security;
using System.Security.Principal;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Core.UI.Exchange;

public class ParseResult
{
	public Exception Exception;

	public string EntityXML { get; set; }

	public string EntityName { get; set; }

	public string Caption { get; set; }

	public int LineNumber { get; set; }

	public int LinePosition { get; set; }

	public string ErrorMessage { get; set; } = "";

	public string ExceptionMessageFirstLine
	{
		get
		{
			if (Exception == null)
			{
				return string.Empty;
			}
			return Exception.Message.FirstLine();
		}
	}

	private static string GetMachineName()
	{
		try
		{
			return Environment.MachineName;
		}
		catch (SecurityException)
		{
			return "Permission Denied";
		}
	}

	private static string GetWindowsIdentity()
	{
		try
		{
			return WindowsIdentity.GetCurrent().Name;
		}
		catch (SecurityException)
		{
			return "Permission Denied";
		}
	}
}
