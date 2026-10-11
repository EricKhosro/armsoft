using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.ReportViewers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.ReportViewers;

public class BankAccountStatementReportViewer : IBBankAccountStatementReportViewer
{
	private DateEditControl _startDate;

	private DateEditControl _endDate;

	private DropDownControl _account;

	private CheckEditExt _showInNCC;

	private CheckEditExt _showDailyRems;

	public override bool ShowDialog(bool useParamValues, bool isRefreshMode)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = Caption(),
			Name = Name,
			HelpId = HelpId
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		_startDate = dialogWindow.AddDateControl(container, "StartDate", Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: true, Resources.DatePeriodBegin);
		_startDate.EditValue = (useParamValues ? StartDate : Settings.HostingEnvironment.DateBegin);
		_endDate = dialogWindow.AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, storeValue: true, Resources.DatePeriodEnd);
		_endDate.EditValue = (useParamValues ? EndDate : Settings.HostingEnvironment.DateEnd);
		dialogWindow.AddSpecialDateButton(container, _startDate, _endDate);
		_account = dialogWindow.AddDropDownControl("Account", Resources.Account, new AccountInBanks(isKeySettlAccount: true, BankingModesSE.InternetBank.Value), isRequired: true);
		_account.Code = (useParamValues ? Account : "");
		_account.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Account_EditValueChanged).Handler;
		_showInNCC = dialogWindow.AddBooleanControl("ShowInNCC", string.Format(Resources.ShowInNCC, "AMD"), storeValue: true);
		_showDailyRems = dialogWindow.AddBooleanControl("ShowDailyRems", Resources.ShowDailyRemTurn, storeValue: true);
		_showInNCC.IsChecked = useParamValues && ShowInNCC;
		_showDailyRems.IsChecked = useParamValues && ShowDailyRemains;
		if (!isRefreshMode)
		{
			dialogWindow.AddStoreValuesButton(container);
		}
		else
		{
			Account_EditValueChanged();
		}
		dialogWindow.WindowValidation += ParametersDialogValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			StartDate = _startDate.EditValue.Value;
			EndDate = _endDate.EditValue.Value;
			Account = _account.Code;
			ShowInNCC = _showInNCC.IsChecked.Value;
			ShowDailyRemains = _showDailyRems.IsChecked.Value;
			return true;
		}
		return false;
	}

	private void ParametersDialogValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
		if (string.IsNullOrWhiteSpace(Settings.InternetBankService.EnsureLoggedIn(_account.Code)))
		{
			args.isValid = false;
		}
	}

	private void Account_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		Account_EditValueChanged();
	}

	private void Account_EditValueChanged()
	{
		if (string.IsNullOrWhiteSpace(_account.Code) || !_account.DoValidate())
		{
			return;
		}
		AccountInBankCacheRow accountInBankBySettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankBySettlementAccount(_account.Code);
		if (accountInBankBySettlementAccount != null)
		{
			if (accountInBankBySettlementAccount.Curr != "AMD")
			{
				_showInNCC.IsReadOnly = false;
				_showDailyRems.IsChecked = false;
				_showDailyRems.IsReadOnly = true;
			}
			else
			{
				_showInNCC.IsChecked = false;
				_showInNCC.IsReadOnly = true;
				_showDailyRems.IsReadOnly = false;
			}
		}
	}
}
