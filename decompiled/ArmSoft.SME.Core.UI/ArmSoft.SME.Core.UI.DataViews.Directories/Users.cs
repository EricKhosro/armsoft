using System.Collections.Generic;
using System.Windows;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Authentication;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.DataViews.Directories;

public class Users : DataView<UsersDataProvider.DataRow, UsersDataProvider.Param>
{
	private readonly CustomerInfo _customerInfo;

	public override string HelpId => "AdminTools_Users";

	public override bool AllowAdd
	{
		get
		{
			if (!ConfigHelper.ASAdminLogin)
			{
				return _customerInfo != null;
			}
			return true;
		}
	}

	public override bool AllowView => true;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowDelete
	{
		get
		{
			if (!ConfigHelper.ASAdminLogin)
			{
				return _customerInfo != null;
			}
			return true;
		}
	}

	public override bool AllowCreateCopy => false;

	public Users()
	{
		DataProvider = new UsersDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
		using CloudClient cloudClient = Settings.CloudClient;
		if (!ConfigHelper.ASAdminLogin && cloudClient.AS7X.IsMeOwnerForCustomer(Settings.LoginService.CustomerId))
		{
			_customerInfo = cloudClient.AS7X.GetCustomerInfo(Settings.LoginService.CustomerId);
		}
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("UserId", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Name");
		}
	}

	public override void Delete()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		UsersDataProvider.DataRow dataRow = (UsersDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		if (dataRow.UserId == Settings.LoginService.UserId)
		{
			throw new ASException(Resources.UserCanntRemoveMyself);
		}
		if (MessageBox.Show(string.Format(Resources.ConfRecordDel, dataRow.Name), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
		{
			return;
		}
		Settings.CreateApiClient().UserRoutes.ValidateDelete(dataRow.UserId);
		Settings.CreateApiClient().UserRoutes.DeleteFromUsersMapping(dataRow.UserId);
		if (ConfigHelper.ASAdminLogin)
		{
			Settings.CreateApiClient().AdminCloud.DeleteCloudKeyMapping(ADHelper.ASAdminB2CToken, Settings.LoginService.Key, dataRow.UserId);
			Settings.CreateApiClient().UserRoutes.UpdateUsersByASAdmin(ADHelper.ASAdminB2CToken);
		}
		else
		{
			using CloudClient cloudClient = Settings.CloudClient;
			cloudClient.Token = Settings.GetApplicationToken();
			cloudClient.AS7X.DeleteUser(Settings.LoginService.Key, dataRow.UserId);
			Settings.CreateApiClient().UserRoutes.Delete(ADHelper.ADB2CToken, dataRow.UserId);
		}
		Settings.CoreCacheService.User.Refresh();
		Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.UserId);
	}

	public static List<short> CheckForAvailableSubsystems(int userId, string userName)
	{
		List<short> availableSubsystems = GetAvailableSubsystems(userId, Settings.LoginService.DatabaseId);
		if (availableSubsystems.Count == 0)
		{
			throw new ASException(string.Format(Resources.AccessForUserToAllSubsystemsDenied, userName, Settings.LoginService.Database), 64);
		}
		return availableSubsystems;
	}

	public static List<short> GetAvailableSubsystems(int userId, int dbId)
	{
		List<short> list = new List<short>();
		if (ConfigHelper.ASAdminLogin)
		{
			foreach (ArmSoft.SME.Common.Models.Authentication.Role userRole in Settings.CreateApiClient().AdminCloud.GetUserRoles(ADHelper.ASAdminB2CToken, dbId, userId))
			{
				if (userRole.Allow)
				{
					list.Add(userRole.RoleId);
				}
			}
		}
		else
		{
			using CloudClient cloudClient = Settings.CloudClient;
			foreach (ArmSoft.Cloud.Client.AS7X.Role role in cloudClient.AS7X.GetUserRoles(dbId, userId).Roles)
			{
				if (role.Allow)
				{
					list.Add(role.RoleId);
				}
			}
		}
		return list;
	}
}
