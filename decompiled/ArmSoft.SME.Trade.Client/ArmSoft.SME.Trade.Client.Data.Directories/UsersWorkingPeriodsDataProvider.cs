using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class UsersWorkingPeriodsDataProvider : DataProvider<UsersWorkingPeriodsDataProvider.DataRow, UsersWorkingPeriodsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int UserID { get; set; }

		public string UserName { get; set; } = string.Empty;

		public string UserDescription { get; set; } = string.Empty;

		public bool UserUseCurrDate { get; set; }

		public DateTime? PeriodStart { get; set; }

		public DateTime? PeriodEnd { get; set; }
	}

	public class Param
	{
		public string UserID { get; set; }
	}

	public override string Name => "UsersWorkingPeriods";

	public UsersWorkingPeriodsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
