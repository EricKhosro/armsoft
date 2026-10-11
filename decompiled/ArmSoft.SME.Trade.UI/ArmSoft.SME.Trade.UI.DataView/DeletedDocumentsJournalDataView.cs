using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10117)]
public class DeletedDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public DeletedDocumentsJournalDataView()
		: base(ReportTypesSE.DeletedDocument, (DocumentTypeSE)null)
	{
		DataProvider = new DeletedDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}

	public override void Delete()
	{
		DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(Resources.ConfDocFullDelete, Settings.DocumentService.GetFullCaption(dataRow.GetDocType())), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			DefinitionMappingsService.TryResolveDocumentDefinition(dataRow.DocType, out var definition);
			if (definition.IsExchangeable)
			{
				string fileName = Settings.GetExportDirectory() + "\\Export_DeletedDocuments" + DateTime.Now.ToString("MMdd_HHmmss") + ".xml";
				Settings.HostingEnvironment.CurrentViewPanel.ExportToXML(fileName);
			}
			Settings.DocumentService.Delete(dataRow.ISN, new PODeleteRequest
			{
				IsFullDelete = true
			});
			Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.GetDocISN());
		}
	}

	public override void DeleteSelected()
	{
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(Resources.DeleteErrors, "DeleteSelectedErrors");
		List<DocumentsJournalDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DocumentsJournalDataProvider.DataRow>();
		if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(Resources.ConfSelDocFullDelete, selectedRows.Count), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
		{
			return;
		}
		string fileName = Settings.GetExportDirectory() + "\\Export_DeletedDocuments" + DateTime.Now.ToString("MMdd_HHmmss") + ".xml";
		if (Settings.HostingEnvironment.CurrentViewPanel.IDV.ExistExchangeableRow)
		{
			Settings.HostingEnvironment.CurrentViewPanel.ExportToXML(fileName);
		}
		DocumentDeleteSelectedRequestModel request = new DocumentDeleteSelectedRequestModel
		{
			DocISNs = selectedRows.Select((DocumentsJournalDataProvider.DataRow row) => row.ISN).ToList(),
			DeleteRequest = new PODeleteRequest
			{
				IsFullDelete = true,
				WithoutValidation = true
			}
		};
		DocumentDeleteSelectedResponseModel documentDeleteSelectedResponseModel = DataProcessingRequestHelper.Execute<DocumentDeleteSelectedRequestModel, DocumentDeleteSelectedResponseModel>(DPRType.Other, "DeleteSelectedDocuments", request);
		if (documentDeleteSelectedResponseModel.Exceptions != null && documentDeleteSelectedResponseModel.Exceptions.Count != 0)
		{
			foreach (DocumentDeleteResponseModel exception in documentDeleteSelectedResponseModel.Exceptions)
			{
				enumerateErrorsReportViewer.AddError(exception.Exception.Message, exception.DocISN);
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
		ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(Resources.RemovedDocsCount, documentDeleteSelectedResponseModel.SuccessCount), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
	}
}
