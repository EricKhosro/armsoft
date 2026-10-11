using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Parameter;

namespace ArmSoft.SME.Common.Parameters;

public interface IParametersService
{
	string GetStringParam(ParameterDefinition<string> definition, int userId = -1);

	string GetCurrentUserStringParam(ParameterDefinition<string> definition);

	bool GetCurrentUserBoolParam(ParameterDefinition<bool> definition);

	DateTime? GetCurrentUserDateParam(ParameterDefinition<DateTime?> definition);

	DateOnly? GetCurrentUserDateOnlyParam(ParameterDefinition<DateOnly?> definition);

	int GetCurrentUserIntegerParam(ParameterDefinition<int> definition);

	decimal GetCurrentUserDecimalParam(ParameterDefinition<decimal> definition);

	string GetCurrentUserPrintTemplate(short docType);

	void SetStringParam(short parId, string parValue, int userId = -1, bool isDefault = true);

	void SetDateParam(short parId, DateTime? parValue, int userId = -1, bool isDefault = false);

	void SetDateOnlyParam(short parId, DateOnly? parValue, int userId = -1, bool isDefault = false);

	void SetIntegerParam(short parId, int parValue, int userId = -1, bool isDefault = false);

	void SetBoolParam(short parId, bool parValue, int userId = -1, bool isDefault = false);

	void SetDecimalParam(short parId, decimal parValue, int userId = -1, bool isDefault = false);

	void SetPrintTemplate(short docType, string parValue);

	string GetGeneralValue(short id);

	void Refresh();

	void Clear();

	Dictionary<short, string> GetValuesFromService(int userId = -1, bool alsoRoleParams = false);

	Dictionary<short, string> GetGeneralValuesFromService();

	List<ParameterForUsersResponseModel> GetValuesForUsersFromService(short paramId);

	bool GetBoolParamFromService(ParameterDefinition<bool> definition, int userId = -1);

	void SetCurrentUserValues(Dictionary<ParameterDefinition, string> values);

	void SetValue(short paramId, string paramValue, int userId = -1, bool isDefault = false);

	void SetValues(List<ParameterStoreRequestModel> parameters);

	int AnalyticsCount(short paramId);

	Task SetStringParamAsync(short parId, string parValue, int userId = -1, bool isDefault = true);

	Task SetDateParamAsync(short parId, DateTime? parValue, int userId = -1, bool isDefault = false);

	Task SetDateOnlyParamAsync(short parId, DateOnly? parValue, int userId = -1, bool isDefault = false);

	Task SetIntegerParamAsync(short parId, int parValue, int userId = -1, bool isDefault = false);

	Task SetBoolParamAsync(short parId, bool parValue, int userId = -1, bool isDefault = false);

	Task SetDecimalParamAsync(short parId, decimal parValue, int userId = -1, bool isDefault = false);

	Task SetPrintTemplateAsync(short docType, string parValue);

	Task<string> GetCurrentUserStringParamAsync(ParameterDefinition<string> definition);

	Task<DateTime?> GetCurrentUserDateParamAsync(ParameterDefinition<DateTime?> definition);

	Task<DateOnly?> GetCurrentUserDateOnlyParamAsync(ParameterDefinition<DateOnly?> definition);

	Task<int> GetCurrentUserIntegerParamAsync(ParameterDefinition<int> definition);

	Task<decimal> GetCurrentUserDecimalParamAsync(ParameterDefinition<decimal> definition);

	Task<bool> GetCurrentUserBoolParamAsync(ParameterDefinition<bool> definition);

	Task<string> GetCurrentUserPrintTemplateAsync(short docType);

	Task<string> GetStringParamAsync(ParameterDefinition<string> definition, int userId = -1);

	Task<string> GetStringParamAsync(short parId, string defaultValue, bool fromCache = false, bool userParam = false, int userId = -1);

	Task<List<ParameterResponseModel>> GetValuesAsync(int userId, bool alsoRoleParams = false);

	Task<List<ParameterResponseModel>> GetGeneralValuesAsync();

	Task<string> GetValueFromParamsAsync(short id, bool fromCache, string defaultValue, bool userParam, int userId = -1);

	Task<List<ParameterForUsersResponseModel>> GetValuesForUsersAsync(short paramId);

	Task<string> GetValueAsync(ParameterDefinition parameterDefinition, int userId = -1);

	Task SetValuesAsync(List<ParameterStoreRequestModel> parameters);

	Task SetValueAsync(short paramId, string paramValue, int userId = -1, bool isDefault = false);

	Task SetUserValuesAsync(List<ParametrUpdateModel> updateParams);

	Task SetUserValuesAsync(ParametrUpdateModel updateParam);

	Task<bool> ExistAsync(short paramId);

	Task<string> GetRoleValueAsync(int id, string roleCode);

	Task SetRoleValueAsync(int id, string roleCode, string value);

	Task DeleteParamFromDB(short parId);
}
