using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.ASReport;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.SmartEnums;
using DevExpress.Xpf.Editors;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.ReportViewers.Cash_Bank;

public class CashierShiftReportViewer : AsReport
{
	public class CashierShiftReportDialog : DialogWindow
	{
		private DropDownControl _cashDesk;

		private DropDownControl _cashierShiftNumber;

		private CashierShiftReportViewer _reportViewer;

		private DropDownControl _template;

		private CheckEditExt _print;

		private int _reportCount;

		public CashierShiftReportDialog(CashierShiftReportViewer report)
		{
			_reportViewer = report;
			Caption = _reportViewer.Caption();
			Name = _reportViewer.Name;
			HelpId = _reportViewer.HelpId;
			TouchMode = CashierDisplayInfo.GetInstance().TouchMode != POSScreenModesSE.Keyboard.Value;
		}

		internal bool MyShowDialog(bool useParamValues, bool isRefreshMode)
		{
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_cashDesk = AddDropDownControl(container, "CashDesk", ArmSoft.SME.Common.Resources.Cash, new Cashes(), isRequired: true);
			_cashierShiftNumber = AddDropDownControl("CashierShiftNumber", ArmSoft.SME.Trade.Common.Resources.CashierShift, new CashierShift(), isRequired: true);
			_cashDesk.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CashDesk_EditValueChanged).Handler;
			string text = Settings.ParametersService.DefaultCash();
			_cashDesk.Code = (useParamValues ? _reportViewer.CashDesk : text);
			_cashierShiftNumber.Code = (useParamValues ? _reportViewer.ShiftNumber : TradeSettings.CreateTradeClient().CommonServices.GetOpenCashierShiftNumberByCashDesk(text));
			string text2 = string.Empty;
			IReadOnlyList<TemplatesCacheRow> templates = Settings.CoreCacheService.Template.GetTemplates(short.Parse(PrintTemplateDocumentTypeSE.CashierShiftReport));
			_reportCount = templates.Count;
			if (_reportCount == 1)
			{
				text2 = templates[0].Name;
			}
			else if (_reportCount > 1)
			{
				_template = AddDropDownControl("Template", ArmSoft.SME.Common.Resources.PrintTemplate, new Template(short.Parse(PrintTemplateDocumentTypeSE.CashierShiftReport)), isRequired: false, allowAdditionButtons: false, storeValue: true);
			}
			_print = AddBooleanControl("Print", ArmSoft.SME.Common.Resources.Print, storeValue: true);
			if (TouchMode)
			{
				_cancelButton.Height = 40.0;
				_okButton.Height = 40.0;
			}
			WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(CashierShiftReportDialogValidation).Handler;
			if (ShowDialog().Value)
			{
				_reportViewer.CashDesk = _cashDesk.Code.Trim();
				_reportViewer.ShiftNumber = _cashierShiftNumber.Code.Trim();
				CashierShiftCacheRow.RowKey key = new CashierShiftCacheRow.RowKey
				{
					CashDeskCode = _reportViewer.CashDesk,
					ShiftNumber = _reportViewer.ShiftNumber
				};
				CashierShiftCacheRow item = TradeSettings.TradeCacheService.CashierShift.GetItem(key);
				if (_reportCount > 1)
				{
					text2 = _template.Code;
				}
				bool value = _print.IsChecked.Value;
				if (value || !string.IsNullOrWhiteSpace(text2))
				{
					CashierShiftReport printableElement = new CashierShiftReport
					{
						Caption = ArmSoft.SME.Common.Resources.Print,
						CashierShiftRow = item
					};
					if (value)
					{
						Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(printableElement, PrintMode.PrintDirect, text2, CheckPrinterInfo.GetInstance().CheckPrinterName, 1, includePrintInfo: false);
					}
					else
					{
						Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(printableElement, PrintMode.PrintPreview, text2, null, 1, includePrintInfo: false);
					}
					return false;
				}
				return true;
			}
			return false;
		}

