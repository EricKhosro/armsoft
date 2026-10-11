using System.Diagnostics;
using System.Windows;
using System.Xml;
using ArmSoft.SME.Client.Data.MetaData;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.MetaData;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews;

[DataView(30)]
public class DataViewDefinitions : DataView<DataViewDefinitionsDataProvider.DataRow, DataViewDefinitionsDataProvider.Param>
{
	public override string HelpId => "Extensions";

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override bool AllowDelete => AllowOperation;

	public DataViewDefinitions()
	{
		DataProvider = new DataViewDefinitionsDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Group");
			dvp.Set_ColumnSortOrder("Name");
		}
	}

	public override void InitContextFunctions()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		DataViewDefinitionsDataProvider.DataRow dataRow = (DataViewDefinitionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ViewToXML", Resources.View, ViewToXML);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportToXML", Resources.Export, ExportToXML);
			if (dataRow.Enabled)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "DisableDataView", Resources.Disable, DisableDataView);
			}
			else
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "EnableDataView", Resources.Enable, EnableDataView);
			}
			if (dataRow.Default)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "SetDefaultOffDataView", Resources.SetDefaultOff, SetDefaultOffDataView);
			}
			else
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "SetDefaultOnDataView", Resources.SetDefaultOn, SetDefaultOnDataView);
			}
		}
	}

	public override void Delete()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		DataViewDefinitionsDataProvider.DataRow dataRow = (DataViewDefinitionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			if (dataRow.System)
			{
				throw new ASException(Resources.SystemDataViewDefDeleteNotAllowed);
			}
			if (MessageBox.Show(string.Format(Resources.ConfDocDel, dataRow.Name), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				Settings.ServiceProvider.GetRequiredService<IDVDService>().Delete(dataRow.Group, dataRow.Name);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.GetKey());
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
		DataViewDefinitionsDataProvider.DataRow dataRow = (DataViewDefinitionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		string text = Settings.ServiceProvider.GetRequiredService<IDVDService>().LoadXML(dataRow.Group, dataRow.Name);
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

	private void DisableDataView()
	{
		ChangeEnabled(value: false);
	}

	private void EnableDataView()
	{
		ChangeEnabled(value: true);
	}

	private static void ChangeEnabled(bool value)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			DataViewDefinitionsDataProvider.DataRow dataRow = (DataViewDefinitionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				Settings.ServiceProvider.GetRequiredService<IDVDService>().ChangeEnabled(dataRow.Group, dataRow.Name, value);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.GetKey());
			}
		}
	}

	private void SetDefaultOffDataView()
	{
		ChangeDefault(value: false);
	}

	private void SetDefaultOnDataView()
	{
		ChangeDefault(value: true);
	}

	private static void ChangeDefault(bool value)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			DataViewDefinitionsDataProvider.DataRow dataRow = (DataViewDefinitionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				Settings.ServiceProvider.GetRequiredService<IDVDService>().ChangeDefault(dataRow.Group, dataRow.Name, value);
				Settings.HostingEnvironment.CurrentViewPanel.Refresh();
			}
		}
	}
}
