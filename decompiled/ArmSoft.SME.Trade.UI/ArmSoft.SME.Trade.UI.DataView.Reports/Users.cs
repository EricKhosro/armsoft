using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews.Directories;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Navigation;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DataViews.Reports;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.Navigation;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

[DataView(12)]
public class Users : ArmSoft.SME.Core.UI.DataViews.Directories.Users
{
	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowUserAccess", ArmSoft.SME.Common.Resources.EditAccessMenuList, ShowUserAccess);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowUserCashesAccess", ArmSoft.SME.Trade.Common.Resources.CashesAccess, ShowUserCashesAccess);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowUserStorageOptions", ArmSoft.SME.Trade.Common.Resources.StorageOptions, ShowUserStorageOptions);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowDocOptions", ArmSoft.SME.Common.Resources.DocAdditionalOptions, ShowDocOptions, "", "", storeOpContext: true);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowFavoriteGoods", ArmSoft.SME.Trade.Common.Resources.MTFavorites, ShowFavoriteGoods, "favorite_16.svg");
			Settings.HostingEnvironment.CurrentViewPanel.AddContextSeparator("ContextFunctions");
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CopyUserAccessInfoFromOtherUser", ArmSoft.SME.Common.Resources.CopyUserAccessInfo, CopyUserAccessInfoFromOtherUser);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextSeparator("ContextFunctions");
		}
	}

	public override void Add()
	{
		new UserWindow(null, DialogWindowMode.NewMode, Settings.HostingEnvironment).ShowDialog();
	}

	public override void Edit()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			new UserWindow((UsersDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow(), DialogWindowMode.EditMode, Settings.HostingEnvironment).ShowDialog();
		}
	}

	public override void View()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			new UserWindow((UsersDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow(), DialogWindowMode.ViewMode, Settings.HostingEnvironment).ShowDialog();
		}
	}

	private void ShowUserAccess()
	{
		UsersDataProvider.DataRow dataRow = (UsersDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			if (dataRow.Admin)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.AdminHaveAllAllow, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			string userName = (string.IsNullOrWhiteSpace(dataRow.Caption) ? dataRow.Name : dataRow.Caption.Trim());
			ArmSoft.SME.Core.UI.DataViews.Directories.Users.CheckForAvailableSubsystems(dataRow.UserId, userName);
			Dictionary<short, AsNavigationTreeNode> accessTree = new UserAccessManager(checkAccess: false).AccessTree;
			UserAccess userAccess = new UserAccess(userName, dataRow.LocalUserId);
			userAccess.UserId = dataRow.UserId;
			userAccess.AllNavigationTreeNodes = accessTree;
			userAccess.Show();
		}
	}

	private void ShowUserCashesAccess()
	{
		UsersDataProvider.DataRow dataRow = (UsersDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			string userName = (string.IsNullOrWhiteSpace(dataRow.Caption) ? dataRow.Name : dataRow.Caption.Trim());
			ArmSoft.SME.Core.UI.DataViews.Directories.Users.CheckForAvailableSubsystems(dataRow.UserId, userName);
			new UserCashesAccess(dataRow.LocalUserId ?? dataRow.UserId, userName).Show();
		}
	}

	private void ShowUserStorageOptions()
	{
		UsersDataProvider.DataRow dataRow = (UsersDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			string userName = (string.IsNullOrWhiteSpace(dataRow.Caption) ? dataRow.Name : dataRow.Caption.Trim());
			ArmSoft.SME.Core.UI.DataViews.Directories.Users.CheckForAvailableSubsystems(dataRow.UserId, userName);
			new UserStorageOptions(dataRow.LocalUserId ?? dataRow.UserId, userName).Show();
		}
	}

	private void ShowDocOptions()
	{
		UsersDataProvider.DataRow dataRow = (UsersDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			string userName = (string.IsNullOrWhiteSpace(dataRow.Caption) ? dataRow.Name : dataRow.Caption.Trim());
			ArmSoft.SME.Core.UI.DataViews.Directories.Users.CheckForAvailableSubsystems(dataRow.UserId, userName);
			new DocDefaultValues(userName, dataRow.LocalUserId ?? dataRow.UserId, dataRow.Admin).ShowDialog();
		}
	}

	private void ShowFavoriteGoods()
	{
		UsersDataProvider.DataRow dataRow = (UsersDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			string userName = (string.IsNullOrWhiteSpace(dataRow.Caption) ? dataRow.Name : dataRow.Caption.Trim());
			ArmSoft.SME.Core.UI.DataViews.Directories.Users.CheckForAvailableSubsystems(dataRow.UserId, userName);
			MTFavorites mTFavorites = new MTFavorites(dataRow.LocalUserId ?? dataRow.UserId);
			mTFavorites.UserId = dataRow.LocalUserId ?? dataRow.UserId;
			mTFavorites.UserName = userName;
			mTFavorites.Show();
		}
	}

	private void CopyUserAccessInfoFromOtherUser()
	{
		List<UsersDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<UsersDataProvider.DataRow>();
		List<int> list = new List<int>();
		if (selectedOrFocusedRows == null)
		{
			return;
		}
		foreach (UsersDataProvider.DataRow item in selectedOrFocusedRows)
		{
			string userName = (string.IsNullOrWhiteSpace(item.Caption) ? item.Name : item.Caption.Trim());
			ArmSoft.SME.Core.UI.DataViews.Directories.Users.CheckForAvailableSubsystems(item.UserId, userName);
			list.Add(item.LocalUserId ?? item.UserId);
		}
		new CopyUserAccessAndOptionsDialogWindow(list).ShowDialog();
	}
}
