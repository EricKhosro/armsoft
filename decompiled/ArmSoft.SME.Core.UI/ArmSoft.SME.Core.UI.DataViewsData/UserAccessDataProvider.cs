using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Core.UI.DataViews.Directories;
using ArmSoft.SME.Core.UI.Navigation;

namespace ArmSoft.SME.Core.UI.DataViewsData;

public class UserAccessDataProvider : DataProvider<UserAccessDataProvider.DataRow, UserAccessDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public struct UserAccessRowKey
		{
			public short SubSystem;

			public short AccessID;
		}

		public short SubSystem { get; set; }

		public short AccessID { get; set; }

		public bool View { get; set; }

		public bool Operation { get; set; }

		public bool IsViewChangeable { get; set; }

		public bool IsOperChangeable { get; set; }

		public string Menu { get; set; }

		public bool Matched(object key)
		{
			UserAccessRowKey userAccessRowKey = (UserAccessRowKey)key;
			if (userAccessRowKey.SubSystem == SubSystem)
			{
				return userAccessRowKey.AccessID == AccessID;
			}
			return false;
		}
	}

	public class Param
	{
		public int UserId { get; set; } = -1;

		public Dictionary<short, AsNavigationTreeNode> AllNavigationTreeNodes { get; set; }
	}

	private readonly DataProviderResult<DataRow> _result = new DataProviderResult<DataRow>();

	private List<NavigatorNodeAccess> _userAccesses;

	public override string Name => "UserAccess";

	public UserAccessDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.UserAccessHeader
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Menu",
				Caption = Resources.UserAccessMenu,
				Source = "Menu"
			},
			new Column
			{
				Name = "AccessID",
				Caption = "AccessID",
				Source = "AccessID"
			},
			new Column
			{
				Name = "SubSystem",
				Caption = Resources.SubSystem,
				Source = "SubSystem"
			},
			new Column
			{
				Name = "View",
				Caption = Resources.UserAccessView,
				Source = "View",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "Operation",
				Caption = Resources.UserAccessOper,
				Source = "Operation",
				Type = SupportedType.Boolean
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		_result.Rows = new List<DataRow>();
		List<short> availableSubsystems = Users.GetAvailableSubsystems(param.UserId, ArmSoft.SME.Core.UI.Settings.LoginService.DatabaseId).ToList();
		_userAccesses = ArmSoft.SME.Core.UI.Settings.CreateApiClient().UserAccessRoutes.GetUserAvailableAccessTree(param.UserId, string.Join(",", availableSubsystems));
		foreach (KeyValuePair<short, AsNavigationTreeNode> item in param.AllNavigationTreeNodes.Where((KeyValuePair<short, AsNavigationTreeNode> kv) => availableSubsystems.Contains(kv.Key)))
		{
			Fill(item.Key, item.Value, string.Empty);
		}
		if (keys != null)
		{
			if (keys.Count > 1)
			{
				throw new ASException(Resources.NotSupportSeveralKeys);
			}
			DataRow.UserAccessRowKey keyPair = (DataRow.UserAccessRowKey)keys[0];
			return new DataProviderResult<DataRow>
			{
				Rows = { _result.Rows.FirstOrDefault((DataRow x) => x.SubSystem == keyPair.SubSystem && x.AccessID == keyPair.AccessID) }
			};
		}
		return _result;
	}

	private void Fill(short subsystem, AsNavigationTreeNode node, string prefix)
	{
		NavigatorNodeAccess navigatorNodeAccess = _userAccesses.FirstOrDefault((NavigatorNodeAccess x) => x.SubsystemId == subsystem && x.NodeAccessId == node.Command);
		DataRow item = new DataRow
		{
			SubSystem = subsystem,
			AccessID = node.Command,
			Menu = prefix + node.Header,
			View = (navigatorNodeAccess?.AllowView ?? true),
			Operation = (navigatorNodeAccess?.AllowOperation ?? true),
			IsOperChangeable = node.IsOperChangeable,
			IsViewChangeable = node.IsViewChangeable
		};
		_result.Rows.Add(item);
		foreach (AsNavigationTreeNode children in node.ChildrenList)
		{
			Fill(subsystem, children, prefix + new string(' ', 4));
		}
	}
}
