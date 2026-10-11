using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.MetaData.Document;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews.MetaData;

public abstract class POExtensions : DataView<DocumentExtensionsDataProvider.DataRow, DocumentExtensionsDataProvider.Param>
{
	protected abstract POTypeSE POTypeDef { get; }

	private static IDocumentExtensionService DocumentExtensionService => Settings.ServiceProvider.GetRequiredService<IDocumentExtensionService>();

	public override string HelpId => "Extensions";

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowAdd => AllowOperation;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowExport => true;

	public POExtensions()
	{
		DataProvider = new DocumentExtensionsDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.POTypeDef = POTypeDef;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("POType");
		}
	}

	public override void Add()
	{
		DocumentExtensionsWindow documentExtensionsWindow = new DocumentExtensionsWindow(DialogWindowMode.NewMode, POTypeDef);
		if (documentExtensionsWindow.ShowDialog().Value)
		{
			DocumentExtensionModel documentExtensionModel = new DocumentExtensionModel
			{
				POTypeDef = documentExtensionsWindow.DocumentExtensionDefinition.POTypeDef,
				POType = documentExtensionsWindow.DocumentExtensionDefinition.POType,
				Description = documentExtensionsWindow.Description.Text,
				DocumentValidators = documentExtensionsWindow.DocumentValidators,
				ExtensionXml = documentExtensionsWindow.DocumentExtensionDefinition.ToXmlString()
			};
			DocumentExtensionService.Store(documentExtensionModel);
			Settings.HostingEnvironment.CurrentViewPanel.Update(new DocumentExtensionKey(documentExtensionModel.POTypeDef, documentExtensionModel.POType));
		}
	}

	public override void Edit()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		DocumentExtensionsDataProvider.DataRow dataRow = (DocumentExtensionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			DocumentExtensionModel documentExtensionModel = DocumentExtensionService.Load(new DocumentExtensionRequest(dataRow.POTypeDef, dataRow.POType));
			DocumentExtensionsWindow documentExtensionsWindow = new DocumentExtensionsWindow(DialogWindowMode.EditMode, POTypeDef, documentExtensionModel);
			if (documentExtensionsWindow.ShowDialog().Value)
			{
				documentExtensionModel.DocumentValidators = documentExtensionsWindow.DocumentValidators;
				documentExtensionModel.Description = (string)documentExtensionsWindow["Description"];
				documentExtensionModel.ExtensionXml = documentExtensionsWindow.DocumentExtensionDefinition.ToXmlString();
				DocumentExtensionService.Store(documentExtensionModel);
				Settings.HostingEnvironment.CurrentViewPanel.Update(new DocumentExtensionKey(dataRow.POTypeDef, dataRow.POType));
			}
		}
	}

	public override void Delete()
	{
		if (MessageBox.Show(Resources.ConfExtDel, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			Mouse.OverrideCursor = Cursors.Wait;
			DocumentExtensionsDataProvider.DataRow dataRow = (DocumentExtensionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				DocumentExtensionService.Delete(dataRow.POTypeDef, dataRow.POType);
				Settings.HostingEnvironment.CurrentViewPanel.Update(new DocumentExtensionKey(dataRow.POTypeDef, dataRow.POType));
			}
			Mouse.OverrideCursor = null;
		}
	}

	public override void ExportToXML(string fileName, DocumentExtensionsDataProvider.DataRow row)
	{
		DocumentExtensionsDataProvider.DataRow dataRow = (DocumentExtensionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			string extensionXml = DocumentExtensionService.Load(new DocumentExtensionRequest(dataRow.POTypeDef, dataRow.POType)).ExtensionXml;
			if (!string.IsNullOrWhiteSpace(extensionXml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(extensionXml);
				Settings.CommonCoreHelper.SaveXMLToFile(xmlDocument, fileName);
				Process.Start(new ProcessStartInfo
				{
					FileName = fileName,
					UseShellExecute = true
				});
			}
		}
	}
}
