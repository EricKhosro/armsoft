using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.Report;

namespace ArmSoft.SME.Core.UI.DBIntegrity;

public class CheckIntegrityReport : AsReport
{
	public override string Name => "DBCheckIntegrity";

	public override string Caption()
	{
		return Resources.DataIntegrityChecking;
	}

	public CheckIntegrityReport()
	{
		AllowRefresh = false;
	}

	public void AddError(string header, List<string> errors)
	{
		ReportPanel.AddString("");
		ReportPanel.AddString(new string(" "[0], 5) + header);
		ReportPanel.AddString("");
		int num = 0;
		foreach (string error in errors)
		{
			num++;
			if (error.InStr("\r\n") > 0)
			{
				string[] array = error.Split("\r\n");
				bool flag = true;
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (flag)
					{
						ReportPanel.AddString((num + ". ").LeftAlign(5) + text);
						flag = false;
					}
					else
					{
						ReportPanel.AddString(new string(" "[0], 5) + text);
					}
				}
			}
			else
			{
				ReportPanel.AddString((num + ". ").LeftAlign(5) + error);
			}
		}
	}
}
