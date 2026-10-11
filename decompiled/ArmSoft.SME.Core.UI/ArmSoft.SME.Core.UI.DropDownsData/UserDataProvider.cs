using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DropDownsData;

public class UserDataProvider : DataProvider<UserDataProvider.DataRow, UserDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public bool Login { get; set; }

		public bool Admin { get; set; }

		public int? LocalUserId { get; set; }

		public int CloudUserId { get; set; }

		public int UserId => LocalUserId ?? CloudUserId;
	}

	public class Param
	{
		public bool OnlyDbUsers { get; set; }
	}

	public override string Name => "User";

	public UserDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.User
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Name",
				Caption = Resources.UserNameWord,
				Source = "Name"
			},
			new Column
			{
				Name = "Description",
				Caption = Resources.UserDescription,
				Source = "Description"
			},
			new Column
			{
				Name = "UserId",
				Caption = Resources.Code,
				Source = "UserId"
			},
			new Column
			{
				Name = "Login",
				Caption = Resources.UserLoginEnable,
				Source = "Login",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "Admin",
				Caption = Resources.UserIsAdmin,
				Source = "Admin",
				Type = SupportedType.Boolean
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override Task<Schema> GetSchemaAsync(Param param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		if (settings != null && settings.ReloadData)
		{
			ArmSoft.SME.Core.UI.Settings.CoreCacheService.User.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		IReadOnlyList<UsersCacheRow> list = ArmSoft.SME.Core.UI.Settings.CoreCacheService.User.GetList();
		List<int> list2 = null;
		if (param.OnlyDbUsers)
		{
			list2 = ArmSoft.SME.Core.UI.Settings.CommonCoreService.GetDatabaseUsers();
		}
		foreach (UsersCacheRow item2 in list)
		{
			if (!param.OnlyDbUsers || (list2.Contains(item2.UserId) && item2.Login))
			{
				DataRow item = new DataRow
				{
					Name = item2.Name,
					Description = item2.Description,
					Admin = item2.Admin,
					CloudUserId = item2.CloudUserId,
					LocalUserId = item2.LocalUserId,
					Login = item2.Login
				};
				dataProviderResult.Rows.Add(item);
			}
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.Name).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
