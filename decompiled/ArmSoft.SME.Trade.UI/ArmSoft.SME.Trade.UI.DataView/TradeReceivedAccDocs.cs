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
using ArmSoft.TaxService.Models.AccDocs;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(18)]
public class TradeReceivedAccDocs : ReceivedAccDocs
{
	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ImportAccDocs", "ImportAccDocs", Resources.ReceivedTaxInvImport, ImportAccDocs);
		}
	}

	private void ImportAccDocs()
	{
		try
		{
			List<TaxBaseDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<TaxBaseDataProvider.DataRow>();
			List<string> list = new List<string> { Resources.ReceivedTaxInvImport };
			List<TaxDocument> list2 = new List<TaxDocument>();
			foreach (TaxBaseDataProvider.DataRow item in selectedOrFocusedRows)
			{
				if (item.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocGoods.Value)
				{
					AccDocGoods taxAccDoc = Settings.TaxService.Client.AccDocGoods.Get(item.ISN, loadItems: true);
					list2.Add(Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument(taxAccDoc));
				}
				else if (item.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocServices.Value)
				{
					AccDocServices taxAccDoc2 = Settings.TaxService.Client.AccDocServices.Get(item.ISN, loadItems: true);
					list2.Add(Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument(taxAccDoc2));
				}
			}
			foreach (TaxDocument item2 in list2)
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
			TaxServiceResponse taxServiceResponse = Settings.TaxServiceHelper.MapReceivedTaxDocuments(list2);
			List<TaxDocumentRepRow> list3 = new List<TaxDocumentRepRow>();
			foreach (TaxServiceItems item3 in taxServiceResponse.Items)
			{
				list3.Add(TaxServiceHelper.BuildResultRow(item3.IsAssigned, item3.SerialNumReason));
			}
			list.Add(Convert.ToString(taxServiceResponse.NotAssignedCount));
			list.Add(Convert.ToString(taxServiceResponse.AssignedCount));
			if (list3.Count == 0)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(Resources.NoImportedInv, Resources.ReceivedTaxInvImport, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else
			{
				new TaxDocumentReportViewer.ImportReceivedTaxInvoiceRepViewer(list, list3).Show();
			}
		}
		catch (Exception ex)
		{
			Mouse.OverrideCursor = null;
			Settings.ErrorHandler(ex);
		}
	}
}
