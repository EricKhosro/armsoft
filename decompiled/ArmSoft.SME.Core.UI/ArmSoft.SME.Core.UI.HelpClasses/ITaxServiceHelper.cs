using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.ASReport;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.TaxService.Models.AccDocs;
using ArmSoft.TaxService.Models.Invoices;

namespace ArmSoft.SME.Core.UI.HelpClasses;

public interface ITaxServiceHelper
{
	string ShowSaveFileDialog(string fileName);

	bool TaxServiceAuthenticationWithSavedUser(out string exceptionMessage);

	bool EnsureTaxServiceAuthenticationCompleted();

	void LogOut();

	TaxDocument ConvertTaxInvoiceToTaxDocument(Good taxInvoice);

	TaxDocument ConvertTaxInvoiceToTaxDocument(Excise taxInvoice);

	TaxDocument ConvertTaxInvoiceToTaxDocument(Service taxInvoice);

	TaxDocument ConvertTaxInvoiceToTaxDocument(VatRefund taxInvoice);

	TaxDocument ConvertTaxInvoiceToTaxDocument(Leasing taxInvoice);

	TaxDocument ConvertTaxAccDocToTaxDocument(AccDocGoods taxAccDoc);

	TaxDocument ConvertTaxAccDocToTaxDocument(AccDocTransportation taxAccDoc);

	TaxDocument ConvertTaxAccDocToTaxDocument(AccDocServices taxAccDoc);

	TaxDocument ConvertTaxAccDocToTaxDocument(AccDocExportEEU taxAccDoc);

	TaxDocument ConvertTaxAccDocToTaxDocument(AccDocTraceableG taxAccDoc);

	TaxServiceResponse FindMTMoveDocuments(List<AccDoc> taxDocuments, bool assignTaxNumToDocNum);

	TaxServiceResponse MapTaxDocuments(List<TaxDocument> taxDocuments, DateTime minDate, DateTime maxDate, bool assignTaxNumToDocNum);

	TaxDocumentRepRow AssignTaxDocumentData(Guid isn, string taxSeriaNumber, DateOnly? submitDate, decimal total = 0m, bool assignTaxNumToDocNum = false, short docType = 0);

	TaxServiceResponse MapReceivedTaxDocuments(List<TaxDocument> taxDocuments);

	List<TaxDocumentRepRow> ImportTaxDocuments(List<TaxDocument> taxDocuments);

	Guid? GetProductTaxISN(Guid? docISN, int itemId, int rowId = -1, string operation = null);
}
