using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataViewDataProvider.Offline;

public class LocalDBDocumentLogDataProvider : DataProvider<LocalDBDocumentLogDataProvider.DataRow, LocalDBDocumentLogDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateTime Date { get; set; }

		public int UserID { get; set; }

		public string UserName { get; set; }

		public byte State { get; set; }

		public string StateName { get; set; }

		public byte Op { get; set; }

		public string OpName { get; set; }

		public string Comment { get; set; }

		public byte SyncState { get; set; }

		public string SyncStateName { get; set; }
	}

	public class Param : ExtendableParameter
	{
		public Guid ISN { get; set; }
	}

	public override string Name => "LocalDocumentLog";

	public LocalDBDocumentLogDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = "LocalDBDocumentLogDataProvider"
		};
		int num = 6;
		List<Column> list = new List<Column>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Column> span = CollectionsMarshal.AsSpan(list);
		span[0] = new Column
		{
			Name = "Date",
			Caption = Resources.DateWord,
			Source = "Date",
			Type = SupportedType.DateTime
		};
		span[1] = new Column
		{
			Name = "UserName",
			Caption = Resources.UserName,
			Source = "UserName",
			Type = SupportedType.String
		};
		span[2] = new Column
		{
			Name = "StateName",
			Caption = Resources.State,
			Source = "StateName",
			Type = SupportedType.String
		};
		span[3] = new Column
		{
			Name = "OpName",
			Caption = Resources.Operation,
			Source = "OpName",
			Type = SupportedType.String
		};
		span[4] = new Column
		{
			Name = "SyncStateName",
			Caption = Resources.SyncState,
			Source = "SyncStateName",
			Type = SupportedType.String
		};
		span[5] = new Column
		{
			Name = "Comment",
			Caption = Resources.Comment,
			Source = "Comment",
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
		sQLiteCommand.CommandText = "SELECT fDATE, fUSERID, fSTATE, fOP, fCOMMENT,fSYNCSTATE\r\n                            FROM DOCUMENTSLOG WHERE fISN=@ISN ORDER BY _rowid_;";
		sQLiteCommand.Parameters.Add("@ISN", DbType.String).Value = param.ISN.ToString("D");
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		while (sQLiteDataReader.Read())
		{
			DataRow dataRow = new DataRow
			{
				Date = sQLiteDataReader.GetDateTime(0),
				UserID = sQLiteDataReader.GetInt32(1),
				State = sQLiteDataReader.GetByte(2),
				Op = sQLiteDataReader.GetByte(3),
				Comment = sQLiteDataReader.GetString(4),
				SyncState = sQLiteDataReader.GetByte(5)
			};
			dataRow.StateName = SmartEnum<DocumentStateSE, byte>.FromValue(dataRow.State).Caption;
			dataRow.SyncStateName = SmartEnum<LocalDBDocumentSyncStateSE, short>.FromValue(dataRow.SyncState).Caption;
			dataRow.OpName = SmartEnum<DocumentLogOpSE, byte>.FromValue(dataRow.Op).Caption;
			dataRow.UserName = ArmSoft.SME.Core.UI.Settings.CoreCacheService.User.GetNameById(dataRow.UserID);
			dataProviderResult.Rows.Add(dataRow);
		}
		return dataProviderResult;
	}
}
