using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.b2b.V2;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class OrganizationsDataProvider : DataProvider<OrganizationsDataProvider.DataRow, OrganizationsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string TaxCode { get; set; }

		public string FullCaption { get; set; }

		public string Caption { get; set; }

		public string Address { get; set; }

		public string BusinessAddress { get; set; }

		public string ManagerPost { get; set; }

		public string ManagerFIO { get; set; }

		public string ChiefaccPost { get; set; }

		public string ChiefaccFIO { get; set; }
	}

	public class Param
	{
		public string OrgName { get; set; }
	}

	public override string Name => "B2BPartners";

	public OrganizationsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.Partners
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "TaxCode",
				Caption = Resources.OptionsTaxCode,
				Source = "TaxCode"
			},
			new Column
			{
				Name = "FullCaption",
				Caption = Resources.FullCaption,
				Source = "FullCaption"
			},
			new Column
			{
				Name = "Caption",
				Caption = Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "Address",
				Caption = Resources.Address,
				Source = "Address"
			},
			new Column
			{
				Name = "BusinessAddress",
				Caption = Resources.BusinessAddress,
				Source = "BusinessAddress"
			},
			new Column
			{
				Name = "ManagerPost",
				Caption = Resources.ManagerPost,
				Source = "ManagerPost"
			},
			new Column
			{
				Name = "ManagerFIO",
				Caption = Resources.ManagerFIO,
				Source = "ManagerFIO"
			},
			new Column
			{
				Name = "ChiefaccPost",
				Caption = Resources.AccountantPost,
				Source = "ChiefaccPost"
			},
			new Column
			{
				Name = "ChiefaccFIO",
				Caption = Resources.ChAccFIO,
				Source = "ChiefaccFIO"
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
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		using B2BClient b2BClient = ArmSoft.SME.Core.UI.Settings.B2BClient;
		foreach (Organization organization in b2BClient.BusinessUnits.GetOrganizations(new OrganizationRequest
		{
			Name = param.OrgName,
			IncludeAddresses = true
		}))
		{
			DataRow dataRow = new DataRow
			{
				TaxCode = organization.TaxCode,
				FullCaption = organization.FullName + (string.IsNullOrWhiteSpace(organization.LegalStatusShort) ? string.Empty : (" " + organization.LegalStatusShort)),
				Address = organization.LegalAddress.FullPath,
				BusinessAddress = (string.IsNullOrWhiteSpace(organization.BusinessAddress.FullPath) ? organization.LegalAddress.FullPath : organization.BusinessAddress.FullPath),
				ManagerPost = organization.Manager.Position,
				ManagerFIO = organization.Manager.Name,
				ChiefaccPost = organization.Accountant.Position,
				ChiefaccFIO = organization.Accountant.Name
			};
			dataRow.Caption = dataRow.FullCaption.Left(50);
			dataProviderResult.Rows.Add(dataRow);
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
