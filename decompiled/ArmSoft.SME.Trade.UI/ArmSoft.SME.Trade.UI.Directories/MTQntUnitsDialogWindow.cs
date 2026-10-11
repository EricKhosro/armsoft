using System.Windows;
using System.Windows.Media;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(20)]
public class MTQntUnitsDialogWindow : DirectoryDialogWindow<MTQntUnit>
{
	private DropDownControl _mtCode;

	private DropDownControl _qntUnit;

	private PopupCalcEditExt _coef;

	private ImageEdit _myImageEdit;

	public MTQntUnitsDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public MTQntUnitsDialogWindow(MTQntUnit directory, DialogWindowMode mode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_mtCode = AddDropDownControl(Definition.Fields["MTCode"], new GoodsFilter());
		_qntUnit = AddDropDownControl(Definition.Fields["Code"], new ArmSoft.SME.Trade.UI.DropDown.QntUnits());
		_coef = AddNumericControl(Definition.Fields["Coef"]);
		_myImageEdit = new ImageEdit
		{
			Name = "Image",
			ShowMenu = true,
			ShowMenuMode = ShowMenuMode.Hover,
			ShowLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Always,
			IsTabStop = false,
			Height = 120.0,
			Width = 120.0,
			Stretch = Stretch.Uniform
		};
		_myImageEdit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ImageEditValueCahnged).Handler;
		AddControl("Image", ArmSoft.SME.Trade.Common.Resources.Picture, _myImageEdit);
	}

	private void ImageEditValueCahnged(object sender, EditValueChangedEventArgs e)
	{
		CommonEventHandlers.ConvertImageEditValueChanged(sender, new ImageEditValueChangedEventArgs(e));
		Directory.ImageEdited = true;
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_mtCode.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_qntUnit.IsReadOnly = Mode != DialogWindowMode.NewMode;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(Directory.MTCode);
		if (item != null)
		{
			MTQntUnitsDataProvider.DataRow.MTQntUnitsRowKey key = new MTQntUnitsDataProvider.DataRow.MTQntUnitsRowKey
			{
				MTID = item.Id,
				QntUnit = Directory.Code
			};
			dvp.Update(key);
		}
	}
}
