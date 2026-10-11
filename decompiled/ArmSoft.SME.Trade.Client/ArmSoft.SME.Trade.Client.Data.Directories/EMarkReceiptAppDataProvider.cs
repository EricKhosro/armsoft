using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class EMarkReceiptAppDataProvider : DataProvider<EMarkReceiptAppDataProvider.DataRow, EMarkReceiptAppDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithKey
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public string CRN { get; set; }

		public bool Enabled { get; set; }

		public Guid Id { get; set; }

		public bool Matched(object key)
		{
			return (string)key == Code;
		}

		public object GetKey()
		{
			return Code;
		}

		public string GetCode()
		{
			return Code;
		}

		public string GetName()
		{
			return Name;
		}
	}

	public class Param
	{
	}

	public override string Name => "EMarkReceiptApp";

	public EMarkReceiptAppDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
