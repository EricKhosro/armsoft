using System;
using System.Windows;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(16)]
public class ItemBarCodesDialogWindow : DirectoryDialogWindow<MTBarCode>
{
	private DropDownControl _itemCode;

	private DropDownControl _qntUnit;

	private ButtonEditExt _barCode;

	private string _barCodeOld;

	private bool _setNextBarCode;

	public ItemBarCodesDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public ItemBarCodesDialogWindow(MTBarCode directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void Validate(DialogWindowValidationArgs eventArg)
	{
		base.Validate(eventArg);
		if (_setNextBarCode)
		{
			if (Settings.ParametersService.InternalBarcodeEANFormat())
			{
				Settings.ParametersService.SetValue(10030, StringExtensions.NextDocNum(Settings.ParametersService.InternalBarcodeNext(), 10));
			}
			else
			{
				Settings.ParametersService.SetValue(10030, StringExtensions.NextDocNum(Settings.ParametersService.InternalBarcodeNext(), 20));
			}
			_setNextBarCode = false;
		}
	}

	protected override void ConstructUI()
	{
		_itemCode = AddDropDownControl(Definition.Fields["MTCode"], new GoodsFilter());
		_itemCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemCode_EditValueChanged).Handler;
		_qntUnit = AddDropDownControl(Definition.Fields["QntUnit"], new MTQntUnits());
		_barCode = AddButtonEditControl(Definition.Fields["BarCode"], 1);
		_barCode.AllowMagicButton = true;
		_barCode.MagicToolTip = ArmSoft.SME.Trade.Common.Resources.NewBarcode;
		_barCode.IsReadOnly = Mode == DialogWindowMode.ViewMode;
		_barCode.MagicClick += new SafeEventHandler<MagicClickEventArgs>(BarCode_MagicClick).Handler;
	}

	private void ItemCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		string text = Convert.ToString(e.NewValue);
		if (!string.IsNullOrWhiteSpace(text))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item != null)
			{
				_qntUnit.DataView = new MTQntUnits(item.MTCode);
			}
		}
	}

	private void BarCode_MagicClick(object sender, MagicClickEventArgs e)
	{
		if (!_barCode.DoValidate() || string.IsNullOrWhiteSpace(_barCode.Text) || ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.GenerateBarcodeConfirm, _barCode.Text.Trim()), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
		{
			if (Settings.ParametersService.InternalBarcodeEANFormat())
			{
				_barCode.EditValue = MaterialHelpers.GenerateInnerBarcode();
			}
			else
			{
				_barCode.EditValue = Settings.ParametersService.InternalBarcodeNext();
			}
			_setNextBarCode = true;
			_barCode.SelectAll();
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (Directory.StoredInDb && !string.IsNullOrWhiteSpace(_barCodeOld) && _barCodeOld != Directory.BarCode)
		{
			dvp.Update(_barCodeOld);
		}
		dvp.Update(Directory.BarCode);
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		SetFocusOnActivation = _itemCode;
		_barCodeOld = _barCode.Text;
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_itemCode.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_qntUnit.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		Directory.BarCodeOld = (string.IsNullOrWhiteSpace(_barCodeOld) ? _barCode.Text.Trim() : _barCodeOld);
	}
}
