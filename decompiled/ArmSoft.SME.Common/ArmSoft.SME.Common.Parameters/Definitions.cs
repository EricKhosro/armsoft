using System;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Helpers;

namespace ArmSoft.SME.Common.Parameters;

public class Definitions : AllParamDefinitions
{
	public static ParameterDefinition<decimal> VAT => new ParameterDefinition<decimal>(10, () => Resources.OptionsVAT, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, 20m);

	public static ParameterDefinition<string> VATType => new ParameterDefinition<string>(344, () => Resources.VATType, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, "1");

	public static ParameterDefinition<int> DBVersion => new ParameterDefinition<int>(2, () => Resources.DBVersion, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> CompanyName => new ParameterDefinition<string>(50, () => Resources.CompanyName, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true);

	public static ParameterDefinition<bool> QuestWhenCloseWind => new ParameterDefinition<bool>(35, () => Resources.ConfirmWhenClosingWindow, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> TaxCode => new ParameterDefinition<string>(63, () => Resources.OptionsTaxCode, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true);

	public static ParameterDefinition<bool> SpreadsheetMode => new ParameterDefinition<bool>(350, () => string.Format(Resources.SpreadSheetMode, Environment.NewLine), userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> FilterPopupModeExcel => new ParameterDefinition<bool>(373, () => Resources.FilterPopupModeExcel, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AutoFilterRowOldStyle => new ParameterDefinition<bool>(378, () => Resources.AutoFilterRowOldStyle, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> DBExclusiveMode => new ParameterDefinition<bool>(374, () => Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> DBExclusiveModeMsg => new ParameterDefinition<string>(375, () => Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> AutoBackUp => new ParameterDefinition<bool>(21, () => Resources.OptionAutoBackUpTab, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<int> AutoBackUpInterval => new ParameterDefinition<int>(22, () => Resources.OptionAutoBackUpDay, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 1);

	public static ParameterDefinition<bool> BackUpPrompt => new ParameterDefinition<bool>(23, () => Resources.OptionAutoBackUpPrompt, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> OnlineBackUp => new ParameterDefinition<bool>(370, () => Resources.OnlineBackups, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> LocalBackUp => new ParameterDefinition<bool>(371, () => Resources.LocalBackups, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> LocalBackUpPath => new ParameterDefinition<string>(372, () => Resources.SelectLocalDefaultBackupPath, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DropBoxAccountEMail => new ParameterDefinition<string>(376, () => Resources.DropboxUser, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DropBoxAccountToken => new ParameterDefinition<string>(377, () => Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> UserAWPNavigatorName => new ParameterDefinition<string>(7010, () => Resources.AssignUserAWPNavName, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> UserCardNumber => new ParameterDefinition<string>(379, () => Resources.Unknown, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> AllowOpFromHistoryPanel => new ParameterDefinition<bool>(385, () => Resources.AllowOpFromHistoryPanel, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> UseWindowsNotifications => new ParameterDefinition<bool>(386, () => Resources.UseWindowsOSNotifications, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<int> QueryTimeout => new ParameterDefinition<int>(387, () => Resources.QueryTimeout, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, 30);

	public static ParameterDefinition<int> ReportQueryTimeout => new ParameterDefinition<int>(388, () => Resources.ReportQueryTimeout, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, 300);

	public static ParameterDefinition<string> DBVersionNew => new ParameterDefinition<string>(1000, () => Resources.DBVersion, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "9.0.0");

	public static ParameterDefinition<int> DBVersionChangeNum => new ParameterDefinition<int>(1001, () => Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, 0);

	public static ParameterDefinition<bool> IsBetaVersion => new ParameterDefinition<bool>(1002, () => Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, defaultValue: false);

	public static ParameterDefinition<string> FavoriteItems => new ParameterDefinition<string>(6000, () => Resources.RibbonPageFavorites, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> NavigatorState => new ParameterDefinition<string>(7000, () => Resources.Explorer, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DefaultTemplate => new ParameterDefinition<string>(400, () => Resources.PrintTemplate, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> KeepUICacheUpToDate => new ParameterDefinition<bool>(1101, () => Resources.KeepUICacheUpToDate, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ImportExchangeRates => new ParameterDefinition<bool>(219, () => Resources.ImportExchangeRatesOnLogin, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> StoreImagesLocal => new ParameterDefinition<bool>(1004, () => Resources.StoreImagesLocal, userParam: true, roleParam: false, useFromCacheService: true, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<int> SumPrecisionDigitsCount => new ParameterDefinition<int>(1005, () => Resources.PrecisionDigitsCount, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, 2);

	public static ParameterDefinition<int> QuantityPrecisionDigitsCount => new ParameterDefinition<int>(1006, () => "Resources.Unknown", userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, 3);

	public static ParameterDefinition<string> UserPIN => new ParameterDefinition<string>(1007, () => Resources.PINCode, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<DateOnly?> CPACodesLastSyncDate => new ParameterDefinition<DateOnly?>(1008, () => Resources.RefreshCPACodes, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: false, ArmSoft.SME.Common.Constants.Common.MinDateForCPACodes);

	public static ParameterDefinition<bool> AutoSave => new ParameterDefinition<bool>(1009, () => Resources.AutoSave, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<int> AutoSaveInterval => new ParameterDefinition<int>(1010, () => Resources.AutoSaveInterval, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 5);

	public static ParameterDefinition<bool> AllowEditCodeOfDirectories => new ParameterDefinition<bool>(1011, () => Resources.EditCodeOfDirectories, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> EInvoicingTin => new ParameterDefinition<string>(1012, () => "Resources.Unknown", userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: false, EncryptHelper.Encrypt(""));

	public static ParameterDefinition<string> EInvoicingLogin => new ParameterDefinition<string>(1013, () => "Resources.Unknown", userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: false, EncryptHelper.Encrypt(""));

	public static ParameterDefinition<string> EInvoicingPassword => new ParameterDefinition<string>(1014, () => "Resources.Unknown", userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: false, EncryptHelper.Encrypt(""));

	public static ParameterDefinition<bool> EInvoicingSaveUser => new ParameterDefinition<bool>(1015, () => "Resources.Unknown", userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: false, defaultValue: false);

	public static ParameterDefinition<int> DemoModeDbSizeMB => new ParameterDefinition<int>(1016, () => "Resources.Unknown", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, 70);

	public static ParameterDefinition<int> NotificationsCheckInterval => new ParameterDefinition<int>(1017, () => Resources.CheckFrequencyInSeconds, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, 300);

	public static ParameterDefinition<DateOnly?> BanksLastSyncDate => new ParameterDefinition<DateOnly?>(1018, () => Resources.ImportBanksFromB2B, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: false, null);

	public static ParameterDefinition<string> BanksConnectionModes => new ParameterDefinition<string>(1019, () => Resources.Banks, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "{\"157\":\"0\",\"115\":\"0\",\"151\":\"0\",\"250\":\"0\",\"214\":\"0\",\"166\":\"0\",\"193\":\"0\",\"247\":\"0\",\"163\":\"0\",\"777\":\"0\",\"888\":\"0\"}");

	public static ParameterDefinition<string> CPACodeForServices => new ParameterDefinition<string>(1020, () => Resources.CPACodeForServices, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "4");
}
