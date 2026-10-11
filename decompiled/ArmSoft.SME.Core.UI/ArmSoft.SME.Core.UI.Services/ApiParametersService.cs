using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using ArmSoft.SME.Common.Models.Parameter;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Core.UI.Services;

public class ApiParametersService : ParametersService
{
	private Dictionary<short, string> _cachedParameters = new Dictionary<short, string>();

	public override Dictionary<short, string> GetValuesFromService(int userId = -1, bool alsoRoleParams = false)
	{
		if (userId == -1)
		{
			userId = Settings.LoginService.UserId;
		}
		if (Settings.IsOffline)
		{
			return GetValuesFromLocalDB();
		}
		return Settings.CreateApiClient().Parameter.GetValues(userId, alsoRoleParams).ToDictionary((ParameterResponseModel x) => x.Id, (ParameterResponseModel y) => y.Value);
	}

	public override Dictionary<short, string> GetGeneralValuesFromService()
	{
		return Settings.CreateApiClient().Parameter.GetGeneralValues().ToDictionary((ParameterResponseModel x) => x.Id, (ParameterResponseModel y) => y.Value);
	}

	public override List<ParameterForUsersResponseModel> GetValuesForUsersFromService(short paramId)
	{
		return Settings.CreateApiClient().Parameter.GetValuesForUsers(paramId);
	}

	public static void StoreValuesInLocalDB(Dictionary<short, string> keyValuePairs)
	{
		using SQLiteTransaction sQLiteTransaction = Settings.LocalDB.Connection.BeginTransaction();
		foreach (KeyValuePair<short, string> keyValuePair in keyValuePairs)
		{
			StoreValueInLocalDB(keyValuePair.Key, keyValuePair.Value);
		}
		sQLiteTransaction.Commit();
	}

	private static Dictionary<short, string> GetValuesFromLocalDB()
	{
		Dictionary<short, string> dictionary = new Dictionary<short, string>();
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "TYPES[smallint],[string]; select fPARID, fVALUE from PARAMS;";
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		while (sQLiteDataReader.Read())
		{
			dictionary.Add(sQLiteDataReader.GetInt16(0), sQLiteDataReader.GetString(1));
		}
		return dictionary;
	}

	private static string GeValueFromLocalDB(short paramId)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "select fVALUE from PARAMS where fPARID = @PARAMID;";
		sQLiteCommand.Parameters.Add("@PARAMID", DbType.Int16).Value = paramId;
		return sQLiteCommand.ExecuteScalar().ToString();
	}

	private static void StoreValueInLocalDB(short parId, string parValue)
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

	public override void Refresh()
	{
		if (Settings.IsOffline)
		{
			_cachedParameters = GetValuesFromLocalDB();
			return;
		}
		_cachedParameters = GetValuesFromService(-1, alsoRoleParams: true);
		if (Settings.OfflineModeIsAvailable)
		{
			StoreValuesInLocalDB(_cachedParameters);
		}
	}

	protected override string GetValue(ParameterDefinition definition, int userId)
	{
		return GetValue(definition.Id, definition.UseFromCacheUI, definition.UserParam, userId);
	}

	public override string GetValue(short id, bool fromCache, bool userParam, int userId = -1)
	{
		string value;
		if (fromCache)
		{
			if (!_cachedParameters.TryGetValue(id, out value))
			{
				if (Settings.IsOffline)
				{
					if (userId != -1)
					{
						throw new NotImplementedException();
					}
					value = GeValueFromLocalDB(id);
				}
				else
				{
					value = GetValueFromService(id, userId);
					_cachedParameters.Add(id, value);
					if (Settings.OfflineModeIsAvailable)
					{
						StoreValueInLocalDB(id, value);
					}
				}
			}
		}
		else
		{
			value = GetValueFromService(id, userId);
		}
		return value;
	}

	private static string GetValueFromService(short id, int userId = -1)
	{
		return Settings.CreateApiClient().Parameter.GetValue(id, userId);
	}

	public override string GetGeneralValue(short id)
	{
		return Settings.CreateApiClient().Parameter.GetGeneralValue(id);
	}

	public override void SetValues(List<ParameterStoreRequestModel> requestModel)
	{
		Settings.CreateApiClient().Parameter.SetValues(requestModel);
		foreach (ParameterStoreRequestModel item in requestModel)
		{
			_cachedParameters.Remove(item.Id);
		}
	}

	public override void SetCurrentUserValues(Dictionary<ParameterDefinition, string> values)
	{
		List<ParameterStoreRequestModel> list = new List<ParameterStoreRequestModel>();
		foreach (KeyValuePair<ParameterDefinition, string> value in values)
		{
			list.Add(new ParameterStoreRequestModel
			{
				Id = value.Key.Id,
				Value = value.Value,
				UserId = (value.Key.UserParam ? Settings.LoginService.UserId : (-1))
			});
		}
		SetValues(list);
	}

	public override void SetValue(short id, string value, int userId = -1, bool isDefault = false)
	{
		Settings.CreateApiClient().Parameter.SetValue(id, value, userId, isDefault);
		_cachedParameters.Remove(id);
	}

	public override void Clear()
	{
		_cachedParameters.Clear();
	}
}
