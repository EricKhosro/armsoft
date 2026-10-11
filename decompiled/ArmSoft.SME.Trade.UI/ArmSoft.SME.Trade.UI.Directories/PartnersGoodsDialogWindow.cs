using System.Windows;
using ArmSoft.SME.Common;
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

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(22)]
public class PartnersGoodsDialogWindow : DirectoryDialogWindow<PartnersGood>
{
	private DropDownControl _partner;

	private TextEditExt _tin;

	private TextEditExt _partItemCode;

	private TextEditExt _partItemName;

	private DropDownControl _itemType;

	private DropDownControl _internalQntUnit;

	private TextEditExt _externalQntUnitName;

	private PopupCalcEditExt _coeff;

	private DropDownControl _item;

	private string _partCodeOld = "";

	private string _partItemCodeOld = "";

	private string _partnerItemUnitOld = "";

	private string _partItemUnitNameOld = "";

	public bool IsFilteredPartner { get; set; }

	protected override bool ViewModeIsWithNext => true;

	public PartnersGoodsDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public PartnersGoodsDialogWindow(PartnersGood directory, DialogWindowMode mode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_partner = AddDropDownControl(Definition.Fields["Code"], new PartnersFilter(), allowAdditionButtons: true);
		_partner.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Partner_EditValueChanged).Handler;
		_tin = AddStringControl(Definition.Fields["PartTIN"]);
		_itemType = AddSmartEnumDropDownControl(Definition.Fields["Type"]);
		_itemType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemType_EditValueChanged).Handler;
		_partItemCode = AddStringControl(Definition.Fields["PartItemCode"]);
		_partItemName = AddStringControl(Definition.Fields["PartItemCaption"]);
		_externalQntUnitName = AddStringControl(Definition.Fields["PartItemUnitName"]);
		_coeff = AddNumericControl(Definition.Fields["Coeff"]);
		_item = AddDropDownControl(Definition.Fields["ItemCode"], new GoodsFilter(), allowAdditionButtons: true);
		_item.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Item_EditValueChanged).Handler;
		_item.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(Item_ClickDropDown).Handler;
		_internalQntUnit = AddDropDownControl(Definition.Fields["ItemUnit"], new ArmSoft.SME.Trade.UI.DropDown.QntUnits());
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		if (!string.IsNullOrWhiteSpace(_internalQntUnit.Code) && !string.IsNullOrWhiteSpace(_externalQntUnitName.Text))
		{
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(_internalQntUnit.Code);
			if (item != null && _externalQntUnitName.Text.ToUpper() != item.Brief.ToUpper() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.InternalExternalUnitsDiff, Caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, this, 60) == MessageBoxResult.No)
			{
				args.isValid = false;
			}
		}
		if (args.isValid)
		{
			base.DialogValidation(args);
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.NewModeWithNext)
		{
			_itemType.Code = GoodsTypesSE.Good.Value;
		}
		else if (Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_item.Code);
			if (item != null)
			{
				_itemType.Code = item.Type;
			}
		}
		_partCodeOld = _partner.Code;
		_partItemCodeOld = _partItemCode.Text;
		_partnerItemUnitOld = Directory.PartItemUnit;
		_partItemUnitNameOld = _externalQntUnitName.Text;
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		Directory.PartCodeOld = (string.IsNullOrWhiteSpace(_partCodeOld) ? _partner.Code : _partCodeOld);
		Directory.PartItemCodeOld = (string.IsNullOrEmpty(_partItemCodeOld) ? _partItemCode.Text : _partItemCodeOld);
		Directory.PartItemUnitOld = _partnerItemUnitOld;
		Directory.PartItemUnitNameOld = (string.IsNullOrWhiteSpace(_partItemUnitNameOld) ? _externalQntUnitName.Text : _partItemUnitNameOld);
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_partner.IsReadOnly = IsFilteredPartner;
		_tin.IsReadOnly = true;
		_itemType.IsReadOnly = Mode == DialogWindowMode.EditMode;
		if (Mode == DialogWindowMode.EditMode && string.IsNullOrWhiteSpace(_item.Code))
		{
			_internalQntUnit.IsReadOnly = true;
		}
	}

	private void Partner_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_partner.Code))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partner.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.PartnerCodeIsNotExist, _partner.Code));
			}
			_tin.Text = item.TaxCode;
		}
	}

	private void ItemType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null)
		{
			if (e.NewValue.ToString() == ItemTypesSE.Goods.Value)
			{
				_item.DataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value);
			}
			else if (e.NewValue.ToString() == ItemTypesSE.Services.Value)
			{
				_item.DataView = new GoodsFilter(shortMode: true, ItemTypesSE.Services.Value);
			}
			else
			{
				_item.DataView = new GoodsFilter();
			}
			_item.DoValidate();
		}
	}

	private void Item_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		GoodsFilter goodsFilter;
		if (_itemType.Code == ItemTypesSE.Goods.Value)
		{
			goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value);
		}
		else
		{
			goodsFilter = ((!(_itemType.Code == ItemTypesSE.Services.Value)) ? new GoodsFilter() : new GoodsFilter(shortMode: true, ItemTypesSE.Services.Value));
		}
		ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
		{
			LookUpValue = _item.Code,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			_item.Code = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void Item_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_item.Code))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_item.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _item.Code));
			}
			_internalQntUnit.DataView = new MTQntUnits(item.MTCode);
			_internalQntUnit.Code = item.Unit;
			_internalQntUnit.IsReadOnly = false;
		}
		else
		{
			_internalQntUnit.IsReadOnly = true;
			_internalQntUnit.Code = "";
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(Directory.Code);
		if (item == null)
		{
			return;
		}
		PartnersGoodsDataProvider.DataRow.PartnerGoodsRowKey key = new PartnersGoodsDataProvider.DataRow.PartnerGoodsRowKey
		{
			PartID = item.Id,
			PartItemCode = Directory.PartItemCode,
			PartItemUnit = Directory.PartItemUnit
		};
		dvp.Update(key);
		if (Directory.PartItemCode != Directory.PartItemCodeOld || Directory.Code != _partCodeOld)
		{
			item = TradeSettings.TradeCacheService.Partner.GetItem(_partCodeOld);
			if (item != null)
			{
				key = new PartnersGoodsDataProvider.DataRow.PartnerGoodsRowKey
				{
					PartID = item.Id,
					PartItemCode = Directory.PartItemCodeOld,
					PartItemUnit = Directory.PartItemUnitOld
				};
				dvp.Update(key);
			}
		}
	}
}
