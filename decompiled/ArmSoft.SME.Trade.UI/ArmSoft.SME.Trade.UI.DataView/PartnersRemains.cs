using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.Data.Reports;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.ReportViewers.Partner;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10051)]
public class PartnersRemains : TradeDataView<PartnersRemainsDataProvider.DataRow, PartnersRemainsDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly PartnersRemains _dataView;

		private readonly DateEditControl _remDate;

		private readonly PartnerContractGroupControls _partnerContractControl;

		private readonly DropDownControl _repCurrType;

		private readonly CheckEditExt _detailsByContracts;

		private readonly CheckEditExt _showLinks;

		public DataViewDialog(PartnersRemains dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = dataView.InnerCaption;
			Name = dataView.Name + (dataView._isShowForReval ? "Reval" : "");
			HelpId = dataView.HelpId;
			LayoutGroup container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_remDate = AddDateControl(container, "Date", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true, !useParamValues);
			_remDate.EditValue = (useParamValues ? dataView.Date : Settings.HostingEnvironment.CurrentDate);
			AddSpecialDateButton(container, _remDate);
			DXTabControl tc = AddTabControl(null);
			LayoutGroup layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
			LayoutGroup lgPartCont = AddVerticalLayoutGroup(layoutGroup);
			_partnerContractControl = AddPartnerContractGroupControls();
			_partnerContractControl.ConstructDlgUI(lgPartCont, !useParamValues);
			if (useParamValues)
			{
				_partnerContractControl.PartnerGroup.Code = dataView.PartnerGroup;
				_partnerContractControl.PartnerCode.Code = dataView.PartnerCode;
				_partnerContractControl.Contract.Code = dataView.Contract;
				_partnerContractControl.ContractCurrCode.Code = dataView.CurrCode;
			}
			AddLine(layoutGroup);
			_repCurrType = AddDropDownControl(layoutGroup, "ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, new SmartEnum(typeof(ReportCurrTypesSE)), isRequired: true, allowAdditionButtons: false, !dataView._isShowForReval);
			_repCurrType.Code = (useParamValues ? dataView.ReportCurrTypes : "1");
			_detailsByContracts = AddBooleanControl(layoutGroup, "DetailsByContracts", ArmSoft.SME.Trade.Common.Resources.DetailByContrac, !dataView._isShowForReval);
			_detailsByContracts.EditValue = useParamValues && dataView.DetailsByContracts;
			AddLine(layoutGroup);
			_showLinks = AddBooleanControl(layoutGroup, "ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, !useParamValues);
			_showLinks.EditValue = useParamValues && dataView.ShowLinks;
			if (!isRefreshMode)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_remDate.EditValue.Value);
				base.DialogValidation(args);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.Date = _remDate.EditValue.Value;
			_dataView.PartnerGroup = _partnerContractControl.PartnerGroup.Code;
			_dataView.PartnerCode = _partnerContractControl.PartnerCode.Code;
			_dataView.Contract = _partnerContractControl.Contract.Code;
			_dataView.CurrCode = _partnerContractControl.ContractCurrCode.Code;
			_dataView.ReportCurrTypes = _repCurrType.Code;
			_dataView.DetailsByContracts = _detailsByContracts.IsChecked.Value;
			_dataView.ShowLinks = _showLinks.IsChecked.Value;
		}
	}

	private bool _isShowForReval;

	private List<ViewColumn> potentiallyColumns;

	public override string HelpId => "Purchases_Reports_PartnerReports_Remains";

	public override bool DialogSupported => true;

	public override string Caption => base.Caption + " - " + Date.FormatDDMMYY();

	private string InnerCaption => base.Caption ?? "";

	public override LoadIndication LoadingIndication => LoadIndication.AlwaysInside;

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public bool ShowForReval
	{
		set
		{
			_isShowForReval = value;
		}
	}

	private static string PartnerB2BStateExpression
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte item in new List<byte> { 3, 5, 6, 7, 8, 9, 10, 11, 14, 254 })
			{
				if (SmartEnum<PartnerStatusInB2BSE, byte>.TryFromValue(item, out var result))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(11, 2, stringBuilder2);
					handler.AppendLiteral("[");
					handler.AppendFormatted("B2BStatusName");
					handler.AppendLiteral("] = '");
					handler.AppendFormatted(result.Caption);
					handler.AppendLiteral("' or ");
					stringBuilder2.Append(ref handler);
				}
			}
			return stringBuilder.ToString().Left(stringBuilder.Length - 4);
		}
	}

	public DateOnly Date
	{
		get
		{
			return (DateOnly)Parameters["Date"].Value;
		}
		set
		{
			Parameters["Date"].Value = value;
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

	public string PartnerGroup
	{
		get
		{
			return (string)Parameters["PartnerGroup"].Value;
		}
		set
		{
			Parameters["PartnerGroup"].Value = value;
		}
	}

	public string Contract
	{
		get
		{
			return (string)Parameters["Contract"].Value;
		}
		set
		{
			Parameters["Contract"].Value = value;
		}
	}

	public string CurrCode
	{
		get
		{
			return (string)Parameters["CurrCode"].Value;
		}
		set
		{
			Parameters["CurrCode"].Value = value;
		}
	}

	public string ReportCurrTypes
	{
		get
		{
			return (string)Parameters["ReportCurrTypes"].Value;
		}
		set
		{
			Parameters["ReportCurrTypes"].Value = value;
		}
	}

	public bool DetailsByContracts
	{
		get
		{
			return (bool)Parameters["DetailsByContracts"].Value;
		}
		set
		{
			Parameters["DetailsByContracts"].Value = value;
		}
	}

	public bool ShowLinks
	{
		get
		{
			return (bool)Parameters["ShowLinks"].Value;
		}
		set
		{
			Parameters["ShowLinks"].Value = value;
		}
	}

	public PartnersRemains()
	{
		DataProvider = new PartnersRemainsDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("Date", ArmSoft.SME.Common.Resources.DateWord, Settings.HostingEnvironment.CurrentDate, "dd/MM/yy");
		AddParameter("PartnerGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Code, string.Empty);
		AddParameter("Contract", ArmSoft.SME.Common.Resources.Contract, string.Empty);
		AddParameter("CurrCode", ArmSoft.SME.Common.Resources.Curr, string.Empty);
		AddParameter("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, "1");
		AddParameter("DetailsByContracts", ArmSoft.SME.Trade.Common.Resources.DetailByContrac, false);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, true);
		FillColumnsFromDataProvider();
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(PartnerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Partner, "", ""));
		potentiallyColumns.AddRange(PartnerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Contract, "", "", includeCode: false, includeCaption: false, includeTaxCode: true, onlyContract: true));
		AddConditionalFormatting();
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["RemNCC"].IsSystem = true;
		Columns["RemCurr"].IsSystem = true;
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public override string GetDefaultFunction()
	{
		return "OpenPartnerCard";
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartRepCard).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenPartnerCard", ArmSoft.SME.Trade.Common.Resources.PartnerCard, OpenPartnerCard);
			}
			if (DetailsByContracts)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateDoc", ArmSoft.SME.Common.Resources.CreateDoc, CreateDoc, "doc_new_16.svg");
			}
		}
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		PartnersRemainsDataProvider.DataRow dataRow = row as PartnersRemainsDataProvider.DataRow;
		dataRow.PartnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(dataRow.PartCode);
		dataRow.ContractCacheRow = TradeSettings.TradeCacheService.Partner.GetContract(dataRow.PartCode, dataRow.ContractCode);
	}

	private void OpenPartnerCard()
	{
		List<AccRestRow> list = new List<AccRestRow>();
		foreach (PartnersRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PartnersRemainsDataProvider.DataRow>())
		{
			AccRestRow accRestRow = new AccRestRow
			{
				PartID = selectedOrFocusedRow.PartID,
				AccCode = selectedOrFocusedRow.ContractCode,
				PartnerCode = selectedOrFocusedRow.PartCode
			};
			if (ReportCurrTypes != "1")
			{
				accRestRow.CurrCode = selectedOrFocusedRow.CurrCode;
			}
			list.Add(accRestRow);
		}
		PartnerTAccountReportViewer partnerTAccountReportViewer = new PartnerTAccountReportViewer(list);
		partnerTAccountReportViewer.ReportCurrTypes = ReportCurrTypes;
		partnerTAccountReportViewer.DateBegin = Settings.HostingEnvironment.DateBegin;
		partnerTAccountReportViewer.DateEnd = Settings.HostingEnvironment.DateEnd;
		partnerTAccountReportViewer.Show(useParamValues: true);
	}

	private void CreateDoc()
	{
		try
		{
			ContextMenu contextMenu = new ContextMenu();
			contextMenu.FontFamily = Settings.FontFamily;
			contextMenu.FontSize = Settings.GetFontSize();
			HashSet<short> types = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnersGroupRevaluation.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemMove.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemGroupCorrect.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value
			};
			foreach (SmartEnumDataProvider.DataRow docType in Settings.DocumentService.GetDocTypes(types))
			{
				MenuItem menuItem = new MenuItem
				{
					Header = docType.Caption,
					Tag = docType.Code
				};
				menuItem.Click += CreateNewDocuments;
				contextMenu.Items.Add(menuItem);
			}
			contextMenu.ToolTip = ArmSoft.SME.Common.Resources.NewDocuments;
			contextMenu.PlacementTarget = Settings.HostingEnvironment.LayoutManager;
			contextMenu.VerticalOffset = 0.0;
			contextMenu.HorizontalOffset = 0.0;
			contextMenu.IsOpen = contextMenu.Items.Count > 0;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewDocuments(object sender, RoutedEventArgs e)
	{
		try
		{
			if (Settings.HostingEnvironment.CurrentViewPanel == null)
			{
				return;
			}
			short num = Convert.ToInt16(((MenuItem)sender).Tag);
			if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
			{
				switch (num)
				{
				case 2:
					CreateNewCashInDoc();
					break;
				case 3:
					CreateNewCashOutDoc();
					break;
				case 1:
					CreateNewPayOrdDoc();
					break;
				case 25:
					CreatePartnersGroupRevaluation();
					break;
				case 27:
					CreatePartnerRemMove();
					break;
				case 28:
					CreatePartnerRemGroupCorrect();
					break;
				case 4:
					CreateNewBankInDoc();
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewCashInDoc()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		CashIn cashIn = Settings.DocumentService.Create<CashIn>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn);
		cashIn.DocDate = Settings.HostingEnvironment.CurrentDate;
		PartnersRemainsDataProvider.DataRow dataRow = (PartnersRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			cashIn.CurrCode = dataRow.CurrCode;
			if (dataRow.RemCurr > 0m)
			{
				cashIn.Summ = dataRow.RemCurr;
				cashIn.SummNCC = dataRow.RemNCC;
				cashIn.Summ = dataRow.RemCurr;
			}
			cashIn.PartnerID = dataRow.PartID;
			cashIn.PartnerContractGroup.PartnerCode = dataRow.PartCode;
			cashIn.PartnerContractGroup.ContractCode = dataRow.ContractCode;
			cashIn.PayerName = dataRow.PartName;
		}
		Settings.DocumentService.CreateWindow(cashIn).Show();
	}

	private void CreateNewCashOutDoc()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		CashOut cashOut = Settings.DocumentService.Create<CashOut>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut);
		cashOut.DocDate = Settings.HostingEnvironment.CurrentDate;
		PartnersRemainsDataProvider.DataRow dataRow = (PartnersRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			cashOut.CurrCode = dataRow.CurrCode;
			if (dataRow.RemCurr < 0m)
			{
				cashOut.Summ = Math.Abs(dataRow.RemCurr);
				cashOut.SummNCC = Math.Abs(dataRow.RemNCC);
				cashOut.PartnerContractGroup.SummaPart = Math.Abs(dataRow.RemCurr);
			}
			cashOut.PartnerID = dataRow.PartID;
			cashOut.ReceiverName = dataRow.PartName;
			cashOut.PartnerContractGroup.PartnerCode = dataRow.PartCode;
			cashOut.PartnerContractGroup.ContractCode = dataRow.ContractCode;
			cashOut.ReceiverName = dataRow.PartName;
		}
		Settings.DocumentService.CreateWindow(cashOut).Show();
	}

	private void CreateNewPayOrdDoc()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		PayOrder payOrder = Settings.DocumentService.Create<PayOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd);
		payOrder.DocDate = Settings.HostingEnvironment.CurrentDate;
		PartnersRemainsDataProvider.DataRow dataRow = (PartnersRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			payOrder.CurrCode = dataRow.CurrCode;
			if (dataRow.RemNCC < 0m)
			{
				payOrder.Summ = Math.Abs(dataRow.RemCurr);
				payOrder.SummNCC = Math.Abs(dataRow.RemNCC);
				payOrder.PartnerContractGroup.SummaPart = Math.Abs(dataRow.RemCurr);
			}
			payOrder.PartnerContractGroup.PartnerCode = dataRow.PartCode;
			payOrder.PartnerID = dataRow.PartID;
			payOrder.ReceiverName = dataRow.PartName;
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(dataRow.PartCode);
			if (item != null)
			{
				payOrder.ReceiverSettlAcc = item.SettlAcc;
				payOrder.ReceiverTaxCode = item.TaxCode;
				if (!string.IsNullOrWhiteSpace(payOrder.ReceiverTaxCode))
				{
					payOrder.ReceiverLegalStatus = ReceiverLegalStatusTypeSE.LegalEntity.Value;
				}
				else
				{
					payOrder.ReceiverLegalStatus = ReceiverLegalStatusTypeSE.FizEntity.Value;
				}
			}
			payOrder.PartnerContractGroup.ContractCode = dataRow.ContractCode;
		}
		Settings.DocumentService.CreateWindow(payOrder).Show();
	}

	private void CreatePartnersGroupRevaluation()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		PartnersGroupRevaluation partnersGroupRevaluation = Settings.DocumentService.Create<PartnersGroupRevaluation>(25);
		partnersGroupRevaluation.DocDate = Settings.HostingEnvironment.CurrentDate;
		foreach (PartnersRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PartnersRemainsDataProvider.DataRow>())
		{
			if (selectedOrFocusedRow.CurrCode != "AMD")
			{
				partnersGroupRevaluation.RevaluationList.Add(new PartnersGroupRevaluation.RevaluationSpecificationRow
				{
					PartID = selectedOrFocusedRow.PartID,
					PartCode = selectedOrFocusedRow.PartCode,
					PartName = selectedOrFocusedRow.PartName,
					Contract = selectedOrFocusedRow.ContractCode,
					CurrCode = selectedOrFocusedRow.CurrCode
				});
			}
		}
		if (partnersGroupRevaluation.RevaluationList.Count == 0)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotExistRev);
		}
		Settings.DocumentService.CreateWindow(partnersGroupRevaluation).Show();
	}

	private void CreatePartnerRemMove()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			PartnerRemMove partnerRemMove = Settings.DocumentService.Create<PartnerRemMove>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemMove);
			partnerRemMove.DocDate = Settings.HostingEnvironment.CurrentDate;
			PartnersRemainsDataProvider.DataRow dataRow = (PartnersRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				partnerRemMove.PartnerContractGroup.PartnerCode = dataRow.PartCode;
				partnerRemMove.PartnerContractGroup.ContractCode = dataRow.ContractCode;
				partnerRemMove.CurrCode = dataRow.CurrCode;
				partnerRemMove.Summ = dataRow.RemCurr;
				partnerRemMove.SummNCC = dataRow.RemNCC;
			}
			if (partnerRemMove.CurrCode != "AMD")
			{
				partnerRemMove.ExchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(partnerRemMove.CurrCode, partnerRemMove.DocDate);
			}
			Settings.DocumentService.CreateWindow(partnerRemMove).Show();
		}
	}

	private void CreatePartnerRemGroupCorrect()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		PartnerRemGroupCorrect partnerRemGroupCorrect = Settings.DocumentService.Create<PartnerRemGroupCorrect>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemGroupCorrect);
		partnerRemGroupCorrect.DocDate = Settings.HostingEnvironment.CurrentDate;
		List<PartnersRemainsDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PartnersRemainsDataProvider.DataRow>();
		partnerRemGroupCorrect.CurrCode = selectedOrFocusedRows.FirstOrDefault().CurrCode;
		foreach (PartnersRemainsDataProvider.DataRow item in selectedOrFocusedRows)
		{
			if (partnerRemGroupCorrect.CurrCode != item.CurrCode)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.AccRemNotSameCurr);
			}
			partnerRemGroupCorrect.PartRemList.Add(new PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow
			{
				Contract = item.ContractCode,
				PartCode = item.PartCode,
				PartName = item.PartName,
				PartID = item.PartID,
				Summa = -item.RemCurr,
				SummaNCC = -item.RemNCC
			});
		}
		if (partnerRemGroupCorrect.CurrCode != "AMD")
		{
			partnerRemGroupCorrect.ExchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(partnerRemGroupCorrect.CurrCode, partnerRemGroupCorrect.DocDate);
		}
		Settings.DocumentService.CreateWindow(partnerRemGroupCorrect).Show();
	}

	private void CreateNewBankInDoc()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		BankIn bankIn = Settings.DocumentService.Create<BankIn>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn);
		bankIn.DocDate = Settings.HostingEnvironment.CurrentDate;
		PartnersRemainsDataProvider.DataRow dataRow = (PartnersRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			bankIn.CurrCode = dataRow.CurrCode;
			if (dataRow.RemCurr > 0m)
			{
				bankIn.Summ = Math.Abs(dataRow.RemCurr);
				bankIn.SummNCC = Math.Abs(dataRow.RemNCC);
				bankIn.PartnerContractGroup.SummaPart = Math.Abs(dataRow.RemCurr);
			}
			bankIn.PartnerContractGroup.PartnerCode = dataRow.PartCode;
			bankIn.PartnerID = dataRow.PartID;
			bankIn.PartnerContractGroup.ContractCode = dataRow.ContractCode;
		}
		Settings.DocumentService.CreateWindow(bankIn).Show();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		bool flag = ReportCurrTypes != "1";
		dvp.Set_ColumnVisible("ContractCode", DetailsByContracts);
		dvp.Set_ColumnVisible("ContractName", DetailsByContracts);
		dvp.Set_ColumnVisible("ContractDate", DetailsByContracts);
		dvp.Set_ColumnVisible("CurrCode", DetailsByContracts | flag);
		dvp.Set_ColumnVisible("RemCurr", flag);
		dvp.Set_ColumnVisible("RemNCC", !(ReportCurrTypes == "2"));
		dvp.Set_ColumnSortOrder("PartCode");
		if (DetailsByContracts && !dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("ContractCode");
		}
		dvp.Set_ColumnFooterSum("RemNCC", value: true);
		dvp.Set_ColumnFooterSum("RemCurr", !string.IsNullOrWhiteSpace(CurrCode));
		Columns.AddDirectoryLinkColumn(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner, "PartCode", "PartName", "", "", ShowLinks);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Date = Date;
		ParametersProvider.PartnerGroup = PartnerGroup;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.Contract = Contract;
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.ReportCurrTypes = ReportCurrTypes;
		ParametersProvider.DetailsByContracts = DetailsByContracts;
	}

	private void AddConditionalFormatting()
	{
		UserDefinedFormat customFormat = new UserDefinedFormat
		{
			ForegroundColor = "#E50000"
		};
		AsCondititionFormat item = new AsCondititionFormat
		{
			Expression = PartnerB2BStateExpression,
			FieldName = "B2BStatusName",
			FormatName = "Custom",
			CustomFormat = customFormat,
			IsFixed = true
		};
		ConditionalFormattingList.Add(item);
	}
}
