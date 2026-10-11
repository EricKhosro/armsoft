using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using Ardalis.SmartEnum;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.DataViewsData;

public class ApiKeysDataProvider : DataProvider<ApiKeysDataProvider.DataRow, ApiKeysDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int Id { get; set; }

		public short ApiKeyType { get; set; }

		public string ApiKeyTypeName { get; set; }

		public string Description { get; set; }

		public string Secret { get; set; } = "";

		public int UserId { get; set; }

		public string UserName { get; set; } = "";

		public bool IsEnabled { get; set; }
	}

	public class Param
	{
	}

	public override string Name => "ApiKeys";

	public ApiKeysDataProvider(ApiClient client)
		: base(client)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = DataProviderTypeSE.ApiKeys.Caption
		};
		int num = 5;
		List<Column> list = new List<Column>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Column> span = CollectionsMarshal.AsSpan(list);
		span[0] = new Column
		{
			Name = "ApiKeyTypeName",
			Caption = Resources.TypeWord,
			Source = "ApiKeyTypeName"
		};
		span[1] = new Column
		{
			Name = "Description",
			Caption = Resources.Description,
			Source = "Description"
		};
		span[2] = new Column
		{
			Name = "UserName",
			Caption = Resources.UserName,
			Source = "UserName"
		};
		span[3] = new Column
		{
			Name = "Secret",
			Caption = Resources.ApiKey,
			Source = "Secret"
		};
		span[4] = new Column
		{
			Name = "IsEnabled",
			Caption = Resources.TemplateEnabled,
			Type = SupportedType.Boolean,
			Source = "IsEnabled"
		};
		schema.Columns = list;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		try
		{
			using (CloudClient cloudClient = ArmSoft.SME.Core.UI.Settings.CloudClient)
			{
				cloudClient.Token = ArmSoft.SME.Core.UI.Settings.GetApplicationToken();
				foreach (CloudPublicApiKeyInfo cloudPublicApiKey in cloudClient.AS7X.GetCloudPublicApiKeys(ArmSoft.SME.Core.UI.Settings.LoginService.DatabaseId))
				{
					if (SmartEnum<ApiKeyTypesSE, short>.TryFromValue((short)cloudPublicApiKey.ApiType, out var result))
					{
						dataProviderResult.Rows.Add(new DataRow
						{
							Id = cloudPublicApiKey.Id,
							ApiKeyType = result.Value,
							ApiKeyTypeName = result.Caption,
							Description = cloudPublicApiKey.Description,
							Secret = cloudPublicApiKey.SecretHint,
							UserId = cloudPublicApiKey.UserID,
							UserName = cloudPublicApiKey.UserName,
							IsEnabled = cloudPublicApiKey.IsEnabled
						});
					}
				}
			}
			return dataProviderResult;
		}
		catch (RESTException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new ASException(Resources.AccessDenied, ex);
		}
	}
}
