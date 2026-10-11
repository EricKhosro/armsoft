using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents.Banks_and_Cashes;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class PartnerBonusGroupCorrectRowAddEditDialog<T> : DialogWindow where T : PartnerBonusRemGroupCorrectBase, new()
{
	private PartnerContractGroupControls _partnerContractContainer;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _summaNCC;

	private readonly PartnerBonusGroupCorrectBaseWindow<T>.PartBonusGroupCorrectSpecificationRow _sourceRow;

	private readonly bool _allowAddRowAndNotViewMode;

	private readonly PartnerBonusGroupCorrectBaseWindow<T> _partnerGroupCorrectWindow;

	public int AddedRows { get; set; }

	public PartnerBonusGroupCorrectRowAddEditDialog(DialogWindowMode mode, PartnerBonusGroupCorrectBaseWindow<T> partnerBonusGroupCorrectWindow, PartnerBonusGroupCorrectBaseWindow<T>.PartBonusGroupCorrectSpecificationRow sourceRow = null)
	{
		Mode = mode;
		_partnerGroupCorrectWindow = partnerBonusGroupCorrectWindow;
		_allowAddRowAndNotViewMode = _partnerGroupCorrectWindow._partRemGrid.AllowAdd && mode != DialogWindowMode.ViewMode;
		AddedRows = 0;
		Owner = partnerBonusGroupCorrectWindow;
		if (sourceRow != null)
		{
			_sourceRow = sourceRow;
		}
		Caption = ((_partnerGroupCorrectWindow.PO.POType == DocumentTypeSE.BonusGroupCorrect.Value) ? ArmSoft.SME.Trade.Common.Resources.BonusGroupCorrect : ArmSoft.SME.Trade.Common.Resources.PartnerRemGroupCorrection);
	}

	public bool? Invoke()
	{
		_partnerContractContainer = new PartnerContractGroupControls(this);
		if (_partnerGroupCorrectWindow.PO.POType == DocumentTypeSE.BonusGroupCorrect.Value)
		{
			_partnerContractContainer.ConstructDlgUI(null, storeValue: true, "", isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: true, isShowSummaDebt: true, "", ArmSoft.SME.Trade.Common.Resources.AccumulatedBonus, showOnlyBonusContracts: true);
		}
		else
		{
			_partnerContractContainer.ConstructDlgUI(null, storeValue: true, "", isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: true, isShowSummaDebt: true);
		}
		_partnerContractContainer.PartnerCode.IsRequired = true;
		_partnerContractContainer.ContractCurrCode.IsRequired = true;
		_partnerContractContainer.ContractCurrCode.Code = _partnerGroupCorrectWindow._currCode.Code;
		_partnerContractContainer.ContractCurrCode.IsReadOnly = true;
		_partnerContractContainer.Contract.DataView = new Contracts("", _partnerGroupCorrectWindow._currCode.Code);
		_partnerContractContainer.PartnerCodeEditValueChanged += new SafeEventHandler<EventArgs>(PartnerContractContainer_PartnerCodeEditValueChanged).Handler;
		_partnerContractContainer.ContractCodeEditValueChanged += new SafeEventHandler<EventArgs>(PartnerContractContainer_ContractCodeEditValueChanged).Handler;
		if (_partnerGroupCorrectWindow.PO.POType == DocumentTypeSE.BonusGroupCorrect.Value)
		{
			_summa = AddNumericControl("Summa", string.Format(ArmSoft.SME.Trade.Common.Resources.Bonus, ArmSoft.SME.Common.Resources.Unit) + " (+/-)", 16, 2, isRequired: true, allowNegativeNumbers: true);
		}
		else
		{
			_summa = AddNumericControl("Summa", ArmSoft.SME.Common.Resources.Summa, 16, 2, isRequired: true, allowNegativeNumbers: true);
		}
		_summa.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged).Handler;
		_summaNCC = AddNumericControl("SummaNCC", ArmSoft.SME.Trade.Common.Resources.SumNCC, 16, 2, isRequired: true, allowNegativeNumbers: true);
		_summa.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SummaNCC_EditValueChanged).Handler;
		if (_partnerGroupCorrectWindow.PO.POType == DocumentTypeSE.BonusGroupCorrect.Value)
		{
			((LayoutItemWithRequired)(object)_summaNCC.Parent).Visibility = Visibility.Collapsed;
		}
		WindowActivated += PartRemGroupCorrectRow_WindowActivated;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(PartRemGroupCorrectRow_WindowValidation).Handler;
		NextPressed += new SafeEventHandler<EventArgs>(PartRemGroupCorrectRow_NextPressed).Handler;
		return ShowDialog();
	}

	private void PartRemGroupCorrectRow_WindowActivated(DialogWindow window)
	{
		if (_sourceRow != null)
		{
			_partnerContractContainer.PartnerCode.Code = _sourceRow.PartCode;
			_partnerContractContainer.Contract.Code = _sourceRow.Contract;
			_summa.EditValue = _sourceRow.Summa;
			_summaNCC.EditValue = _sourceRow.SummaNCC;
			if (!_allowAddRowAndNotViewMode)
			{
				SetControlsReadonly(value: true);
			}
		}
	}

	private void PartRemGroupCorrectRow_WindowValidation(object sender, DialogWindowValidationArgs args)
	{
		try
		{
			PartnerBonusGroupCorrectBaseWindow<T>.PartBonusGroupCorrectSpecificationRow partBonusGroupCorrectSpecificationRow;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				partBonusGroupCorrectSpecificationRow = new PartnerBonusGroupCorrectBaseWindow<T>.PartBonusGroupCorrectSpecificationRow();
				_partnerGroupCorrectWindow._partRemList.Add(partBonusGroupCorrectSpecificationRow);
			}
			else
			{
				partBonusGroupCorrectSpecificationRow = _sourceRow;
			}
			DocumentGridAddingRowEventArgs e = new DocumentGridAddingRowEventArgs(_partnerGroupCorrectWindow._partRemGrid);
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partnerContractContainer.PartnerCode.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.PartnerCodeIsNotExist, _partnerContractContainer.PartnerCode.Code));
			}
			partBonusGroupCorrectSpecificationRow.PartCode = item.PartCode;
			partBonusGroupCorrectSpecificationRow.PartName = item.Caption;
			partBonusGroupCorrectSpecificationRow.Contract = _partnerContractContainer.Contract.Code;
			partBonusGroupCorrectSpecificationRow.Summa = _summa.Value;
			partBonusGroupCorrectSpecificationRow.SummaNCC = _summaNCC.Value;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				AddedRows++;
				e.RowNumber = _partnerGroupCorrectWindow._partRemList.Count - 1;
				_partnerGroupCorrectWindow._partRemGrid.InnerAddRow(e);
			}
			_partnerGroupCorrectWindow._partRemGrid.View.CommitEditing();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				args.SetFocusWhenInvalidSave = _partnerContractContainer.PartnerCode;
			}
		}
	}

	private void PartRemGroupCorrectRow_NextPressed(object sender, EventArgs e)
	{
		_partnerContractContainer.PartnerCode.Code = "";
		_partnerContractContainer.ContractCurrCode.Code = _partnerGroupCorrectWindow._currCode.Code;
		_partnerContractContainer.ContractCurrCode.IsReadOnly = true;
		_summa.EditValue = 0m;
		_summaNCC.EditValue = 0m;
		ControlsHelper.SetFocus(_partnerContractContainer.PartnerCode);
	}

	private void Summa_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_partnerGroupCorrectWindow._currCode.Code != "AMD" && _partnerGroupCorrectWindow._currExChngRate.Base != 0m)
		{
			_summaNCC.EditValue = (Convert.ToDecimal(_summa.EditValue) * _partnerGroupCorrectWindow._currExChngRate.Rate / _partnerGroupCorrectWindow._currExChngRate.Base).RoundAs(2);
		}
		else
		{
			_summaNCC.EditValue = Convert.ToDecimal(_summa.EditValue);
		}
	}

	private void SummaNCC_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_partnerGroupCorrectWindow._currCode.Code != "AMD" && _partnerGroupCorrectWindow._currExChngRate.Base != 0m)
		{
			_summa.Value = (Convert.ToDecimal(_summaNCC.EditValue) * _partnerGroupCorrectWindow._currExChngRate.Base / _partnerGroupCorrectWindow._currExChngRate.Rate).RoundAs(2);
		}
		else
		{
			_summa.Value = Convert.ToDecimal(_summaNCC.EditValue);
		}
	}

	private void PartnerContractContainer_ContractCodeEditValueChanged(object sender, EventArgs e)
	{
		if (!_partnerContractContainer.Contract.DoValidate())
		{
			_partnerContractContainer.ContractCurrCode.Code = _partnerGroupCorrectWindow._currCode.Code;
			_partnerContractContainer.ContractCurrCode.IsReadOnly = true;
			ControlsHelper.SetFocus(_partnerContractContainer.Contract);
		}
	}

	private void PartnerContractContainer_PartnerCodeEditValueChanged(object sender, EventArgs e)
	{
		if (_partnerContractContainer.PartnerCode.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partnerContractContainer.PartnerCode.Code);
			if (item != null)
			{
				_partnerContractContainer.Contract.DataView = new Contracts(item.PartCode, _partnerGroupCorrectWindow._currCode.Code, isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: false, isValueMemberPartner: false, _partnerGroupCorrectWindow.PO.POType == DocumentTypeSE.BonusGroupCorrect.Value);
				_partnerContractContainer.Contract.IsRequired = true;
				_partnerContractContainer.Contract.Code = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item, _partnerGroupCorrectWindow._currCode.Code)?.Contract;
			}
		}
	}
}
