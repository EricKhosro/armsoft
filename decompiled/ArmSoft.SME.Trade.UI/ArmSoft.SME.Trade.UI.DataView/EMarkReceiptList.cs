using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ArmSoft.EMarkReceiptClient.Configuration;
using ArmSoft.EMarkReceiptClient.Exceptions;
using ArmSoft.EMarkReceiptClient.Models;
using ArmSoft.EMarkReceiptClient.Services;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10128)]
public class EMarkReceiptList : TradeDataView<EMarkReceiptListDataProvider.DataRow, EMarkReceiptListDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly EMarkReceiptList _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly DropDownControl _material;

		private readonly DropDownControl _docState;

		public DataViewDialog(EMarkReceiptList dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : Settings.HostingEnvironment.DateBegin);
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : Settings.HostingEnvironment.DateEnd);
			AddSpecialDateButton(container, _startDate, _endDate);
			_material = AddDropDownControl("MTCode", ArmSoft.SME.Common.Resources.Product, new GoodsFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_material.Code = (useParamValues ? dataView.MTCode : "");
			_docState = AddDropDownControl("DocState", ArmSoft.SME.Common.Resources.State, new DocumentState(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_docState.Code = (useParamValues ? dataView.DocState : DocumentStateSE.DraftOrProcessed.ValueUI);
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue.Value;
			_dataView.EndDate = _endDate.EditValue.Value;
			_dataView.MTCode = _material.Code;
			_dataView.DocState = _docState.Code;
		}
	}

	public override bool DialogSupported => true;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowDocumentHistory => true;

	public override string HelpId => "list_of_stamps_withdrawn_from_circulation";

	public DateOnly StartDate
	{
		get
		{
			return ((DateTime)Parameters["StartDate"].Value).ToDateOnly();
		}
		set
		{
			Parameters["StartDate"].Value = value.ToDateTime();
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return ((DateTime)Parameters["EndDate"].Value).ToDateOnly();
		}
		set
		{
			Parameters["EndDate"].Value = value.ToDateTime();
		}
	}

	public string MTCode
	{
		get
		{
			return (string)Parameters["MTCode"].Value;
		}
		set
		{
			Parameters["MTCode"].Value = value;
		}
	}

	public string DocState
	{
		get
		{
			return (string)Parameters["DocState"].Value;
		}
		set
		{
			Parameters["DocState"].Value = value;
		}
	}

	public EMarkReceiptList()
	{
		DataProvider = new EMarkReceiptListDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("MTCode", ArmSoft.SME.Common.Resources.Product, "");
		AddParameter("DocState", ArmSoft.SME.Common.Resources.State, "", "", includeCaptionToolTip: false, new SmartEnum(typeof(DocumentStateSE)));
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("DocDate");
			dvp.Set_ColumnSortOrder("DocNum");
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (AllowOperation)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportEMarkReceipts", ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDoc, ExportEMarkReceipts);
		}
	}

	private void ExportEMarkReceipts()
	{
		List<Guid> list = (from r in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<EMarkReceiptListDataProvider.DataRow>()
			where string.IsNullOrWhiteSpace(r.EMarkReceiptNumber)
			group r by r.ISN into g
			select g.Key).ToList();
		if (list == null || list.Count == 0)
		{
			return;
		}
		string text = Settings.ParametersService.EMarkReceiptDevice();
		if (string.IsNullOrWhiteSpace(text))
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.UserFieldIsEmpty, Settings.LoginService.UserName, ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDevice));
		}
		ArmSoft.SME.Trade.Common.Models.Directories.EMarkReceiptApp eMarkReceiptApp = Settings.DirectoryService.Load<ArmSoft.SME.Trade.Common.Models.Directories.EMarkReceiptApp>(DirectoryTypeSE.EMarkReceipt.Value, text, withoutAccessCheck: true);
		EMarkApiClient eMarkApiClient = new EMarkApiClient(new EMarkOptions
		{
			Token = eMarkReceiptApp.Token,
			RefreshToken = eMarkReceiptApp.RefreshToken,
			CRN = eMarkReceiptApp.CRN,
			Datamatrix_file_id = eMarkReceiptApp.DeviceId
		});
		PrintRequest printRequest = new PrintRequest
		{
			receipt = new List<EMarkReceiptItem>()
		};
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDoc, "ExportEMarkReceipts");
		foreach (Guid item in list)
		{
			printRequest.receipt.Clear();
			EMarkReceipt eMarkReceipt = Settings.DocumentService.Load<EMarkReceipt>(item, DocumentTypeSE.EMarkReceipt.Value, withoutAccessCheck: true);
			try
			{
				foreach (EMarkReceipt.EMarkReceiptSpecificationRow mT in eMarkReceipt.MTList)
				{
					string text2 = mT.MarkingList.FirstOrDefault();
					if (string.IsNullOrWhiteSpace(text2))
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrorsInList);
					}
					printRequest.receipt.Add(new EMarkReceiptItem
					{
						product_name = mT.Name,
						quantity = Convert.ToInt32(mT.Quantity),
						measurement_unit = mT.UnitBrief,
						tnved = mT.CPACode,
						gtin = ((text2.StartsWith("01") && text2.Substring(0, 16).IsNumeric()) ? text2.Substring(2, 14) : text2.Substring(0, 14)),
						emarks = mT.MarkingList.ToList()
					});
				}
				PrintResponse printResponse = eMarkApiClient.Print(printRequest);
				if (printResponse.succeed)
				{
					IDocumentService documentService = Settings.DocumentService;
					Guid iSN = eMarkReceipt.ISN;
					int num = 1;
					List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
					CollectionsMarshal.SetCount(list2, num);
					CollectionsMarshal.AsSpan(list2)[0] = new PropertyWithValue
					{
						Name = "EMarkReceiptNumber",
						Value = printResponse.receipt_id.ToString()
					};
					documentService.EditFields(iSN, list2, new StoreRequest
					{
						WithoutAccessCheck = true,
						LogComment = $"{ArmSoft.SME.Common.Resources.VatExport} {ArmSoft.SME.Trade.Common.Resources.EMarkReceiptApp} - N{printResponse.receipt_id}"
					});
				}
			}
			catch (Exception ex)
			{
				if (list.Count == 1)
				{
					if (ex is EMarkApiException ex2)
					{
						throw new ASException(ex2.Message);
					}
					Settings.ErrorHandler(ex);
					return;
				}
				enumerateErrorsReportViewer.AddError(ex.Message + " - " + DocumentHelpers.ConfigureDocumentDescription(ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum, DocumentTypeSE.EMarkReceipt.Caption, eMarkReceipt.DocNum, eMarkReceipt.DocDate));
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.Update(list.Cast<object>().ToList());
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
		else
		{
			MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded);
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.MTCode = MTCode;
		ParametersProvider.DocState = DocState;
	}
}
