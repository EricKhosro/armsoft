using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using ArmSoft.SME.Common.Models.Parameter;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Core.UI.Services;

public class SQLiteParametersService : ParametersService
{
	private readonly Dictionary<short, string> _cachedParameters = new Dictionary<short, string>();

	public override void Clear()
	{
		_cachedParameters.Clear();
	}

	public override void Refresh()
	{
		_cachedParameters.Clear();
	}

	protected override string GetValue(ParameterDefinition definition, int userId)
	{
		string value;
		if (definition.UseFromCacheUI)
		{
			if (!_cachedParameters.TryGetValue(definition.Id, out value))
			{
				value = GetValueFromDB(definition.Id, definition.DefaultValue, definition.UserParam, userId);
				_cachedParameters.Add(definition.Id, value);
			}
		}
		else
		{
			value = GetValueFromDB(definition.Id, definition.DefaultValue, definition.UserParam, userId);
		}
		return value;
	}

	public override string GetValue(short id, bool fromCache, bool userParam, int userId = -1)
	{
		string value;
		if (fromCache)
		{
			if (!_cachedParameters.TryGetValue(id, out value))
			{
				value = GetValueFromDB(id, "", userParam, userId);
				_cachedParameters.Add(id, value);
			}
		}
		else
		{
			value = GetValueFromDB(id, "", userParam, userId);
		}
		return value;
	}

	public override string GetGeneralValue(short id)
	{
		return GetGeneralValueFromDB(id, "");
	}

	private static string GetValueFromDB(short id, string defaultValue, bool userParam, int userId)
	{
		string result = "";
		if (!userParam || userId == -1)
		{
			result = GetGeneralValueFromDB(id, defaultValue);
		}
		else
		{
			using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
			sQLiteCommand.CommandText = "TYPES [string]; SELECT fVALUE FROM USERPARAMS WHERE fPARAMID = @PARAMID AND fUSERID = @USERID";
			sQLiteCommand.Parameters.Add("@PARAMID", DbType.Int16).Value = id;
			sQLiteCommand.Parameters.Add("@USERID", DbType.Int32).Value = userId;
			object obj = sQLiteCommand.ExecuteScalar();
			result = ((obj != null) ? obj.ToString() : GetGeneralValueFromDB(id, defaultValue));
		}
		return result;
	}

	private static string GetGeneralValueFromDB(short paramId, string defaultValue)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "TYPES [string]; \r\n                                select fVALUE from PARAMS where fPARID = @PARAMID;";
		sQLiteCommand.Parameters.Add("@PARAMID", DbType.Int16).Value = paramId;
		object obj = sQLiteCommand.ExecuteScalar();
		if (obj == null)
		{
			sQLiteCommand.CommandText = "insert into PARAMS (fPARID, fVALUE) \r\n                                                values (@PARAMID, @Value)";
			sQLiteCommand.Parameters.Add("@Value", DbType.String).Value = defaultValue;
			sQLiteCommand.ExecuteNonQuery();
			return defaultValue;
		}
		return obj.ToString();
	}

	public override void SetValue(short id, string value, int userId = -1, bool isDefault = false)
	{
		if (userId == -1)
		{
			SetGeneralValueInDB(id, value);
			_cachedParameters.Remove(id);
		}
		else if (isDefault)
		{
			DeleteUserParamFromDB(id, userId);
			_cachedParameters.Remove(id);
		}
		else
		{
			UpdateUserParamValueInDB(id, value, userId);
		}
	}

	private static void SetGeneralValueInDB(short parId, string parValue)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "UPDATE PARAMS SET fVALUE = @PARAMVALUE WHERE fPARID = @PARAMID";
		sQLiteCommand.Parameters.Add("@PARAMID", DbType.Int16).Value = parId;
		sQLiteCommand.Parameters.Add("@PARAMVALUE", DbType.String).Value = parValue;
		if (sQLiteCommand.ExecuteNonQuery() == 0)
		{
			sQLiteCommand.CommandText = "INSERT INTO PARAMS(fPARID, fVALUE) values (@PARAMID, @PARAMVALUE)";
			sQLiteCommand.ExecuteNonQuery();
		}
	}

	private static void DeleteUserParamFromDB(short parId, int userId)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "DELETE from USERPARAMS WHERE fPARAMID = @PARAMID and fUSERID = @USERID";
		sQLiteCommand.Parameters.Add("@USERID", DbType.Int32).Value = userId;
		sQLiteCommand.Parameters.Add("@PARAMID", DbType.Int16).Value = parId;
		sQLiteCommand.ExecuteNonQuery();
	}

	private static void UpdateUserParamValueInDB(short parId, string parValue, int userId)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "UPDATE USERPARAMS SET fVALUE = @PARAMVALUE WHERE fPARAMID = @PARAMID AND fUSERID = @USERID;";
		sQLiteCommand.Parameters.Add("@USERID", DbType.Int32).Value = userId;
		sQLiteCommand.Parameters.Add("@PARAMID", DbType.Int16).Value = parId;
		sQLiteCommand.Parameters.Add("@PARAMVALUE", DbType.String).Value = parValue;
		if (sQLiteCommand.ExecuteNonQuery() == 0)
		{
			sQLiteCommand.CommandText = "INSERT INTO USERPARAMS(fUSERID, fPARAMID, fVALUE) VALUES(@USERID, @PARAMID, @PARAMVALUE)";
			sQLiteCommand.ExecuteNonQuery();
		}
	}

	public override Dictionary<short, string> GetValuesFromService(int userId = -1, bool alsoRoleParams = false)
	{
		throw new NotImplementedException();
	}

	public override Dictionary<short, string> GetGeneralValuesFromService()
	{
		throw new NotImplementedException();
	}

	public override List<ParameterForUsersResponseModel> GetValuesForUsersFromService(short paramId)
	{
		throw new NotImplementedException();
	}

	public override void SetValues(List<ParameterStoreRequestModel> requestModel)
	{
		throw new NotImplementedException();
	}

	public override void SetCurrentUserValues(Dictionary<ParameterDefinition, string> values)
	{
		throw new NotImplementedException();
	}
}
