using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.SME.Common;
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

[DataView(10058)]
public class ReceivedServicesJournal : TradeDataView<ReceivedServicesJournalDataProvider.DataRow, ReceivedServicesJournalDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly ReceivedServicesJournal _dataView;

		private DateEditControl _startDate;

		private DateEditControl _endDate;

		private DropDownControl _docType;

		private DropDownControl _partnerCode;

		private DropDownControl _itemCode;

		private DropDownControl _docState;

		private List<short> _docTypes;

		public DataViewDialog(ReceivedServicesJournal dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.OperationDatePeriod, useLongDate: false, isRequired: true, !useParamValues);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", string.Empty, useLongDate: false, isRequired: true, !useParamValues);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			int num = 3;
			List<short> list = new List<short>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<short> span = CollectionsMarshal.AsSpan(list);
			span[0] = DocumentTypeSE.MTIncome.Value;
			span[1] = DocumentTypeSE.MTIncomeRetail.Value;
			span[2] = DocumentTypeSE.ReceivedService.Value;
			_docTypes = list;
			_docType = AddDropDownControl("DocType", ArmSoft.SME.Common.Resources.DocumentType, new DocumentType(_docTypes), isRequired: true, !useParamValues);
			_docType.Code = (useParamValues ? dataView.DocType : DocumentTypeSE.ReceivedService.Value.ToString());
			_partnerCode = AddDropDownControl("PartnerCode", ArmSoft.SME.Common.Resources.Partner, new PartnersFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_partnerCode.Code = (useParamValues ? dataView.PartnerCode : string.Empty);
			_itemCode = AddDropDownControl("ItemCode", ArmSoft.SME.Trade.Common.Resources.MaterialOrService, new GoodsFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_itemCode.Code = (useParamValues ? dataView.ItemCode : string.Empty);
			_itemCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(ItemCode_ClickDropDown).Handler;
			_docState = AddDropDownControl("DocState", ArmSoft.SME.Common.Resources.State, new DocumentState(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_docState.Code = (useParamValues ? dataView.DocState : DocumentStateSE.DraftOrProcessed.ValueUI);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.DocType = _docType.Code;
			_dataView.PartnerCode = _partnerCode.Code;
			_dataView.ItemCode = _itemCode.Code;
			_dataView.DocState = _docState.Code;
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

		private void ItemCode_ClickDropDown(object sender, DropDownControlEventArgs e)
		{
			short num = 0;
			if (!string.IsNullOrWhiteSpace(_docType.Code))
			{
				num = short.Parse(_docType.Code);
			}
			if (!_docTypes.Contains(num))
			{
				e.Cancel = true;
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ChooseDocType);
			}
			bool flag = num == DocumentTypeSE.ReceivedService.Value;
			GoodsFilter goodsFilter = new GoodsFilter(shortMode: false, flag ? ItemTypesSE.Services : ItemTypesSE.Goods);
			ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
			{
				LookUpValue = _itemCode.Code,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser.Show())
			{
				_itemCode.Code = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
			}
			e.Cancel = true;
		}
	}

	private bool ParamAllowDiscountInPurchase;

	public override string HelpId => "Purchases_Reports_ReceivedServicesJournal";

	public override bool DialogSupported => true;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowPreview => true;

	public override bool AllowDocumentHistory => true;

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public DateOnly? StartDate
	{
		get
		{
			return (DateOnly)Parameters["StartDate"].Value;
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
			return (DateOnly)Parameters["EndDate"].Value;
		}
		set
		{
			Parameters["EndDate"].Value = value;
		}
	}

	public string DocType
	{
		get
		{
			return (string)Parameters["DocType"].Value;
		}
		set
		{
			Parameters["DocType"].Value = value;
		}
	}

	public string PartnerCode
	{
		get
		{
			return (string)Parameters["PartnerCode"].Value;
		}
		set
		{
			Parameters["PartnerCode"].Value = value;
		}
	}

	public string ItemCode
	{
		get
		{
			return (string)Parameters["ItemCode"].Value;
		}
		set
		{
			Parameters["ItemCode"].Value = value;
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

	public ReceivedServicesJournal()
	{
		DataProvider = new ReceivedServicesJournalDataProvider(TradeSettings.CreateTradeClient());
		ParamAllowDiscountInPurchase = Settings.ParametersService.AllowDiscountInPurchase();
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yyyy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yyyy");
		AddParameter("DocType", ArmSoft.SME.Common.Resources.DocumentType, string.Empty);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("ItemCode", ArmSoft.SME.Common.Resources.Service, string.Empty);
		AddParameter("DocState", ArmSoft.SME.Common.Resources.State, string.Empty);
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		bool flag = DocType == ((short)20).ToString().PadLeft(2, '0');
		bool value = flag || DocType == ((short)7).ToString().PadLeft(2, '0');
		dvp.Set_ColumnVisible("ISN", value: false);
		dvp.Set_ColumnVisible("Storage", value);
		dvp.Set_ColumnVisible("StorageCaption", value);
		if (ParamAllowDiscountInPurchase)
		{
			dvp.Set_ColumnVisible("Discount", value);
			dvp.Set_ColumnVisible("DiscountPrice", value);
		}
		dvp.Set_ColumnVisible("ExtraPercent", flag);
		dvp.Set_ColumnVisible("SalePrice", flag);
		dvp.Set_ColumnVisible("SaleSumma", flag);
		dvp.Set_ColumnVisible("UseLife", value);
		dvp.Set_ColumnVisible("Country", value);
		dvp.Set_ColumnVisible("Producer", value);
		dvp.Set_ColumnVisible("Property1", value);
		dvp.Set_ColumnVisible("Property2", value);
		dvp.Set_ColumnVisible("DocState", value: false);
		dvp.Set_ColumnVisible("DocType", value: false);
		dvp.Set_ColumnVisible("VATType", value: false);
		dvp.Set_ColumnSortOrder("DocDate");
		dvp.Set_ColumnSortOrder("DocNum");
		dvp.Set_ColumnFooterSum("Quantity", value: true);
		dvp.Set_ColumnFooterSum("Summ", value: true);
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		if (Convert.ToInt16(DocType) != DocumentTypeSE.ReceivedService.Value && !TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
		{
			throw new ASException(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName);
		}
		ParametersProvider.StartDate = StartDate.Value;
		ParametersProvider.EndDate = EndDate.Value;
		ParametersProvider.DocType = Convert.ToInt16(DocType).ToString();
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.ItemCode = ItemCode;
		ParametersProvider.DocState = DocState;
	}
}