		private void CashDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
			{
				if (Convert.ToString(e.OldValue) != _cashDesk.Code)
				{
					_cashierShiftNumber.DataView = new CashierShift(_cashDesk.Code.Trim());
				}
			}
			else
			{
				_cashierShiftNumber.DataView = new CashierShift();
			}
		}

		private void CashierShiftReportDialogValidation(object sender, DialogWindowValidationArgs e)
		{
			if (_print.IsChecked.Value && _reportCount == 0)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.PrintTemplateNotFound);
			}
			if (_print.IsChecked.Value && _reportCount > 1 && string.IsNullOrWhiteSpace(_template.Code))
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.PrintTemplate));
			}
		}
	}

	private class CashierShiftBigRowCell : AsRepRowCell
	{
		public CashierShiftBigRowCell()
		{
			Color backGround = Color.FromRgb(226, 239, 219);
			FontWeight = FontWeights.Bold;
			if (!Settings.IsDarkTheme)
			{
				BackGround = backGround;
			}
			Alignment = TextAlignment.Left;
			FontSize = (int)(Settings.GetFontSize() + 2.0);
		}
	}

	private CashierShiftReportResult _cashierShiftReport;

	public override string Name => "CashierShiftReport";

	public string CashDesk
	{
		get
		{
			return (string)_parameters["CashDesk"].Value;
		}
		set
		{
			_parameters["CashDesk"].Value = value;
		}
	}

	public string ShiftNumber
	{
		get
		{
			return (string)_parameters["ShiftNumber"].Value;
		}
		set
		{
			_parameters["ShiftNumber"].Value = value;
		}
	}

	public CashierShiftReportViewer()
	{
		HelpId = "POS_Reports_ShiftReport";
		DialogSupported = true;
		AddParameter("CashDesk", ArmSoft.SME.Common.Resources.Cash, string.Empty);
		AddParameter("ShiftNumber", ArmSoft.SME.Trade.Common.Resources.CashierShift, string.Empty);
	}

	public override string Caption()
	{
		return ArmSoft.SME.Trade.Common.Resources.CashierShiftReport;
	}

	public override bool ShowDialog(bool useParamValues, bool isRefreshMode)
	{
		return new CashierShiftReportDialog(this).MyShowDialog(useParamValues, isRefreshMode);
	}

	public override bool BeforeShowReport()
	{
		string tableName = "CashierShiftReport";
		ReportPanel.ClearAll();
		List<AsRepColumn> listOfRepColumns = GetListOfRepColumns();
		ReportPanel.AddTableDefinition(tableName, listOfRepColumns);
		AddHeaderLines(tableName);
		CashierShiftCacheRow item = TradeSettings.TradeCacheService.CashierShift.GetItem(new CashierShiftCacheRow.RowKey
		{
			ShiftNumber = ShiftNumber,
			CashDeskCode = CashDesk
		});
		if (item != null)
		{
			List<AsRepRowCell> list = new List<AsRepRowCell>();
			AsRepRowCell asRepRowCell = new AsRepRowCell();
			asRepRowCell.Alignment = TextAlignment.Left;
			asRepRowCell.ColumnSpan = 2;
			asRepRowCell.Value = ArmSoft.SME.Trade.Common.Resources.CashierShiftStart + "  -  " + item.StartDate.FormatDDMMYY() + " " + item.StartDate.ToString("dd/MM/yyyy HH:mm").Right(5);
			AsRepRowCell item2 = asRepRowCell;
			list.Add(item2);
			ReportPanel.AddTableSpanRow(tableName, list);
			list = new List<AsRepRowCell>();
			item2 = new AsRepRowCell
			{
				Alignment = TextAlignment.Left,
				ColumnSpan = 2
			};
			if (item.EndDate.HasValue)
			{
				item2.Value = ArmSoft.SME.Trade.Common.Resources.CashierShiftEnd + "  -  " + item.EndDate.FormatDDMMYY() + " " + item.EndDate.Value.ToString("dd/MM/yyyy HH:mm").Right(5);
			}
			else
			{
				item2.Value = ArmSoft.SME.Trade.Common.Resources.CashierShiftEnd + "  -  ";
			}
			list.Add(item2);
			ReportPanel.AddTableSpanRow(tableName, list);
		}
		List<object> values = new List<object> { " ", " " };
		ReportPanel.AddTableRow(tableName, values);
		_cashierShiftReport = DataProcessingRequestHelper.Execute<CashierShiftReportParam, CashierShiftReportResult>(DPRType.Report, "CashierShiftReport", new CashierShiftReportParam(CashDesk, ShiftNumber));
		AddSaleLines(tableName);
		AddReturnLines(tableName);
		AddCashInLines(tableName);
		AddCashOutLines(tableName);
		AddOtherLines(tableName);
		CashDeskCashFlow(tableName, item);
		AddStatisticLines(tableName);
		return true;
	}

	private void CashDeskCashFlow(string tableName, CashierShiftCacheRow cashierShiftRow)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		CashierShiftBigRowCell item = new CashierShiftBigRowCell
		{
			Alignment = TextAlignment.Center,
			ColumnSpan = 2,
			Value = ArmSoft.SME.Trade.Common.Resources.CashDeskCashFlow
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		list = new List<AsRepRowCell>();
		AsRepRowCell item2 = new AsRepRowCell
		{
			Alignment = TextAlignment.Left,
			Value = ArmSoft.SME.Trade.Common.Resources.InitRem,
			FontWeight = FontWeights.Bold
		};
		list.Add(item2);
		item2 = new AsRepRowCell
		{
			Alignment = TextAlignment.Right,
			ValueStyle = AsRepColStyle.CurrencyStyle,
			FontWeight = FontWeights.Bold,
			Value = cashierShiftRow.InitialRemain
		};
		list.Add(item2);
		ReportPanel.AddTableSpanRow(tableName, list);
		string text = "  ";
		List<object> values = new List<object>
		{
			text + ArmSoft.SME.Trade.Common.Resources.Sale,
			_cashierShiftReport.SalesCashSum
		};
		ReportPanel.AddTableRow(tableName, values);
		int num = 2;
		List<object> list2 = new List<object>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<object> span = CollectionsMarshal.AsSpan(list2);
		span[0] = text + ArmSoft.SME.Trade.Common.Resources.ReturnWord;
		span[1] = -_cashierShiftReport.ReturnsCashSum;
		values = list2;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list3 = new List<object>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<object> span2 = CollectionsMarshal.AsSpan(list3);
		span2[0] = text + ArmSoft.SME.Trade.Common.Resources.PrePayment;
		span2[1] = _cashierShiftReport.CashInCashSumm;
		values = list3;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list4 = new List<object>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<object> span3 = CollectionsMarshal.AsSpan(list4);
		span3[0] = text + ArmSoft.SME.Trade.Common.Resources.PrepaymentReturn;
		span3[1] = -_cashierShiftReport.CashOutCashSumm;
		values = list4;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list5 = new List<object>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<object> span4 = CollectionsMarshal.AsSpan(list5);
		span4[0] = text + ArmSoft.SME.Trade.Common.Resources.AddedBonus;
		span4[1] = _cashierShiftReport.AddedBonus;
		values = list5;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list6 = new List<object>(num);
		CollectionsMarshal.SetCount(list6, num);
		Span<object> span5 = CollectionsMarshal.AsSpan(list6);
		span5[0] = text + ArmSoft.SME.Trade.Common.Resources.OtherCashin;
		span5[1] = _cashierShiftReport.OtherCashin;
		values = list6;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list7 = new List<object>(num);
		CollectionsMarshal.SetCount(list7, num);
		Span<object> span6 = CollectionsMarshal.AsSpan(list7);
		span6[0] = text + ArmSoft.SME.Trade.Common.Resources.OtherCashOut;
		span6[1] = _cashierShiftReport.OtherCashOut;
		values = list7;
		ReportPanel.AddTableRow(tableName, values);
		list = new List<AsRepRowCell>();
		item2 = new AsRepRowCell
		{
			Alignment = TextAlignment.Left,
			FontWeight = FontWeights.Bold,
			Value = ArmSoft.SME.Common.Resources.EndRem
		};
		list.Add(item2);
		item2 = new AsRepRowCell
		{
			Alignment = TextAlignment.Right,
			FontWeight = FontWeights.Bold,
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = cashierShiftRow.InitialRemain + _cashierShiftReport.SalesCashSum + _cashierShiftReport.AddedBonus - _cashierShiftReport.ReturnsCashSum + _cashierShiftReport.CashInTotalSum - _cashierShiftReport.CashOutTotalSum
		};
		list.Add(item2);
		ReportPanel.AddTableSpanRow(tableName, list);
		num = 2;
		List<object> list8 = new List<object>(num);
		CollectionsMarshal.SetCount(list8, num);
		Span<object> span7 = CollectionsMarshal.AsSpan(list8);
		span7[0] = text;
		span7[1] = text;
		values = list8;
		ReportPanel.AddTableRow(tableName, values);
	}

	private void AddStatisticLines(string tableName)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		CashierShiftBigRowCell item = new CashierShiftBigRowCell
		{
			ColumnSpan = 2,
			Alignment = TextAlignment.Center,
			Value = ArmSoft.SME.Trade.Common.Resources.Statistics
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		string text = "  ";
		List<object> values = new List<object>
		{
			text + ArmSoft.SME.Trade.Common.Resources.CountOfBills,
			_cashierShiftReport.CountOfBills
		};
		ReportPanel.AddTableRow(tableName, values);
		int num = 2;
		List<object> list2 = new List<object>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<object> span = CollectionsMarshal.AsSpan(list2);
		span[0] = text + ArmSoft.SME.Trade.Common.Resources.CountOfRows;
		span[1] = _cashierShiftReport.CountOfAllBillRows;
		values = list2;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list3 = new List<object>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<object> span2 = CollectionsMarshal.AsSpan(list3);
		span2[0] = text + ArmSoft.SME.Trade.Common.Resources.AverageSumOfBills;
		span2[1] = _cashierShiftReport.AverageSumOfBills;
		values = list3;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list4 = new List<object>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<object> span3 = CollectionsMarshal.AsSpan(list4);
		span3[0] = text + ArmSoft.SME.Trade.Common.Resources.AverageSumOfRows;
		span3[1] = _cashierShiftReport.AverageSumOfRows;
		values = list4;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list5 = new List<object>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<object> span4 = CollectionsMarshal.AsSpan(list5);
		span4[0] = text + ArmSoft.SME.Trade.Common.Resources.AverageQuantityOfRows;
		span4[1] = _cashierShiftReport.AverageRowQty;
		values = list5;
		ReportPanel.AddTableRow(tableName, values);
	}

	private List<AsRepColumn> GetListOfRepColumns()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		AsRepColumn item = new AsRepColumn(ArmSoft.SME.Common.Resources.Code, 211.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Left,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		item = new AsRepColumn(ArmSoft.SME.Common.Resources.Name, 165.0)
		{
			ValuesStyle = AsRepColStyle.CurrencyStyle,
			CaptionAlignment = TextAlignment.Right,
			DataAlignment = TextAlignment.Right
		};
		list.Add(item);
		return list;
	}

	private void AddHeaderLines(string tableName)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell asRepRowCell = new AsRepRowCell
		{
			ColumnSpan = 2,
			FontWeight = FontWeights.Bold
		};
		if (!Settings.IsDarkTheme)
		{
			asRepRowCell.BackGround = Color.FromRgb(226, 239, 219);
		}
		asRepRowCell.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 4;
		asRepRowCell.Alignment = TextAlignment.Center;
		asRepRowCell.Value = ArmSoft.SME.Trade.Common.Resources.CashierShiftReport;
		list.Add(asRepRowCell);
		ReportPanel.AddTableSpanRow(tableName, list);
		list = new List<AsRepRowCell>();
		asRepRowCell = new AsRepRowCell
		{
			Alignment = TextAlignment.Center,
			FontSize = Convert.ToInt32(Settings.GetFontSize()) + 2,
			ColumnSpan = 2,
			Value = ArmSoft.SME.Common.Resources.Cash + " : " + CashDesk
		};
		list.Add(asRepRowCell);
		ReportPanel.AddTableSpanRow(tableName, list);
		list = new List<AsRepRowCell>();
		asRepRowCell = new AsRepRowCell
		{
			Alignment = TextAlignment.Center,
			FontSize = Convert.ToInt32(Settings.GetFontSize()) + 2,
			ColumnSpan = 2,
			Value = ArmSoft.SME.Trade.Common.Resources.CashierShiftNum + " : " + ShiftNumber
		};
		list.Add(asRepRowCell);
		ReportPanel.AddTableSpanRow(tableName, list);
	}

	private void AddSaleLines(string tableName)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		CashierShiftBigRowCell item = new CashierShiftBigRowCell
		{
			Value = ArmSoft.SME.Trade.Common.Resources.Sale
		};
		list.Add(item);
		item = new CashierShiftBigRowCell
		{
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = _cashierShiftReport.SalesTotalSum
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		string text = "  ";
		List<object> values = new List<object>
		{
			text + ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint,
			_cashierShiftReport.SalesCashSum
		};
		ReportPanel.AddTableRow(tableName, values);
		int num = 2;
		List<object> list2 = new List<object>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<object> span = CollectionsMarshal.AsSpan(list2);
		span[0] = text + ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog;
		span[1] = _cashierShiftReport.SalesPlasticCardSum;
		values = list2;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list3 = new List<object>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<object> span2 = CollectionsMarshal.AsSpan(list3);
		span2[0] = text + ArmSoft.SME.Trade.Common.Resources.WithPrepayment;
		span2[1] = _cashierShiftReport.SalesWithPrepaymentSum;
		values = list3;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list4 = new List<object>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<object> span3 = CollectionsMarshal.AsSpan(list4);
		span3[0] = text + ArmSoft.SME.Trade.Common.Resources.PayByBonus;
		span3[1] = _cashierShiftReport.SalesPayedByBonusSum;
		values = list4;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list5 = new List<object>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<object> span4 = CollectionsMarshal.AsSpan(list5);
		span4[0] = text + ArmSoft.SME.Trade.Common.Resources.CreditSale;
		span4[1] = _cashierShiftReport.SalesCreditSum;
		values = list5;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list6 = new List<object>(num);
		CollectionsMarshal.SetCount(list6, num);
		Span<object> span5 = CollectionsMarshal.AsSpan(list6);
		span5[0] = text;
		span5[1] = text;
		values = list6;
		ReportPanel.AddTableRow(tableName, values);
	}

	private void AddReturnLines(string tableName)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		CashierShiftBigRowCell item = new CashierShiftBigRowCell
		{
			Value = ArmSoft.SME.Trade.Common.Resources.ReturnWord
		};
		list.Add(item);
		item = new CashierShiftBigRowCell
		{
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = _cashierShiftReport.ReturnsTotalSum
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		string text = "  ";
		List<object> values = new List<object>
		{
			text + ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint,
			_cashierShiftReport.ReturnsCashSum
		};
		ReportPanel.AddTableRow(tableName, values);
		int num = 2;
		List<object> list2 = new List<object>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<object> span = CollectionsMarshal.AsSpan(list2);
		span[0] = text + ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog;
		span[1] = _cashierShiftReport.ReturnsPlasticCardSum;
		values = list2;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list3 = new List<object>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<object> span2 = CollectionsMarshal.AsSpan(list3);
		span2[0] = text + ArmSoft.SME.Trade.Common.Resources.WithPrepayment;
		span2[1] = _cashierShiftReport.ReturnWithPrepaymentSum;
		values = list3;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list4 = new List<object>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<object> span3 = CollectionsMarshal.AsSpan(list4);
		span3[0] = text + ArmSoft.SME.Trade.Common.Resources.PayByBonus;
		span3[1] = _cashierShiftReport.ReturnPayedByBonusSum;
		values = list4;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list5 = new List<object>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<object> span4 = CollectionsMarshal.AsSpan(list5);
		span4[0] = text;
		span4[1] = text;
		values = list5;
		ReportPanel.AddTableRow(tableName, values);
	}

	private void AddCashInLines(string tableName)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		CashierShiftBigRowCell item = new CashierShiftBigRowCell
		{
			Value = ArmSoft.SME.Trade.Common.Resources.PrePayment
		};
		list.Add(item);
		item = new CashierShiftBigRowCell
		{
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = ""
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		string text = "  ";
		List<object> values = new List<object>
		{
			text + ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint,
			_cashierShiftReport.CashInCashSumm
		};
		ReportPanel.AddTableRow(tableName, values);
		int num = 2;
		List<object> list2 = new List<object>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<object> span = CollectionsMarshal.AsSpan(list2);
		span[0] = text + ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog;
		span[1] = _cashierShiftReport.CashInByPlastic;
		values = list2;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list3 = new List<object>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<object> span2 = CollectionsMarshal.AsSpan(list3);
		span2[0] = text;
		span2[1] = text;
		values = list3;
		ReportPanel.AddTableRow(tableName, values);
	}

	private void AddCashOutLines(string tableName)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		CashierShiftBigRowCell item = new CashierShiftBigRowCell
		{
			Value = ArmSoft.SME.Trade.Common.Resources.PrepaymentReturn
		};
		list.Add(item);
		item = new CashierShiftBigRowCell
		{
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = ""
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		string text = "  ";
		List<object> values = new List<object>
		{
			text + ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint,
			_cashierShiftReport.CashOutCashSumm
		};
		ReportPanel.AddTableRow(tableName, values);
		int num = 2;
		List<object> list2 = new List<object>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<object> span = CollectionsMarshal.AsSpan(list2);
		span[0] = text;
		span[1] = text;
		values = list2;
		ReportPanel.AddTableRow(tableName, values);
	}

	private void AddOtherLines(string tableName)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		CashierShiftBigRowCell item = new CashierShiftBigRowCell
		{
			Value = ArmSoft.SME.Trade.Common.Resources.DiscountShort
		};
		list.Add(item);
		item = new CashierShiftBigRowCell
		{
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = _cashierShiftReport.DiscountSum
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		list = new List<AsRepRowCell>();
		item = new CashierShiftBigRowCell
		{
			Value = ArmSoft.SME.Trade.Common.Resources.AccumulateBonus
		};
		list.Add(item);
		item = new CashierShiftBigRowCell
		{
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = _cashierShiftReport.AccumulatedBonus
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		list = new List<AsRepRowCell>();
		item = new CashierShiftBigRowCell
		{
			Value = ArmSoft.SME.Trade.Common.Resources.AddedBonus
		};
		list.Add(item);
		item = new CashierShiftBigRowCell
		{
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = _cashierShiftReport.AddedBonus
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		list = new List<AsRepRowCell>();
		item = new CashierShiftBigRowCell
		{
			Value = ArmSoft.SME.Trade.Common.Resources.Cancelled
		};
		list.Add(item);
		item = new CashierShiftBigRowCell
		{
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = _cashierShiftReport.Calcelled
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		list = new List<AsRepRowCell>();
		item = new CashierShiftBigRowCell
		{
			Value = ArmSoft.SME.Trade.Common.Resources.Postponed
		};
		list.Add(item);
		item = new CashierShiftBigRowCell
		{
			ValueStyle = AsRepColStyle.CurrencyStyle,
			Value = _cashierShiftReport.Postponed
		};
		list.Add(item);
		ReportPanel.AddTableSpanRow(tableName, list);
		List<object> values = new List<object> { "", "" };
		ReportPanel.AddTableRow(tableName, values);
	}
}
