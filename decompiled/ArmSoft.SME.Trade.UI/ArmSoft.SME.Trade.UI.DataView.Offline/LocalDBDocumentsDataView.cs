using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataViewDataProvider.Offline;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.DataView.Offline;

[DataView(10124)]
public class LocalDBDocumentsDataView : DataView<LocalDBDocumentsDataProvider.DataRow, LocalDBDocumentsDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly LocalDBDocumentsDataView _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly DropDownControl _docState;

		private readonly DropDownControl _syncState;

		public DataViewDialog(LocalDBDocumentsDataView dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = dataView.Caption;
			Name = dataView.Name;
			HelpId = dataView.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			string documentState = ArmSoft.SME.Common.Resources.DocumentState;
			Type? typeFromHandle = typeof(DocumentStateSE);
			int num = 3;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<string> span = CollectionsMarshal.AsSpan(list);
			span[0] = DocumentStateSE.DraftOrProcessed.Value.ToString();
			span[1] = DocumentStateSE.Draft.Value.ToString();
			span[2] = DocumentStateSE.Processed.Value.ToString();
			_docState = AddDropDownControl(null, "DocState", documentState, new SmartEnum(typeFromHandle, list), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_docState.Code = ((useParamValues && dataView.DocState != null) ? Convert.ToString(dataView.DocState) : DocumentStateSE.DraftOrProcessed.ValueUI);
			string syncState = ArmSoft.SME.Common.Resources.SyncState;
			Type? typeFromHandle2 = typeof(LocalDBDocumentSyncStateSE);
			num = 2;
			List<string> list2 = new List<string>(num);
			CollectionsMarshal.SetCount(list2, num);
			Span<string> span2 = CollectionsMarshal.AsSpan(list2);
			span2[0] = LocalDBDocumentSyncStateSE.Sent.Value.ToString();
			span2[1] = LocalDBDocumentSyncStateSE.NotSent.Value.ToString();
			_syncState = AddDropDownControl(null, "SyncState", syncState, new SmartEnum(typeFromHandle2, list2), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_syncState.Code = (useParamValues ? dataView.SyncState : "");
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.DocState = _docState.Code;
			_dataView.SyncState = _syncState.Code;
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
	}

	private static readonly string _formatingExpression = $"[{"SyncState"}] = {LocalDBDocumentSyncStateSE.NotSent.Value}";

	public override string HelpId => "Sales_Journals_ECR_Offline";

	public override string Caption => Resources.OfflineECRChecksJournal;

	public override bool AllowDocumentHistory => true;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowDeleteSelected
	{
		get
		{
			if (AllowOperation)
			{
				return Settings.ParametersService.AllowGroupDelete();
			}
			return false;
		}
	}

	public DateOnly? StartDate
	{
		get
		{
			return (DateOnly?)Parameters["StartDate"].Value;
		}
		set
		{
			Parameters["StartDate"].Value = value;
		}
	}

	public DateOnly? EndDate
	{
		get
		{
			return (DateOnly?)Parameters["EndDate"].Value;
		}
		set
		{
			Parameters["EndDate"].Value = value;
		}
	}

	public string DocState
	{
		get
		{
			return Convert.ToString(Parameters["DocState"].Value);
		}
		set
		{
			Parameters["DocState"].Value = value;
		}
	}

	public string SyncState
	{
		get
		{
			return Convert.ToString(Parameters["SyncState"].Value);
		}
		set
		{
			Parameters["SyncState"].Value = value;
		}
	}

	public override bool DialogSupported => true;

	public LocalDBDocumentsDataView()
	{
		DataProvider = new LocalDBDocumentsDataProvider();
		FillColumnsFromDataProvider();
		AddParameter("StartDate", Resources.DatePeriodBegin, null, "dd/MM/yy");
		AddParameter("EndDate", Resources.DatePeriodEnd, null, "dd/MM/yy");
		AddParameter("SyncState", Resources.SyncState, "", "", includeCaptionToolTip: false, new SmartEnum(typeof(LocalDBDocumentSyncStateSE)));
		AddParameter("DocState", Resources.DocumentState, "", "", includeCaptionToolTip: false, new DocumentState());
		AddConditionalFormatting();
	}

	private void AddConditionalFormatting()
	{
		ConditionalFormattingList.AddRange(new _003C_003Ez__ReadOnlyArray<AsFormatting>(new AsFormatting[2]
		{
			new AsCondititionFormat
			{
				Expression = _formatingExpression,
				FieldName = "SyncStateName",
				FormatName = "RedText",
				IsFixed = true
			},
			new AsCondititionFormat
			{
				Expression = _formatingExpression,
				FieldName = "SyncStateName",
				FormatName = "BoldText",
				IsFixed = true
			}
		}));
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.DocState = DocState;
		ParametersProvider.SyncState = SyncState;
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		ConfigureContextButtons();
	}

	private void ConfigureContextButtons()
	{
		LocalDBDocumentsDataProvider.DataRow dataRow = (LocalDBDocumentsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (((!AllowOperation || !Settings.LoginService.IsAdmin) && !Settings.UserAccessService.IsSubsystemAccessible(2)) || dataRow == null)
		{
			return;
		}
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ViewDocument", Resources.View, ViewDocument);
		if (!Settings.IsOffline)
		{
			List<LocalDBDocumentsDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<LocalDBDocumentsDataProvider.DataRow>();
			if (selectedRows != null && selectedRows.Count > 0)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "SendSelected", Resources.SendSelected, SendSelected);
			}
			else if (dataRow.SyncState == LocalDBDocumentSyncStateSE.NotSent.Value)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "Send", Resources.Send, Send);
			}
		}
	}

	private void Send()
	{
		try
		{
			LocalDBDocumentsDataProvider.DataRow dataRow = (LocalDBDocumentsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			Settings.LocalDBDocumentService.SendToServer(dataRow.ISN);
			SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)dataRow.DocType);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		}
	}

	private void SendSelected()
	{
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(Resources.SendErrors, "SendSelectedErrors");
		foreach (LocalDBDocumentsDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<LocalDBDocumentsDataProvider.DataRow>())
		{
			try
			{
				if (selectedRow.SyncState == (short)LocalDBDocumentSyncStateSE.NotSent)
				{
					Settings.LocalDBDocumentService.SendToServer(selectedRow.ISN);
				}
			}
			catch (Exception ex)
			{
				enumerateErrorsReportViewer.AddError(ex);
			}
		}
		SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(DocumentTypeSE.MTBill);
		SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(DocumentTypeSE.MTReturnFromCustomer);
		SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(DocumentTypeSE.MTBillReturn);
		Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
	}

	public override void Delete()
	{
		try
		{
			LocalDBDocumentsDataProvider.DataRow dataRow = (LocalDBDocumentsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null && ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(Resources.ConfDocDel, dataRow.DocTypeName), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				Settings.LocalDBDocumentService.Delete(dataRow.ISN);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		}
	}

	public override void DeleteSelected()
	{
		try
		{
			List<LocalDBDocumentsDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<LocalDBDocumentsDataProvider.DataRow>();
			if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(Resources.ConfSelDocFullDelete, selectedRows.Count), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
			{
				return;
			}
			foreach (LocalDBDocumentsDataProvider.DataRow item in selectedRows)
			{
				Settings.LocalDBDocumentService.Delete(item.ISN);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		}
	}

	public override void DocumentHistory()
	{
		try
		{
			LocalDBDocumentsDataProvider.DataRow dataRow = (LocalDBDocumentsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				new LocalDBDocumentLogDataView(dataRow.GetDocISN()).Show();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override string GetDefaultFunction()
	{
		return "ViewDocument";
	}

	private void ViewDocument()
	{
		try
		{
			TradeDocument currentRowDocument = GetCurrentRowDocument();
			if (currentRowDocument != null)
			{
				if (currentRowDocument.POType == DocumentTypeSE.MTBill.Value)
				{
					MTBillWindow mTBillWindow = new MTBillWindow((MTBill)currentRowDocument);
					mTBillWindow.IsOfflineDoc = true;
					mTBillWindow.Show(WindowViewMode.View);
				}
				else if (currentRowDocument.POType == DocumentTypeSE.MTBillReturn.Value)
				{
					MTBillReturnWindow mTBillReturnWindow = new MTBillReturnWindow((MTBillReturn)currentRowDocument);
					mTBillReturnWindow.IsOfflineDoc = true;
					mTBillReturnWindow.Show(WindowViewMode.View);
				}
				else if (currentRowDocument.POType == DocumentTypeSE.MTReturnFromCustomer.Value)
				{
					MTReturnFromCustomerWindow mTReturnFromCustomerWindow = new MTReturnFromCustomerWindow((MTReturnFromCustomer)currentRowDocument);
					mTReturnFromCustomerWindow.IsOfflineDoc = true;
					mTReturnFromCustomerWindow.Show(WindowViewMode.View);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private TradeDocument GetCurrentRowDocument()
	{
		LocalDBDocumentsDataProvider.DataRow dataRow = (LocalDBDocumentsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			return (TradeDocument)Settings.LocalDBDocumentService.Load(dataRow.ISN);
		}
		return null;
	}
}
