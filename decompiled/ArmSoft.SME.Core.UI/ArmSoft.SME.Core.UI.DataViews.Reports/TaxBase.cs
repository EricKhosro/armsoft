using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData.Reports;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

public abstract class TaxBase : DataView<TaxBaseDataProvider.DataRow, TaxBaseDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly TaxBase _dataView;

		private readonly DateEditControl _creationStartDate;

		private readonly DateEditControl _creationEndDate;

		private readonly DateEditControl _submitStartDate;

		private readonly DateEditControl _submitEndDate;

		private readonly DateEditControl _supplyStartDate;

		private readonly DateEditControl _supplyEndDate;

		private readonly TextEditExt _supplierTIN;

		private readonly TextEditExt _supplierName;

		private readonly TextEditExt _buyerTIN;

		private readonly TextEditExt _buyerName;

		private readonly TextEditExt _taxSerialNumber;

		private readonly MultiSelectDropDownControl _taxDocTypes;

		private readonly CheckEditExt _showCorrectionDocSumsAsNegatives;

		public DataViewDialog(TaxBase dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Name = "EInvoicing" + dataView.TaxDocType.ToString() + dataView.TaxDocStatus;
			Caption = _dataView.Caption;
			AllowParameterTemplates = false;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			LayoutGroupExt container2 = AddHorizontalLayoutGroup();
			_creationStartDate = AddDateControl(container, "CreationStartDate", ArmSoft.SME.Common.Resources.CreationDatePeriod, useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.CreationDatePeriodBegin);
			_creationStartDate.EditValue = (useParamValues ? dataView.ParamCreationStartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_creationStartDate.ControlVisibility = ((!_dataView.Parameters.ContainsKey("CreationStartDate")) ? Visibility.Collapsed : Visibility.Visible);
			_creationEndDate = AddDateControl(container, "CreationEndDate", "", useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.CreationDatePeriodEnd);
			_creationEndDate.EditValue = (useParamValues ? dataView.ParamCreationEndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			_creationEndDate.ControlVisibility = ((!_dataView.Parameters.ContainsKey("CreationEndDate")) ? Visibility.Collapsed : Visibility.Visible);
			AddSpecialDateButton(container, _creationStartDate, _creationEndDate);
			_submitStartDate = AddDateControl(container2, "SubmitStartDate", ArmSoft.SME.Common.Resources.SubmissionDatePeriod, useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.SubmissionDatePeriodBegin);
			_submitStartDate.EditValue = (useParamValues ? dataView.ParamSubmitStartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_submitStartDate.ControlVisibility = ((!_dataView.Parameters.ContainsKey("SubmitStartDate")) ? Visibility.Collapsed : Visibility.Visible);
			_submitEndDate = AddDateControl(container2, "SubmitEndDate", "", useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.SubmissionDatePeriodEnd);
			_submitEndDate.EditValue = (useParamValues ? dataView.ParamSubmitEndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			_submitEndDate.ControlVisibility = ((!_dataView.Parameters.ContainsKey("SubmitEndDate")) ? Visibility.Collapsed : Visibility.Visible);
			AddSpecialDateButton(container2, _submitStartDate, _submitEndDate);
			if (!isRefreshMode)
			{
				if (_creationStartDate.ControlVisibility == Visibility.Visible)
				{
					AddStoreValuesButton(container);
				}
				else
				{
					AddStoreValuesButton(container2);
				}
			}
			container = AddHorizontalLayoutGroup();
			_supplyStartDate = AddDateControl(container, "SupplyStartDate", ArmSoft.SME.Common.Resources.SupplyDatePeriod, useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.SupplyDatePeriodBegin);
			_supplyStartDate.EditValue = (useParamValues ? dataView.ParamSupplyStartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_supplyStartDate.ControlVisibility = ((!_dataView.Parameters.ContainsKey("SupplyStartDate")) ? Visibility.Collapsed : Visibility.Visible);
			_supplyEndDate = AddDateControl(container, "SupplyEndDate", "", useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.SupplyDatePeriodEnd);
			_supplyEndDate.EditValue = (useParamValues ? dataView.ParamSupplyEndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			_supplyEndDate.ControlVisibility = ((!_dataView.Parameters.ContainsKey("SupplyEndDate")) ? Visibility.Collapsed : Visibility.Visible);
			AddSpecialDateButton(container, _supplyStartDate, _supplyEndDate);
			_supplierTIN = AddStringControl("SupplierTin", ArmSoft.SME.Common.Resources.SupplierTaxCode, 20, isRequired: false, !useParamValues);
			_supplierTIN.EditValue = (useParamValues ? dataView.ParamSupplierTIN : "");
			_supplierTIN.ControlVisibility = ((!_dataView.Parameters.ContainsKey("SupplierTin")) ? Visibility.Collapsed : Visibility.Visible);
			_supplierName = AddStringControl("SupplierName", ArmSoft.SME.Common.Resources.SupplierName, 255, isRequired: false, !useParamValues);
			_supplierName.EditValue = (useParamValues ? dataView.ParamSupplierName : "");
			_supplierName.ControlVisibility = ((!_dataView.Parameters.ContainsKey("SupplierName")) ? Visibility.Collapsed : Visibility.Visible);
			_buyerTIN = AddStringControl("BuyerTin", ArmSoft.SME.Common.Resources.BuyerTaxCode, 20, isRequired: false, !useParamValues);
			_buyerTIN.EditValue = (useParamValues ? dataView.ParamBuyerTIN : "");
			_buyerTIN.ControlVisibility = ((!_dataView.Parameters.ContainsKey("BuyerTin")) ? Visibility.Collapsed : Visibility.Visible);
			_buyerName = AddStringControl("BuyerName", ArmSoft.SME.Common.Resources.BuyerName, 255, isRequired: false, !useParamValues);
			_buyerName.EditValue = (useParamValues ? dataView.ParamBuyerName : "");
			_buyerName.ControlVisibility = ((!_dataView.Parameters.ContainsKey("BuyerName")) ? Visibility.Collapsed : Visibility.Visible);
			_taxSerialNumber = AddStringControl("TaxSerialNumber", ArmSoft.SME.Common.Resources.TaxSerialNumber, 11, isRequired: false, !useParamValues);
			_taxSerialNumber.EditValue = (useParamValues ? dataView.ParamTaxSerialNumber : "");
			_taxSerialNumber.ControlVisibility = ((!_dataView.Parameters.ContainsKey("TaxSerialNumber")) ? Visibility.Collapsed : Visibility.Visible);
			List<string> values = new List<string>();
			if (_dataView.TaxDocType == TaxDocType.AccDoc)
			{
				int num = 5;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount(list, num);
				Span<string> span = CollectionsMarshal.AsSpan(list);
				span[0] = TaxAccAndInvoiceDocsTypeSE.AccDocGoods.Value.ToString();
				span[1] = TaxAccAndInvoiceDocsTypeSE.AccDocServices.Value.ToString();
				span[2] = TaxAccAndInvoiceDocsTypeSE.AccDocExportEEU.Value.ToString();
				span[3] = TaxAccAndInvoiceDocsTypeSE.AccDocTraceableG.Value.ToString();
				span[4] = TaxAccAndInvoiceDocsTypeSE.AccDocTransportation.Value.ToString();
				values = list;
			}
			else if (_dataView.TaxDocType == TaxDocType.Invoice)
			{
				int num = 5;
				List<string> list2 = new List<string>(num);
				CollectionsMarshal.SetCount(list2, num);
				Span<string> span2 = CollectionsMarshal.AsSpan(list2);
				span2[0] = TaxAccAndInvoiceDocsTypeSE.Goods.Value.ToString();
				span2[1] = TaxAccAndInvoiceDocsTypeSE.Services.Value.ToString();
				span2[2] = TaxAccAndInvoiceDocsTypeSE.Excise.Value.ToString();
				span2[3] = TaxAccAndInvoiceDocsTypeSE.VatReturn.Value.ToString();
				span2[4] = TaxAccAndInvoiceDocsTypeSE.Leasing.Value.ToString();
				values = list2;
			}
			_taxDocTypes = AddMultiSelectDropDownControl("TaxDocTypes", ArmSoft.SME.Common.Resources.TypeWord, new SmartEnum(typeof(TaxAccAndInvoiceDocsTypeSE), values), isRequired: false, !useParamValues);
			_taxDocTypes.SelectedString = (useParamValues ? dataView.ParamTaxDocTypes : "");
			_taxDocTypes.ControlVisibility = ((!_dataView.Parameters.ContainsKey("TaxDocTypes")) ? Visibility.Collapsed : Visibility.Visible);
			_showCorrectionDocSumsAsNegatives = AddBooleanControl("ShowOutSumsAsNegatives", ArmSoft.SME.Common.Resources.ShowCorrectionDocSummsAsNegatives, !useParamValues);
			_showCorrectionDocSumsAsNegatives.IsChecked = useParamValues && dataView.ParamShowOutSumsAsNegatives;
			_showCorrectionDocSumsAsNegatives.ControlVisibility = ((!_dataView.Parameters.ContainsKey("ShowOutSumsAsNegatives")) ? Visibility.Collapsed : Visibility.Visible);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.ParamCreationStartDate = _creationStartDate.EditValue;
			_dataView.ParamCreationEndDate = _creationEndDate.EditValue;
			_dataView.ParamSubmitStartDate = _submitStartDate.EditValue;
			_dataView.ParamSubmitEndDate = _submitEndDate.EditValue;
			_dataView.ParamSupplyStartDate = _supplyStartDate.EditValue;
			_dataView.ParamSupplyEndDate = _supplyEndDate.EditValue;
			_dataView.ParamSupplierTIN = _supplierTIN.Text;
			_dataView.ParamSupplierName = _supplierName.Text;
			_dataView.ParamBuyerTIN = _buyerTIN.Text;
			_dataView.ParamBuyerName = _buyerName.Text;
			_dataView.ParamTaxSerialNumber = _taxSerialNumber.Text;
			_dataView.ParamShowOutSumsAsNegatives = Convert.ToBoolean(_showCorrectionDocSumsAsNegatives.IsChecked);
			_dataView.ParamTaxDocTypes = _taxDocTypes.SelectedString;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				if (_dataView.TaxDocStatus == Status.Draft)
				{
					ValidationService.ValidateWrongPeriod(_creationStartDate.EditValue, _creationEndDate.EditValue);
				}
				else
				{
					ValidationService.ValidateWrongPeriod(_submitStartDate.EditValue, _submitEndDate.EditValue);
				}
				ValidationService.ValidateWrongPeriod(_supplyStartDate.EditValue, _supplyEndDate.EditValue);
				List<(string, string)> list = new List<(string, string)>();
				if (_dataView.TaxDocStatus == Status.Approved)
				{
					list.Add((ArmSoft.SME.Common.Resources.SupplierTaxCode, _supplierTIN.Text));
					list.Add((ArmSoft.SME.Common.Resources.SupplierName, _supplierName.Text));
					list.Add((ArmSoft.SME.Common.Resources.TaxSerialNumber, _taxSerialNumber.Text));
				}
				else
				{
					list.Add((ArmSoft.SME.Common.Resources.BuyerTaxCode, _buyerTIN.Text));
					list.Add((ArmSoft.SME.Common.Resources.BuyerName, _buyerName.Text));
					if (_dataView.TaxDocStatus != Status.Draft)
					{
						list.Add((ArmSoft.SME.Common.Resources.TaxSerialNumber, _taxSerialNumber.Text));
					}
				}
				ValidationService.OnlyOneNotEmpty(list);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	public abstract TaxDocType TaxDocType { get; }

	public abstract Status TaxDocStatus { get; }

	public override bool AllowDelete => TaxDocStatus == Status.Draft;

	public override bool DialogSupported => true;

	public DateOnly? ParamCreationStartDate
	{
		get
		{
			if (Parameters.TryGetValue("CreationStartDate", out var value))
			{
				return (DateOnly?)value.Value;
			}
			return null;
		}
		set
		{
			if (Parameters.TryGetValue("CreationStartDate", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public DateOnly? ParamCreationEndDate
	{
		get
		{
			if (Parameters.TryGetValue("CreationEndDate", out var value))
			{
				return (DateOnly?)value.Value;
			}
			return null;
		}
		set
		{
			if (Parameters.TryGetValue("CreationEndDate", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public DateOnly? ParamSubmitStartDate
	{
		get
		{
			if (Parameters.TryGetValue("SubmitStartDate", out var value))
			{
				return (DateOnly?)value.Value;
			}
			return null;
		}
		set
		{
			if (Parameters.TryGetValue("SubmitStartDate", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public DateOnly? ParamSubmitEndDate
	{
		get
		{
			if (Parameters.TryGetValue("SubmitEndDate", out var value))
			{
				return (DateOnly?)value.Value;
			}
			return null;
		}
		set
		{
			if (Parameters.TryGetValue("SubmitEndDate", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public DateOnly? ParamSupplyStartDate
	{
		get
		{
			if (Parameters.TryGetValue("SupplyStartDate", out var value))
			{
				return (DateOnly?)value.Value;
			}
			return null;
		}
		set
		{
			if (Parameters.TryGetValue("SupplyStartDate", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public DateOnly? ParamSupplyEndDate
	{
		get
		{
			if (Parameters.TryGetValue("SupplyEndDate", out var value))
			{
				return (DateOnly?)value.Value;
			}
			return null;
		}
		set
		{
			if (Parameters.TryGetValue("SupplyEndDate", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public string ParamSupplierTIN
	{
		get
		{
			if (Parameters.TryGetValue("SupplierTin", out var value))
			{
				return (string)value.Value;
			}
			return "";
		}
		set
		{
			if (Parameters.TryGetValue("SupplierTin", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public string ParamSupplierName
	{
		get
		{
			if (Parameters.TryGetValue("SupplierName", out var value))
			{
				return (string)value.Value;
			}
			return "";
		}
		set
		{
			if (Parameters.TryGetValue("SupplierName", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public string ParamBuyerTIN
	{
		get
		{
			if (Parameters.TryGetValue("BuyerTin", out var value))
			{
				return (string)value.Value;
			}
			return "";
		}
		set
		{
			if (Parameters.TryGetValue("BuyerTin", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public string ParamBuyerName
	{
		get
		{
			if (Parameters.TryGetValue("BuyerName", out var value))
			{
				return (string)value.Value;
			}
			return "";
		}
		set
		{
			if (Parameters.TryGetValue("BuyerName", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public string ParamTaxSerialNumber
	{
		get
		{
			if (Parameters.TryGetValue("TaxSerialNumber", out var value))
			{
				return (string)value.Value;
			}
			return "";
		}
		set
		{
			if (Parameters.TryGetValue("TaxSerialNumber", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public bool ParamShowOutSumsAsNegatives
	{
		get
		{
			if (Parameters.TryGetValue("ShowOutSumsAsNegatives", out var value))
			{
				return (bool)value.Value;
			}
			return false;
		}
		set
		{
			if (Parameters.TryGetValue("ShowOutSumsAsNegatives", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public string ParamTaxDocTypes
	{
		get
		{
			if (Parameters.TryGetValue("TaxDocTypes", out var value))
			{
				return (string)value.Value;
			}
			return "";
		}
		set
		{
			if (Parameters.TryGetValue("TaxDocTypes", out var value2))
			{
				value2.Value = value;
			}
		}
	}

	public TaxBase()
	{
		ColHeadersIn2Lines = true;
	}

	public void AddParameters()
	{
		foreach (TaxBaseDataProvider.TaxParameter parameter in ((TaxBaseDataProvider)DataProvider).GetParameters())
		{
			AddParameter(parameter.Name, parameter.Caption, parameter.DefaultValue, parameter.Format);
		}
	}

	public override string GetDefaultFunction()
	{
		return "ViewTaxDoc";
	}

	public override void InitContextFunctions()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ViewTaxDoc", Resources.View, ViewTaxDoc);
		}
	}

	private void ViewTaxDoc()
	{
		TaxBaseDataProvider.DataRow dataRow = (TaxBaseDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		dynamic val = null;
		if (TaxDocType == TaxDocType.AccDoc)
		{
			if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocGoods.Value)
			{
				val = Settings.TaxService.Client.AccDocGoods.Get(dataRow.ISN, loadItems: true);
			}
			else if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocTransportation.Value)
			{
				val = Settings.TaxService.Client.AccDocTransportation.Get(dataRow.ISN, loadItems: true);
			}
			else if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocServices.Value)
			{
				val = Settings.TaxService.Client.AccDocServices.Get(dataRow.ISN, loadItems: true);
			}
			else if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocExportEEU.Value)
			{
				val = Settings.TaxService.Client.AccDocExportEEU.Get(dataRow.ISN, loadItems: true);
			}
			else if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.AccDocTraceableG.Value)
			{
				val = Settings.TaxService.Client.AccDocTraceableG.Get(dataRow.ISN, loadItems: true);
			}
			if (val != null)
			{
				dynamic val2 = Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument(val);
				if (val2 != null)
				{
					new TaxDocumentWindow<TaxDocument>(val2).Show(WindowViewMode.View);
				}
			}
		}
		else if (TaxDocType == TaxDocType.Invoice)
		{
			if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Excise.Value)
			{
				val = Settings.TaxService.Client.Excise.Get(dataRow.ISN, loadItems: true);
			}
			else if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
			{
				val = Settings.TaxService.Client.Goods.Get(dataRow.ISN, loadItems: true);
			}
			else if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Services.Value)
			{
				val = Settings.TaxService.Client.Services.Get(dataRow.ISN, loadItems: true);
			}
			else if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.VatReturn.Value)
			{
				val = Settings.TaxService.Client.VatRefund.Get(dataRow.ISN, loadItems: true);
			}
			else if (dataRow.TaxDocInnerType == TaxAccAndInvoiceDocsTypeSE.Leasing.Value)
			{
				val = Settings.TaxService.Client.Leasing.Get(dataRow.ISN, loadItems: true);
			}
			if (val != null)
			{
				dynamic val3 = Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(val);
				new TaxDocumentWindow<TaxDocument>(val3).Show(WindowViewMode.View);
			}
		}
	}

	public override void Delete()
	{
		TaxBaseDataProvider.DataRow dataRow = (TaxBaseDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null || MessageBox.Show(string.Format(Resources.ConfDocDel, dataRow.TaxDocInnerTypeName), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes || !Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			return;
		}
		InvoiceType result2;
		if (TaxDocType == TaxDocType.AccDoc)
		{
			if (Enum.TryParse<AccDocType>(dataRow.TaxDocInnerType, out var result))
			{
				Settings.TaxService.Client.Dispatcher.RemoveDraft(result, dataRow.ISN);
			}
		}
		else if (TaxDocType == TaxDocType.Invoice && Enum.TryParse<InvoiceType>(dataRow.TaxDocInnerType, out result2))
		{
			Settings.TaxService.Client.Dispatcher.RemoveDraft(result2, dataRow.ISN);
		}
		Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.ISN);
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.CreationStartDate = ParamCreationStartDate;
		ParametersProvider.CreationEndDate = ParamCreationEndDate;
		ParametersProvider.SubmitStartDate = ParamSubmitStartDate;
		ParametersProvider.SubmitEndDate = ParamSubmitEndDate;
		ParametersProvider.SupplyStartDate = ParamSupplyStartDate;
		ParametersProvider.SupplyEndDate = ParamSupplyEndDate;
		ParametersProvider.SupplierTin = ParamSupplierTIN;
		ParametersProvider.SupplierName = ParamSupplierName;
		ParametersProvider.BuyerTin = ParamBuyerTIN;
		ParametersProvider.BuyerName = ParamBuyerName;
		ParametersProvider.TaxSerialNumber = ParamTaxSerialNumber;
		ParametersProvider.ShowOutSumsAsNegatives = ParamShowOutSumsAsNegatives;
		ParametersProvider.TaxDocTypes = ((!string.IsNullOrWhiteSpace(ParamTaxDocTypes)) ? (from p in ParamTaxDocTypes.Split(",").ToList()
			select Convert.ToInt16(p)).ToList() : null);
	}
}
