using System;
using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.Data.Reports;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.ReportViewers.Cash_Bank;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.DataView;

public class CashBankRemainsBase<R, P> : TradeDataView<CashBankRemainsDataProvider.DataRow, CashBankRemainsDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly CashBankRemainsBase<R, P> _dataView;

		private readonly DateEditControl _remDate;

		private readonly DropDownControl _accCode;

		private readonly DropDownControl _curr;

		private readonly DropDownControl _reportCurrTypes;

		public DataViewDialog(CashBankRemainsBase<R, P> dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = (dataView._isCashDeskRems ? ArmSoft.SME.Trade.Common.Resources.KassaRemains : ArmSoft.SME.Trade.Common.Resources.BankRemains);
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_remDate = AddDateControl(container, "Date", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true, !useParamValues);
			_remDate.EditValue = (useParamValues ? dataView.Date : Settings.HostingEnvironment.CurrentDate);
			AddSpecialDateButton(container, _remDate);
			if (dataView._isCashDeskRems)
			{
				_accCode = AddDropDownControl("Code", ArmSoft.SME.Common.Resources.Cash, new ArmSoft.SME.Trade.UI.DropDown.Cashes(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			}
			else
			{
				_accCode = AddDropDownControl("Code", ArmSoft.SME.Common.Resources.Account, new ArmSoft.SME.Trade.UI.DropDown.AccountInBanks(isKeySettlAccount: false), isRequired: false, allowAdditionButtons: false, !useParamValues);
				_accCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AccInBankCode_EditValueChanged).Handler;
			}
			_curr = AddDropDownControl("CurrCode", ArmSoft.SME.Common.Resources.Curr, new Currency(shortMode: true), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_curr.Code = (useParamValues ? dataView.CurrCode : string.Empty);
			_accCode.Code = (useParamValues ? dataView.Code : string.Empty);
			AddLine();
			_reportCurrTypes = AddDropDownControl("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, new SmartEnum(typeof(ReportCurrTypesSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			_reportCurrTypes.Code = (useParamValues ? dataView.ReportCurrTypes : "1");
		}

		private void AccInBankCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_accCode.Code))
			{
				AccountInBankCacheRow item = TradeSettings.TradeCacheService.AccountInBank.GetItem(_accCode.Code);
				if (item != null)
				{
					_curr.Code = item.Curr;
					_curr.IsReadOnly = true;
				}
			}
			else
			{
				_curr.IsReadOnly = false;
				_curr.Code = string.Empty;
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_remDate.EditValue.Value);
				base.DialogValidation(args);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.Date = _remDate.EditValue.Value;
			_dataView.Code = _accCode.Code;
			_dataView.CurrCode = _curr.Code;
			_dataView.ReportCurrTypes = _reportCurrTypes.Code;
		}
	}

	private readonly bool _isCashDeskRems;

	public override bool DialogSupported => true;

	public override string Caption => base.Caption + " - " + Date.FormatDDMMYY();

	public DateOnly Date
	{
		get
		{
			return (DateOnly)Parameters["Date"].Value;
		}
		set
		{
			Parameters["Date"].Value = value;
		}
	}

	public string Code
	{
		get
		{
			return (string)Parameters["Code"].Value;
		}
		set
		{
			Parameters["Code"].Value = value;
		}
	}

	public string CurrCode
	{
		get
		{
			return (string)Parameters["CurrCode"].Value;
		}
		set
		{
			Parameters["CurrCode"].Value = value;
		}
	}

	public string ReportCurrTypes
	{
		get
		{
			return (string)Parameters["ReportCurrTypes"].Value;
		}
		set
		{
			Parameters["ReportCurrTypes"].Value = value;
		}
	}

	public CashBankRemainsBase(bool isCashDeskRems = false)
	{
		_isCashDeskRems = isCashDeskRems;
		AddParameter("Date", ArmSoft.SME.Common.Resources.DateWord, Settings.HostingEnvironment.CurrentDate, "dd/MM/yy");
		AddParameter("Code", isCashDeskRems ? ArmSoft.SME.Common.Resources.Cash : ArmSoft.SME.Common.Resources.Account, string.Empty);
		AddParameter("CurrCode", ArmSoft.SME.Common.Resources.Curr, string.Empty);
		AddParameter("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, string.Empty);
	}

	public override string GetDefaultFunction()
	{
		return "OpenTAccount";
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (_isCashDeskRems)
		{
			if (Settings.UserAccessService.GetProviderPermission(10072).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenTAccount", ArmSoft.SME.Trade.Common.Resources.CashDeskTAccount, OpenTAccount);
			}
			if (Settings.UserAccessService.GetDocumentPermission(DocumentTypeSE.CashOut).AllowAdd)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateNewCashOutDoc", ArmSoft.SME.Trade.Common.Resources.CloseCashBalance, CreateNewCashOutDoc);
			}
		}
		else if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.BankTAccount).AllowView)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenTAccount", ArmSoft.SME.Trade.Common.Resources.BankTAccount, OpenTAccount);
		}
	}

	private void CreateNewCashOutDoc()
	{
		CashOut cashOut = Settings.DocumentService.Create<CashOut>(DocumentTypeSE.CashOut);
		cashOut.DocDate = Settings.HostingEnvironment.CurrentDate;
		CashBankRemainsDataProvider.DataRow dataRow = (CashBankRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			cashOut.OpType = CashOutOpTypesSE.RevenueOutput.Value;
			cashOut.CashDesk = dataRow.Code;
			cashOut.CashierShiftNumber = TradeSettings.CreateTradeClient().CommonServices.GetOpenCashierShiftNumberByCashDesk(dataRow.Code);
			cashOut.CurrCode = dataRow.CurrCode;
			if (dataRow.RemCurr > 0m)
			{
				cashOut.Summ = Math.Abs(dataRow.RemCurr);
			}
			if (dataRow.RemNCC > 0m)
			{
				cashOut.SummNCC = Math.Abs(dataRow.RemNCC);
			}
			cashOut.ReceiverName = ArmSoft.SME.Trade.Common.Resources.CloseCashBalance;
		}
		Settings.DocumentService.CreateWindow(cashOut).Show();
	}

	private void OpenTAccount()
	{
		List<AccRestRow> list = new List<AccRestRow>();
		foreach (CashBankRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<CashBankRemainsDataProvider.DataRow>())
		{
			AccRestRow accRestRow = new AccRestRow
			{
				AccCode = selectedOrFocusedRow.Code
			};
			accRestRow.CurrCode = selectedOrFocusedRow.CurrCode;
			list.Add(accRestRow);
		}
		if (_isCashDeskRems)
		{
			CashTAccountReportViewer cashTAccountReportViewer = new CashTAccountReportViewer(list);
			cashTAccountReportViewer.ReportCurrTypes = ReportCurrTypes;
			cashTAccountReportViewer.DateBegin = Settings.HostingEnvironment.DateBegin;
			cashTAccountReportViewer.DateEnd = Settings.HostingEnvironment.DateEnd;
			cashTAccountReportViewer.Show(useParamValues: true);
		}
		else
		{
			BankTAccountReportViewer bankTAccountReportViewer = new BankTAccountReportViewer(list);
			bankTAccountReportViewer.ReportCurrTypes = ReportCurrTypes;
			bankTAccountReportViewer.DateBegin = Settings.HostingEnvironment.DateBegin;
			bankTAccountReportViewer.DateEnd = Settings.HostingEnvironment.DateEnd;
			bankTAccountReportViewer.Show(useParamValues: true);
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("SettlAccountCode", value: false);
		dvp.Set_ColumnVisible("RemCurr", ReportCurrTypes != "1");
		dvp.Set_ColumnVisible("RemNCC", ReportCurrTypes != "2");
		dvp.Set_ColumnFooterSum("RemNCC", value: true);
		dvp.Set_ColumnFooterSum("RemCurr", !string.IsNullOrWhiteSpace(CurrCode));
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Code");
			dvp.Set_ColumnSortOrder("CurrCode");
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Date = Date;
		ParametersProvider.Code = Code;
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.ReportCurrTypes = ReportCurrTypes;
	}
}
