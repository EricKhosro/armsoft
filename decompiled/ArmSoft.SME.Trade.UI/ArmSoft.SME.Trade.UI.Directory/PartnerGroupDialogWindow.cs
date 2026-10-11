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

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(31)]
public class PartnerGroupDialogWindow : DirectoryDialogWindow<PartnerGroup>
{
	private TextEditExt _name;

	private DropDownControl _parent;

	private string _parentOld;

	private string _codeOld;

	public PartnerGroupDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public PartnerGroupDialogWindow(PartnerGroup directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
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
		_parent = AddDropDownControl(Definition.Fields["Parent"], new PartnerGroups());
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _name;
		}
		base.ConstructUI();
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode)
		{
			_parentOld = Directory.Parent;
			_codeOld = Directory.Code;
			string code = "";
			if (!string.IsNullOrWhiteSpace(Directory.Parent))
			{
				PartnerGroupCacheRow item = TradeSettings.TradeCacheService.PartnerGroup.GetItem(Directory.Parent);
				if (item != null)
				{
					code = item.Code;
				}
			}
			_parent.Code = code;
		}
		else
		{
			_parent.Code = Directory.Parent;
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
			if (Directory.StoredInDb)
			{
				IReadOnlyList<PartnerCacheRow> partnersList = TradeSettings.TradeCacheService.Partner.GetPartnersList(Directory.Code);
				if (partnersList != null && partnersList.Count != 0 && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.PartnerBelongToPartGroup, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
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
		}
		else
		{
			dvp.Refresh();
		}
	}
}
