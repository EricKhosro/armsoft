using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10046)]
public class GoodsStockTakingList : TradeDataView<GoodsStockTakingListDataProvider.DataRow, GoodsStockTakingListDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly GoodsStockTakingList _dataView;

		private DateEditControl _startDate;

		private DateEditControl _endDate;

		private DropDownControl _docState;

		private DropDownControl _storage;

		public DataViewDialog(GoodsStockTakingList dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.GoodsStockTakingRep;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			LayoutGroupExt container2 = AddVerticalLayoutGroup();
			string storage = ArmSoft.SME.Common.Resources.Storage;
			bool? checkViewOrOperationAccess = true;
			_storage = AddDropDownControl(container2, "Storage", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_storage.Code = (useParamValues ? dataView.Storage : string.Empty);
			_docState = AddDropDownControl(container2, "DocState", ArmSoft.SME.Common.Resources.State, new DocumentState(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_docState.Code = (useParamValues ? dataView.DocState : DocumentStateSE.DraftOrProcessed.ValueUI);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.DocState = _docState.Code;
			_dataView.Storage = _storage.Code;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override string HelpId => "Purchases_Reports_InventoryJournal";

	public override bool DialogSupported => true;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowPreview => true;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowDocumentHistory => true;

	public override string Caption => ArmSoft.SME.Trade.Common.Resources.GoodsStockTakingRep + " " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY();

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

	public string Storage
	{
		get
		{
			return Convert.ToString(Parameters["Storage"].Value);
		}
		set
		{
			Parameters["Storage"].Value = value;
		}
	}

	public GoodsStockTakingList()
	{
		DataProvider = new GoodsStockTakingListDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, "");
		AddParameter("DocState", ArmSoft.SME.Common.Resources.State, "");
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("ISN", value: false);
		dvp.Set_ColumnVisible("DocType", value: false);
		dvp.Set_ColumnVisible("DocState", value: false);
		dvp.Set_ColumnVisible("UnitCode", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("DocDate");
			dvp.Set_ColumnSortOrder("DocNum");
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return null;
		}
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		base.ApplyParametersToDataProvider();
		ParametersProvider.StartDate = StartDate.Value;
		ParametersProvider.EndDate = EndDate.Value;
		ParametersProvider.DocState = DocState;
		ParametersProvider.Storage = Storage;
	}
}
