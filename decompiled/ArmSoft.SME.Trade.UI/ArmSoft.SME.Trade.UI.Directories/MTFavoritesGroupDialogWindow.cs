using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(13)]
public class MTFavoritesGroupDialogWindow : DirectoryDialogWindow<MTFavoritesGroup>
{
	private TextEditExt _groupCaption;

	private TextEditExt _hotKey;

	private ImageEdit _image;

	private string _oldHotKey;

	public MTFavoritesGroupDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public MTFavoritesGroupDialogWindow(int userID, DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
		Directory.UserId = userID;
	}

	public MTFavoritesGroupDialogWindow(MTFavoritesGroup directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		_groupCaption = AddStringControl(Definition.Fields["Name"]);
		_hotKey = AddStringControl(Definition.Fields["HotKey"]);
		_hotKey.PreviewTextInput += new SafeEventHandler<TextCompositionEventArgs>(HotKey_PreviewTextInput).Handler;
		_hotKey.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(HotKey_PreviewKeyDown).Handler;
		_image = new ImageEdit
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
		_image.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ImageEditValueCahnged).Handler;
		AddControl("Img", ArmSoft.SME.Trade.Common.Resources.Picture, _image);
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode != DialogWindowMode.NewMode)
		{
			_oldHotKey = Directory.HotKey;
		}
		else
		{
			SetFocusOnActivation = _code;
		}
	}

	private void ImageEditValueCahnged(object sender, EditValueChangedEventArgs e)
	{
		CommonEventHandlers.ConvertImageEditValueChanged(sender, new ImageEditValueChangedEventArgs(e));
		Directory.ImageEdited = true;
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		Directory.HotKeyOld = _oldHotKey;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		dvp.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
		{
			MTCode = Directory.Code,
			UserID = Directory.UserId,
			MTUnitCode = "",
			MTID = Directory.Id
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
