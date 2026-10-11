using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.ASReport;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData.Reports;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Models.Invoices;
using DevExpress.Data;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(19)]
public class SubmittedTaxInvoices : TaxBase
{
	public override TaxDocType TaxDocType => TaxDocType.Invoice;

	public override Status TaxDocStatus => Status.Issued;

	public override string HelpId => "e-Invoicing_WriteOutTaxInvoices";

	public SubmittedTaxInvoices()
	{
		DataProvider = new SubmittedTaxInvoicesDataProvider();
		AddParameters();
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("CreationDate", value: false);
		dvp.Set_ColumnVisible("SupplierTin", value: false);
		dvp.Set_ColumnVisible("SupplierName", value: false);
		dvp.Set_ColumnVisible("SupplierAddress", value: false);
		dvp.Set_ColumnVisible("SupplierTaxRegimeCaption", value: false);
		dvp.Set_ColumnVisible("SupplierTaxRegimeStart", value: false);
		dvp.Set_ColumnVisible("FinalUse", value: false);
		dvp.Set_ColumnVisible("CancelledDate", value: false);
		dvp.Set_ColumnVisible("CancelResponseDate", value: false);
		dvp.Set_ColumnVisible("ReceivedOrSubmitted", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("SubmitDate", ColumnSortOrder.Descending);
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "AssignTaxDocumentData", Resources.AssignSeriaNumber, AssignTaxDocumentData);
		}
	}

	protected virtual void AssignTaxDocumentData()
	{
		uint num = 0u;
		uint num2 = 0u;
		InvoiceDetails invoiceDetails = new InvoiceDetails();
		DialogWindow dialogWindow = new DialogWindow
		{
			Name = "AssignTaxInvNums",
			Caption = Resources.AssignTaxInvNums,
			MinWidth = 450.0
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		dialogWindow.AddStoreValuesButton(container);
		dialogWindow.AddBooleanControl(container, "AssignTaxInvNumToDocNum", Resources.AssignTaxInvNumToDocNum, storeValue: true);
		if (!Convert.ToBoolean(dialogWindow.ShowDialog()))
		{
			return;
		}
		List<TaxBaseDataProvider.DataRow> list = new List<TaxBaseDataProvider.DataRow>();
		bool assignTaxNumToDocNum = (bool)dialogWindow["AssignTaxInvNumToDocNum"];
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			list = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<TaxBaseDataProvider.DataRow>();
		}
		if (!Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			return;
		}
		List<AssignTaxInvNumsRepRow> list2 = new List<AssignTaxInvNumsRepRow>();
		foreach (TaxBaseDataProvider.DataRow item5 in list)
		{
			TaxDocumentRepRow taxDocumentRepRow = null;
			bool flag;
			try
			{
				flag = false;
				invoiceDetails.Date = item5.SupplyDate.ToDateTime().Value;
				invoiceDetails.TaxCode = item5.BuyerTin;
				invoiceDetails.Passport = item5.BuyerTin;
				invoiceDetails.Sum = item5.Sum;
				taxDocumentRepRow = Settings.TaxServiceHelper.AssignTaxDocumentData(item5.ISN, item5.TaxSerialNumber, item5.SubmitDate.ToDateOnly(), item5.Sum, assignTaxNumToDocNum, 0);
				if (taxDocumentRepRow.Imported)
				{
					TaxServiceItems item = new TaxServiceItems
					{
						InvoiceDetails = invoiceDetails,
						IsAssigned = taxDocumentRepRow.Imported,
						DocNumbers = (taxDocumentRepRow.Imported ? item5.TaxSerialNumber : ""),
						SerialNumReason = taxDocumentRepRow.SeralNum_Reason
					};
					list2.Add(TaxServiceHelper.BuildResultRow(item));
					num++;
					flag = true;
				}
			}
			catch (Exception ex)
			{
				flag = false;
				_ = ex.Message;
			}
			if (flag)
			{
				continue;
			}
			invoiceDetails.Date = item5.SupplyDate.ToDateTime().Value;
			invoiceDetails.TaxCode = item5.BuyerTin;
			invoiceDetails.Passport = item5.BuyerTin;
			invoiceDetails.Sum = item5.Sum;
			if (item5.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
			{
				Good good = Settings.TaxService.Client.Goods.Get(item5.ISN, loadItems: true);
				TaxDocument taxDocument = Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(good);
				ITaxServiceHelper taxServiceHelper = Settings.TaxServiceHelper;
				int num3 = 1;
				List<TaxDocument> list3 = new List<TaxDocument>(num3);
				CollectionsMarshal.SetCount(list3, num3);
				CollectionsMarshal.AsSpan(list3)[0] = taxDocument;
				TaxServiceResponse taxServiceResponse = taxServiceHelper.MapTaxDocuments(list3, good.DeliveredAt, good.DeliveredAt, assignTaxNumToDocNum);
				if (taxServiceResponse.AssignedCount == 0 || taxDocumentRepRow == null)
				{
					list2.Add(TaxServiceHelper.BuildResultRow(taxServiceResponse.Items[0]));
				}
				else if (taxDocumentRepRow != null)
				{
					TaxServiceItems item2 = new TaxServiceItems
					{
						InvoiceDetails = invoiceDetails,
						IsAssigned = taxDocumentRepRow.Imported,
						DocNumbers = (taxDocumentRepRow.Imported ? item5.TaxSerialNumber : ""),
						SerialNumReason = taxDocumentRepRow.SeralNum_Reason
					};
					list2.Add(TaxServiceHelper.BuildResultRow(item2));
				}
				num += taxServiceResponse.AssignedCount;
				num2 += taxServiceResponse.NotAssignedCount;
			}
			else if (item5.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Services.Value)
			{
				Service service = Settings.TaxService.Client.Services.Get(item5.ISN, loadItems: true);
				TaxDocument taxDocument2 = Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(service);
				ITaxServiceHelper taxServiceHelper2 = Settings.TaxServiceHelper;
				int num3 = 1;
				List<TaxDocument> list4 = new List<TaxDocument>(num3);
				CollectionsMarshal.SetCount(list4, num3);
				CollectionsMarshal.AsSpan(list4)[0] = taxDocument2;
				TaxServiceResponse taxServiceResponse2 = taxServiceHelper2.MapTaxDocuments(list4, service.DeliveredAt, service.DeliveredAt, assignTaxNumToDocNum);
				if (taxServiceResponse2.AssignedCount == 0 || taxDocumentRepRow == null)
				{
					list2.Add(TaxServiceHelper.BuildResultRow(taxServiceResponse2.Items[0]));
				}
				else if (taxDocumentRepRow != null)
				{
					TaxServiceItems item3 = new TaxServiceItems
					{
						InvoiceDetails = invoiceDetails,
						IsAssigned = taxDocumentRepRow.Imported,
						DocNumbers = (taxDocumentRepRow.Imported ? item5.TaxSerialNumber : ""),
						SerialNumReason = taxDocumentRepRow.SeralNum_Reason
					};
					list2.Add(TaxServiceHelper.BuildResultRow(item3));
				}
				num += taxServiceResponse2.AssignedCount;
				num2 += taxServiceResponse2.NotAssignedCount;
			}
			else if (item5.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Excise.Value)
			{
				Excise excise = Settings.TaxService.Client.Excise.Get(item5.ISN, loadItems: true);
				TaxDocument taxDocument3 = Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(excise);
				ITaxServiceHelper taxServiceHelper3 = Settings.TaxServiceHelper;
				int num3 = 1;
				List<TaxDocument> list5 = new List<TaxDocument>(num3);
				CollectionsMarshal.SetCount(list5, num3);
				CollectionsMarshal.AsSpan(list5)[0] = taxDocument3;
				TaxServiceResponse taxServiceResponse3 = taxServiceHelper3.MapTaxDocuments(list5, excise.DeliveredAt, excise.DeliveredAt, assignTaxNumToDocNum);
				if (taxServiceResponse3.AssignedCount == 0 || taxDocumentRepRow == null)
				{
					list2.Add(TaxServiceHelper.BuildResultRow(taxServiceResponse3.Items[0]));
				}
				else if (taxDocumentRepRow != null)
				{
					TaxServiceItems item4 = new TaxServiceItems
					{
						InvoiceDetails = invoiceDetails,
						IsAssigned = taxDocumentRepRow.Imported,
						DocNumbers = (taxDocumentRepRow.Imported ? item5.TaxSerialNumber : ""),
						SerialNumReason = taxDocumentRepRow.SeralNum_Reason
					};
					list2.Add(TaxServiceHelper.BuildResultRow(item4));
				}
				num += taxServiceResponse3.AssignedCount;
				num2 += taxServiceResponse3.NotAssignedCount;
			}
		}
		List<string> list6 = new List<string>
		{
			Resources.AssignTaxInvNums + " " + Resources.EInvoicing,
			Convert.ToString(num),
			Convert.ToString(num2)
		};
		if (!string.IsNullOrWhiteSpace(list6[0]))
		{
			new TaxDocumentReportViewer.AssignInvNumReportViewer(list6, list2, shortMode: false).Show();
		}
	}
}
