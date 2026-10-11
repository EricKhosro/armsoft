using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class AsFontsPanel : UserControl, IComponentConnector
{
	public AsFontsPanel()
	{
		InitializeComponent();
		FontSizeLayoutItem.Label = ArmSoft.SME.Common.Resources.FontSize;
		ThemeLayoutItem.Label = ArmSoft.SME.Common.Resources.Themes;
		liDarkThemeCheckBox.Label = ArmSoft.SME.Common.Resources.UseDarkTheme;
	}

	public void FillSettings()
	{
		FontSizeComboBox.EditValue = Settings.GetFontSize(getFromRegistry: true);
		FontSizeComboBox.Items.AddRange(new object[5] { "10", "11", "12", "13", "14" });
		DarkThemeCheckBox.IsChecked = Settings.IsApplicationThemeDark();
		ThemeComboBox.EditValue = Settings.ApplicationTheme();
		ThemeComboBox.Items.AddRange(new object[2]
		{
			ApplicationThemeSE.Office.Value,
			ApplicationThemeSE.Win11.Value
		});
	}

	public bool SaveSettings()
	{
		bool flag = false;
		if (DarkThemeCheckBox.IsChecked != Settings.IsApplicationThemeDark())
		{
			bool flag2 = DarkThemeCheckBox.IsChecked.HasValue && DarkThemeCheckBox.IsChecked.Value;
			RegistryHelper.StoreToRegistry("ApplicationThemeIsDark", flag2);
			flag = true;
		}
		double num = RegistryHelper.ReadDoubleFromRegistry("FontSize");
		string text = RegistryHelper.ReadStringFromRegistry("ApplicationTheme");
		flag = flag || num != Convert.ToDouble(FontSizeComboBox.EditValue) || text != (string)ThemeComboBox.EditValue;
		RegistryHelper.StoreToRegistry("FontSize", Convert.ToDouble(FontSizeComboBox.EditValue).ToString());
		((DependencyObject)Settings.HostingEnvironment).SetValue(Control.FontSizeProperty, (object)Convert.ToDouble(FontSizeComboBox.EditValue));
		RegistryHelper.StoreToRegistry("ApplicationTheme", (string)ThemeComboBox.EditValue);
		return flag;
	}
}
