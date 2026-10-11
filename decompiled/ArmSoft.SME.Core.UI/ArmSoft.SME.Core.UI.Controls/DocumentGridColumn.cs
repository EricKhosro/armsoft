using System.Windows;
using ArmSoft.SME.Core.UI.DataViews;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridColumn : GridColumn
{
	private IDropDownView _idv;

	public bool AllowNegativeNumbers { get; set; }

	public bool AllowAdditionButtons { get; set; }

	public bool TrimValue { get; set; }

	public bool DropDownEnabled { get; set; }

	public short Precision { get; set; }

	public bool IsCommentClickDropDown { get; set; }

	public bool IsOwnProcessingClickDropDown { get; set; }

	public bool IsRaiseClickDropDownEventInReadOnlyMode { get; set; }

	public IDropDownView DataView
	{
		get
		{
			return _idv;
		}
		set
		{
			IDropDownView idv = _idv;
			_idv = value;
			if (_idv != null && (idv == null || idv == _idv))
			{
				Width = SizeCalculator.WidthForString(_idv.ValueMemberLength()) + (double)(AllowAdditionButtons ? 20 : 0);
			}
		}
	}

	public DocumentGridColumn()
	{
		AllowNegativeNumbers = false;
	}

	public ButtonInfo AddAdditionalButton(string imageSource, string toolTip = "")
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		ButtonEditSettings buttonEditSettings = (ButtonEditSettings)EditSettings;
		DXImage content = new DXImage
		{
			Source = ImageService.GetImage(imageSource),
			Width = 16.0,
			Height = 16.0,
			Margin = new Thickness(2.0, 0.0, 2.0, 0.0),
			ToolTip = toolTip
		};
		ButtonInfo buttonInfo = new ButtonInfo
		{
			GlyphKind = GlyphKind.Custom,
			Content = content
		};
		buttonEditSettings.Buttons.Add(buttonInfo);
		return buttonInfo;
	}
}
