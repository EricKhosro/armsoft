using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using ArmSoft.SME.Common.Models.Layout;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.Services;

public class SQLiteLayoutService : ILayoutService
{
	public bool Store(LayoutRow layout)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.Parameters.Add("@UserID", DbType.Int32).Value = ((!layout.UserId.HasValue) ? new int?(Settings.LoginService.UserId) : layout.UserId);
		sQLiteCommand.Parameters.Add("@Name", DbType.String).Value = layout.Name;
		sQLiteCommand.Parameters.Add("@Caption", DbType.String).Value = layout.Caption;
		sQLiteCommand.Parameters.Add("@Type", DbType.Byte).Value = layout.LayoutType;
		sQLiteCommand.Parameters.Add("@Body", DbType.String, layout.Body.Length).Value = layout.Body;
		sQLiteCommand.CommandText = "UPDATE LAYOUTVALUES SET fTYPE = @Type, fBODY = @Body\r\n                                 WHERE fUSERID = @UserID AND fNAME = @Name AND fCAPTION = @Caption ";
		if (sQLiteCommand.ExecuteNonQuery() == 0)
		{
			sQLiteCommand.CommandText = "INSERT INTO LAYOUTVALUES(fTYPE, fBODY, fUSERID, fNAME, fCAPTION)\r\n                                                     VALUES(@Type, @Body, @UserID, @Name, @Caption)";
			return sQLiteCommand.ExecuteNonQuery() != 0;
		}
		return false;
	}

	public List<LayoutRow> GetList(string name)
	{
		List<LayoutRow> list = new List<LayoutRow>();
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "TYPES [tinyint], [string], [string];\r\n                                    SELECT fTYPE, fBODY, fCAPTION FROM LAYOUTVALUES\r\n                                       WHERE fUSERID = @UserID AND fNAME = @Name;";
		sQLiteCommand.Parameters.Add("@UserID", DbType.Int32).Value = Settings.LoginService.UserId;
		sQLiteCommand.Parameters.Add("@Name", DbType.String).Value = name;
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader(CommandBehavior.SequentialAccess);
		int ordinal = sQLiteDataReader.GetOrdinal("fTYPE");
		int ordinal2 = sQLiteDataReader.GetOrdinal("fBODY");
		int ordinal3 = sQLiteDataReader.GetOrdinal("fCAPTION");
		while (sQLiteDataReader.Read())
		{
			list.Add(new LayoutRow
			{
				Name = name,
				LayoutType = sQLiteDataReader.GetByte(ordinal),
				Body = SQLiteHelper.ColumnToString(sQLiteDataReader, ordinal2),
				Caption = sQLiteDataReader.GetString(ordinal3)
			});
		}
		return list;
	}

	public bool Delete(LayoutRow layout)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "DELETE FROM LAYOUTVALUES\r\n                                WHERE fUSERID = @UserID AND fNAME = @Name AND fCAPTION = @Caption;";
		sQLiteCommand.Parameters.Add("@UserID", DbType.Int32).Value = ((!layout.UserId.HasValue) ? new int?(Settings.LoginService.UserId) : layout.UserId);
		sQLiteCommand.Parameters.Add("@Name", DbType.String).Value = layout.Name;
		sQLiteCommand.Parameters.Add("@Caption", DbType.String).Value = layout.Caption;
		return sQLiteCommand.ExecuteNonQuery() != 0;
	}

	public bool EditCaption(LayoutRow layout, string newCaption)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "update LAYOUTVALUES set fCAPTION = @NewCaption\r\n                                where fUSERID = @UserID and fNAME = @Name and fCAPTION = @Caption;";
		sQLiteCommand.Parameters.Add("@UserID", DbType.Int32).Value = ((!layout.UserId.HasValue) ? new int?(Settings.LoginService.UserId) : layout.UserId);
		sQLiteCommand.Parameters.Add("@Name", DbType.String).Value = layout.Name;
		sQLiteCommand.Parameters.Add("@Caption", DbType.String).Value = layout.Caption;
		sQLiteCommand.Parameters.Add("@NewCaption", DbType.String).Value = newCaption;
		return sQLiteCommand.ExecuteNonQuery() != 0;
	}

	public string GetBody(LayoutRow layout)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "TYPES [string];\r\n                                select fBODY from LAYOUTVALUES\r\n                                where fUSERID = @UserID and fNAME = @Name and fCAPTION = @Caption;";
		sQLiteCommand.Parameters.Add("@UserID", DbType.Int32).Value = ((!layout.UserId.HasValue) ? new int?(Settings.LoginService.UserId) : layout.UserId);
		sQLiteCommand.Parameters.Add("@Name", DbType.String).Value = layout.Name;
		sQLiteCommand.Parameters.Add("@Caption", DbType.String).Value = layout.Caption;
		string result = null;
		using (SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader(CommandBehavior.SequentialAccess))
		{
			if (sQLiteDataReader.Read())
			{
				result = SQLiteHelper.ColumnToString(sQLiteDataReader, 0);
			}
		}
		return result;
	}

	public bool IsDeleteLayoutRow(string name, string caption)
	{
		if (ExistsLayoutRow(name, caption))
		{
			using (SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand())
			{
				sQLiteCommand.CommandText = "DELETE FROM LAYOUTVALUES \r\n                                    WHERE fUSERID = @UserID AND fNAME = @Name AND fCAPTION = @Caption;";
				sQLiteCommand.Parameters.Add("@UserID", DbType.Int32).Value = Settings.LoginService.UserId;
				sQLiteCommand.Parameters.Add("@Name", DbType.String).Value = name;
				sQLiteCommand.Parameters.Add("@Caption", DbType.String).Value = caption;
				return sQLiteCommand.ExecuteNonQuery() != 0;
			}
		}
		return false;
	}

	public bool ExistsLayoutRow(string name, string caption)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "TYPES [tinyint];\r\n                              SELECT fTYPE FROM LAYOUTVALUES WHERE fUSERID = @UserID AND fNAME = @Name AND fCAPTION = @Caption;";
		sQLiteCommand.Parameters.Add("@UserID", DbType.Int32).Value = Settings.LoginService.UserId;
		sQLiteCommand.Parameters.Add("@Name", DbType.String).Value = name;
		sQLiteCommand.Parameters.Add("@Caption", DbType.String).Value = caption;
		return sQLiteCommand.ExecuteScalar() != null;
	}
}
