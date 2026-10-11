using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class CashDataProvider : DataProvider<CashDataProvider.DataRow, CashDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public bool IsDefault { get; set; }

		public string NextNumIn { get; set; }

		public string NextNumOut { get; set; }

		public string NextMTBillNum { get; set; }

		public string NextMTBillReturnNum { get; set; }

		public string NextCashierShiftNum { get; set; }

		public bool IsECR { get; set; }

		public string CashSummLimitType { get; set; } = string.Empty;

		public byte[] TS { get; set; }

		public DataRow(string code, string caption, bool isDefault, string nextNumIn, string nextNumOut, string nextMTBillNum, string nextMTBillReturnNum, string nextCashierShiftNum, bool isECR, string cashSummLimitType, byte[] ts)
		{
			Code = code;
			Caption = caption;
			IsDefault = isDefault;
			NextNumIn = nextNumIn;
			NextNumOut = nextNumOut;
			NextMTBillNum = nextMTBillNum;
			NextMTBillReturnNum = nextMTBillReturnNum;
			NextCashierShiftNum = nextCashierShiftNum;
			IsECR = isECR;
			CashSummLimitType = cashSummLimitType;
			TS = ts;
		}

		public DataRow()
		{
		}
	}

	public class Param
	{
		public bool WithoutCheckAccess { get; set; }

		public bool ShowOnlyECRCashes { get; set; }
	}

	public override string Name => "Cash";

	public CashDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Common.Resources.Cashes
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Code",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "Code"
			},
			new Column
			{
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "NextNumIn",
				Caption = ArmSoft.SME.Common.Resources.DocNextCashInNumbers,
				Source = "NextNumIn"
			},
			new Column
			{
				Name = "NextNumOut",
				Caption = ArmSoft.SME.Common.Resources.DocNextCashOutNumbers,
				Source = "NextNumOut"
			},
			new Column
			{
				Name = "NextMTBillNum",
				Caption = ArmSoft.SME.Trade.Common.Resources.DocNextMTBillNumber,
				Source = "NextMTBillNum"
			},
			new Column
			{
				Name = "NextMTBillReturnNum",
				Caption = ArmSoft.SME.Trade.Common.Resources.DocNextMTBillReturnNumber,
				Source = "NextMTBillReturnNum"
			},
			new Column
			{
				Name = "NextCashierShiftNum",
				Caption = ArmSoft.SME.Trade.Common.Resources.CashierShiftNextNum,
				Source = "NextCashierShiftNum"
			},
			new Column
			{
				Name = "IsDefault",
				Caption = ArmSoft.SME.Common.Resources.Default,
				Source = "IsDefault",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "IsECR",
				Caption = ArmSoft.SME.Common.Resources.ECR,
				Source = "IsECR",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "CashSummLimitType",
				Caption = ArmSoft.SME.Common.Resources.CashLimit,
				Source = "CashSummLimitType"
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
			TradeSettings.TradeCacheService.Cash.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		if (param.WithoutCheckAccess)
		{
			FillWithoutCheckAccess(dataProviderResult);
		}
		else if (ArmSoft.SME.Core.UI.Settings.IsOffline)
		{
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(ArmSoft.SME.Core.UI.Settings.ParametersService.DefaultCash());
			DataRow item2 = new DataRow(item.Code, item.Caption, item.IsDefault, item.NextNumIn, item.NextNumOut, item.NextMTBillNum, item.NextMTBillReturnNum, item.NextCashierShiftNum, item.IsECR, item.CashSummLimitType, item.TS);
			dataProviderResult.Rows.Add(item2);
		}
		else
		{
			string availableCashesDelimited = TradeSettings.CreateTradeClient().CommonServices.GetAvailableCashesDelimited();
			if (string.IsNullOrWhiteSpace(availableCashesDelimited))
			{
				FillWithoutCheckAccess(dataProviderResult);
			}
			else
			{
				string[] array = availableCashesDelimited.Split(',');
				foreach (string key in array)
				{
					CashCacheRow item3 = TradeSettings.TradeCacheService.Cash.GetItem(key);
					if (item3 != null)
					{
						DataRow item4 = new DataRow(item3.Code, item3.Caption, item3.IsDefault, item3.NextNumIn, item3.NextNumOut, item3.NextMTBillNum, item3.NextMTBillReturnNum, item3.NextCashierShiftNum, item3.IsECR, item3.CashSummLimitType, item3.TS);
						dataProviderResult.Rows.Add(item4);
					}
				}
			}
		}
		_ = param.ShowOnlyECRCashes;
		if (param.ShowOnlyECRCashes)
		{
			dataProviderResult.Rows.RemoveAll((DataRow x) => !x.IsECR);
		}
		dataProviderResult.Rows = (from x in dataProviderResult.Rows
			orderby x.IsDefault descending, x.Code
			select x).ToList();
		return dataProviderResult;
	}

	private static void FillWithoutCheckAccess(DataProviderResult<DataRow> result)
	{
		foreach (CashCacheRow item2 in TradeSettings.TradeCacheService.Cash.GetList())
		{
			DataRow item = new DataRow(item2.Code, item2.Caption, item2.IsDefault, item2.NextNumIn, item2.NextNumOut, item2.NextMTBillNum, item2.NextMTBillReturnNum, item2.NextCashierShiftNum, item2.IsECR, item2.CashSummLimitType, item2.TS);
			result.Rows.Add(item);
		}
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
