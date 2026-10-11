using System;
using System.Collections.Generic;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.InternetBank;

namespace ArmSoft.SME.Trade.UI.DataViewDataProvider.Reports;

public class IBBankAccountsRemainsDataProvider : DataProvider<IBBankAccountsRemainsDataProvider.DataRow, IBBankAccountsRemainsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string AccountCode { get; set; }

		public string AccountName { get; set; }

		public string Currency { get; set; }

		public decimal AvailableBalance { get; set; }

		public decimal Balance { get; set; }
	}

	public class Param
	{
		public List<string> Account { get; set; }
	}

	public override string Name => "OnlineBankAccountsRemains";

	public IBBankAccountsRemainsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = DataProviderTypeSE.IBBankAccountsRemains.Caption
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "AccountCode",
				Caption = Resources.AccountInBank,
				Source = "AccountCode"
			},
			new Column
			{
				Name = "AccountName",
				Caption = Resources.Name,
				Source = "AccountName"
			},
			new Column
			{
				Name = "Currency",
				Caption = Resources.Currency,
				Source = "Currency"
			},
			new Column
			{
				Name = "AvailableBalance",
				Caption = Resources.AccBalance,
				Source = "AvailableBalance",
				Type = SupportedType.Decimal,
				FooterSum = true,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "Balance",
				Caption = Resources.AccAvailableBalance,
				Source = "Balance",
				Type = SupportedType.Decimal,
				FooterSum = true,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		foreach (string item in param.Account)
		{
			AccountBalanceRow accountBalances = ArmSoft.SME.Core.UI.Settings.InternetBankService.GetAccountBalances(item, DateTime.Now);
			ArmSoft.InternetBank.Web.API.Common.Models.AccountDetails accountDetails = ArmSoft.SME.Core.UI.Settings.InternetBankService.GetAccountDetails(item);
			dataProviderResult.Rows.Add(new DataRow
			{
				AccountCode = accountDetails.AccountNumber,
				AccountName = accountDetails.Name,
				Currency = accountDetails.Currency,
				Balance = accountBalances.ClosingExpectedBalance.GetValueOrDefault(),
				AvailableBalance = accountBalances.ClosingBalance.GetValueOrDefault()
			});
			InternetBankHelper.LogIBOperation(ArmSoft.SME.Core.UI.Settings.ParametersService.TaxCode(), item, IBOperation.AccountBalances);
		}
		return dataProviderResult;
	}
}
