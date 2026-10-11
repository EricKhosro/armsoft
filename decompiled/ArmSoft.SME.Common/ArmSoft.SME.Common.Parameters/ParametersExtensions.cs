using System;
using System.Threading.Tasks;

namespace ArmSoft.SME.Common.Parameters;

public static class ParametersExtensions
{
	public static Task<decimal> VATAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParamAsync(Definitions.VAT);
	}

	public static decimal VAT(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParam(Definitions.VAT);
	}

	public static Task VAT_setAsync(this IParametersService ps, decimal value)
	{
		return ps.SetDecimalParamAsync(10, value);
	}

	public static void VAT_set(this IParametersService ps, decimal value)
	{
		ps.SetDecimalParam(10, value);
	}

	public static Task<string> VATTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.VATType);
	}

	public static string VATType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.VATType);
	}

	public static Task VATType_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(344, value);
	}

	public static void VATType_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(344, value);
	}

	public static Task<int> DBVersionAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.DBVersion);
	}

	public static int DBVersion(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.DBVersion);
	}

	public static Task DBVersion_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(2, value);
	}

	public static void DBVersion_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(2, value);
	}

	public static Task<string> CompanyNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.CompanyName);
	}

	public static string CompanyName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.CompanyName);
	}

	public static Task CompanyName_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(50, value);
	}

	public static void CompanyName_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(50, value);
	}

	public static Task<bool> QuestWhenCloseWindAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.QuestWhenCloseWind);
	}

	public static bool QuestWhenCloseWind(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.QuestWhenCloseWind);
	}

	public static Task QuestWhenCloseWind_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(35, value, userId, isDefault);
	}

	public static void QuestWhenCloseWind_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(35, value, userId, isDefault);
	}

	public static Task<string> TaxCodeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.TaxCode);
	}

	public static string TaxCode(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.TaxCode);
	}

	public static Task TaxCode_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(63, value);
	}

	public static void TaxCode_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(63, value);
	}

	public static Task<bool> SpreadsheetModeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.SpreadsheetMode);
	}

	public static bool SpreadsheetMode(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.SpreadsheetMode);
	}

	public static Task SpreadsheetMode_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(350, value, userId, isDefault);
	}

	public static void SpreadsheetMode_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(350, value, userId, isDefault);
	}

	public static Task<bool> FilterPopupModeExcelAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.FilterPopupModeExcel);
	}

	public static bool FilterPopupModeExcel(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.FilterPopupModeExcel);
	}

	public static Task FilterPopupModeExcel_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(373, value, userId, isDefault);
	}

	public static void FilterPopupModeExcel_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(373, value, userId, isDefault);
	}

	public static Task<bool> AutoFilterRowOldStyleAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AutoFilterRowOldStyle);
	}

	public static bool AutoFilterRowOldStyle(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AutoFilterRowOldStyle);
	}

	public static Task AutoFilterRowOldStyle_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(378, value, userId, isDefault);
	}

	public static void AutoFilterRowOldStyle_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(378, value, userId, isDefault);
	}

	public static Task<bool> DBExclusiveModeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.DBExclusiveMode);
	}

	public static bool DBExclusiveMode(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.DBExclusiveMode);
	}

	public static Task DBExclusiveMode_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(374, value);
	}

	public static void DBExclusiveMode_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(374, value);
	}

	public static Task<string> DBExclusiveModeMsgAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DBExclusiveModeMsg);
	}

	public static string DBExclusiveModeMsg(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DBExclusiveModeMsg);
	}

	public static Task DBExclusiveModeMsg_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(375, value);
	}

	public static void DBExclusiveModeMsg_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(375, value);
	}

	public static Task<bool> AutoBackUpAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AutoBackUp);
	}

	public static bool AutoBackUp(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AutoBackUp);
	}

	public static Task AutoBackUp_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(21, value);
	}

	public static void AutoBackUp_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(21, value);
	}

	public static Task<int> AutoBackUpIntervalAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.AutoBackUpInterval);
	}

	public static int AutoBackUpInterval(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.AutoBackUpInterval);
	}

	public static Task AutoBackUpInterval_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(22, value);
	}

	public static void AutoBackUpInterval_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(22, value);
	}

	public static Task<bool> BackUpPromptAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.BackUpPrompt);
	}

	public static bool BackUpPrompt(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.BackUpPrompt);
	}

	public static Task BackUpPrompt_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(23, value);
	}

	public static void BackUpPrompt_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(23, value);
	}

	public static Task<bool> OnlineBackUpAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.OnlineBackUp);
	}

	public static bool OnlineBackUp(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.OnlineBackUp);
	}

	public static Task OnlineBackUp_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(370, value);
	}

	public static void OnlineBackUp_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(370, value);
	}

	public static Task<bool> LocalBackUpAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.LocalBackUp);
	}

	public static bool LocalBackUp(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.LocalBackUp);
	}

	public static Task LocalBackUp_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(371, value);
	}

	public static void LocalBackUp_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(371, value);
	}

	public static Task<string> LocalBackUpPathAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.LocalBackUpPath);
	}

	public static string LocalBackUpPath(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.LocalBackUpPath);
	}

	public static Task LocalBackUpPath_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(372, value);
	}

	public static void LocalBackUpPath_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(372, value);
	}

	public static Task<string> DropBoxAccountEMailAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DropBoxAccountEMail);
	}

	public static string DropBoxAccountEMail(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DropBoxAccountEMail);
	}

	public static Task DropBoxAccountEMail_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(376, value);
	}

	public static void DropBoxAccountEMail_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(376, value);
	}

	public static Task<string> DropBoxAccountTokenAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DropBoxAccountToken);
	}

	public static string DropBoxAccountToken(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DropBoxAccountToken);
	}

	public static Task DropBoxAccountToken_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(377, value);
	}

	public static void DropBoxAccountToken_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(377, value);
	}

	public static Task<string> UserAWPNavigatorNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.UserAWPNavigatorName);
	}

	public static string UserAWPNavigatorName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.UserAWPNavigatorName);
	}

	public static Task UserAWPNavigatorName_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(7010, value, userId, isDefault);
	}

	public static void UserAWPNavigatorName_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(7010, value, userId, isDefault);
	}

	public static Task<string> UserCardNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.UserCardNumber);
	}

	public static string UserCardNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.UserCardNumber);
	}

	public static Task UserCardNumber_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(379, value, userId, isDefault);
	}

	public static void UserCardNumber_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(379, value, userId, isDefault);
	}

	public static Task<bool> AllowOpFromHistoryPanelAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowOpFromHistoryPanel);
	}

	public static bool AllowOpFromHistoryPanel(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowOpFromHistoryPanel);
	}

	public static Task AllowOpFromHistoryPanel_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(385, value, userId, isDefault);
	}

	public static void AllowOpFromHistoryPanel_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(385, value, userId, isDefault);
	}

	public static Task<bool> UseWindowsNotificationsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.UseWindowsNotifications);
	}

	public static bool UseWindowsNotifications(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.UseWindowsNotifications);
	}

	public static Task UseWindowsNotifications_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(386, value, userId, isDefault);
	}

	public static void UseWindowsNotifications_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(386, value, userId, isDefault);
	}

	public static Task<int> QueryTimeoutAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.QueryTimeout);
	}

	public static int QueryTimeout(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.QueryTimeout);
	}

	public static Task QueryTimeout_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(387, value);
	}

	public static void QueryTimeout_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(387, value);
	}

	public static Task<int> ReportQueryTimeoutAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.ReportQueryTimeout);
	}

	public static int ReportQueryTimeout(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.ReportQueryTimeout);
	}

	public static Task ReportQueryTimeout_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(388, value);
	}

	public static void ReportQueryTimeout_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(388, value);
	}

	public static Task<string> DBVersionNewAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DBVersionNew);
	}

	public static string DBVersionNew(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DBVersionNew);
	}

	public static Task DBVersionNew_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(1000, value);
	}

	public static void DBVersionNew_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(1000, value);
	}

	public static Task<int> DBVersionChangeNumAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.DBVersionChangeNum);
	}

	public static int DBVersionChangeNum(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.DBVersionChangeNum);
	}

	public static Task DBVersionChangeNum_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(1001, value);
	}

	public static void DBVersionChangeNum_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(1001, value);
	}

	public static Task<bool> IsBetaVersionAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.IsBetaVersion);
	}

	public static bool IsBetaVersion(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.IsBetaVersion);
	}

	public static Task IsBetaVersion_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(1002, value);
	}

	public static void IsBetaVersion_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(1002, value);
	}

	public static Task<string> FavoriteItemsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.FavoriteItems);
	}

	public static string FavoriteItems(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.FavoriteItems);
	}

	public static Task FavoriteItems_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(6000, value, userId, isDefault);
	}

	public static void FavoriteItems_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(6000, value, userId, isDefault);
	}

	public static Task<string> NavigatorStateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NavigatorState);
	}

	public static string NavigatorState(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NavigatorState);
	}

	public static Task NavigatorState_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(7000, value, userId, isDefault);
	}

	public static void NavigatorState_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(7000, value, userId, isDefault);
	}

	public static Task<string> DefaultTemplateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultTemplate);
	}

	public static string DefaultTemplate(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultTemplate);
	}

	public static Task DefaultTemplate_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(400, value, userId, isDefault);
	}

	public static void DefaultTemplate_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(400, value, userId, isDefault);
	}

	public static Task<bool> KeepUICacheUpToDateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.KeepUICacheUpToDate);
	}

	public static bool KeepUICacheUpToDate(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.KeepUICacheUpToDate);
	}

	public static Task KeepUICacheUpToDate_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(1101, value, userId, isDefault);
	}

	public static void KeepUICacheUpToDate_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(1101, value, userId, isDefault);
	}

	public static Task<bool> ImportExchangeRatesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ImportExchangeRates);
	}

	public static bool ImportExchangeRates(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ImportExchangeRates);
	}

	public static Task ImportExchangeRates_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(219, value);
	}

	public static void ImportExchangeRates_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(219, value);
	}

	public static Task<bool> StoreImagesLocalAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.StoreImagesLocal);
	}

	public static bool StoreImagesLocal(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.StoreImagesLocal);
	}

	public static Task StoreImagesLocal_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(1004, value, userId, isDefault);
	}

	public static void StoreImagesLocal_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(1004, value, userId, isDefault);
	}

	public static Task<int> SumPrecisionDigitsCountAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.SumPrecisionDigitsCount);
	}

	public static int SumPrecisionDigitsCount(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.SumPrecisionDigitsCount);
	}

	public static Task SumPrecisionDigitsCount_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(1005, value);
	}

	public static void SumPrecisionDigitsCount_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(1005, value);
	}

	public static Task<int> QuantityPrecisionDigitsCountAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.QuantityPrecisionDigitsCount);
	}

	public static int QuantityPrecisionDigitsCount(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.QuantityPrecisionDigitsCount);
	}

	public static Task QuantityPrecisionDigitsCount_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(1006, value);
	}

	public static void QuantityPrecisionDigitsCount_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(1006, value);
	}

	public static Task<string> UserPINAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.UserPIN);
	}

	public static string UserPIN(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.UserPIN);
	}

	public static Task UserPIN_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(1007, value, userId, isDefault);
	}

	public static void UserPIN_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(1007, value, userId, isDefault);
	}

	public static Task<DateOnly?> CPACodesLastSyncDateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.CPACodesLastSyncDate);
	}

	public static DateOnly? CPACodesLastSyncDate(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.CPACodesLastSyncDate);
	}

	public static Task CPACodesLastSyncDate_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(1008, value);
	}

	public static void CPACodesLastSyncDate_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(1008, value);
	}

	public static Task<bool> AutoSaveAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AutoSave);
	}

	public static bool AutoSave(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AutoSave);
	}

	public static Task AutoSave_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(1009, value);
	}

	public static void AutoSave_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(1009, value);
	}

	public static Task<int> AutoSaveIntervalAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.AutoSaveInterval);
	}

	public static int AutoSaveInterval(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.AutoSaveInterval);
	}

	public static Task AutoSaveInterval_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(1010, value);
	}

	public static void AutoSaveInterval_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(1010, value);
	}

	public static Task<bool> AllowEditCodeOfDirectoriesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowEditCodeOfDirectories);
	}

	public static bool AllowEditCodeOfDirectories(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowEditCodeOfDirectories);
	}

	public static Task AllowEditCodeOfDirectories_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(1011, value, userId, isDefault);
	}

	public static void AllowEditCodeOfDirectories_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(1011, value, userId, isDefault);
	}

	public static Task<string> EInvoicingTinAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.EInvoicingTin);
	}

	public static string EInvoicingTin(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.EInvoicingTin);
	}

	public static Task EInvoicingTin_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(1012, value, userId, isDefault);
	}

	public static void EInvoicingTin_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(1012, value, userId, isDefault);
	}

	public static Task<string> EInvoicingLoginAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.EInvoicingLogin);
	}

	public static string EInvoicingLogin(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.EInvoicingLogin);
	}

	public static Task EInvoicingLogin_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(1013, value, userId, isDefault);
	}

	public static void EInvoicingLogin_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(1013, value, userId, isDefault);
	}

	public static Task<string> EInvoicingPasswordAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.EInvoicingPassword);
	}

	public static string EInvoicingPassword(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.EInvoicingPassword);
	}

	public static Task EInvoicingPassword_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(1014, value, userId, isDefault);
	}

	public static void EInvoicingPassword_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(1014, value, userId, isDefault);
	}

	public static Task<bool> EInvoicingSaveUserAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.EInvoicingSaveUser);
	}

	public static bool EInvoicingSaveUser(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.EInvoicingSaveUser);
	}

	public static Task EInvoicingSaveUser_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(1015, value, userId, isDefault);
	}

	public static void EInvoicingSaveUser_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(1015, value, userId, isDefault);
	}

	public static Task<int> DemoModeDbSizeMBAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.DemoModeDbSizeMB);
	}

	public static int DemoModeDbSizeMB(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.DemoModeDbSizeMB);
	}

	public static Task DemoModeDbSizeMB_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(1016, value);
	}

	public static void DemoModeDbSizeMB_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(1016, value);
	}

	public static Task<int> NotificationsCheckIntervalAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.NotificationsCheckInterval);
	}

	public static int NotificationsCheckInterval(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.NotificationsCheckInterval);
	}

	public static Task NotificationsCheckInterval_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(1017, value);
	}

	public static void NotificationsCheckInterval_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(1017, value);
	}

	public static Task<DateOnly?> BanksLastSyncDateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.BanksLastSyncDate);
	}

	public static DateOnly? BanksLastSyncDate(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.BanksLastSyncDate);
	}

	public static Task BanksLastSyncDate_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(1018, value);
	}

	public static void BanksLastSyncDate_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(1018, value);
	}

	public static Task<string> BanksConnectionModesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.BanksConnectionModes);
	}

	public static string BanksConnectionModes(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.BanksConnectionModes);
	}

	public static Task BanksConnectionModes_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(1019, value);
	}

	public static void BanksConnectionModes_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(1019, value);
	}

	public static Task<string> CPACodeForServicesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.CPACodeForServices);
	}

	public static string CPACodeForServices(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.CPACodeForServices);
	}

	public static Task CPACodeForServices_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(1020, value);
	}

	public static void CPACodeForServices_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(1020, value);
	}
}
