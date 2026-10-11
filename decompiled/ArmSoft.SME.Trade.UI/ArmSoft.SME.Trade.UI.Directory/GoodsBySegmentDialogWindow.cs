using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(15)]
public class GoodsBySegmentDialogWindow : DirectoryDialogWindow<ArmSoft.SME.Trade.Common.Models.Directories.GoodsBySegment>
{
	private readonly string _segment = string.Empty;

	private DropDownControl _itemType;

	private DropDownControl _segmentCode;

	private DropDownControl _mtCode;

	private string _oldCode;

	private string _oldMTCode;

	protected override bool ViewModeIsWithNext => true;

	public GoodsBySegmentDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public GoodsBySegmentDialogWindow(ArmSoft.SME.Trade.Common.Models.Directories.GoodsBySegment directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
		_segment = directory.Code;
	}

	protected override void ConstructUI()
	{
		_itemType = AddDropDownControl("ItemType", ArmSoft.SME.Common.Resources.TypeWord, new SmartEnum(typeof(GoodsTypesSE)));
		_itemType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemType_EditValueChanged).Handler;
		_segmentCode = AddDropDownControl(Definition.Fields["Code"], new ArmSoft.SME.Trade.UI.DropDown.Segments());
		_mtCode = AddDropDownControl(Definition.Fields["MTCode"], new GoodsFilter());
		_mtCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(MTCode_ClickDropDown).Handler;
		_segmentCode.Code = _segment;
		base.ConstructUI();
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		Directory.OldCode = (string.IsNullOrWhiteSpace(_oldCode) ? _segmentCode.Code : _oldCode);
		Directory.Code = _segmentCode.Code;
		Directory.OldMTCode = (string.IsNullOrWhiteSpace(_oldMTCode) ? _mtCode.Code : _oldMTCode);
		Directory.MTCode = _mtCode.Code;
	}

	private void ItemType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null)
		{
			if (e.NewValue.ToString() == ItemTypesSE.Goods.Value)
			{
				_mtCode.DataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods);
			}
			else if (e.NewValue.ToString() == ItemTypesSE.Services.Value)
			{
				_mtCode.DataView = new GoodsFilter(shortMode: true, ItemTypesSE.Services);
			}
			else
			{
				_mtCode.DataView = new GoodsFilter();
			}
			_mtCode.DoValidate();
		}
	}

	private void MTCode_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		GoodsFilter goodsFilter;
		if (_itemType.Code == ItemTypesSE.Goods.Value)
		{
			goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods);
		}
		else
		{
			goodsFilter = ((!(_itemType.Code == ItemTypesSE.Services.Value)) ? new GoodsFilter() : new GoodsFilter(shortMode: true, ItemTypesSE.Services));
		}
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

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.NewMode || Mode == DialogWindowMode.NewModeWithNext)
		{
			_itemType.Code = ItemTypesSE.Goods.Value;
		}
		else if (Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item != null)
			{
				_itemType.Code = item.Type;
			}
		}
		_oldCode = _segmentCode.Code;
		_oldMTCode = _mtCode.Code;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_itemType.Code = ItemTypesSE.Goods.Value;
		_mtCode.Code = "";
		_oldCode = _segmentCode.Code;
		_oldMTCode = _mtCode.Code;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (_oldMTCode != Directory.MTCode || _oldCode != Directory.Code)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_oldMTCode);
			if (item != null)
			{
				GoodsBySegmentDataProvider.DataRow.GoodsBySegmentRowKey key = new GoodsBySegmentDataProvider.DataRow.GoodsBySegmentRowKey
				{
					MTID = item.Id,
					SegmentCode = _oldCode
				};
				dvp.Update(key);
			}
		}
		MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(Directory.MTCode);
		if (item2 != null)
		{
			GoodsBySegmentDataProvider.DataRow.GoodsBySegmentRowKey key2 = new GoodsBySegmentDataProvider.DataRow.GoodsBySegmentRowKey
			{
				MTID = item2.Id,
				SegmentCode = Directory.Code
			};
			dvp.Update(key2);
		}
	}
}
