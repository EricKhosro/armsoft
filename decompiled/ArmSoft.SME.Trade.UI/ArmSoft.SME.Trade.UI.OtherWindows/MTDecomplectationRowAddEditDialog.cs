using System;
using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class MTDecomplectationRowAddEditDialog : DialogWindow
{
	private DropDownControl _mtGroup;

	private DropDownControl _mtCode;

	private DropDownControl _qntUnit;

	private PopupCalcEditExt _quantity;

	private PopupCalcEditExt _pricePart;

	private PopupCalcEditExt _summaPart;

	private int _rowID;

	private MaterialCacheRow.ComplectPartRow _mtComplectsRow;

	private static List<MaterialCacheRow.ComplectPartRow> _mtComplectPartsRows;

	private readonly MTDecomplectationWindow.MTDecomplectationSpecificationRow _sourceRow;

	private bool _editableMode;

	private readonly bool _allowAddRowAndNotViewMode;

	private readonly MTDecomplectationWindow _mtDecomplectation;

	public int AddedRows { get; set; }

	public MTDecomplectationRowAddEditDialog(DialogWindowMode mode, MTDecomplectationWindow mtDecomplectation, MTDecomplectationWindow.MTDecomplectationSpecificationRow sourceRow = null)
	{
		Mode = mode;
		_mtDecomplectation = mtDecomplectation;
		_allowAddRowAndNotViewMode = (!mtDecomplectation.PO.StoredInDb || mtDecomplectation.PO.State == DocumentStateSE.Processed.Value) && mode != DialogWindowMode.ViewMode;
		if (_allowAddRowAndNotViewMode)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtDecomplectation._complect.Code);
			if (item != null)
			{
				_mtComplectPartsRows = item.ComplectParts;
			}
		}
		AddedRows = 0;
		Owner = mtDecomplectation;
		if (sourceRow != null)
		{
			_sourceRow = sourceRow;
		}
		Caption = ArmSoft.SME.Trade.Common.Resources.MTComplectPart;
	}

	public bool? Invoke()
	{
		if (_allowAddRowAndNotViewMode)
		{
			_mtGroup = AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups());
			_mtGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTGroup_EditValueChanged).Handler;
		}
		_mtCode = AddDropDownControl(_mtDecomplectation.Definition.Grids["ComplectPartsList"].Columns["ItemCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value), allowAdditionButtons: true);
		_mtCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(MTCode_ClickDropDown).Handler;
		_mtCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTCode_EditValueChanged).Handler;
		_mtCode.EditAdditionalValueChanged += new SafeEventHandler<EventArgs>(MTCode_EditAdditionalValueChanged).Handler;
		_qntUnit = AddDropDownControl(_mtDecomplectation.Definition.Grids["ComplectPartsList"].Columns["UnitCode"], new QntUnits());
		_qntUnit.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(QntUnit_ClickDropDown).Handler;
		_qntUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(QntUnit_EditValueChanged).Handler;
		_quantity = AddNumericControl(_mtDecomplectation.Definition.Grids["ComplectPartsList"].Columns["Quantity"]);
		_quantity.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Quantity_EditValueChanged).Handler;
		_pricePart = AddNumericControl(_mtDecomplectation.Definition.Grids["ComplectPartsList"].Columns["PricePart"]);
		_pricePart.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PricePart_EditValueChanged).Handler;
		_summaPart = AddNumericControl(_mtDecomplectation.Definition.Grids["ComplectPartsList"].Columns["SummaPart"]);
		_summaPart.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SummaPart_EditValueChanged).Handler;
		WindowActivated += DecomplectationRow_WindowActivated;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(DecomplectationRow_WindowValidation).Handler;
		NextPressed += new SafeEventHandler<EventArgs>(DecomplectationRow_NextPressed).Handler;
		return ShowDialog();
	}

	private void DecomplectationRow_WindowActivated(DialogWindow window)
	{
		_editableMode = false;
		if (_sourceRow != null)
		{
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				_rowID = _mtDecomplectation._maxRowID;
			}
			else
			{
				_rowID = _sourceRow.RowID;
			}
			_mtCode.Code = _sourceRow.ItemCode;
			_qntUnit.Code = _sourceRow.UnitCode;
			_quantity.Value = _sourceRow.Quantity;
			_pricePart.Value = _sourceRow.PricePart;
			_summaPart.Value = _sourceRow.SummaPart;
			if (!string.IsNullOrWhiteSpace(_mtCode.Code) && Mode != DialogWindowMode.ViewMode)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
				if (item != null)
				{
					_qntUnit.DataView = new MTQntUnits(item.MTCode);
					_qntUnit.DoValidate();
				}
				_mtComplectsRow = GetComplectPartsRowBySourceID(_sourceRow.ItemID);
			}
			if (!_allowAddRowAndNotViewMode)
			{
				SetControlsReadonly(value: true);
			}
		}
		else if (Mode == DialogWindowMode.NewModeWithNext)
		{
			_rowID = _mtDecomplectation._maxRowID;
		}
		ControlsHelper.SetFocus(_mtCode);
		_editableMode = true;
	}

	private void MTCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		UpdateMaterialData();
	}

	private void MTGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_mtGroup.DoValidate() && !string.IsNullOrWhiteSpace(_mtCode.Code) && _mtCode.DoValidate() && TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code) != null)
		{
			_qntUnit.DataView = new MTQntUnits(_mtCode.Code);
		}
		if (!string.IsNullOrWhiteSpace(_mtGroup.Code) && !string.IsNullOrWhiteSpace(_mtCode.Code) && !_mtCode.DoValidate())
		{
			CleanAllField();
		}
	}

	private void MTCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_mtCode.Code) && _mtCode.DoValidate())
		{
			if (_editableMode)
			{
				UpdateMaterialData();
			}
		}
		else
		{
			_qntUnit.DataView = new MTQntUnits(_mtGroup.Code, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Goods.Value);
			_qntUnit.Code = "";
			_mtComplectsRow = null;
		}
	}

	private void MTCode_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, _mtGroup.Code, isShowOnlyInPriceList: true);
		ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
		{
			LookUpValue = _mtCode.Code,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			_mtCode.Code = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void QntUnit_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_mtCode.Code) && _mtCode.DoValidate())
		{
			return;
		}
		MTQntUnits mTQntUnits = new MTQntUnits(_mtGroup.Code, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Goods.Value);
		ModalBrowser modalBrowser = new ModalBrowser(mTQntUnits)
		{
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			MTQntUnitDataProvider.DataRow.Key key = (MTQntUnitDataProvider.DataRow.Key)mTQntUnits.KeyMember(modalBrowser.SelectedRowIndex);
			if (key != null)
			{
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(key.MTID);
				if (itemById != null)
				{
					_mtCode.Code = itemById.MTCode;
					_mtCode.DoValidate();
				}
			}
			_qntUnit.Code = mTQntUnits.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void QntUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_qntUnit.Code) && _qntUnit.DoValidate() && !string.IsNullOrWhiteSpace(_mtCode.Code) && _mtCode.DoValidate() && _editableMode && TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code) == null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
		}
	}

	private void UpdateMaterialData()
	{
		try
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
			}
			_qntUnit.DataView = new MTQntUnits(_mtCode.Code);
			_editableMode = false;
			_qntUnit.Code = item.Unit;
			_mtComplectsRow = GetComplectPartsRowBySourceID(item.Id);
			if (_mtComplectsRow != null)
			{
				_quantity.EditValue = (_mtComplectsRow.Quantity * _mtDecomplectation._quantity.Value).RoundAs(3);
				_pricePart.EditValue = _mtComplectsRow.PricePart;
			}
			else
			{
				_quantity.EditValue = 0m;
				_pricePart.EditValue = 0m;
			}
			_summaPart.EditValue = PriceHelper.CalculateSum(_pricePart.Value, _quantity.Value);
			_summaPart.DoValidate();
			_editableMode = true;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static MaterialCacheRow.ComplectPartRow GetComplectPartsRowBySourceID(int sourceId)
	{
		MaterialCacheRow.ComplectPartRow result = null;
		if (_mtComplectPartsRows != null)
		{
			foreach (MaterialCacheRow.ComplectPartRow mtComplectPartsRow in _mtComplectPartsRows)
			{
				if (mtComplectPartsRow.ItemId == sourceId)
				{
					result = mtComplectPartsRow;
					break;
				}
			}
		}
		return result;
	}

	private void Quantity_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			_summaPart.EditValue = PriceHelper.CalculateSum(_pricePart.Value, _quantity.Value);
			_summaPart.DoValidate();
			_editableMode = true;
		}
	}

	private void PricePart_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			_summaPart.EditValue = PriceHelper.CalculateSum(_pricePart.Value, _quantity.Value);
			_summaPart.DoValidate();
			_editableMode = true;
		}
	}

	private void SummaPart_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode && _quantity.Value > 0m)
		{
			_editableMode = false;
			_pricePart.EditValue = (_summaPart.Value / _quantity.Value).RoundAs(3);
			_pricePart.DoValidate();
			_editableMode = true;
		}
	}

	private void DecomplectationRow_WindowValidation(object sender, DialogWindowValidationArgs args)
	{
		try
		{
			foreach (MTDecomplectationWindow.MTDecomplectationSpecificationRow mtComplectParts in _mtDecomplectation._mtComplectPartsList)
			{
				if (_mtCode.Code == mtComplectParts.ItemCode && ((mtComplectParts.RowID != _rowID && Mode == DialogWindowMode.EditMode && _mtCode.Code != _sourceRow?.ItemCode) || Mode == DialogWindowMode.NewModeWithNext))
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.WrongComplectPart, _mtCode.Code, _mtDecomplectation._complect.Code));
				}
			}
			MTDecomplectationWindow.MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTDecomplectationSpecificationRow = new MTDecomplectationWindow.MTDecomplectationSpecificationRow();
				_mtDecomplectation._mtComplectPartsList.Add(mTDecomplectationSpecificationRow);
			}
			else
			{
				mTDecomplectationSpecificationRow = _sourceRow;
			}
			DocumentGridAddingRowEventArgs e = new DocumentGridAddingRowEventArgs(_mtDecomplectation._mtComplectPartsGrid);
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
			}
			mTDecomplectationSpecificationRow.ItemID = item.Id;
			mTDecomplectationSpecificationRow.ItemCode = item.MTCode;
			mTDecomplectationSpecificationRow.ItemName = item.Caption;
			mTDecomplectationSpecificationRow.UnitCode = _qntUnit.Code;
			QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(mTDecomplectationSpecificationRow.UnitCode);
			if (item2 != null)
			{
				mTDecomplectationSpecificationRow.UnitBrief = item2.Brief;
			}
			else
			{
				mTDecomplectationSpecificationRow.UnitBrief = "";
			}
			mTDecomplectationSpecificationRow.Quantity = _quantity.Value;
			mTDecomplectationSpecificationRow.PricePart = _pricePart.Value;
			mTDecomplectationSpecificationRow.SummaPart = _summaPart.Value;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTDecomplectationSpecificationRow.RowID = _rowID;
				_mtDecomplectation._maxRowID++;
				AddedRows++;
				e.RowNumber = _mtDecomplectation._mtComplectPartsList.Count - 1;
				_mtDecomplectation._mtComplectPartsGrid.InnerAddRow(e);
			}
			_mtDecomplectation._mtComplectPartsGrid.Grid.UpdateTotalSummary();
			_mtDecomplectation._mtComplectPartsGrid.View.CommitEditing();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				args.SetFocusWhenInvalidSave = _mtCode;
			}
		}
	}

	private void DecomplectationRow_NextPressed(object sender, EventArgs e)
	{
		_editableMode = false;
		CleanAllField(isCleanItemCodeField: false);
		_rowID = _mtDecomplectation._maxRowID;
		ControlsHelper.SetFocus(_mtCode);
		_editableMode = true;
	}

	private void CleanAllField(bool isCleanItemCodeField = true)
	{
		if (isCleanItemCodeField)
		{
			_mtCode.Code = "";
			_qntUnit.Code = "";
			_pricePart.EditValue = 0m;
			_summaPart.EditValue = 0m;
		}
		_quantity.EditValue = 0m;
	}
}
