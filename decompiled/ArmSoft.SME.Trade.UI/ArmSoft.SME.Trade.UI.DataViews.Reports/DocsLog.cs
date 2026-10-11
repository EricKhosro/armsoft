using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataViews.Reports;

[DataView(4)]
public class DocsLog : DataView<DocsLogDataProvider.DataRow, DocsLogDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly DocsLog _dataView;

		private readonly DateEditControl _docStartDate;

		private readonly DateEditControl _docEndDate;

		private readonly DateEditControl _opStartDate;

		private readonly DateEditControl _opEndDate;

		private MultiSelectDropDownControl _docType;

		private readonly DropDownControl _userName;

		public DataViewDialog(DocsLog dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_docStartDate = AddDateControl(container, "DocStartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_docStartDate.EditValue = (useParamValues ? dataView.ParamDocStartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_docEndDate = AddDateControl(container, "DocEndDate", "", useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_docEndDate.EditValue = (useParamValues ? dataView.ParamDocEndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _docStartDate, _docEndDate);
			container = AddHorizontalLayoutGroup();
			_opStartDate = AddDateControl(container, "OpStartDate", ArmSoft.SME.Common.Resources.OperationDatePeriod, useLongDate: false, isRequired: false, !useParamValues);
			_opStartDate.EditValue = (useParamValues ? dataView.ParamOpStartDate : ((DateOnly?)null));
			_opEndDate = AddDateControl(container, "OpEndDate", "", useLongDate: false, isRequired: false, !useParamValues);
			_opEndDate.EditValue = (useParamValues ? dataView.ParamOpEndDate : ((DateOnly?)null));
			AddSpecialDateButton(container, _opStartDate, _opEndDate);
			_docType = AddMultiSelectDropDownControl("DocType", ArmSoft.SME.Common.Resources.TypeWord, new DocumentType());
			_docType.SelectedString = (useParamValues ? _dataView.ParamDocType : string.Empty);
			_userName = AddDropDownControl("UserName", ArmSoft.SME.Common.Resources.User, new UsersFilter(), isRequired: false, allowAdditionButtons: false, storeValue: true);
			_userName.Code = (useParamValues ? dataView.ParamUserName : string.Empty);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.ParamDocStartDate = _docStartDate.EditValue;
			_dataView.ParamDocEndDate = _docEndDate.EditValue;
			_dataView.ParamOpStartDate = _opStartDate.EditValue;
			_dataView.ParamOpEndDate = _opEndDate.EditValue;
			_dataView.ParamDocType = _docType.SelectedString;
			_dataView.ParamUserName = _userName.Code;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_docStartDate.EditValue, _docEndDate.EditValue);
				ValidationService.ValidateWrongPeriod(_opStartDate.EditValue, _opEndDate.EditValue);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	public override string HelpId => "AdminTools_DocumentsHistory";

	public override bool DialogSupported => true;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowDocumentHistory => true;

	public override bool AllowExport => true;

	public DateOnly? ParamDocStartDate
	{
		get
		{
			return (DateOnly?)Parameters["DocStartDate"].Value;
		}
		set
		{
			Parameters["DocStartDate"].Value = value;
		}
	}

	public DateOnly? ParamDocEndDate
	{
		get
		{
			return (DateOnly?)Parameters["DocEndDate"].Value;
		}
		set
		{
			Parameters["DocEndDate"].Value = value;
		}
	}

	public DateOnly? ParamOpStartDate
	{
		get
		{
			return (DateOnly?)Parameters["OpStartDate"].Value;
		}
		set
		{
			Parameters["OpStartDate"].Value = value;
		}
	}

	public DateOnly? ParamOpEndDate
	{
		get
		{
			return (DateOnly?)Parameters["OpEndDate"].Value;
		}
		set
		{
			Parameters["OpEndDate"].Value = value;
		}
	}

	public string ParamDocType
	{
		get
		{
			return Convert.ToString(Parameters["DocType"].Value);
		}
		set
		{
			Parameters["DocType"].Value = value;
		}
	}

	public string ParamUserName
	{
		get
		{
			return Convert.ToString(Parameters["UserName"].Value);
		}
		set
		{
			Parameters["UserName"].Value = value;
		}
	}

	public DocsLog()
	{
		DataProvider = new DocsLogDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("DocStartDate", Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("DocEndDate", Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("OpStartDate", Resources.OperationDatePeriodBegin, null, "dd/MM/yy");
		AddParameter("OpEndDate", Resources.OperationDatePeriodEnd, null, "dd/MM/yy");
		AddParameter("DocType", Resources.TypeWord, "", "", includeCaptionToolTip: true, new DocumentType());
		AddParameter("UserName", Resources.User, "");
		FillColumnsFromDataProvider();
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.DocStartDate = ParamDocStartDate;
		ParametersProvider.DocEndDate = ParamDocEndDate;
		ParametersProvider.OpStartDate = ParamOpStartDate;
		ParametersProvider.OpEndDate = ParamOpEndDate;
		ParametersProvider.DocType = ParamDocType.ConvertSelected<short>();
		ParametersProvider.UserId = Settings.CoreCacheService.User.GetItemByName(ParamUserName)?.UserId ?? (-1);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
			dvp.Set_ColumnSortOrder("DocDate");
			dvp.Set_ColumnSortOrder("OrderNum");
			dvp.Set_ColumnSortOrder("DocType");
			dvp.Set_ColumnSortOrder("DocNum");
		}
		dvp.Set_ColumnVisible("UserID", value: false);
		dvp.Set_ColumnVisible("State", value: false);
		dvp.Set_ColumnVisible("DocTypeCode", value: false);
		dvp.Set_ColumnVisible("ISN", value: false);
	}
}
