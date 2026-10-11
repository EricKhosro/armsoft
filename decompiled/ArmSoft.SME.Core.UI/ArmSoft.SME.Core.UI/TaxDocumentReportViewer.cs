using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.ASReport;
using ArmSoft.SME.Core.UI.Report;

namespace ArmSoft.SME.Core.UI;

public class TaxDocumentReportViewer : AsReport
{
	public class ImportReceivedTaxInvoiceRepViewer : AsReport
	{
		private readonly string _repCaption = "";

		public override string Name => "ImportReceivedTaxInvoiceRepViewer";

		public ImportReceivedTaxInvoiceRepViewer(List<string> repHeadersFooters, List<TaxDocumentRepRow> repRows, string reportCaption = "")
		{
			_repCaption = reportCaption;
			AllowRefresh = false;
			ReportPanel.IsSpreadSheetMode = false;
			ReportPanel.UnmovableHeaderLinesCount = 4;
			if (string.IsNullOrWhiteSpace(_repCaption))
			{
				AsRepRowCell asRepRowCell = ReportPanel.AddString(Resources.ReceivedTaxInvImport);
				asRepRowCell.Alignment = TextAlignment.Center;
				asRepRowCell.FontWeight = FontWeights.Bold;
				asRepRowCell.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 4;
			}
			else
			{
				AsRepRowCell asRepRowCell2 = ReportPanel.AddString(_repCaption);
				asRepRowCell2.Alignment = TextAlignment.Center;
				asRepRowCell2.FontWeight = FontWeights.Bold;
				asRepRowCell2.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 4;
			}
			AsRepRowCell asRepRowCell3 = ReportPanel.AddString(repHeadersFooters[0]);
			asRepRowCell3.Alignment = TextAlignment.Center;
			asRepRowCell3.FontWeight = FontWeights.Bold;
			asRepRowCell3.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 1;
			ReportPanel.AddString("");
			ReportPanel.AddTableDefinition(Name, GetListOfRepColumns());
			ReportPanel.AddTableColumnsHeader(Name);
			foreach (TaxDocumentRepRow repRow in repRows)
			{
				AddReportRow(repRow);
			}
			if (Convert.ToInt32(repHeadersFooters[1]) != 0)
			{
				ReportPanel.UnmovableFooterLinesCount++;
				AsRepRowCell asRepRowCell4 = ReportPanel.AddString(string.Format(Resources.NotImportedDocsCount, repHeadersFooters[1]));
				asRepRowCell4.Alignment = TextAlignment.Left;
				asRepRowCell4.FontWeight = FontWeights.Bold;
				asRepRowCell4.FontSize = Convert.ToInt32(Settings.GetFontSize());
			}
			if (Convert.ToInt32(repHeadersFooters[2]) != 0)
			{
				ReportPanel.UnmovableFooterLinesCount++;
				AsRepRowCell asRepRowCell5 = ReportPanel.AddString(string.Format(Resources.ImportedDocsCount, repHeadersFooters[2]));
				asRepRowCell5.Alignment = TextAlignment.Left;
				asRepRowCell5.FontSize = Convert.ToInt32(Settings.GetFontSize());
			}
		}

		public override string Caption()
		{
			if (string.IsNullOrWhiteSpace(_repCaption))
			{
				return Resources.ReceivedTaxInvImport;
			}
			return _repCaption;
		}

		private static List<AsRepColumn> GetListOfRepColumns()
		{
			List<AsRepColumn> list = new List<AsRepColumn>();
			AsRepColumn item = new AsRepColumn(Resources.Imported, 110.0)
			{
				CaptionAlignment = TextAlignment.Center,
				ValuesStyle = AsRepColStyle.TextStyle,
				DataAlignment = TextAlignment.Center
			};
			list.Add(item);
			item = new AsRepColumn(Resources.SerialNum_Reason, 600.0)
			{
				ValuesStyle = AsRepColStyle.TextStyle,
				CaptionAlignment = TextAlignment.Center,
				DataAlignment = TextAlignment.Left
			};
			list.Add(item);
			return list;
		}

		private void AddReportRow(TaxDocumentRepRow row)
		{
			List<AsRepRowCell> list = new List<AsRepRowCell>();
			AsRepRowCell item = new AsRepRowCell
			{
				Value = row.Imported.ToYesNo()
			};
			list.Add(item);
			item = new AsRepRowCell
			{
				Value = row.SeralNum_Reason
			};
			list.Add(item);
			ReportPanel.AddTableSpanRow(Name, list);
		}
	}

