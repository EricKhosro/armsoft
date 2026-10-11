using System.Diagnostics;
using System.Windows;
using System.Xml;
using ArmSoft.SME.Client.Data.MetaData;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.MetaData;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews;

[DataView(28)]
public class UserScriptsDefinitions : DataView<UserScriptsDefinitionsDataProvider.DataRow, UserScriptsDefinitionsDataProvider.Param>
{
	public override string HelpId => "Extensions";

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override bool AllowDelete => AllowOperation;

	public UserScriptsDefinitions()
	{
		DataProvider = new UserScriptsDefinitionsDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Name");
		}
	}

	public override void InitContextFunctions()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ViewToXML", Resources.View, ViewToXML);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportToXML", Resources.Export, ExportToXML);
		}
	}

	public override void Delete()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			UserScriptsDefinitionsDataProvider.DataRow dataRow = (UserScriptsDefinitionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null && MessageBox.Show(string.Format(Resources.ConfDocDel, dataRow.Name), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				Settings.ServiceProvider.GetRequiredService<IUserScriptService>().Delete(dataRow.Name);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Name);
			}
		}
	}

	private void ViewToXML()
	{
		ExportOrViewXML(isExport: false);
	}

	private void ExportToXML()
	{
		ExportOrViewXML(isExport: true);
	}

	private static void ExportOrViewXML(bool isExport)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		UserScriptsDefinitionsDataProvider.DataRow dataRow = (UserScriptsDefinitionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		string text = Settings.ServiceProvider.GetRequiredService<IUserScriptService>().LoadXML(dataRow.Name);
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		string text2 = (isExport ? Environment.GetFileNameForExportToXML() : Settings.GetTempFileName(".xml"));
		if (!string.IsNullOrWhiteSpace(text2))
		{
			Settings.CommonCoreHelper.SaveXMLToFile(xmlDocument, text2);
			if (!isExport)
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = text2,
					UseShellExecute = true
				});
			}
		}
	}
}
