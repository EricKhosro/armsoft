using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(41)]
public class UserCashesAccessDialogWindow : DirectoryDialogWindow<UserCashesAccess>
{
	private CheckEditExt _allow;

	private DropDownControl _dropDown;

	public UserCashesAccessDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public UserCashesAccessDialogWindow(UserCashesAccess directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		UserCashesAccessDataProvider.DataRow dataRow = (UserCashesAccessDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Caption = dataRow.Caption;
		Mode = Mode;
		HelpId = HelpId;
		_dropDown = AddDropDownControl(Definition.Fields["Code"], new Cashes(withoutCheckAccess: true), allowAdditionButtons: true);
		_dropDown.IsReadOnly = true;
		_allow = AddBooleanControl(Definition.Fields["Allow"]);
		SetFocusOnActivation = _allow;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		UserCashesAccessDataProvider.DataRow dataRow = (UserCashesAccessDataProvider.DataRow)dvp.FocusedRow();
		UserCashesAccessDataProvider.DataRow.UserCashesAccessRowKey key = new UserCashesAccessDataProvider.DataRow.UserCashesAccessRowKey
		{
			CashDesk = dataRow.CashDesk,
			UserId = dataRow.UserId
		};
		dvp.Update(key);
	}
}
