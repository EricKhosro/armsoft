using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(28)]
public class StorageDialogWindow : DirectoryDialogWindow<Storage>
{
	private TextEditExt _name;

	private DropDownCommentControl _response;

	private TextEditExt _address;

	private CheckEditExt _isRetail;

	public StorageDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public StorageDialogWindow(Storage directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		_name = AddStringControl(Definition.Fields["Name"]);
		_response = AddDropDownCommentControl(Definition.Fields["Response"], new Employees());
		_address = AddStringControl(Definition.Fields["Address"]);
		if (Settings.ParametersService.RetailAccounting())
		{
			_isRetail = AddBooleanControl(Definition.Fields["IsRetail"]);
			TextBlock element = new TextBlock
			{
				Text = ArmSoft.SME.Trade.Common.Resources.StorageWarning,
				Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326"),
				TextWrapping = TextWrapping.Wrap,
				FontWeight = FontWeights.Thin
			};
			DXImage element2 = new DXImage
			{
				Width = 16.0,
				Height = 16.0,
				Source = ImageService.GetImage("warning_circle_16.svg"),
				VerticalAlignment = VerticalAlignment.Top
			};
			LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
			layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Left;
			layoutGroupExt.Width = 390.0;
			layoutGroupExt.Children.Add(element2);
			layoutGroupExt.Children.Add(element);
		}
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _name;
		}
		base.ConstructUI();
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
		if (Settings.ParametersService.RetailAccounting())
		{
			_isRetail.IsReadOnly = Mode == DialogWindowMode.EditMode && Directory.StorageExistsInOtherDocs;
		}
	}
}
