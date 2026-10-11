using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.ASReport;

namespace ArmSoft.SME.Trade.UI.ReportViewers;

public class ImportDataFromASAccountantReportViewer : AsReport
{
	public override string Name => "ImportDataFromASAccountantReportViewer";

	public override string Caption()
	{
		return ArmSoft.SME.Trade.Common.Resources.ImportDataFromASAccountant;
	}

	public ImportDataFromASAccountantReportViewer(List<string> repHeadersFooters, List<ImportDataFromASAccountantReportResult> repRows)
	{
		ReportPanel.UnmovableHeaderLinesCount = 4;
		AsRepRowCell asRepRowCell = ReportPanel.AddString(ArmSoft.SME.Trade.Common.Resources.DataImportFromASAccountant);
		asRepRowCell.Alignment = TextAlignment.Center;
		asRepRowCell.FontWeight = FontWeights.Bold;
		asRepRowCell.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 4;
		AsRepRowCell asRepRowCell2 = ReportPanel.AddString(repHeadersFooters[0]);
		asRepRowCell2.Alignment = TextAlignment.Center;
		asRepRowCell2.FontWeight = FontWeights.Bold;
		asRepRowCell2.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 1;
		ReportPanel.AddString("");
		ReportPanel.AddTableDefinition(Name, GetListOfRepColumns());
		ReportPanel.AddTableColumnsHeader(Name);
		foreach (ImportDataFromASAccountantReportResult repRow in repRows)
		{
			AddReportRow(repRow);
		}
		if (Convert.ToInt32(repHeadersFooters[1]) != 0)
		{
			ReportPanel.UnmovableFooterLinesCount++;
			AsRepRowCell asRepRowCell3 = ReportPanel.AddString(string.Format(ArmSoft.SME.Common.Resources.NotImportedItemsCount, repHeadersFooters[1]));
			asRepRowCell3.Alignment = TextAlignment.Left;
			asRepRowCell3.FontWeight = FontWeights.Bold;
			asRepRowCell3.FontSize = Convert.ToInt32(Settings.GetFontSize());
		}
		if (Convert.ToInt32(repHeadersFooters[2]) != 0)
		{
			ReportPanel.UnmovableFooterLinesCount++;
			AsRepRowCell asRepRowCell4 = ReportPanel.AddString(string.Format(ArmSoft.SME.Common.Resources.ImportedItemsCount, repHeadersFooters[2]));
			asRepRowCell4.Alignment = TextAlignment.Left;
			asRepRowCell4.FontSize = Convert.ToInt32(Settings.GetFontSize());
		}
	}

	private static List<AsRepColumn> GetListOfRepColumns()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		AsRepColumn item = new AsRepColumn(ArmSoft.SME.Common.Resources.Imported, 100.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Center,
			DataAlignment = TextAlignment.Center
		};
		list.Add(item);
		item = new AsRepColumn(ArmSoft.SME.Common.Resources.TypeWord, 165.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Center,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		item = new AsRepColumn(ArmSoft.SME.Common.Resources.Code, 150.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Center,
			DataAlignment = TextAlignment.Left
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

	private void AddReportRow(ImportDataFromASAccountantReportResult row)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell item = new AsRepRowCell
		{
			Value = row.Imported
		};
		list.Add(item);
		item = new AsRepRowCell
		{
			Value = row.Type
		};
		list.Add(item);
		item = new AsRepRowCell
		{
			Value = row.DocNumOrCode
		};
		list.Add(item);
		item = new AsRepRowCell
		{
			Value = row.Reason
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(Name, list);
	}
}