	public class AssignInvNumReportViewer : AsReport
	{
		private readonly bool _shortMode;

		public override string Name => "AssignTaxInvNums";

		public override string Caption()
		{
			if (_shortMode)
			{
				return Resources.AssignAccDocTransportationInvNums;
			}
			return Resources.AssignTaxInvNums;
		}

		public AssignInvNumReportViewer(List<string> repHeadersFooters, List<AssignTaxInvNumsRepRow> repRows, bool shortMode)
		{
			_shortMode = shortMode;
			AllowRefresh = false;
			ReportPanel.IsSpreadSheetMode = false;
			ReportPanel.UnmovableHeaderLinesCount = 4;
			AsRepRowCell asRepRowCell = ReportPanel.AddString(_shortMode ? Resources.AssignAccDocTransportationInvNums : Resources.AssignTaxInvNums);
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
			foreach (AssignTaxInvNumsRepRow repRow in repRows)
			{
				AddReportRow(repRow);
			}
			if (Convert.ToInt32(repHeadersFooters[1]) != 0)
			{
				ReportPanel.UnmovableFooterLinesCount++;
				AsRepRowCell asRepRowCell3 = ReportPanel.AddString(string.Format(Resources.AssignedDocCount, repHeadersFooters[1]));
				asRepRowCell3.Alignment = TextAlignment.Left;
				asRepRowCell3.FontSize = Convert.ToInt32(Settings.GetFontSize());
			}
			if (Convert.ToInt32(repHeadersFooters[2]) != 0)
			{
				ReportPanel.UnmovableFooterLinesCount++;
				AsRepRowCell asRepRowCell4 = ReportPanel.AddString(string.Format(Resources.NotAssignedDocCount, repHeadersFooters[2]));
				asRepRowCell4.Alignment = TextAlignment.Left;
				asRepRowCell4.FontWeight = FontWeights.Bold;
				asRepRowCell4.FontSize = Convert.ToInt32(Settings.GetFontSize());
			}
		}

		private List<AsRepColumn> GetListOfRepColumns()
		{
			List<AsRepColumn> list = new List<AsRepColumn>();
			AsRepColumn item = new AsRepColumn(Resources.DateWord, 80.0)
			{
				ValuesStyle = AsRepColStyle.TextStyle,
				CaptionAlignment = TextAlignment.Center,
				DataAlignment = TextAlignment.Center
			};
			list.Add(item);
			if (!_shortMode)
			{
				item = new AsRepColumn(Resources.OptionsTaxCode, 70.0)
				{
					ValuesStyle = AsRepColStyle.TextStyle,
					CaptionAlignment = TextAlignment.Center,
					DataAlignment = TextAlignment.Center
				};
				list.Add(item);
				item = new AsRepColumn(Resources.Summa, 120.0)
				{
					ValuesStyle = AsRepColStyle.TextStyle,
					CaptionAlignment = TextAlignment.Center,
					DataAlignment = TextAlignment.Right
				};
				list.Add(item);
			}
			item = new AsRepColumn(Resources.Assigned, 90.0)
			{
				CaptionAlignment = TextAlignment.Center,
				ValuesStyle = AsRepColStyle.TextStyle,
				DataAlignment = TextAlignment.Center
			};
			list.Add(item);
			item = new AsRepColumn(Resources.SerialNum_NotAssignReason, 310.0)
			{
				ValuesStyle = AsRepColStyle.TextStyle,
				CaptionAlignment = TextAlignment.Center,
				DataAlignment = TextAlignment.Left
			};
			list.Add(item);
			item = new AsRepColumn(Resources.DocumentsNumbers, 255.0)
			{
				CaptionAlignment = TextAlignment.Center,
				ValuesStyle = AsRepColStyle.TextStyle,
				DataAlignment = TextAlignment.Left
			};
			list.Add(item);
			return list;
		}

		private void AddReportRow(AssignTaxInvNumsRepRow row)
		{
			List<AsRepRowCell> list = new List<AsRepRowCell>();
			AsRepRowCell item = new AsRepRowCell
			{
				Value = row.Date.FormatDDMMYY()
			};
			list.Add(item);
			if (!_shortMode)
			{
				item = new AsRepRowCell
				{
					Value = row.TIN
				};
				list.Add(item);
				item = new AsRepRowCell
				{
					Value = row.Sum.RoundAs(2)
				};
				list.Add(item);
			}
			item = new AsRepRowCell
			{
				Value = row.Assigned
			};
			list.Add(item);
			item = new AsRepRowCell
			{
				Value = row.SeralNum_NotAssignReason
			};
			list.Add(item);
			item = new AsRepRowCell
			{
				Value = row.DocNumbers
			};
			list.Add(item);
			ReportPanel.AddTableSpanRow(Name, list);
		}
	}

