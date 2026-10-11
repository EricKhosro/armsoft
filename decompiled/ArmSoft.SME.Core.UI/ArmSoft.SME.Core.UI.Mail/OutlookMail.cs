using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ArmSoft.SME.Common;
using Microsoft.Office.Interop.Outlook;

namespace ArmSoft.SME.Core.UI.Mail;

public class OutlookMail
{
	private static Application StartOutlook()
	{
		try
		{
			ApplicationClass applicationClass = new ApplicationClass();
			((_Application)applicationClass).GetNamespace("MAPI").Logon("", "", Missing.Value, Missing.Value);
			return applicationClass;
		}
		catch
		{
			throw new ASException(Resources.DifferentPrivilegeOutlook);
		}
	}

	public static void Send(string to, string subject, string body, string htmlBody, string attachmentPath, bool display = true)
	{
		MailItem mailItem = null;
		Application application = null;
		try
		{
			application = StartOutlook();
			mailItem = (MailItem)application.CreateItem(OlItemType.olMailItem);
			mailItem.To = to;
			mailItem.Subject = subject;
			mailItem.Body = body;
			mailItem.HTMLBody = htmlBody;
			mailItem.BodyFormat = (string.IsNullOrWhiteSpace(htmlBody) ? OlBodyFormat.olFormatPlain : OlBodyFormat.olFormatHTML);
			if (!string.IsNullOrWhiteSpace(attachmentPath) && File.Exists(attachmentPath))
			{
				mailItem.Attachments.Add(attachmentPath, OlAttachmentType.olByValue, 1, Path.GetFileName(attachmentPath));
			}
			if (display)
			{
				mailItem.Display(Type.Missing);
			}
		}
		finally
		{
			if (mailItem != null)
			{
				Marshal.ReleaseComObject(mailItem);
			}
			if (application != null)
			{
				Marshal.ReleaseComObject(application);
			}
		}
	}
}
