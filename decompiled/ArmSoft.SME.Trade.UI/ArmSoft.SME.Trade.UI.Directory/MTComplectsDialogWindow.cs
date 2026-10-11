using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(17)]
public class MTComplectsDialogWindow : DirectoryDialogWindow<MTComplectsPart>
{
	private DropDownControl _itemType;

	private DropDownControl _mtDest;

	private DropDownControl _mtSource;

	private DropDownControl _qntUnit;

	private PopupCalcEditExt _quantity;

	private PopupCalcEditExt _complectQuantity;

	private PopupCalcEditExt _summPart;

	private PopupCalcEditExt _summ;

	protected override bool ViewModeIsWithNext => true;

	public bool IsFilteredComplect { get; set; }

	public MTComplectsDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public MTComplectsDialogWindow(MTComplectsPart directory, DialogWindowMode mode = DialogWindowMode.NewModeWithNext, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_mtDest = AddDropDownControl(Definition.Fields["Code"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods));
		if (IsFilteredComplect)
		{
			((LayoutItemWithRequired)(object)_mtDest.Parent).Visibility = Visibility.Collapsed;
		}
		_itemType = AddSmartEnumDropDownControl(Definition.Fields["ItemType"]);
		_itemType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemType_EditValueChanged).Handler;
		_mtSource = AddDropDownControl(Definition.Fields["MTCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods));
		_mtSource.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTSource_EditValueChanged).Handler;
		_qntUnit = AddDropDownControl(Definition.Fields["QntUnit"], new MTQntUnits());
		_qntUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(QntUnit_EditValueChanged).Handler;
		_quantity = AddNumericControl(Definition.Fields["Quantity"]);
		_complectQuantity = AddNumericControl(Definition.Fields["ComplectQuantity"]);
		_summPart = AddNumericControl(Definition.Fields["SummaPart"]);
		_summ = AddNumericControl(Definition.Fields["Summa"]);
		base.ConstructUI();
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtSource.Code = "";
		_qntUnit.Code = "";
		_quantity.Value = 1m;
		_complectQuantity.Value = 1m;
		_summPart.Value = 0m;
	}

	private void MTSource_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		InnerType_EditValueChanged();
	}

	private void InnerType_EditValueChanged()
	{
		if (string.IsNullOrWhiteSpace(_mtSource.Code))
		{
			_qntUnit.DataView = new MTQntUnits();
		}
		else
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtSource.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtSource.Code));
			}
			_qntUnit.DataView = new MTQntUnits(item.MTCode);
			_qntUnit.Code = item.Unit;
		}
		_qntUnit.DoValidate();
	}

	private void ItemType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		string value = ItemTypesSE.All.Value;
		if (e.NewValue.ToString() == ItemTypesSE.Goods.Value)
		{
			value = ItemTypesSE.Goods.Value;
			((LayoutItemWithRequired)(object)_summPart.Parent).Visibility = Visibility.Visible;
			((LayoutItemWithRequired)(object)_summ.Parent).Visibility = Visibility.Collapsed;
		}
		else
		{
			value = ItemTypesSE.Services.Value;
			((LayoutItemWithRequired)(object)_summPart.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_summ.Parent).Visibility = Visibility.Visible;
		}
		_mtSource.DataView = new GoodsFilter(shortMode: true, value.ToString(), "", isShowOnlyInPriceList: true);
	}

	private void QntUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_itemType.Code == ItemTypesSE.Goods.Value || !(_summ.Value != 0m) || !_mtSource.DoValidate())
		{
			return;
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtSource.Code);
		if (e.OldValue != null && e.NewValue != null)
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(item.MTCode, Convert.ToString(e.OldValue));
			MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(item.MTCode, Convert.ToString(e.NewValue));
			if (qntUnit != null && qntUnit2 != null)
			{
				_summ.Value = (_summ.Value * qntUnit2.Coef / qntUnit.Coef).RoundAs(2);
			}
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		SetFocusOnActivation = _itemType;
		if (Mode == DialogWindowMode.NewModeWithNext)
		{
			_itemType.Code = ItemTypesSE.Goods.Value;
			_quantity.EditValue = 1m;
			_complectQuantity.EditValue = 1m;
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_itemType.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_mtDest.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_mtSource.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtDest.Code, throwExceptionNotExits: true);
		Directory.Id = item.Id;
		if (Mode != DialogWindowMode.EditMode)
		{
			Directory.Code = item.MTCode;
		}
		item = TradeSettings.TradeCacheService.Material.GetItem(_mtSource.Code, throwExceptionNotExits: true);
		Directory.MTID = item.Id;
		if (Mode != DialogWindowMode.EditMode)
		{
			Directory.MTCode = item.MTCode;
		}
		if (Directory.Id == Directory.MTID)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidComplectMt);
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		MTComplectsPartsDataProvider.DataRow.MTComplectsRowKey key = new MTComplectsPartsDataProvider.DataRow.MTComplectsRowKey
		{
			MTDestID = Directory.Id,
			MTSourceID = Directory.MTID
		};
		dvp.Update(key);
	}
}
