using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Parameter;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Core.UI.Services;

public abstract class ParametersService : IParametersService
{
	public abstract string GetValue(short id, bool fromCache, bool userParam, int userId = -1);

	protected abstract string GetValue(ParameterDefinition definition, int userId);

	public abstract string GetGeneralValue(short id);

	public abstract void SetValue(short id, string value, int userId = -1, bool isDefault = false);

	public abstract void Clear();

	public abstract void Refresh();

	public abstract Dictionary<short, string> GetValuesFromService(int userId = -1, bool alsoRoleParams = false);

	public abstract Dictionary<short, string> GetGeneralValuesFromService();

	public abstract List<ParameterForUsersResponseModel> GetValuesForUsersFromService(short paramId);

	public abstract void SetValues(List<ParameterStoreRequestModel> requestModel);

	public abstract void SetCurrentUserValues(Dictionary<ParameterDefinition, string> values);

	public bool GetBoolParam(ParameterDefinition<bool> definition, int userId = -1)
	{
		return Convertor.ConvertStringToBool(GetValue(definition, userId));
	}

	public bool GetBoolParamFromService(ParameterDefinition<bool> definition, int userId = -1)
	{
		return Convertor.ConvertStringToBool(GetValue(definition.Id, fromCache: false, definition.UserParam, userId));
	}

	public bool GetCurrentUserBoolParam(ParameterDefinition<bool> definition)
	{
		return GetBoolParam(definition, Settings.LoginService.UserId);
	}

	public DateTime? GetCurrentUserDateParam(ParameterDefinition<DateTime?> definition)
	{
		return GetDateParam(definition, Settings.LoginService.UserId);
	}

	public DateOnly? GetCurrentUserDateOnlyParam(ParameterDefinition<DateOnly?> definition)
	{
		return GetDateOnlyParam(definition, Settings.LoginService.UserId);
	}

	public int GetCurrentUserIntegerParam(ParameterDefinition<int> definition)
	{
		return GetIntegerParam(definition, Settings.LoginService.UserId);
	}

	public decimal GetCurrentUserDecimalParam(ParameterDefinition<decimal> definition)
	{
		return GetDecimalParam(definition, Settings.LoginService.UserId);
	}

	public string GetStringParam(ParameterDefinition<string> definition, int userId = -1)
	{
		return Convertor.ConvertStringToString(GetValue(definition, userId));
	}

	public DateTime? GetDateParam(ParameterDefinition<DateTime?> definition, int userId = -1)
	{
		return Convertor.ConvertStringToDate(GetValue(definition, userId));
	}

	public DateOnly? GetDateOnlyParam(ParameterDefinition<DateOnly?> definition, int userId = -1)
	{
		return Convertor.ConvertStringToDateOnly(GetValue(definition, userId));
	}

	public int GetIntegerParam(ParameterDefinition<int> definition, int userId = -1)
	{
		return Convertor.ConvertStringToInt(GetValue(definition, userId));
	}

	public decimal GetDecimalParam(ParameterDefinition<decimal> definition, int userId = -1)
	{
		return Convertor.ConvertStringToDecimal(GetValue(definition, userId));
	}

	public void SetStringParam(short parId, string parValue, int userId = -1, bool isDefault = true)
	{
		SetValue(parId, parValue, userId, isDefault);
	}

	public void SetDateParam(short parId, DateTime? parValue, int userId = -1, bool isDefault = false)
	{
		SetValue(parId, Convertor.ConvertDateToString(parValue), userId, isDefault);
	}

	public void SetDateOnlyParam(short parId, DateOnly? parValue, int userId = -1, bool isDefault = false)
	{
		SetValue(parId, Convertor.ConvertDateOnlyToString(parValue), userId, isDefault);
	}

	public void SetIntegerParam(short parId, int parValue, int userId = -1, bool isDefault = false)
	{
		SetValue(parId, parValue.ToString(), userId, isDefault);
	}

	public void SetBoolParam(short parId, bool parValue, int userId = -1, bool isDefault = false)
	{
		SetValue(parId, parValue.ToString(), userId, isDefault);
	}

	public void SetDecimalParam(short parId, decimal parValue, int userId = -1, bool isDefault = false)
	{
		SetValue(parId, parValue.ToString(), userId, isDefault);
	}

	public void SetPrintTemplate(short docType, string parValue, int userId = -1)
	{
		SetStringParam((short)(400 + docType), parValue, userId, isDefault: false);
	}

	public string GetCurrentUserPrintTemplate(short docType)
	{
		return Convertor.ConvertStringToString(GetValue((short)(400 + docType), fromCache: true, userParam: false, Settings.LoginService.UserId));
	}

