using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.ASReport;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.DataViewsData.Reports;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.Services;
using ArmSoft.TaxService.Models.Invoices;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

[DataView(17)]
public class TradeReceivedTaxInvoices : ReceivedTaxInvoices
{
	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ImportTaxInvoices", "ImportTaxInvoices", Resources.ReceivedTaxInvImport, ImportTaxInvoices);
		}
	}

	private void ImportTaxInvoices()
	{
		List<string> list = new List<string>();
		List<TaxDocumentRepRow> list2 = new List<TaxDocumentRepRow>();
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			List<TaxBaseDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<TaxBaseDataProvider.DataRow>();
			list.Add(Resources.ReceivedTaxInvImport);
			List<TaxDocument> list3 = new List<TaxDocument>();
			foreach (TaxBaseDataProvider.DataRow item in selectedOrFocusedRows)
			{
				if (item.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
				{
					Good taxInvoice = Settings.TaxService.Client.Goods.Get(item.ISN, loadItems: true);
					list3.Add(Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(taxInvoice));
				}
				else if (item.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Excise.Value)
				{
					Excise taxInvoice2 = Settings.TaxService.Client.Excise.Get(item.ISN, loadItems: true);
					list3.Add(Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(taxInvoice2));
				}
				else if (item.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Services.Value)
				{
					Service taxInvoice3 = Settings.TaxService.Client.Services.Get(item.ISN, loadItems: true);
					list3.Add(Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(taxInvoice3));
				}
			}
			foreach (TaxDocument item2 in list3)
			{
				if (!item2.BuyerHasNoTin)
				{
					IReadOnlyList<PartnerCacheRow> partnersByTaxCode = TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode(item2.SupplierTIN);
					if (partnersByTaxCode.Count == 0)
					{
						TradeDirectoryService.StorePartnerFromB2BData(item2.SupplierTIN, item2.SupplierBankAccountNumber, item2.SupplierBankName);
					}
					else
					{
						TradeDirectoryService.StorePartnerBankAccount(partnersByTaxCode[0].PartCode, item2.SupplierBankAccountNumber, item2.SupplierBankName);
					}
				}
			}
			TaxServiceResponse taxServiceResponse = Settings.TaxServiceHelper.MapReceivedTaxDocuments(list3);
			foreach (TaxServiceItems item3 in taxServiceResponse.Items)
			{
				list2.Add(TaxServiceHelper.BuildResultRow(item3.IsAssigned, item3.SerialNumReason));
			}
			list.Add(Convert.ToString(taxServiceResponse.NotAssignedCount));
			list.Add(Convert.ToString(taxServiceResponse.AssignedCount));
			if (list2.Count == 0)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(Resources.NoImportedInv, Resources.ReceivedTaxInvImport, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else
			{
				new TaxDocumentReportViewer.ImportReceivedTaxInvoiceRepViewer(list, list2).Show();
			}
		}
		catch (Exception ex)
		{
			Mouse.OverrideCursor = null;
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Mouse.OverrideCursor = null;
		}
	}
}
