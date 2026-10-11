using System.Linq;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.Report;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

public class MessageWithHeaderReport : AsReport
{
	private string _lastHeader;

	private int _count;

	private readonly string _caption;

	public override string Name => "OnlineStoreErrors";

	public MessageWithHeaderReport(string caption)
	{
		AllowRefresh = false;
		_lastHeader = "Unknown";
		_count = 0;
		_caption = caption;
	}

	public override string Caption()
	{
		return _caption;
	}

	public void AddError(string header, string msg)
	{
		if (_lastHeader != header)
		{
			ReportPanel.AddString("");
			ReportPanel.AddString(string.Concat(string.Concat(Enumerable.Repeat(" ", 5)), header));
			ReportPanel.AddString("");
			_lastHeader = header;
			_count = 0;
		}
		_count++;
		if (msg.InStr("\r\n") > 0)
		{
			string[] array = msg.Split("\r\n");
			bool flag = true;
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (flag)
				{
					ReportPanel.AddString((_count + ". ").LeftAlign(5) + text);
					flag = false;
				}
				else
				{
					ReportPanel.AddString(string.Concat(string.Concat(Enumerable.Repeat(" ", 5)), text));
				}
			}
		}
		else
		{
			ReportPanel.AddString((_count + ". ").LeftAlign(5) + msg);
		}
	}
}