	public string GetCurrentUserStringParam(ParameterDefinition<string> definition)
	{
		return GetStringParam(definition, Settings.LoginService.UserId);
	}

	public Task<string> GetCurrentUserStringParamAsync(ParameterDefinition<string> definition)
	{
		throw new NotImplementedException();
	}

	public Task<DateTime?> GetCurrentUserDateParamAsync(ParameterDefinition<DateTime?> definition)
	{
		throw new NotImplementedException();
	}

	public Task<DateOnly?> GetCurrentUserDateOnlyParamAsync(ParameterDefinition<DateOnly?> definition)
	{
		throw new NotImplementedException();
	}

	public Task<int> GetCurrentUserIntegerParamAsync(ParameterDefinition<int> definition)
	{
		throw new NotImplementedException();
	}

	public Task<decimal> GetCurrentUserDecimalParamAsync(ParameterDefinition<decimal> definition)
	{
		throw new NotImplementedException();
	}

	public Task<bool> GetCurrentUserBoolParamAsync(ParameterDefinition<bool> definition)
	{
		throw new NotImplementedException();
	}

	public Task<string> GetCurrentUserPrintTemplateAsync(short docType)
	{
		throw new NotImplementedException();
	}

	public void SetPrintTemplate(short docType, string parValue)
	{
		throw new NotImplementedException();
	}

	public Task SetStringParamAsync(short parId, string parValue, int userId = -1, bool isDefault = true)
	{
		throw new NotImplementedException();
	}

	public Task SetDateParamAsync(short parId, DateTime? parValue, int userId = -1, bool isDefault = false)
	{
		throw new NotImplementedException();
	}

	public Task SetDateOnlyParamAsync(short parId, DateOnly? parValue, int userId = -1, bool isDefault = false)
	{
		throw new NotImplementedException();
	}

	public Task SetIntegerParamAsync(short parId, int parValue, int userId = -1, bool isDefault = false)
	{
		throw new NotImplementedException();
	}

	public Task SetBoolParamAsync(short parId, bool parValue, int userId = -1, bool isDefault = false)
	{
		throw new NotImplementedException();
	}

	public Task SetDecimalParamAsync(short parId, decimal parValue, int userId = -1, bool isDefault = false)
	{
		throw new NotImplementedException();
	}

	public Task SetPrintTemplateAsync(short docType, string parValue)
	{
		throw new NotImplementedException();
	}

	public Task<string> GetStringParamAsync(ParameterDefinition<string> definition, int userId)
	{
		throw new NotImplementedException();
	}

	public Task<string> GetStringParamAsync(short parId, string defaultValue, bool fromCache = false, bool userParam = false, int userId = -1)
	{
		throw new NotImplementedException();
	}

	public Task<List<ParameterResponseModel>> GetValuesAsync(int userId, bool alsoRoleParams = false)
	{
		throw new NotImplementedException();
	}

	public Task<List<ParameterResponseModel>> GetGeneralValuesAsync()
	{
		throw new NotImplementedException();
	}

	public Task<List<ParameterForUsersResponseModel>> GetValuesForUsersAsync(short paramId)
	{
		throw new NotImplementedException();
	}

	public Task<string> GetValueFromParamsAsync(short id, bool fromCache, string defaultValue, bool userParam, int userId = -1)
	{
		throw new NotImplementedException();
	}

	Task IParametersService.SetValuesAsync(List<ParameterStoreRequestModel> parameters)
	{
		throw new NotImplementedException();
	}

	Task IParametersService.SetValueAsync(short paramId, string paramValue, int userId, bool isDefault)
	{
		throw new NotImplementedException();
	}

	public Task SetUserValuesAsync(List<ParametrUpdateModel> updateParams)
	{
		throw new NotImplementedException();
	}

	public Task SetUserValuesAsync(ParametrUpdateModel updateParam)
	{
		throw new NotImplementedException();
	}

	public Task<bool> ExistAsync(short paramId)
	{
		throw new NotImplementedException();
	}

	public int AnalyticsCount(short paramId)
	{
		throw new NotImplementedException();
	}

	public Task<string> GetValueAsync(ParameterDefinition parameterDefinition, int userId = -1)
	{
		throw new NotImplementedException();
	}

	public Task<string> GetRoleValueAsync(int id, string roleCode)
	{
		throw new NotImplementedException();
	}

	public Task SetRoleValueAsync(int id, string roleCode, string value)
	{
		throw new NotImplementedException();
	}

	public Task DeleteParamFromDB(short parId)
	{
		throw new NotImplementedException();
	}
}
