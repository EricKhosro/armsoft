using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10043)]
public class GoodsCostCalculation : TradeDataView<GoodsCostCalculationDataProvider.DataRow, GoodsCostCalculationDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly GoodsCostCalculation _dataView;

		private DateEditControl _startDate;

		private DateEditControl _endDate;

		private DropDownControl _partnerCode;

		private DropDownControl _docOwner;

		private CheckEditExt _showSums;

		private CheckEditExt _showSummWithVAT;

		private CheckEditExt _showCurrSumms;

		public DataViewDialog(GoodsCostCalculation dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = dataView.Caption;
			HelpId = dataView.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = Settings.HostingEnvironment.DateBegin;
			_endDate = AddDateControl(container, "DateEnd", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = Settings.HostingEnvironment.DateEnd;
			AddSpecialDateButton(container, _startDate, _endDate);
			_partnerCode = AddDropDownControl("PartnerCode", ArmSoft.SME.Common.Resources.Partner, new PartnersFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_partnerCode.Code = string.Empty;
			_docOwner = AddDropDownControl("DocOwner", ArmSoft.SME.Common.Resources.DocOwner, new UsersFilter(), isRequired: false, allowAdditionButtons: false, storeValue: true);
			if (Settings.LoginService.IsAdmin || !Settings.ParametersService.DenyAccessToOtherOwnerDocs())
			{
				_docOwner.Code = string.Empty;
			}
			else
			{
				_docOwner.Code = Settings.LoginService.UserName;
				_docOwner.IsReadOnly = true;
			}
			AddLine();
			bool flag = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
			_showSums = AddBooleanControl("ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, flag);
			_showSums.IsChecked = (flag & useParamValues) && flag;
			_showSums.IsReadOnly = !flag;
			_showSummWithVAT = AddBooleanControl("ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, flag);
			_showSummWithVAT.IsChecked = (flag & useParamValues) && _dataView.ShowSumsWithVAT;
			_showSummWithVAT.IsReadOnly = !flag;
			_showCurrSumms = AddBooleanControl("ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, Settings.ParametersService.MTCurr()), flag);
			_showCurrSumms.IsChecked = (flag & useParamValues) && _dataView.ShowCurrSums;
			_showCurrSumms.IsReadOnly = !flag;
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

		protected override bool BeforeApplyParam()
		{
			HashSet<short> hashSet = new HashSet<short>();
			if (Settings.UserAccessService.GetDocumentPermission(DocumentTypeSE.MTIncome).AllowView)
			{
				hashSet.Add(DocumentTypeSE.MTIncome.Value);
			}
			if (Settings.UserAccessService.GetDocumentPermission(DocumentTypeSE.MTIncomeRetail).AllowView)
			{
				hashSet.Add(DocumentTypeSE.MTIncomeRetail.Value);
			}
			if (Settings.UserAccessService.GetDocumentPermission(DocumentTypeSE.MTIncomeImport).AllowView)
			{
				hashSet.Add(DocumentTypeSE.MTIncomeImport.Value);
			}
			DocumentsJournal documentsJournal = new DocumentsJournal(this, ReportTypesSE.PurchaseDocument, hashSet)
			{
				StartDate = _startDate.EditValue.Value,
				EndDate = _endDate.EditValue.Value,
				PartnerCode = _partnerCode.Code,
				DocState = DocumentStateSE.Processed.ValueUI,
				DocOwner = _docOwner.Code
			};
			ModalBrowser modalBrowser = new ModalBrowser(documentsJournal)
			{
				Owner = Settings.HostingEnvironment,
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};
			if (modalBrowser.Show())
			{
				_dataView.BaseISN = (Guid)documentsJournal.KeyMember(modalBrowser.SelectedRowIndex);
				_dataView.AdditionalCaption = TradeSettings.CreateTradeClient().CommonServices.GetDocumentAdditionalCaptionForReports(_dataView.BaseISN);
				return true;
			}
			return false;
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.ShowSums = Convert.ToBoolean(_showSums.IsChecked);
			_dataView.ShowSumsWithVAT = Convert.ToBoolean(_showSummWithVAT.IsChecked);
			_dataView.ShowCurrSums = Convert.ToBoolean(_showCurrSumms.IsChecked);
		}
	}

	public override string HelpId => "Cost_Calculation_Report";

	public override bool AllowView => true;

	public override bool AllowEdit => AllowOperation;

	public override bool DialogSupported => BaseISN == Guid.Empty;

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override string Caption
	{
		get
		{
			if (BaseISN == Guid.Empty)
			{
				return ArmSoft.SME.Trade.Common.Resources.GoodsCostCalculation;
			}
			return string.Format(ArmSoft.SME.Trade.Common.Resources.GoodsCostCalculationHeader, AdditionalCaption);
		}
	}

	public Guid BaseISN
	{
		get
		{
			return (Guid)Parameters["BaseISN"].Value;
		}
		set
		{
			Parameters["BaseISN"].Value = value;
		}
	}

	public bool ShowSums
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowSums"].Value);
		}
		set
		{
			Parameters["ShowSums"].Value = value;
		}
	}

	public bool ShowSumsWithVAT
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowSumsWithVAT"].Value);
		}
		set
		{
			Parameters["ShowSumsWithVAT"].Value = value;
		}
	}

	public bool ShowCurrSums
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowCurrSums"].Value);
		}
		set
		{
			Parameters["ShowCurrSums"].Value = value;
		}
	}

	public string AdditionalCaption
	{
		get
		{
			return (string)Parameters["AdditionalCaption"].Value;
		}
		set
		{
			Parameters["AdditionalCaption"].Value = value;
		}
	}

	public GoodsCostCalculation()
	{
		DataProvider = new GoodsCostCalculationDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("BaseISN", ArmSoft.SME.Trade.Common.Resources.ShowAvailables, Guid.Empty);
		AddParameter("ShowSums", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, false);
		AddParameter("ShowSumsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, false);
		AddParameter("ShowCurrSums", ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, false);
		AddParameter("AdditionalCaption", ArmSoft.SME.Trade.Common.Resources.AdditionalCaption, string.Empty);
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		string documentDescription = TradeSettings.CreateTradeClient().CommonServices.GetDocumentDescription(BaseISN, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
		dvp.Set_ColumnCaption("Summ", $"{documentDescription}, {ArmSoft.SME.Trade.Common.Resources.SumNCC}{ArmSoft.SME.Trade.Common.Resources.WithoutNDS}");
		dvp.Set_ColumnCaption("VATSumm", $"{documentDescription}, {ArmSoft.SME.Trade.Common.Resources.SumNCC}{ArmSoft.SME.Trade.Common.Resources.WithNDS}");
		dvp.Set_ColumnCaption("CURSumm", $"{documentDescription}, {string.Format(ArmSoft.SME.Trade.Common.Resources.SummCurrIso, Settings.ParametersService.MTCurr())}");
		dvp.Set_ColumnVisible("Cost", ShowSums && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("Summ", ShowSums && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("Total", ShowSums && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("GoodUnitCost", ShowSums && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("VATCost", ShowSumsWithVAT && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("VATSumm", ShowSumsWithVAT && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("TotalVAT", ShowSumsWithVAT && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("GoodUnitCostVAT", ShowSumsWithVAT && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("CURCost", ShowCurrSums && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("CURSumm", ShowCurrSums && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("TotalCUR", ShowCurrSums && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		dvp.Set_ColumnVisible("GoodUnitCostCUR", ShowCurrSums && Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
	}

	public override string ReportTitle()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(Caption);
		return stringBuilder.ToString();
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.BaseISN = BaseISN;
		ParametersProvider.ShowSums = ShowSums;
		ParametersProvider.ShowSumsWithVAT = ShowSumsWithVAT;
		ParametersProvider.ShowCurrSums = ShowCurrSums;
	}
}