	private readonly bool _shortMode;

	public override string Name => "AssignTaxInvNums";

	public override string Caption()
	{
		if (_shortMode)
		{
			return Resources.AssignAccDocTransportationInvNums;
		}
		return Resources.AssignTaxInvNums;
	}

	public TaxDocumentReportViewer(List<string> repHeadersFooters, List<AssignTaxInvNumsRepRow> repRows, bool shortMode)
	{
		_shortMode = shortMode;
		AllowRefresh = false;
		ReportPanel.IsSpreadSheetMode = false;
		ReportPanel.UnmovableHeaderLinesCount = 4;
		AsRepRowCell asRepRowCell = ReportPanel.AddString(_shortMode ? Resources.AssignAccDocTransportationInvNums : Resources.AssignTaxInvNums);
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
		foreach (AssignTaxInvNumsRepRow repRow in repRows)
		{
			AddReportRow(repRow);
		}
		if (Convert.ToInt32(repHeadersFooters[1]) != 0)
		{
			ReportPanel.UnmovableFooterLinesCount++;
			AsRepRowCell asRepRowCell3 = ReportPanel.AddString(string.Format(Resources.AssignedDocCount, repHeadersFooters[1]));
			asRepRowCell3.Alignment = TextAlignment.Left;
			asRepRowCell3.FontSize = Convert.ToInt32(Settings.GetFontSize());
		}
		if (Convert.ToInt32(repHeadersFooters[2]) != 0)
		{
			ReportPanel.UnmovableFooterLinesCount++;
			AsRepRowCell asRepRowCell4 = ReportPanel.AddString(string.Format(Resources.NotAssignedDocCount, repHeadersFooters[2]));
			asRepRowCell4.Alignment = TextAlignment.Left;
			asRepRowCell4.FontWeight = FontWeights.Bold;
			asRepRowCell4.FontSize = Convert.ToInt32(Settings.GetFontSize());
		}
	}

	private List<AsRepColumn> GetListOfRepColumns()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		AsRepColumn item = new AsRepColumn(Resources.DateWord, 80.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Center,
			DataAlignment = TextAlignment.Center
		};
		list.Add(item);
		if (!_shortMode)
		{
			item = new AsRepColumn(Resources.OptionsTaxCode, 70.0)
			{
				ValuesStyle = AsRepColStyle.TextStyle,
				CaptionAlignment = TextAlignment.Center,
				DataAlignment = TextAlignment.Center
			};
			list.Add(item);
			item = new AsRepColumn(Resources.Summa, 120.0)
			{
				ValuesStyle = AsRepColStyle.TextStyle,
				CaptionAlignment = TextAlignment.Center,
				DataAlignment = TextAlignment.Right
			};
			list.Add(item);
		}
		item = new AsRepColumn(Resources.Assigned, 90.0)
		{
			CaptionAlignment = TextAlignment.Center,
			ValuesStyle = AsRepColStyle.TextStyle,
			DataAlignment = TextAlignment.Center
		};
		list.Add(item);
		item = new AsRepColumn(Resources.SerialNum_NotAssignReason, 310.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Center,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		item = new AsRepColumn(Resources.DocumentsNumbers, 255.0)
		{
			CaptionAlignment = TextAlignment.Center,
			ValuesStyle = AsRepColStyle.TextStyle,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		return list;
	}

	private void AddReportRow(AssignTaxInvNumsRepRow row)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell item = new AsRepRowCell
		{
			Value = row.Date.FormatDDMMYY()
		};
		list.Add(item);
		if (!_shortMode)
		{
			item = new AsRepRowCell
			{
				Value = row.TIN
			};
			list.Add(item);
			item = new AsRepRowCell
			{
				Value = row.Sum.RoundAs(2)
			};
			list.Add(item);
		}
		item = new AsRepRowCell
		{
			Value = row.Assigned
		};
		list.Add(item);
		item = new AsRepRowCell
		{
			Value = row.SeralNum_NotAssignReason
		};
		list.Add(item);
		item = new AsRepRowCell
		{
			Value = row.DocNumbers
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(Name, list);
	}
}
