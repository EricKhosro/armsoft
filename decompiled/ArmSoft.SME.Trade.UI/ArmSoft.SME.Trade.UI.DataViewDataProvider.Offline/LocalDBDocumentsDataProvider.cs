using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataViewDataProvider.Offline;

public class LocalDBDocumentsDataProvider : DataProvider<LocalDBDocumentsDataProvider.DataRow, LocalDBDocumentsDataProvider.Param>
{
	public class DataRow : TradeDocumentDataRow
	{
		public string ECRCheckNum { get; set; }

		public int PartID { get; set; }

		public string PartCode { get; set; }

		public string PartName { get; set; }

		public int EmplID { get; set; }

		public string EmplCode { get; set; }

		public string EmplName { get; set; }

		public int CreatorID { get; set; }

		public int DocOwnerID { get; set; }

		public string DocOwner { get; set; }

		public Guid? BaseISN { get; set; }

		public string Cashdesk { get; set; }

		public string CashdeskCaption { get; set; }

		public string CashierShiftNumber { get; set; }

		public byte SyncState { get; set; }

		public string SyncStateName { get; set; }

		public string Json { get; set; }
	}

	public class Param
	{
		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public string DocState { get; set; }

		public string SyncState { get; set; }

		public string CurrCode { get; set; }

		public string CashDeskCode { get; set; }

		public string CashierShiftNumber { get; set; }

		public bool OrderByAscedning { get; set; } = true;

		public string DocType { get; set; }
	}

	public override string Name => "OfflineECRChecksJournal";

