using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.ASReport;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.ReportViewers.Cash_Bank;

public class ECRReportViewer : AsReport
{
	private DropDownControl _cashes;

	public override string Name => "ECRReport";

	public DateOnly StartDate
	{
		get
		{
			return (DateOnly)_parameters["StartDate"].Value;
		}
		set
		{
			_parameters["StartDate"].Value = value;
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return (DateOnly)_parameters["DateEnd"].Value;
		}
		set
		{
			_parameters["DateEnd"].Value = value;
		}
	}

	public string Cash
	{
		get
		{
			return (string)_parameters["Cash"].Value;
		}
		set
		{
			_parameters["Cash"].Value = value;
		}
	}

	private ECRReportResult _repResult { get; set; }

	private ECRReportParam _reportParam { get; set; }

	public ECRReportViewer()
	{
		DialogSupported = true;
		HelpId = "Z_report";
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("DateEnd", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("Cash", ArmSoft.SME.Common.Resources.Cash, string.Empty);
	}

	public override string Caption()
	{
		return ArmSoft.SME.Trade.Common.Resources.ECRCrnReport;
	}

	public override bool ShowDialog(bool UseParamValues, bool IsRefreshMode)
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableCashDesk())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return false;
		}
		DialogWindow dialogWindow = new DialogWindow();
		dialogWindow.Caption = ArmSoft.SME.Trade.Common.Resources.ECRCrnReport;
		dialogWindow.Name = Name;
		dialogWindow.HelpId = HelpId;
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		if (!IsRefreshMode)
		{
			dialogWindow.AddStoreValuesButton(container);
		}
		DateEditControl dateEditControl = dialogWindow.AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodBegin);
		dateEditControl.EditValue = (UseParamValues ? StartDate : Settings.HostingEnvironment.DateBegin);
		DateEditControl dateEditControl2 = dialogWindow.AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodEnd);
		dateEditControl2.EditValue = (UseParamValues ? EndDate : Settings.HostingEnvironment.DateEnd);
		dialogWindow.AddSpecialDateButton(container, dateEditControl, dateEditControl2);
		_cashes = dialogWindow.AddDropDownControl("Cash", ArmSoft.SME.Common.Resources.Cash, new Cashes(), isRequired: true, allowAdditionButtons: false, storeValue: true);
		_cashes.Code = (UseParamValues ? Cash : string.Empty);
		if (dialogWindow.ShowDialog().Value)
		{
			StartDate = dateEditControl.EditValue.Value;
			EndDate = dateEditControl2.EditValue.Value;
			Cash = _cashes.Code;
			return true;
		}
		return false;
	}

	public override void ExecuteDPR()
	{
		_reportParam = new ECRReportParam();
		_reportParam.StartDate = StartDate;
		_reportParam.EndDate = EndDate;
		_reportParam.CashDesk = Cash;
		_repResult = DataProcessingRequestHelper.Execute<ECRReportParam, ECRReportResult>(DPRType.Report, Name, _reportParam);
	}

	public override bool BeforeShowReport()
	{
		decimal Summ = 0m;
		decimal VatSumm = 0m;
		decimal ReturnVATSumm = 0m;
		decimal ReturnSumm = 0m;
		decimal PrePaymentSumm = 0m;
		decimal PrePaymentReturnSumm = 0m;
		ReportPanel.ClearAll();
		string tableName = "ECRReportHeader";
		List<AsRepColumn> listOfHeaderColumns = GetListOfHeaderColumns();
		ReportPanel.AddTableDefinition(tableName, listOfHeaderColumns);
		AddHeaderLines(tableName);
		AddNewLine(tableName);
		tableName = "ECRReport";
		listOfHeaderColumns = GetListOfRepColumns();
		ReportPanel.AddTableDefinition(tableName, listOfHeaderColumns);
		ReportPanel.AddTableColumnsHeader(tableName);
		AddBodyLines(tableName, ref Summ, ref VatSumm, ref ReturnSumm, ref PrePaymentSumm, ref PrePaymentReturnSumm, ref ReturnVATSumm);
		AddNewLine(tableName);
		tableName = "ECRReportFooter";
		listOfHeaderColumns = GetListOfRepFooterColumns();
		ReportPanel.AddTableDefinition(tableName, GetListOfRepFooterColumns());
		AddFooterLines(tableName, Summ, VatSumm, ReturnSumm, PrePaymentSumm, PrePaymentReturnSumm, ReturnVATSumm);
		return true;
	}

	private void AddNewLine(string TableName, string FieldName = " ")
	{
		List<object> list = new List<object>();
		list.Clear();
		list.Add(FieldName);
		ReportPanel.AddTableRow(TableName, list);
	}

	private void AddNewLine(string TableName, string FieldName, object Value)
	{
		List<object> list = new List<object>();
		list.Clear();
		list.Add(FieldName);
		list.Add(Value);
		ReportPanel.AddTableRow(TableName, list);
	}

	private void AddHeaderLines(string TableName)
	{
		CashCacheRow cashCacheRow = null;
		if (!string.IsNullOrWhiteSpace(Cash))
		{
			cashCacheRow = TradeSettings.TradeCacheService.Cash.GetItem(Cash);
		}
		string fieldName = Settings.ParametersService.CompanyName();
		AddNewLine(TableName, fieldName);
		fieldName = Settings.ParametersService.CompanyAddress();
		AddNewLine(TableName, fieldName);
		fieldName = ArmSoft.SME.Common.Resources.OptionsTaxCode + ":" + string.Empty.PadLeft(5) + Settings.ParametersService.TaxCode();
		AddNewLine(TableName, fieldName);
		if (cashCacheRow != null)
		{
			fieldName = ArmSoft.SME.Trade.Common.Resources.ECRCrnShort + string.Empty.PadLeft(5) + cashCacheRow.ECRCrn;
			AddNewLine(TableName, fieldName);
			fieldName = ArmSoft.SME.Trade.Common.Resources.ECRSNShort + string.Empty.PadLeft(5) + cashCacheRow.ECRSN;
			AddNewLine(TableName, fieldName);
		}
		AddNewLine(TableName);
		fieldName = ArmSoft.SME.Trade.Common.Resources.ECRCashier;
		AddNewLine(TableName, fieldName);
		fieldName = ArmSoft.SME.Trade.Common.Resources.ECROpeations + " - " + ArmSoft.SME.Common.Resources.All;
		AddNewLine(TableName, fieldName);
		fieldName = StartDate.ToString() + " - " + EndDate;
		AddNewLine(TableName, fieldName);
	}

	private void AddNewLineGroup(string TableName, string GroupName)
	{
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell asRepRowCell = new AsRepRowCell();
		asRepRowCell.FontWeight = FontWeights.Bold;
		asRepRowCell.FontSize = Convert.ToInt32((int)Settings.GetFontSize()) + 2;
		asRepRowCell.Value = GroupName;
		list.Add(asRepRowCell);
		ReportPanel.AddTableSpanRow(TableName, list);
	}

	private void AddBodyLines(string TableName, ref decimal Summ, ref decimal VatSumm, ref decimal ReturnSumm, ref decimal PrePaymentSumm, ref decimal PrePaymentReturnSumm, ref decimal ReturnVATSumm)
	{
		List<ECRReportResult.ZreportBodyRow> list = new List<ECRReportResult.ZreportBodyRow>();
		foreach (ECRReportResult.SalesRow sale in _repResult.SaleList)
		{
			List<object> list2 = new List<object>();
			ECRReportResult.ZreportBodyRow zreportBodyRow = new ECRReportResult.ZreportBodyRow();
			if (sale.DbCr)
			{
				Summ += sale.SaleSumm;
				VatSumm += sale.VATSaleSumm - sale.SaleSumm;
			}
			else
			{
				ReturnSumm += sale.VATSaleSumm;
				ReturnVATSumm += sale.VATSaleSumm - sale.SaleSumm;
			}
			zreportBodyRow.Summ = sale.VATSaleSumm;
			zreportBodyRow.Date = sale.Date;
			zreportBodyRow.Count = sale.Count;
			zreportBodyRow.IsPrePayment = false;
			zreportBodyRow.SummDescription = zreportBodyRow.Summ.ToString("0.00");
			list.Add(zreportBodyRow);
		}
		PrePaymentSumm = _repResult.ListOfTotalPrepaymentSumm.Sum((ECRReportResult.ZreportBodyRow s) => s.Summ);
		foreach (ECRReportResult.ZreportBodyRow item in _repResult.ListOfTotalPrepaymentSumm)
		{
			list.Add(item);
		}
		PrePaymentReturnSumm = _repResult.ListOfTotalReturnPrepaymentSumm.Sum((ECRReportResult.ZreportBodyRow s) => s.Summ);
		foreach (ECRReportResult.ZreportBodyRow item2 in _repResult.ListOfTotalReturnPrepaymentSumm)
		{
			list.Add(item2);
		}
		foreach (ECRReportResult.ZreportBodyRow item3 in list.OrderBy((ECRReportResult.ZreportBodyRow d) => d.Date))
		{
			List<object> list2 = new List<object>();
			list2.Add(item3.Date.FormatDDMMYY());
			list2.Add(item3.Count);
			list2.Add(item3.SummDescription);
			ReportPanel.AddTableRow(TableName, list2);
		}
	}

	private void AddFooterLines(string TableName, decimal Summ, decimal VatSumm, decimal ReturnSumm, decimal PrePaymentSumm, decimal PrePaymentReturnSumm, decimal TotalReturnVAT)
	{
		string whole = ArmSoft.SME.Trade.Common.Resources.Whole;
		AddNewLineGroup(TableName, whole);
		decimal num = Summ + VatSumm;
		whole = ArmSoft.SME.Trade.Common.Resources.TotalSale;
		object value = num.ToString("N" + (short)2);
		AddNewLine(TableName, whole, value);
		decimal withUsingPrePaymentSumm = _repResult.WithUsingPrePaymentSumm;
		whole = ArmSoft.SME.Trade.Common.Resources.WithUsingPrePaymentSumm;
		value = withUsingPrePaymentSumm.ToString("N" + (short)2);
		AddNewLine(TableName, whole, value);
		whole = ArmSoft.SME.Trade.Common.Resources.TotalSaleVAT;
		value = VatSumm.ToString("N" + (short)2);
		AddNewLine(TableName, whole, value);
		whole = ArmSoft.SME.Trade.Common.Resources.TotalReturn;
		value = ReturnSumm.ToString("N" + (short)2);
		AddNewLine(TableName, whole, value);
		whole = ArmSoft.SME.Trade.Common.Resources.TotalReturnVAT;
		value = TotalReturnVAT.ToString("N" + (short)2);
		AddNewLine(TableName, whole, value);
		whole = ArmSoft.SME.Trade.Common.Resources.PrePayment;
		value = PrePaymentSumm.ToString("N" + (short)2);
		AddNewLine(TableName, whole, value);
		whole = ArmSoft.SME.Trade.Common.Resources.PrePaymentReturnShort;
		value = PrePaymentReturnSumm.ToString("N" + (short)2);
		AddNewLine(TableName, whole, value);
		decimal num2 = num - withUsingPrePaymentSumm + ReturnSumm + PrePaymentSumm + PrePaymentReturnSumm;
		whole = ArmSoft.SME.Trade.Common.Resources.FiscalIncome;
		value = num2.ToString("N" + (short)2);
		AddNewLine(TableName, whole, value);
		decimal num3 = VatSumm + TotalReturnVAT;
		whole = ArmSoft.SME.Trade.Common.Resources.FiscalIncomeVAT;
		value = num3.ToString("N" + (short)2);
		AddNewLine(TableName, whole, value);
	}

	private List<AsRepColumn> GetListOfHeaderColumns()
	{
		return new List<AsRepColumn>
		{
			new AsRepColumn(230.0)
			{
				ValuesStyle = AsRepColStyle.TextStyle,
				CaptionAlignment = TextAlignment.Left,
				DataAlignment = TextAlignment.Left,
				Caption = "1"
			}
		};
	}

	private List<AsRepColumn> GetListOfRepColumns()
	{
		return new List<AsRepColumn>
		{
			new AsRepColumn(70.0)
			{
				ValuesStyle = AsRepColStyle.TextStyle,
				CaptionAlignment = TextAlignment.Left,
				DataAlignment = TextAlignment.Left,
				Caption = ArmSoft.SME.Common.Resources.DateWord
			},
			new AsRepColumn(55.0)
			{
				ValuesStyle = AsRepColStyle.NumericStyle,
				CaptionAlignment = TextAlignment.Left,
				DataAlignment = TextAlignment.Right,
				Caption = ArmSoft.SME.Common.Resources.Quantity
			},
			new AsRepColumn(105.0)
			{
				ValuesStyle = AsRepColStyle.TextStyle,
				CaptionAlignment = TextAlignment.Right,
				DataAlignment = TextAlignment.Right,
				Caption = ArmSoft.SME.Common.Resources.Summa
			}
		};
	}

	private List<AsRepColumn> GetListOfRepFooterColumns()
	{
		return new List<AsRepColumn>
		{
			new AsRepColumn(143.0)
			{
				ValuesStyle = AsRepColStyle.TextStyle,
				CaptionAlignment = TextAlignment.Left,
				DataAlignment = TextAlignment.Left,
				Caption = "1"
			},
			new AsRepColumn(87.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Left,
				DataAlignment = TextAlignment.Left,
				Caption = "2"
			}
		};
	}
}
