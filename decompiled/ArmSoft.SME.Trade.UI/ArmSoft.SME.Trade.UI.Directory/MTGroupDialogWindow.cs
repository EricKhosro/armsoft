using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(19)]
public class MTGroupDialogWindow : DirectoryDialogWindow<MTGroup>
{
	private TextEditExt _name;

	private DropDownControl _parent;

	private TextEditExt _mtNameEN;

	private TextEditExt _mtNameRU;

	private string _parentOld;

	private string _codeOld;

	public MTGroupDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public MTGroupDialogWindow(MTGroup directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		_name = AddStringControl(Definition.Fields["Name"]);
		_parent = AddDropDownControl(Definition.Fields["Parent"], new GoodsGroups());
		LayoutGroupExt container = AddVerticalLayoutGroup(null, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.OnlineShop);
		_mtNameEN = AddStringControl(container, Definition.Fields["CaptionEn"]);
		_mtNameRU = AddStringControl(container, Definition.Fields["CaptionRu"]);
		base.ConstructUI();
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode != DialogWindowMode.NewMode)
		{
			SetFocusOnActivation = _name;
		}
		if (Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode)
		{
			_parentOld = Directory.Parent;
			_codeOld = Directory.Code;
		}
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		Directory.NewCode = _code.Text.Trim();
		Directory.Code = ((Mode == DialogWindowMode.NewMode || Mode == DialogWindowMode.CreateCopyMode) ? _code.Text.Trim() : _codeOld.Trim());
		Directory.Parent = ((Mode == DialogWindowMode.NewMode || Mode == DialogWindowMode.CreateCopyMode || Mode == DialogWindowMode.EditMode) ? _parent.Code.Trim() : _parentOld.Trim());
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			if (Directory.StoredInDb && _codeOld != _code.Text)
			{
				List<MaterialCacheRow> items = TradeSettings.TradeCacheService.Material.GetItems(_codeOld);
				if (items != null && items.Count != 0 && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ProductBelongToGroup, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
				{
					args.isValid = false;
					return;
				}
			}
			base.DialogValidation(args);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (_parentOld == Directory.Parent && _codeOld == Directory.NewCode)
		{
			base.UpdateDataViewPanel(dvp);
			if (Directory.Code != _codeOld)
			{
				dvp.Update(_codeOld);
			}
		}
		else
		{
			dvp.Refresh();
		}
	}
}
