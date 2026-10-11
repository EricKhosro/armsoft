using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using ArmSoft.SME.Client.Data.Trees;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.MetaData.Tree;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.MetaData.Tree;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews.TreeNodes;

[DataView(29)]
public class Trees : DataView<TreesDataProvider.DataRow, TreesDataProvider.Param>
{
	public override bool AllowAdd => true;

	public override bool AllowEdit => true;

	public override bool AllowDelete => true;

	public Trees()
	{
		DataProvider = new TreesDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Id");
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		TreesDataProvider.DataRow dataRow = (TreesDataProvider.DataRow)currentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			currentViewPanel.AddContextFunction("ContextFunctions", "ViewTreeNodes", Resources.ViewNodes, ViewTreeNodes);
			currentViewPanel.AddContextMenuGroup("Extension", Resources.Extension);
			currentViewPanel.AddContextFunction("Extension", "EditExtension", dataRow.HasExtension ? Resources.Edit : Resources.Add, EditExtension);
			if (dataRow.HasExtension)
			{
				currentViewPanel.AddContextFunction("Extension", "DeleteExtension", Resources.Delete, DeleteExtension);
				currentViewPanel.AddContextFunction("Extension", "ViewExtensionToXML", Resources.ViewToXML, ViewExtensionToXML);
				currentViewPanel.AddContextFunction("Extension", "ExportToXML", Resources.ExportToXML, ExportExtensionToXML);
			}
		}
	}

	private static void ExportOrViewXML(bool isExport)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		TreesDataProvider.DataRow dataRow = (TreesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		string text = Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().LoadXml(dataRow.Id);
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

	public override void Add()
	{
		TreeAddEditDialog treeAddEditDialog = new TreeAddEditDialog();
		if (treeAddEditDialog.ShowDialog().Value && !string.IsNullOrWhiteSpace(treeAddEditDialog.TreeId))
		{
			Settings.HostingEnvironment.CurrentViewPanel.Update(treeAddEditDialog.TreeId);
		}
	}

	public override void Delete()
	{
		TreesDataProvider.DataRow dataRow = (TreesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			if (dataRow.System)
			{
				throw new ASException(Resources.SystemTreeDefDeleteNotAllowed);
			}
			if (MessageBox.Show(string.Format(Resources.ConfRecordDel, dataRow.Id), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				Mouse.OverrideCursor = Cursors.Wait;
				Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().Delete(dataRow.Id);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Id);
				Mouse.OverrideCursor = null;
			}
		}
	}

	public override void Edit()
	{
		TreesDataProvider.DataRow dataRow = (TreesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			if (Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().Get(dataRow.Id) == null)
			{
				throw new RESTException(string.Format(Resources.DataItemNotExists, dataRow.Id));
			}
			if (new TreeAddEditDialog(dataRow.System ? DialogWindowMode.ViewMode : DialogWindowMode.EditMode, dataRow.Id).ShowDialog().Value)
			{
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Id);
			}
		}
	}

	private void ViewTreeNodes()
	{
		TreesDataProvider.DataRow dataRow = (TreesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			new TreeNodes(dataRow.Id).Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}

	private void EditExtension()
	{
		TreesDataProvider.DataRow dataRow = (TreesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			TreeExtensionDefinitionEx treeExtensionDefinitionEx = Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().LoadExtensionDefinition(dataRow.Id);
			TreeExtensionDialog treeExtensionDialog = new TreeExtensionDialog(dataRow.Id, (treeExtensionDefinitionEx == null) ? DialogWindowMode.NewMode : DialogWindowMode.EditMode, treeExtensionDefinitionEx, Settings.HostingEnvironment);
			if (treeExtensionDialog.ShowDialog().Value)
			{
				Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().StoreExtensionXml(new TreeExtensionModel(dataRow.Id, treeExtensionDialog.TreeExtension?.ToXmlString()));
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Id);
			}
		}
	}

	private void DeleteExtension()
	{
		if (MessageBox.Show(Resources.ConfExtDel, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			Mouse.OverrideCursor = Cursors.Wait;
			TreesDataProvider.DataRow dataRow = (TreesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().DeleteExtension(new TreeExtensionModel(dataRow.Id, ""));
			Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Id);
			Mouse.OverrideCursor = null;
		}
	}

	private void ViewExtensionToXML()
	{
		ExportOrViewXML(isExport: false);
	}

	private void ExportExtensionToXML()
	{
		ExportOrViewXML(isExport: true);
	}

	public override void ExportToXML(string fileName, TreesDataProvider.DataRow row)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		TreesDataProvider.DataRow dataRow = (TreesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		string text = Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().LoadXml(dataRow.Id);
		if (!string.IsNullOrWhiteSpace(text))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(text);
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				Settings.CommonCoreHelper.SaveXMLToFile(xmlDocument, fileName);
			}
		}
	}
}
