using System;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class CashPaymentDialog : DialogWindow
{
	private PopupCalcEditExt _cashSum;

	private Button _loadSum;

	private readonly bool _editMode;

	private readonly decimal _remainSum;

	public decimal EnteredCashSum { get; set; }

	public CashPaymentDialog(Window owner, decimal cashSum, decimal remainSum, bool editMode)
	{
		Owner = owner;
		EnteredCashSum = cashSum;
		_remainSum = remainSum;
		_editMode = editMode;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		Caption = ArmSoft.SME.Common.Resources.Cash;
		Name = "Cash";
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_cashSum = AddNumericControl(container, "CashPaymentSum", ArmSoft.SME.Common.Resources.Summa, 16, 2, isRequired: false, allowNegativeNumbers: true);
		_cashSum.EditValue = Math.Abs(EnteredCashSum);
		_loadSum = AddButton(container, "Fill", "fill_left_16.svg", "", "", ArmSoft.SME.Common.Resources.ButtonLoad);
		_loadSum.Click += new SafeEventHandler<RoutedEventArgs>(LoadSum_Click).Handler;
		if (_editMode)
		{
			_cashSum.IsReadOnly = true;
			Mode = DialogWindowMode.ViewMode;
		}
		SetFocusOnActivation = _cashSum;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(CashPaymentDialog_WindowValidation).Handler;
	}

	private void CashPaymentDialog_WindowValidation(object window, DialogWindowValidationArgs args)
	{
		EnteredCashSum = _cashSum.Value;
	}

	private void LoadSum_Click(object sender, RoutedEventArgs e)
	{
		if (!_cashSum.IsReadOnly)
		{
			_cashSum.EditValue = _remainSum;
		}
	}
}
