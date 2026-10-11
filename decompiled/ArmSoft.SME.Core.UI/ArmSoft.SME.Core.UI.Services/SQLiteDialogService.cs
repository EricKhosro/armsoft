using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Common.Models.Dialog;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.Services;

public class SQLiteDialogService : IDialogService
{
	public DialogValues GetValues(string name)
	{
		DialogValues dialogValues = new DialogValues
		{
			StoreValues = false
		};
		using (SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand())
		{
			sQLiteCommand.CommandText = "TYPES [smallint], [string];\r\n                                    SELECT fSTORE, fBODY FROM DIALOGVALUES\r\n                                    WHERE fUSERID = @USERID AND fNAME = @NAME;";
			sQLiteCommand.Parameters.Add("@USERID", DbType.Int32).Value = Settings.LoginService.UserId;
			sQLiteCommand.Parameters.Add("@NAME", DbType.String).Value = name;
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader(CommandBehavior.SequentialAccess);
			if (sQLiteDataReader.Read() && sQLiteDataReader.GetInt16(0) == 1)
			{
				dialogValues.StoreValues = true;
				dialogValues.Values = DialogHelper.GetNameValuePairs(SQLiteHelper.ColumnToString(sQLiteDataReader, 1));
			}
		}
		return dialogValues;
	}

	public void StoreValues(string name, DialogValues values)
	{
		string bodyString = DialogHelper.GetBodyString(values.Values);
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "UPDATE DIALOGVALUES SET fSTORE = @STORE, fBODY = @BODY\r\n                                  WHERE fUSERID = @USERID AND fNAME = @NAME;";
		sQLiteCommand.Parameters.Add("@USERID", DbType.Int32).Value = Settings.LoginService.UserId;
		sQLiteCommand.Parameters.Add("@NAME", DbType.String).Value = name;
		sQLiteCommand.Parameters.Add("@STORE", DbType.Int16).Value = (values.StoreValues ? 1 : 0);
		sQLiteCommand.Parameters.Add("@BODY", DbType.String, bodyString.Length).Value = bodyString;
		if (sQLiteCommand.ExecuteNonQuery() == 0)
		{
			sQLiteCommand.CommandText = "INSERT INTO DIALOGVALUES(fSTORE, fBODY, fUSERID, fNAME)\r\n                                                      VALUES(@STORE, @BODY, @USERID, @NAME);";
			sQLiteCommand.ExecuteNonQuery();
		}
	}

	public List<DialogValuesTemplate> GetTemplates(string group, bool forCurrentUser)
	{
		throw new NotImplementedException();
	}

	public int StoreTemplate(string group, DialogValuesTemplate template)
	{
		throw new NotImplementedException();
	}

	public void UpdateTemplates(string group, List<DialogValuesTemplate> templates)
	{
		throw new NotImplementedException();
	}

	public void DeleteTemplate(int id)
	{
		throw new NotImplementedException();
	}

	public void DeleteTemplates(IEnumerable<int> ids)
	{
		throw new NotImplementedException();
	}

	public List<int> CopyTemplate(int id, IEnumerable<string> userNames)
	{
		throw new NotImplementedException();
	}
}
