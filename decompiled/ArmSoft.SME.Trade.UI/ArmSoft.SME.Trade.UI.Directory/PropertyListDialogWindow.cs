using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(26)]
public class PropertyListDialogWindow : DirectoryDialogWindow<ArmSoft.SME.Trade.Common.Models.Directories.PropertyList>
{
	private TextEditExt _name;

	private DropDownControl _destCode;

	public bool IsFilteredProperty { get; set; }

	protected override bool ViewModeIsWithNext => true;

	public PropertyListDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public PropertyListDialogWindow(ArmSoft.SME.Trade.Common.Models.Directories.PropertyList directory, DialogWindowMode mode = DialogWindowMode.NewModeWithNext, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_destCode = AddDropDownControl(Definition.Fields["DestCode"], new Properties());
		_destCode.EditValueChanged += new SafeEventHandler<RoutedEventArgs>(DestCode_EditValueChanged).Handler;
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		_name = AddStringControl(Definition.Fields["Name"]);
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_destCode.IsReadOnly = Mode == DialogWindowMode.EditMode || IsFilteredProperty;
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.NewModeWithNext)
		{
			SetFocusOnActivation = (string.IsNullOrWhiteSpace(_code.Text) ? ((TextEdit)_code) : ((TextEdit)_name));
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_name.Text = "";
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		PropertyListDataProvider.DataRow.MTPropertyListRowKey key = new PropertyListDataProvider.DataRow.MTPropertyListRowKey
		{
			DestCode = Directory.DestCode,
			Code = Directory.Code
		};
		dvp.Update(key);
	}

	protected override void SetNextCode()
	{
		GetNextPropertyNumberByDestCode();
	}

	private void DestCode_EditValueChanged(object sender, RoutedEventArgs e)
	{
		GetNextPropertyNumberByDestCode();
	}

	private void GetNextPropertyNumberByDestCode()
	{
		_code.EditValue = TradeSettings.TradeCacheService.Property.GetItem(_destCode.Code)?.NextProperty ?? string.Empty;
	}
}
