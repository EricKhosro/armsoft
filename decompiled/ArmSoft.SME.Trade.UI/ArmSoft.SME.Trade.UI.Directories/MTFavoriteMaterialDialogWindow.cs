using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(18)]
public class MTFavoriteMaterialDialogWindow : DirectoryDialogWindow<MTFavoriteMaterial>
{
	private new DropDownControl _code;

	private DropDownControl _qntUnit;

	private DropDownControl _favoriteGroup;

	private TextEditExt _hotKey;

	private string _oldCode;

	private string _oldQntUnit;

	private string _oldHotKey;

	public MTFavoriteMaterialDialogWindow(int userID, DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
		Directory.UserId = userID;
	}

	public MTFavoriteMaterialDialogWindow(MTFavoriteMaterial directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_code = AddDropDownControl(Definition.Fields["Code"], new GoodsFilter());
		_code.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTCode_EditValueChanged).Handler;
		_qntUnit = AddDropDownControl(Definition.Fields["QntUnit"], new MTQntUnits());
		_favoriteGroup = AddDropDownControl(Definition.Fields["GroupCode"], new MTFavorites(Directory.UserId, onlyMaterials: false, onlyGroups: true), allowAdditionButtons: true);
		_hotKey = AddStringControl(Definition.Fields["HotKey"]);
		_hotKey.PreviewTextInput += new SafeEventHandler<TextCompositionEventArgs>(HotKey_PreviewTextInput).Handler;
		_hotKey.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(HotKey_PreviewKeyDown).Handler;
	}

	public void MTCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		string text = Convert.ToString(e.NewValue);
		if (!string.IsNullOrWhiteSpace(text))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item != null)
			{
				_qntUnit.DataView = new MTQntUnits(item.MTCode);
				_qntUnit.Code = item.Unit;
			}
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode != DialogWindowMode.NewMode)
		{
			_qntUnit.DataView = new MTQntUnits(Directory.Code);
			_oldCode = Directory.Code;
			_oldQntUnit = Directory.QntUnit;
			_oldHotKey = Directory.HotKey;
		}
		else
		{
			SetFocusOnActivation = _code;
		}
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		if (!string.IsNullOrWhiteSpace(_oldCode))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_oldCode);
			if (item != null)
			{
				Directory.MTIDOld = item.Id;
			}
		}
		Directory.QntUnitOld = _oldQntUnit;
		Directory.HotKeyOld = _oldHotKey;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if ((_oldCode != null || _oldQntUnit != null) && (_oldCode != Directory.Code || _oldQntUnit != Directory.QntUnit))
		{
			dvp.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
			{
				MTID = TradeSettings.TradeCacheService.Material.GetItem(_oldCode).Id,
				MTUnitCode = _oldQntUnit,
				UserID = Directory.UserId
			});
		}
		dvp.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
		{
			MTID = TradeSettings.TradeCacheService.Material.GetItem(Directory.Code).Id,
			MTUnitCode = Directory.QntUnit,
			UserID = Directory.UserId
		});
	}

	private void HotKey_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		e.Handled = true;
	}

	private void HotKey_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected I4, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Key == 2)
		{
			_hotKey.Text = "";
			e.Handled = true;
		}
		string text = "";
		ModifierKeys modifiers = Keyboard.Modifiers;
		switch (modifiers - 1)
		{
		case 0:
			text = "Alt+";
			break;
		case 1:
			text = "Ctrl+";
			break;
		case 3:
			text = "Shift+";
			break;
		}
		string text2 = ((object)e.Key/*cast due to constrained. prefix*/).ToString();
		text = ((!Regex.IsMatch(text2, "^D[0-9]$")) ? (text + text2) : (text + text2.Substring(1)));
		if (Regex.IsMatch(text, "^(?!(Ctrl\\+0|Shift\\+((NumPad)[0-9])))(Ctrl\\+|Shift\\+){0,1}((NumPad){0,1}[0-9]|F([2356789]|11|12)|[A-Z])$"))
		{
			_hotKey.Text = text;
			e.Handled = true;
		}
	}
}
