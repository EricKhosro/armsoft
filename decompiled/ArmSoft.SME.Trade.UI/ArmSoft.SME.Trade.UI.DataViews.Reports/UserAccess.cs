using System.Collections.Generic;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Navigation;
using ArmSoft.SME.Trade.UI.Navigation;

namespace ArmSoft.SME.Trade.UI.DataViews.Reports;

[DataView(13)]
public class UserAccess : ArmSoft.SME.Core.UI.DataViews.Reports.UserAccess
{
	public UserAccess(string userName, int? localUserId)
		: base(userName, localUserId)
	{
	}

	protected override void FillUserAccessTable()
	{
		if (Settings.CreateApiClient().UserAccessRoutes.UserAccessHasBeenModified(_localUserId ?? UserId))
		{
			return;
		}
		Dictionary<short, AsNavigationTreeNode> accessTree = new UserAccessManager(checkAccess: false).AccessTree;
		List<UserAccessRow> accessRows = new List<UserAccessRow>();
		foreach (KeyValuePair<short, AsNavigationTreeNode> item in accessTree)
		{
			ArmSoft.SME.Core.UI.DataViews.Reports.UserAccess.Fill(item.Key, item.Value, ref accessRows);
		}
		Settings.CreateApiClient().UserAccessRoutes.AddUserAccesses(accessRows, _localUserId ?? UserId);
	}
}
