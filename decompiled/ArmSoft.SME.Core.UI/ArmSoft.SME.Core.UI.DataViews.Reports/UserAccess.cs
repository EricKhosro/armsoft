using System.Collections.Generic;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViewsData;
using ArmSoft.SME.Core.UI.Navigation;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

public class UserAccess : DataView<UserAccessDataProvider.DataRow, UserAccessDataProvider.Param>
{
	protected readonly int? _localUserId;

	private readonly string _userName;

	public int UserId { get; set; }

	public Dictionary<short, AsNavigationTreeNode> AllNavigationTreeNodes { get; set; }

	public override string HelpId => "AdminTools_Users";

	public override string Caption => string.Format(Resources.UserAccessHeader, _userName);

	public UserAccess(string userName, int? localUserId)
	{
		DataProvider = new UserAccessDataProvider();
		FillColumnsFromDataProvider();
		_userName = userName;
		_localUserId = localUserId;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("AccessID", value: false);
		dvp.Set_ColumnVisible("SubSystem", value: false);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.UserId = UserId;
		ParametersProvider.AllNavigationTreeNodes = AllNavigationTreeNodes;
	}

	public override void InitContextFunctions()
	{
		UserAccessDataProvider.DataRow dataRow = (UserAccessDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			if (dataRow.IsViewChangeable)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "EditUserViewAccess", Resources.EditUserAccessView, EditUserViewAccess);
			}
			if (dataRow.IsOperChangeable)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "EditUserOperAccess", Resources.EditUserAccessOper, EditUserOperAccess);
			}
		}
	}

	private void EditUserViewAccess()
	{
		EditUserAccessForFocusedOrSelectedRows();
	}

	private void EditUserOperAccess()
	{
		EditUserAccessForFocusedOrSelectedRows(editOper: true);
	}

	private void EditUserAccessForFocusedOrSelectedRows(bool editOper = false)
	{
		foreach (UserAccessDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<UserAccessDataProvider.DataRow>())
		{
			if ((editOper && selectedOrFocusedRow.IsOperChangeable) || (!editOper && selectedOrFocusedRow.IsViewChangeable))
			{
				EditUserAccess(selectedOrFocusedRow, editOper);
			}
		}
	}

	private void EditUserAccess(UserAccessDataProvider.DataRow row, bool editOper)
	{
		UserAccessDataProvider.DataRow.UserAccessRowKey userAccessRowKey = default;
		FillUserAccessTable();
		UserAccessRow user = new UserAccessRow
		{
			AccessID = row.AccessID,
			SubSystem = row.SubSystem,
			View = (editOper ? row.View : (!row.View)),
			Operation = (editOper ? (!row.Operation) : row.Operation)
		};
		Settings.CreateApiClient().UserAccessRoutes.EditUserAccess(_localUserId ?? UserId, user);
		userAccessRowKey.AccessID = row.AccessID;
		userAccessRowKey.SubSystem = row.SubSystem;
		Settings.HostingEnvironment.CurrentViewPanel.Update(userAccessRowKey);
	}

	protected virtual void FillUserAccessTable()
	{
		if (Settings.CreateApiClient().UserAccessRoutes.UserAccessHasBeenModified(_localUserId ?? UserId))
		{
			return;
		}
		Dictionary<short, AsNavigationTreeNode> accessTree = new UserAccessManagerBase(checkAccess: false).AccessTree;
		List<UserAccessRow> accessRows = new List<UserAccessRow>();
		foreach (KeyValuePair<short, AsNavigationTreeNode> item in accessTree)
		{
			Fill(item.Key, item.Value, ref accessRows);
		}
		Settings.CreateApiClient().UserAccessRoutes.AddUserAccesses(accessRows, _localUserId ?? UserId);
	}

	protected static void Fill(short subsystem, AsNavigationTreeNode node, ref List<UserAccessRow> accessRows)
	{
		accessRows.Add(new UserAccessRow
		{
			AccessID = node.Command,
			SubSystem = subsystem,
			View = true,
			Operation = true
		});
		foreach (AsNavigationTreeNode children in node.ChildrenList)
		{
			Fill(subsystem, children, ref accessRows);
		}
	}
}
