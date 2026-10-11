using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.UI.CustomUIElements;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class IPAddressDataProvider : DataProvider<IPAddressDataProvider.DataRow, IPAddressDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string IP { get; set; }

		public string Port { get; set; }

		public string MacAddress { get; set; }
	}

	public class Param
	{
		public List<IPAndMacAddress> IPAddress { get; set; }
	}

	public override string Name => "IPAddress";

	public IPAddressDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.IPAddress
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "IP",
				Caption = Resources.IPAddress,
				Source = "IP"
			},
			new Column
			{
				Name = "Port",
				Caption = Resources.TCPPort,
				Source = "Port"
			},
			new Column
			{
				Name = "MacAddress",
				Caption = Resources.MacAddress,
				Source = "MacAddress"
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		foreach (IPAndMacAddress item in param.IPAddress)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				IP = item.IP,
				Port = item.Port.ToString(),
				MacAddress = item.MacAddress
			});
		}
		return dataProviderResult;
	}
}
