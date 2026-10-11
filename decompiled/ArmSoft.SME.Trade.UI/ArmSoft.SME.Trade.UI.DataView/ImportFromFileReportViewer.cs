using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.DataView;

public class ImportFromFileReportViewer : AsReport
{
	public override string Name => "ImportFromFileReportViewer";

	public override string Caption()
	{
		return ArmSoft.SME.Trade.Common.Resources.ImportFromFileErrors;
	}

	public ImportFromFileReportViewer(Dictionary<int, string> repRows)
	{
		AllowRefresh = false;
		ReportPanel.UnmovableHeaderLinesCount = 3;
		AsRepRowCell asRepRowCell = ReportPanel.AddString(ArmSoft.SME.Trade.Common.Resources.ImportFromFileErrors);
		asRepRowCell.Alignment = TextAlignment.Center;
		asRepRowCell.FontWeight = FontWeights.Bold;
		asRepRowCell.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 4;
		ReportPanel.AddString("");
		ReportPanel.AddTableDefinition(Name, GetListOfRepColumns());
		ReportPanel.AddTableColumnsHeader(Name);
		foreach (KeyValuePair<int, string> repRow in repRows)
		{
			AddReportRow(repRow.Key, repRow.Value);
		}
	}

	private List<AsRepColumn> GetListOfRepColumns()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		AsRepColumn item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.EntPatternEditCRowNum, 70.0)
		{
			CaptionAlignment = TextAlignment.Center,
			ValuesStyle = AsRepColStyle.TextStyle,
			DataAlignment = TextAlignment.Center
		};
		list.Add(item);
		item = new AsRepColumn(ArmSoft.SME.Common.Resources.Reason, 600.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Center,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		return list;
	}

	private void AddReportRow(int lineNum, string reason)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell item = new AsRepRowCell
		{
			Value = lineNum
		};
		list.Add(item);
		item = new AsRepRowCell
		{
			Value = reason
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(Name, list);
	}
}
