using System.Windows;
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

[DirectoryWindow(21)]
public class OrderMinMaxDialogWindow : DirectoryDialogWindow<OrderMinMax>
{
	private new DropDownControl _code;

	private DropDownControl _storage;

	private PopupCalcEditExt _minQty;

	private PopupCalcEditExt _maxQty;

	private string _storageCodeOld;

	private string _mtCodeOld;

	protected override bool ViewModeIsWithNext => true;

	public OrderMinMaxDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public OrderMinMaxDialogWindow(OrderMinMax directory, DialogWindowMode mode = DialogWindowMode.NewModeWithNext, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void Validate(DialogWindowValidationArgs eventArg)
	{
		base.Validate(eventArg);
	}

	protected override void ConstructUI()
	{
		_storage = AddDropDownControl(Definition.Fields["StorageCode"], new Storages());
		_storage.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(StorageCode_EditValueChanged).Handler;
		_code = AddDropDownControl(Definition.Fields["Code"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods));
		_minQty = AddNumericControl(Definition.Fields["MinQty"]);
		_maxQty = AddNumericControl(Definition.Fields["MaxQty"]);
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(Directory.Code);
		if (item != null)
		{
			OrderMinMaxDataProvider.DataRow.OrderMinMaxRowKey key = new OrderMinMaxDataProvider.DataRow.OrderMinMaxRowKey
			{
				MTID = item.Id,
				StorageCode = Directory.StorageCode
			};
			dvp.Update(key);
		}
	}

	private void StorageCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(e.NewValue.ToString());
		if (item != null)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(item.Code))
			{
				e.Handled = true;
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.NewValue.ToString()), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_storage.Code = e.OldValue.ToString();
			}
			if (Mode == DialogWindowMode.NewModeWithNext || Mode == DialogWindowMode.NewMode)
			{
				_storage.Code = e.NewValue.ToString();
			}
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_storageCodeOld = _storage.Code;
		_mtCodeOld = _code.Code;
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_storage.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		Directory.StorageCodeOld = _storageCodeOld;
		Directory.OldCode = _mtCodeOld;
	}
}
