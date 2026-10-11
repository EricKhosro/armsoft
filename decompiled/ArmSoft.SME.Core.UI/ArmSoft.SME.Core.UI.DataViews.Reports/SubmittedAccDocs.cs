using System;
using System.Collections.Generic;
using System.Linq;
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
using ArmSoft.TaxService.Models.AccDocs;
using DevExpress.Data;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(20)]
public class SubmittedAccDocs : TaxBase
{
	public override TaxDocType TaxDocType => TaxDocType.AccDoc;

	public override Status TaxDocStatus => Status.Issued;

	public override string HelpId => "e-Invoicing_WriteOutAccountingDoc";

	public SubmittedAccDocs()
	{
		DataProvider = new SubmittedAccDocsDataProvider();
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
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "AssignTaxAccDocumentData", Resources.AssignSeriaNumber, AssignTaxAccDocumentData);
		}
	}

	protected virtual void AssignTaxAccDocumentData()
	{
		string serialNumReason = "";
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
		bool assignTaxNumToDocNum = (bool)dialogWindow["AssignTaxInvNumToDocNum"];
		List<TaxBaseDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<TaxBaseDataProvider.DataRow>();
		if (!Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			return;
		}
		List<AssignTaxInvNumsRepRow> list = new List<AssignTaxInvNumsRepRow>();
		bool shortMode = false;
		foreach (TaxBaseDataProvider.DataRow item5 in selectedOrFocusedRows)
		{
			TaxDocumentRepRow taxDocumentRepRow = null;
			bool flag;
			try
			{
				flag = false;
				invoiceDetails.Date = item5.SupplyDate.ToDateTime().Value;
				invoiceDetails.TaxCode = item5.BuyerTin;
				invoiceDetails.Passport = item5.BuyerTin;
				invoiceDetails.Sum = item5.SumWithoutVAT;
				taxDocumentRepRow = Settings.TaxServiceHelper.AssignTaxDocumentData(item5.ISN, item5.TaxSerialNumber, item5.SubmitDate.ToDateOnly(), item5.SumWithoutVAT, assignTaxNumToDocNum, 0);
				if (taxDocumentRepRow.Imported)
				{
					TaxServiceItems item = new TaxServiceItems
					{
						InvoiceDetails = invoiceDetails,
						IsAssigned = taxDocumentRepRow.Imported,
						DocNumbers = (taxDocumentRepRow.Imported ? item5.TaxSerialNumber : ""),
						SerialNumReason = serialNumReason
					};
					list.Add(TaxServiceHelper.BuildResultRow(item));
					num++;
					flag = true;
				}
			}
			catch (Exception ex)
			{
				flag = false;
				serialNumReason = ex.Message;
			}
			if (flag)
			{
				continue;
			}
			if (item5.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocGoods.Value)
			{
				AccDocGoods accDocGoods = Settings.TaxService.Client.AccDocGoods.Get(item5.ISN, loadItems: true);
				TaxDocument taxDocument = Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument(accDocGoods);
				ITaxServiceHelper taxServiceHelper = Settings.TaxServiceHelper;
				int num3 = 1;
				List<TaxDocument> list2 = new List<TaxDocument>(num3);
				CollectionsMarshal.SetCount(list2, num3);
				CollectionsMarshal.AsSpan(list2)[0] = taxDocument;
				TaxServiceResponse taxServiceResponse = taxServiceHelper.MapTaxDocuments(list2, accDocGoods.DeliveredAt, accDocGoods.DeliveredAt, assignTaxNumToDocNum);
				if (taxServiceResponse.AssignedCount == 0 || taxDocumentRepRow == null)
				{
					list.Add(TaxServiceHelper.BuildResultRow(taxServiceResponse.Items[0]));
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
					list.Add(TaxServiceHelper.BuildResultRow(item2));
				}
				num += taxServiceResponse.AssignedCount;
				num2 += taxServiceResponse.NotAssignedCount;
			}
			else if (item5.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocTransportation.Value)
			{
				AccDocTransportation accDocTransportation = Settings.TaxService.Client.AccDocTransportation.Get(item5.ISN, loadItems: true);
				AccDoc accDoc = new AccDoc();
				shortMode = true;
				accDoc.DocDate = item5.SupplyDate.ToDateTime().Value;
				accDoc.SubmissionDate = item5.SubmitDate.ToDateOnly();
				accDoc.TaxSeries = item5.TaxSerialNumber.Left(1);
				accDoc.TaxNumber = item5.TaxSerialNumber.Mid(2);
				accDoc.TotalQuantity = accDocTransportation.Items.Sum((AccDocTransportation.Item x) => x.Quantity);
				accDoc.RowCount = accDocTransportation.Items.Count;
				accDoc.AdditionalData = accDocTransportation.AdditionalInfo;
				ITaxServiceHelper taxServiceHelper2 = Settings.TaxServiceHelper;
				int num3 = 1;
				List<AccDoc> list3 = new List<AccDoc>(num3);
				CollectionsMarshal.SetCount(list3, num3);
				CollectionsMarshal.AsSpan(list3)[0] = accDoc;
				TaxServiceResponse taxServiceResponse2 = taxServiceHelper2.FindMTMoveDocuments(list3, assignTaxNumToDocNum);
				if (taxServiceResponse2.AssignedCount == 0 || taxDocumentRepRow == null)
				{
					list.Add(TaxServiceHelper.BuildResultRow(taxServiceResponse2.Items[0]));
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
					list.Add(TaxServiceHelper.BuildResultRow(item3));
				}
				num += taxServiceResponse2.AssignedCount;
				num2 += taxServiceResponse2.NotAssignedCount;
			}
			else if (item5.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocServices.Value)
			{
				AccDocServices accDocServices = Settings.TaxService.Client.AccDocServices.Get(item5.ISN, loadItems: true);
				TaxDocument taxDocument2 = Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument(accDocServices);
				ITaxServiceHelper taxServiceHelper3 = Settings.TaxServiceHelper;
				int num3 = 1;
				List<TaxDocument> list4 = new List<TaxDocument>(num3);
				CollectionsMarshal.SetCount(list4, num3);
				CollectionsMarshal.AsSpan(list4)[0] = taxDocument2;
				TaxServiceResponse taxServiceResponse3 = taxServiceHelper3.MapTaxDocuments(list4, accDocServices.DeliveredAt, accDocServices.DeliveredAt, assignTaxNumToDocNum);
				if (taxServiceResponse3.AssignedCount == 0 || taxDocumentRepRow == null)
				{
					list.Add(TaxServiceHelper.BuildResultRow(taxServiceResponse3.Items[0]));
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
					list.Add(TaxServiceHelper.BuildResultRow(item4));
				}
				num += taxServiceResponse3.AssignedCount;
				num2 += taxServiceResponse3.NotAssignedCount;
			}
		}
		List<string> list5 = new List<string>
		{
			Resources.AssignTaxInvNums + " " + Resources.EInvoicing,
			Convert.ToString(num),
			Convert.ToString(num2)
		};
		if (!string.IsNullOrWhiteSpace(list5[0]))
		{
			new TaxDocumentReportViewer.AssignInvNumReportViewer(list5, list, shortMode).Show();
		}
	}
}
