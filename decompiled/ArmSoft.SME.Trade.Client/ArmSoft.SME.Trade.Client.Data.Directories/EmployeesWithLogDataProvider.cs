using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class EmployeesWithLogDataProvider : DataProvider<EmployeesWithLogDataProvider.DataRow, EmployeesWithLogDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public int EmployeeID { get; set; }

		public string EmployeeCode { get; set; }

		public string Caption { get; set; }

		public string Address { get; set; }

		public string Phone { get; set; }

		public string Passport { get; set; }

		public string EmployeeCard { get; set; }

		public bool IsSalesConsultant { get; set; }

		public string UserName { get; set; }

		public DateTime? ChangeDate { get; set; }

		public DateTime? CreationDate { get; set; }

		public object GetKey()
		{
			return EmployeeID;
		}

		public string GetCode()
		{
			return EmployeeCode;
		}

		public string GetName()
		{
			return Caption;
		}

		public bool Matched(object key)
		{
			return EmployeeID == (int)key;
		}
	}

	public class Param
	{
		public string EmployeeCode { get; set; }

		public bool ShowExtended { get; set; }
	}

	public override string Name => "EmployeesWithLog";

	public EmployeesWithLogDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