	public LocalDBDocumentsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Common.Resources.OfflineECRChecksJournal
		};
		int num = 18;
		List<Column> list = new List<Column>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Column> span = CollectionsMarshal.AsSpan(list);
		span[0] = new Column
		{
			Name = "DocDate",
			Caption = ArmSoft.SME.Common.Resources.DateWord,
			Source = "DocDate",
			Type = SupportedType.DateOnly
		};
		span[1] = new Column
		{
			Name = "DocNum",
			Caption = ArmSoft.SME.Common.Resources.DocNumber,
			Source = "DocNum",
			Type = SupportedType.String
		};
		span[2] = new Column
		{
			Name = "ECRCheckNum",
			Caption = ArmSoft.SME.Common.Resources.ECRCheckNum,
			Source = "ECRCheckNum",
			Type = SupportedType.String
		};
		span[3] = new Column
		{
			Name = "DocTypeName",
			Caption = ArmSoft.SME.Common.Resources.TypeWord,
			Source = "DocTypeName",
			Type = SupportedType.String
		};
		span[4] = new Column
		{
			Name = "SyncStateName",
			Caption = ArmSoft.SME.Common.Resources.SyncState,
			Source = "SyncStateName",
			Type = SupportedType.String
		};
		span[5] = new Column
		{
			Name = "CurrCode",
			Caption = ArmSoft.SME.Common.Resources.Curr,
			Source = "CurrCode",
			Type = SupportedType.String
		};
		span[6] = new Column
		{
			Name = "Summ",
			Caption = ArmSoft.SME.Common.Resources.Summa,
			Source = "Summ",
			Type = SupportedType.Decimal
		};
		span[7] = new Column
		{
			Name = "DocStateName",
			Caption = ArmSoft.SME.Common.Resources.State,
			Source = "DocStateName",
			Type = SupportedType.String
		};
		span[8] = new Column
		{
			Name = "Cashdesk",
			Caption = ArmSoft.SME.Common.Resources.Cash,
			Source = "Cashdesk",
			Type = SupportedType.String
		};
		span[9] = new Column
		{
			Name = "CashierShiftNumber",
			Caption = ArmSoft.SME.Trade.Common.Resources.CashierShiftNum,
			Source = "CashierShiftNumber",
			Type = SupportedType.String
		};
		span[10] = new Column
		{
			Name = "Comment",
			Caption = ArmSoft.SME.Common.Resources.Comment,
			Source = "Comment",
			Type = SupportedType.String
		};
		span[11] = new Column
		{
			Name = "PartCode",
			Caption = ArmSoft.SME.Common.Resources.Partner,
			Source = "PartCode",
			Type = SupportedType.String
		};
		span[12] = new Column
		{
			Name = "PartName",
			Caption = ArmSoft.SME.Common.Resources.PartnerName,
			Source = "PartName",
			Type = SupportedType.String
		};
		span[13] = new Column
		{
			Name = "EmplCode",
			Caption = ArmSoft.SME.Common.Resources.Employee,
			Source = "EmplCode",
			Type = SupportedType.String
		};
		span[14] = new Column
		{
			Name = "EmplName",
			Caption = ArmSoft.SME.Common.Resources.Name,
			Source = "EmplName",
			Type = SupportedType.String
		};
		span[15] = new Column
		{
			Name = "Creator",
			Caption = ArmSoft.SME.Trade.Common.Resources.LastUser,
			Source = "Creator",
			Type = SupportedType.String
		};
		span[16] = new Column
		{
			Name = "LastModifyDate",
			Caption = ArmSoft.SME.Common.Resources.LastModifyDate,
			Source = "LastModifyDate",
			Type = SupportedType.DateTime
		};
		span[17] = new Column
		{
			Name = "DocOwner",
			Caption = ArmSoft.SME.Common.Resources.DocOwner,
			Source = "DocOwner",
			Type = SupportedType.String
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
		using SQLiteCommand sQLiteCommand = ArmSoft.SME.Core.UI.Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = $"SELECT fISN,fDATE,fORDERNUM,fDOCTYPE,fDOCSTATE,fDOCNUM,fECRCHECKNUM,fCUR,fSUMM,fCOMMENT,\r\n                          fBASEISN,fUSERID,fPARTID,fEMPLID,fCASHDESK,fSHIFTNUMBER,fCHANGEDATE,\r\n                          fOWNERID, fSYNCSTATE, fJSON\r\n                          FROM DOCUMENTS \r\n                          WHERE fSYNCSTATE <> {LocalDBDocumentSyncStateSE.AutoSaved.Value} ";
		if (!string.IsNullOrWhiteSpace(param.SyncState))
		{
			sQLiteCommand.CommandText += " AND fSYNCSTATE = @SYNCSTATE ";
			sQLiteCommand.Parameters.Add("@SYNCSTATE", DbType.Int16).Value = param.SyncState;
		}
		if (param.StartDate.HasValue)
		{
			sQLiteCommand.CommandText += " AND fDATE >= @StartDate ";
			sQLiteCommand.Parameters.Add("@StartDate", DbType.Date).Value = param.StartDate.Value.ToDateTime();
		}
		if (param.EndDate.HasValue)
		{
			sQLiteCommand.CommandText += " AND fDATE <= @EndDate ";
			sQLiteCommand.Parameters.Add("@EndDate", DbType.Date).Value = param.EndDate.Value.ToDateTime();
		}
		if (!string.IsNullOrWhiteSpace(param.DocState))
		{
			if (param.DocState == "0")
			{
				sQLiteCommand.CommandText += $" AND fDOCSTATE IN ({DocumentStateSE.Draft.Value}, {DocumentStateSE.Processed.Value})";
			}
			else
			{
				sQLiteCommand.CommandText += " AND fDOCSTATE = @DocState ";
				sQLiteCommand.Parameters.Add("@DocState", DbType.Byte).Value = Convert.ToByte(param.DocState);
			}
		}
		if (!string.IsNullOrWhiteSpace(param.CurrCode))
		{
			sQLiteCommand.CommandText += " AND fCUR = @CurrCode ";
			sQLiteCommand.Parameters.Add("@CurrCode", DbType.String).Value = param.CurrCode;
		}
		if (!string.IsNullOrWhiteSpace(param.CashDeskCode))
		{
			sQLiteCommand.CommandText += " AND fCASHDESK = @CashDeskCode ";
			sQLiteCommand.Parameters.Add("@CashDeskCode", DbType.String).Value = param.CashDeskCode;
		}
		if (!string.IsNullOrWhiteSpace(param.CashierShiftNumber))
		{
			sQLiteCommand.CommandText += " AND fSHIFTNUMBER = @CashierShiftNumber ";
			sQLiteCommand.Parameters.Add("@CashierShiftNumber", DbType.String).Value = param.CashierShiftNumber;
		}
		if (!string.IsNullOrWhiteSpace(param.DocType))
		{
			sQLiteCommand.CommandText += " AND fDOCTYPE = @DocType ";
			sQLiteCommand.Parameters.Add("@DocType", DbType.String).Value = param.DocType;
		}
		if (!ArmSoft.SME.Core.UI.Settings.LoginService.IsAdmin && ArmSoft.SME.Core.UI.Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			sQLiteCommand.CommandText += " AND fOWNERID = @OwnerID ";
			sQLiteCommand.Parameters.Add("@OwnerID", DbType.Int32).Value = ArmSoft.SME.Core.UI.Settings.LoginService.UserId;
		}
		if (param.OrderByAscedning)
		{
			sQLiteCommand.CommandText += " ORDER BY fDATE ASC,fORDERNUM ";
		}
		else
		{
			sQLiteCommand.CommandText += " ORDER BY fCHANGEDATE DESC,fORDERNUM ";
		}
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		int ordinal = sQLiteDataReader.GetOrdinal("fISN");
		int ordinal2 = sQLiteDataReader.GetOrdinal("fDATE");
		int ordinal3 = sQLiteDataReader.GetOrdinal("fORDERNUM");
		int ordinal4 = sQLiteDataReader.GetOrdinal("fDOCTYPE");
		int ordinal5 = sQLiteDataReader.GetOrdinal("fDOCSTATE");
		int ordinal6 = sQLiteDataReader.GetOrdinal("fDOCNUM");
		int ordinal7 = sQLiteDataReader.GetOrdinal("fECRCHECKNUM");
		int ordinal8 = sQLiteDataReader.GetOrdinal("fCUR");
		int ordinal9 = sQLiteDataReader.GetOrdinal("fSUMM");
		int ordinal10 = sQLiteDataReader.GetOrdinal("fCOMMENT");
		int ordinal11 = sQLiteDataReader.GetOrdinal("fBASEISN");
		int ordinal12 = sQLiteDataReader.GetOrdinal("fUSERID");
		int ordinal13 = sQLiteDataReader.GetOrdinal("fPARTID");
		int ordinal14 = sQLiteDataReader.GetOrdinal("fEMPLID");
		int ordinal15 = sQLiteDataReader.GetOrdinal("fCASHDESK");
		int ordinal16 = sQLiteDataReader.GetOrdinal("fSHIFTNUMBER");
		int ordinal17 = sQLiteDataReader.GetOrdinal("fCHANGEDATE");
		int ordinal18 = sQLiteDataReader.GetOrdinal("fOWNERID");
		int ordinal19 = sQLiteDataReader.GetOrdinal("fSYNCSTATE");
		int ordinal20 = sQLiteDataReader.GetOrdinal("fJSON");
		while (sQLiteDataReader.Read())
		{
			DataRow dataRow = new DataRow
			{
				ISN = Guid.ParseExact(sQLiteDataReader.GetString(ordinal), "D"),
				DocDate = sQLiteDataReader.GetDateTime(ordinal2).ToDateOnly(),
				OrderNum = sQLiteDataReader.GetInt32(ordinal3),
				DocType = sQLiteDataReader.GetInt16(ordinal4),
				DocState = sQLiteDataReader.GetByte(ordinal5),
				DocNum = sQLiteDataReader.GetString(ordinal6),
				ECRCheckNum = sQLiteDataReader.GetString(ordinal7),
				CurrCode = sQLiteDataReader.GetString(ordinal8),
				Summ = sQLiteDataReader.GetDecimal(ordinal9),
				Comment = sQLiteDataReader.GetString(ordinal10),
				BaseISN = (sQLiteDataReader.IsDBNull(ordinal11) ? ((Guid?)null) : new Guid?(Guid.ParseExact(sQLiteDataReader.GetString(ordinal11), "D"))),
				CreatorID = sQLiteDataReader.GetInt32(ordinal12),
				PartID = sQLiteDataReader.GetInt32(ordinal13),
				EmplID = sQLiteDataReader.GetInt32(ordinal14),
				Cashdesk = sQLiteDataReader.GetString(ordinal15),
				CashierShiftNumber = sQLiteDataReader.GetString(ordinal16),
				LastModifyDate = sQLiteDataReader.GetDateTime(ordinal17),
				DocOwnerID = sQLiteDataReader.GetInt32(ordinal18),
				SyncState = sQLiteDataReader.GetByte(ordinal19),
				Json = sQLiteDataReader.GetString(ordinal20)
			};
			dataRow.DocStateName = SmartEnum<DocumentStateSE, byte>.FromValue(dataRow.DocState).Caption;
			dataRow.DocTypeName = SmartEnum<DocumentTypeSE, short>.FromValue(dataRow.DocType).Caption;
			dataRow.SyncStateName = SmartEnum<LocalDBDocumentSyncStateSE, short>.FromValue(dataRow.SyncState).Caption;
			PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(dataRow.PartID);
			if (itemById != null)
			{
				dataRow.PartCode = itemById.PartCode;
				dataRow.PartName = itemById.Caption;
			}
			else
			{
				dataRow.PartCode = string.Empty;
				dataRow.PartName = string.Empty;
			}
			EmployeeCacheRow itemById2 = TradeSettings.TradeCacheService.Employee.GetItemById(dataRow.EmplID);
			if (itemById2 != null)
			{
				dataRow.EmplCode = itemById2.EmployeeCode;
				dataRow.EmplName = itemById2.Caption;
			}
			else
			{
				dataRow.EmplCode = string.Empty;
				dataRow.EmplName = string.Empty;
			}
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(dataRow.Cashdesk);
			if (item != null)
			{
				dataRow.CashdeskCaption = item.Caption;
			}
			else
			{
				dataRow.CashdeskCaption = string.Empty;
			}
			dataRow.Creator = ArmSoft.SME.Core.UI.Settings.CoreCacheService.User.GetNameById(dataRow.CreatorID);
			dataRow.DocOwner = ArmSoft.SME.Core.UI.Settings.CoreCacheService.User.GetNameById(dataRow.DocOwnerID);
			dataProviderResult.Rows.Add(dataRow);
		}
		return dataProviderResult;
	}
}
