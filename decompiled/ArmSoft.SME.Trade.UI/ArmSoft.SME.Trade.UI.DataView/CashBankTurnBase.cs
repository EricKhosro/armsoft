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
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.Data.Reports;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.ReportViewers.Cash_Bank;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.DataView;

public class CashBankTurnBase : TradeDataView<CashBankTurnOverDataProvider.DataRow, CashBankTurnOverDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly CashBankTurnBase _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly DropDownControl _accCode;

		private readonly DropDownControl _curr;

		private readonly DropDownControl _reportCurrTypes;

		public DataViewDialog(CashBankTurnBase dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = (dataView._isCashDeskTurn ? ArmSoft.SME.Trade.Common.Resources.KassaTurn : ArmSoft.SME.Trade.Common.Resources.BankTurn);
			LayoutGroupExt container = AddVerticalLayoutGroup();
			LayoutGroupExt container2 = AddHorizontalLayoutGroup(container);
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container2);
			}
			_startDate = AddDateControl(container2, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : Settings.HostingEnvironment.DateBegin);
			_endDate = AddDateControl(container2, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : Settings.HostingEnvironment.DateEnd);
			AddSpecialDateButton(container2, _startDate, _endDate);
			if (dataView._isCashDeskTurn)
			{
				_accCode = AddDropDownControl(container, "Code", ArmSoft.SME.Common.Resources.Cash, new ArmSoft.SME.Trade.UI.DropDown.Cashes(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			}
			else
			{
				_accCode = AddDropDownControl(container, "Code", ArmSoft.SME.Common.Resources.Account, new ArmSoft.SME.Trade.UI.DropDown.AccountInBanks(isKeySettlAccount: false), isRequired: false, allowAdditionButtons: false, !useParamValues);
				_accCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AccountCode_EditValueChanged).Handler;
			}
			_curr = AddDropDownControl(container, "CurrCode", ArmSoft.SME.Common.Resources.Curr, new Currency(shortMode: true), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_curr.Code = (useParamValues ? dataView.CurrCode : string.Empty);
			_accCode.Code = (useParamValues ? dataView.Code : string.Empty);
			AddLine(container);
			_reportCurrTypes = AddDropDownControl(container, "ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, new SmartEnum(typeof(ReportCurrTypesSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			_reportCurrTypes.Code = (useParamValues ? dataView.ReportCurrTypes : "1");
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
				base.DialogValidation(args);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		private void AccountCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
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
				if (!string.IsNullOrWhiteSpace(e.OldValue.ToString()))
				{
					_curr.Code = string.Empty;
				}
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue.Value;
			_dataView.EndDate = _endDate.EditValue.Value;
			_dataView.Code = _accCode.Code;
			_dataView.CurrCode = _curr.Code;
			_dataView.ReportCurrTypes = _reportCurrTypes.Code;
		}
	}

	private readonly bool _isCashDeskTurn;

	public override bool DialogSupported => true;

	public override string Caption => $"{base.Caption} - {StartDate.FormatDDMMYY()} - {EndDate.FormatDDMMYY()}";

	public DateOnly StartDate
	{
		get
		{
			return (DateOnly)Parameters["StartDate"].Value;
		}
		set
		{
			Parameters["StartDate"].Value = value;
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return (DateOnly)Parameters["EndDate"].Value;
		}
		set
		{
			Parameters["EndDate"].Value = value;
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

	public CashBankTurnBase(bool isCashDeskTurn = false)
	{
		_isCashDeskTurn = isCashDeskTurn;
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("Code", isCashDeskTurn ? ArmSoft.SME.Common.Resources.Cash : ArmSoft.SME.Common.Resources.Account, string.Empty, string.Empty);
		AddParameter("CurrCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, string.Empty);
	}

	public override string GetDefaultFunction()
	{
		return "OpenTAccount";
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (_isCashDeskTurn)
		{
			if (Settings.UserAccessService.GetProviderPermission(10072).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenTAccount", ArmSoft.SME.Trade.Common.Resources.CashDeskTAccount, OpenTAccount);
			}
		}
		else if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.BankTAccount).AllowView)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenTAccount", ArmSoft.SME.Trade.Common.Resources.BankTAccount, OpenTAccount);
		}
	}

	private void OpenTAccount()
	{
		List<AccRestRow> list = new List<AccRestRow>();
		foreach (CashBankTurnOverDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<CashBankTurnOverDataProvider.DataRow>())
		{
			AccRestRow accRestRow = new AccRestRow
			{
				AccCode = selectedOrFocusedRow.Code
			};
			if (ReportCurrTypes != "1")
			{
				accRestRow.CurrCode = selectedOrFocusedRow.CurrCode;
			}
			list.Add(accRestRow);
		}
		if (_isCashDeskTurn)
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
		dvp.Set_ColumnCaption("Code", _isCashDeskTurn ? ArmSoft.SME.Common.Resources.Cash : ArmSoft.SME.Common.Resources.Account);
		dvp.Set_ColumnVisible("StartRemNCC", ReportCurrTypes != ReportCurrTypesSE.FCOnly.Value);
		dvp.Set_ColumnVisible("TurnOverInNCC", ReportCurrTypes != ReportCurrTypesSE.FCOnly.Value);
		dvp.Set_ColumnVisible("TurnOverOutNCC", ReportCurrTypes != ReportCurrTypesSE.FCOnly.Value);
		dvp.Set_ColumnVisible("EndRemNCC", ReportCurrTypes != ReportCurrTypesSE.FCOnly.Value);
		dvp.Set_ColumnVisible("StartRemCurr", ReportCurrTypes != ReportCurrTypesSE.NCOnly.Value);
		dvp.Set_ColumnVisible("TurnOverInCurr", ReportCurrTypes != ReportCurrTypesSE.NCOnly.Value);
		dvp.Set_ColumnVisible("TurnOverOutCurr", ReportCurrTypes != ReportCurrTypesSE.NCOnly.Value);
		dvp.Set_ColumnVisible("EndRemCurr", ReportCurrTypes != ReportCurrTypesSE.NCOnly.Value);
		dvp.Set_ColumnFooterSum("StartRemNCC", value: true);
		dvp.Set_ColumnFooterSum("TurnOverInNCC", value: true);
		dvp.Set_ColumnFooterSum("TurnOverOutNCC", value: true);
		dvp.Set_ColumnFooterSum("EndRemNCC", value: true);
		dvp.Set_ColumnFooterSum("StartRemCurr", !string.IsNullOrWhiteSpace(CurrCode));
		dvp.Set_ColumnFooterSum("TurnOverInCurr", !string.IsNullOrWhiteSpace(CurrCode));
		dvp.Set_ColumnFooterSum("TurnOverOutCurr", !string.IsNullOrWhiteSpace(CurrCode));
		dvp.Set_ColumnFooterSum("EndRemCurr", !string.IsNullOrWhiteSpace(CurrCode));
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Code");
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.Code = Code;
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.ReportCurrTypes = ReportCurrTypes;
	}
}
