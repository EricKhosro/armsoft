using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.HelpClasses;

namespace ArmSoft.SME.Core.UI.OtherWindows;

public class ChangeVersionsWindow
{
	public ChangeVersionsWindow()
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = "Change versions",
			Name = "ChangeDBVersion"
		};
		dialogWindow.AddStringControl("DBVersion", "DB version", 10, isRequired: true).EditValue = Settings.ParametersService.DBVersionNew();
		dialogWindow.AddStringControl("DBVersionChangeNum", "DB change number", 10).EditValue = Settings.ParametersService.DBVersionChangeNum();
		TextEditExt textEditExt = dialogWindow.AddStringControl("UIVersion", "UI version", 10, isRequired: true);
		Dictionary<string, string> dictionary = RegistryHelper.ReadDictionaryFromRegistry("UIVersions");
		if (dictionary.TryGetValue(Settings.LoginService.Database, out var value))
		{
			textEditExt.EditValue = value;
		}
		TextEditExt textEditExt2 = dialogWindow.AddStringControl("UIVersionChangeNum", "UI change number", 10);
		Dictionary<string, string> dictionary2 = RegistryHelper.ReadDictionaryFromRegistry("UIVersionChangeNums");
		if (dictionary2.TryGetValue(Settings.LoginService.Database, out var value2))
		{
			textEditExt2.EditValue = value2;
		}
		if (Convert.ToBoolean(dialogWindow.ShowDialog()))
		{
			Settings.ParametersService.DBVersionNew_set(Convert.ToString(dialogWindow["DBVersion"]));
			Settings.ParametersService.DBVersionChangeNum_set(Convert.ToInt32(dialogWindow["DBVersionChangeNum"]));
			if (dictionary2.ContainsKey(Settings.LoginService.Database))
			{
				dictionary2[Settings.LoginService.Database] = Convert.ToString(dialogWindow["UIVersionChangeNum"]);
			}
			else
			{
				dictionary2.Add(Settings.LoginService.Database, Convert.ToString(dialogWindow["UIVersionChangeNum"]));
			}
			RegistryHelper.StoreToRegistry("UIVersionChangeNums", dictionary2);
			if (dictionary.ContainsKey(Settings.LoginService.Database))
			{
				dictionary[Settings.LoginService.Database] = Convert.ToString(dialogWindow["UIVersion"]);
			}
			else
			{
				dictionary.Add(Settings.LoginService.Database, Convert.ToString(dialogWindow["UIVersion"]));
			}
			RegistryHelper.StoreToRegistry("UIVersions", dictionary);
		}
	}
}
