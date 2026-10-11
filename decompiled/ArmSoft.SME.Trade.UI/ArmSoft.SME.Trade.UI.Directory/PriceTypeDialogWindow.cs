using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(7)]
public class PriceTypeDialogWindow : DirectoryDialogWindow<PriceType>
{
	private DropDownControl _curr;

	private DropDownControl _roundDegree;

	private CheckEditExt _isWithInVAT;

	public PriceTypeDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public PriceTypeDialogWindow(PriceType directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		AddStringControl(Definition.Fields["Name"]);
		_curr = AddDropDownControl(Definition.Fields["Curr"], new ArmSoft.SME.Core.UI.DropDowns.Currency(shortMode: true));
		_roundDegree = AddSmartEnumDropDownControl(Definition.Fields["RoundDegree"]);
		_isWithInVAT = AddBooleanControl(Definition.Fields["IsWithInVAT"]);
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.NewMode)
		{
			_curr.Code = "AMD";
			_roundDegree.Code = RoundDegreesSE.Round10.Value;
			_isWithInVAT.IsChecked = true;
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_curr.IsReadOnly = Directory.PriceTypeExistenceIntoContracts || Directory.PriceTypeExistenceIntoPrices;
	}
}
