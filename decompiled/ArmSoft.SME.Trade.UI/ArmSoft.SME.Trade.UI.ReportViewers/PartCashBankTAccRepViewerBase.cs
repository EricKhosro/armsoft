using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.ASReport;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.Data.Reports;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.ReportViewers;

public class PartCashBankTAccRepViewerBase : AsReport
{
	private class ShowDailyRemsCurrencyRepRowCell : AsRepRowCell
	{
		public ShowDailyRemsCurrencyRepRowCell()
		{
			ValueStyle = AsRepColStyle.CurrencyStyle;
			FontStyle = FontStyles.Italic;
			FontWeight = FontWeights.Bold;
		}
	}

	private class SimpleCurrencyRepRowCell : AsRepRowCell
	{
		public SimpleCurrencyRepRowCell()
		{
			ValueStyle = AsRepColStyle.CurrencyStyle;
			FontWeight = FontWeights.Bold;
		}
	}

	private class TAccountDialog : TradeDialogWindow
	{
		private PartCashBankTAccRepViewerBase _tAccountRepViewer;

		private PartnerContractGroupControls _partnerContractContainer;

		private DropDownControl _accountCode;

		private DateEditControl _dateBegin;

		private DateEditControl _dateEnd;

		private DropDownControl _partCode;

		private DropDownControl _currCode;

		private DropDownControl _reportCurrType;

		private CheckEditExt _showDailyRems;

		private CheckEditExt _groupByContract;

		public TAccountDialog(PartCashBankTAccRepViewerBase repViewer)
		{
			_tAccountRepViewer = repViewer;
		}

		internal bool MyShowDialog(bool useParamValues, bool isRefreshMode)
		{
			if (_tAccountRepViewer.AccType == CommonConst.AccountingsTypes.PartnerContract)
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.PartnerCard;
			}
			else if (_tAccountRepViewer.AccType == CommonConst.AccountingsTypes.Bonus)
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusTAccount;
			}
			else if (_tAccountRepViewer.AccType == CommonConst.AccountingsTypes.Bank)
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.BankTAccount;
			}
			else
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.CashDeskTAccount;
			}
			Name = _tAccountRepViewer.Name;
			HelpId = _tAccountRepViewer.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			bool isShowFromTurns = _tAccountRepViewer._isShowFromTurns;
			_dateBegin = AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !isShowFromTurns, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_dateBegin.EditValue = (useParamValues ? _tAccountRepViewer.DateBegin : Settings.HostingEnvironment.DateBegin);
			_dateEnd = AddDateControl(container, "DateEnd", "", useLongDate: false, isRequired: true, !isShowFromTurns, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_dateEnd.EditValue = (useParamValues ? _tAccountRepViewer.DateEnd : Settings.HostingEnvironment.DateEnd);
			AddSpecialDateButton(container, _dateBegin, _dateEnd);
			if (!_tAccountRepViewer._isByPartner && !_tAccountRepViewer._isByBonus && !_tAccountRepViewer.IsForSelectedRows)
			{
				if (_tAccountRepViewer.AccType == CommonConst.AccountingsTypes.Cash)
				{
					_accountCode = AddDropDownControl("AccountCode", ArmSoft.SME.Common.Resources.Cash, new ArmSoft.SME.Trade.UI.DropDown.Cashes(), isRequired: true, allowAdditionButtons: false, !useParamValues);
					_accountCode.Code = (useParamValues ? _tAccountRepViewer.AccountCode : string.Empty);
					_currCode = AddDropDownControl("CurrCode", ArmSoft.SME.Common.Resources.Curr, new Currency(shortMode: true), isRequired: false, allowAdditionButtons: false, !useParamValues);
					_currCode.Code = (useParamValues ? _tAccountRepViewer.CurrCode : string.Empty);
				}
				else
				{
					_accountCode = AddDropDownControl("AccountCode", ArmSoft.SME.Common.Resources.Account, new ArmSoft.SME.Trade.UI.DropDown.AccountInBanks(isKeySettlAccount: false), isRequired: true, allowAdditionButtons: false, !useParamValues);
					_accountCode.Code = (useParamValues ? _tAccountRepViewer.AccountCode : string.Empty);
				}
			}
			if ((_tAccountRepViewer._isByPartner || _tAccountRepViewer._isByBonus) && !_tAccountRepViewer.IsForSelectedRows)
			{
				LayoutGroupExt lgPartCont = AddVerticalLayoutGroup();
				_partnerContractContainer = AddPartnerContractGroupControls();
				_partnerContractContainer.ConstructDlgUI(lgPartCont, !useParamValues, "", isFilterBySuppliers: false, isFilterByCustomers: false, _tAccountRepViewer._isByPartner, isShowSummaDebt: false, "", "", _tAccountRepViewer._isByBonus);
				_partnerContractContainer.PartnerCode.IsRequired = true;
				if (useParamValues)
				{
					_partnerContractContainer.PartnerGroup.Code = _tAccountRepViewer.PartnerGroup;
					_partnerContractContainer.PartnerCode.Code = _tAccountRepViewer.PartnerCode;
					_partnerContractContainer.Contract.Code = _tAccountRepViewer.ContractCode;
					if (_tAccountRepViewer._isByPartner)
					{
						_partnerContractContainer.ContractCurrCode.Code = _tAccountRepViewer.CurrCode;
					}
				}
			}
			AddLine();
			if (!_tAccountRepViewer._isByBonus)
			{
				_reportCurrType = AddDropDownControl("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, new SmartEnum(typeof(ReportCurrTypesSE)), isRequired: true, allowAdditionButtons: false, !useParamValues);
				_reportCurrType.Code = (useParamValues ? _tAccountRepViewer.ReportCurrTypes : "1");
			}
			if (!_tAccountRepViewer._isByBonus && !_tAccountRepViewer._isByPartner)
			{
				_partCode = AddDropDownControl("PartnerCode", ArmSoft.SME.Common.Resources.Partner, new PartnersFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
				_partCode.Code = (useParamValues ? _tAccountRepViewer.PartnerCode : string.Empty);
			}
			_showDailyRems = AddBooleanControl("ShowDailyRems", ArmSoft.SME.Trade.Common.Resources.MTTAccDlgShowDailyRem, storeValue: true);
			_groupByContract = AddBooleanControl("GroupByContract", (_tAccountRepViewer._isByBonus || _tAccountRepViewer._isByPartner) ? ArmSoft.SME.Trade.Common.Resources.GroupByContract : ArmSoft.SME.Trade.Common.Resources.GroupByPartner, storeValue: true);
			_showDailyRems.IsChecked = useParamValues && _tAccountRepViewer.ShowDailyRems;
			_showDailyRems.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowDailyRems_EditValueChanged).Handler;
			_groupByContract.IsChecked = useParamValues && _tAccountRepViewer.GroupByContract;
			_groupByContract.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GroupByContract_EditValueChanged).Handler;
			_showDailyRems.IsReadOnly = ((isRefreshMode && _groupByContract.IsChecked.Value) ? true : false);
			_groupByContract.IsReadOnly = ((isRefreshMode && _showDailyRems.IsChecked.Value) ? true : false);
			WindowValidation += ParametersDialogValidation;
			if (ShowDialog().Value)
			{
				_tAccountRepViewer.DateBegin = _dateBegin.EditValue.Value;
				_tAccountRepViewer.DateEnd = _dateEnd.EditValue.Value;
				if ((_tAccountRepViewer._isByBonus || _tAccountRepViewer._isByPartner) && !_tAccountRepViewer.IsForSelectedRows)
				{
					_tAccountRepViewer.PartnerGroup = _partnerContractContainer.PartnerGroup.Code;
					_tAccountRepViewer.PartnerCode = _partnerContractContainer.PartnerCode.Code;
					_tAccountRepViewer.ContractCode = _partnerContractContainer.Contract.Code;
					if (_tAccountRepViewer._isByPartner)
					{
						_tAccountRepViewer.CurrCode = _partnerContractContainer.ContractCurrCode.Code;
					}
				}
				if (!_tAccountRepViewer._isByBonus)
				{
					_tAccountRepViewer.ReportCurrTypes = _reportCurrType.Code;
				}
				_tAccountRepViewer.ShowDailyRems = _showDailyRems.IsChecked.Value;
				_tAccountRepViewer.GroupByContract = _groupByContract.IsChecked.Value;
				if (!_tAccountRepViewer._isByBonus && !_tAccountRepViewer._isByPartner && !_tAccountRepViewer.IsForSelectedRows)
				{
					_tAccountRepViewer.AccountCode = _accountCode.Code;
					if (_tAccountRepViewer.AccType == CommonConst.AccountingsTypes.Cash)
					{
						_tAccountRepViewer.AccountCode = _accountCode.Code;
						_tAccountRepViewer.CurrCode = _currCode.Code;
					}
					else
					{
						AccountInBankCacheRow item = TradeSettings.TradeCacheService.AccountInBank.GetItem(_accountCode.Code);
						if (item != null)
						{
							_tAccountRepViewer.CurrCode = item.Curr;
						}
						else
						{
							_tAccountRepViewer.CurrCode = string.Empty;
						}
					}
				}
				if (!_tAccountRepViewer._isByBonus && !_tAccountRepViewer._isByPartner)
				{
					_tAccountRepViewer.PartnerCode = _partCode.Code;
				}
				return true;
			}
			return false;
		}

		private void ParametersDialogValidation(DialogWindow dialog, DialogWindowValidationArgs args)
		{
			ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_dateBegin.EditValue.Value.ToDateTime(), _dateEnd.EditValue.Value.ToDateTime());
			args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_dateEnd.EditValue.Value);
			if (!_tAccountRepViewer._isByBonus && !_tAccountRepViewer.IsForSelectedRows && _reportCurrType.Code != "1" && _tAccountRepViewer.AccType != CommonConst.AccountingsTypes.Bank)
			{
				string value = ((!_tAccountRepViewer._isByPartner) ? _currCode.Code : _partnerContractContainer.ContractCurrCode.Code);
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidateEmptyCurrCode);
				}
			}
		}

		private void GroupByContract_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			try
			{
				_showDailyRems.IsReadOnly = Convert.ToBoolean(_groupByContract.IsChecked);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}

		private void ShowDailyRems_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			try
			{
				_groupByContract.IsReadOnly = Convert.ToBoolean(_showDailyRems.IsChecked);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	protected CommonConst.AccountingsTypes ReportType;

	private Dictionary<string, string> _currenciesKeyValuesDict = new Dictionary<string, string>();

	private const string _listOfRepColumnNameLeaf = "HeadTable";

	private List<AsRepColumn> _listOfRepColumnForAccountLeaf;

	protected List<AccRestRow> SelectedRows;

	private string _currCodeParameterCaption = "";

	private bool _isShowFromTurns;

	protected bool IsForSelectedRows;

	protected string InnerCaption;

	protected string InnerName;

	protected CommonConst.AccountingsTypes AccType;

	private readonly bool _isByPartner;

	private readonly bool _isByBonus;

	public bool ShowFromTurns
	{
		set
		{
			_isShowFromTurns = value;
		}
	}

	public override string Name => InnerName;

	public override bool AllowEdit
	{
		get
		{
			if (AllowOperation)
			{
				return !GroupByContract;
			}
			return false;
		}
	}

	public override bool AllowView => !GroupByContract;

	public override bool AllowDelete
	{
		get
		{
			if (AllowOperation)
			{
				return !GroupByContract;
			}
			return false;
		}
	}

	public override bool AllowCreateCopy
	{
		get
		{
			if (AllowOperation)
			{
				return !GroupByContract;
			}
			return false;
		}
	}

	public override bool AllowPreview => !GroupByContract;

	public override bool AllowDocumentHistory
	{
		get
		{
			if (AllowOperation)
			{
				return !GroupByContract;
			}
			return false;
		}
	}

	public DateOnly DateBegin
	{
		get
		{
			return (DateOnly)_parameters["DateBegin"].Value;
		}
		set
		{
			_parameters["DateBegin"].Value = value;
		}
	}

	public DateOnly DateEnd
	{
		get
		{
			return (DateOnly)_parameters["DateEnd"].Value;
		}
		set
		{
			_parameters["DateEnd"].Value = value;
		}
	}

	public string PartnerGroup
	{
		get
		{
			return (string)_parameters["PartnerGroup"].Value;
		}
		set
		{
			_parameters["PartnerGroup"].Value = value;
		}
	}

	public string PartnerCode
	{
		get
		{
			return (string)_parameters["PartnerCode"].Value;
		}
		set
		{
			_parameters["PartnerCode"].Value = value;
		}
	}

	public string AccountCode
	{
		get
		{
			return (string)_parameters["AccountCode"].Value;
		}
		set
		{
			_parameters["AccountCode"].Value = value;
		}
	}

	public string ContractCode
	{
		get
		{
			return (string)_parameters["ContractCode"].Value;
		}
		set
		{
			_parameters["ContractCode"].Value = value;
		}
	}

	public string ReportCurrTypes
	{
		get
		{
			return (string)_parameters["ReportCurrTypes"].Value;
		}
		set
		{
			_parameters["ReportCurrTypes"].Value = value;
		}
	}

	public string CurrCode
	{
		get
		{
			return (string)_parameters["CurrCode"].Value;
		}
		set
		{
			_parameters["CurrCode"].Value = value;
		}
	}

	public bool GroupByContract
	{
		get
		{
			return (bool)_parameters["GroupByContract"].Value;
		}
		set
		{
			_parameters["GroupByContract"].Value = value;
		}
	}

	public bool ShowDailyRems
	{
		get
		{
			return (bool)_parameters["ShowDailyRems"].Value;
		}
		set
		{
			_parameters["ShowDailyRems"].Value = value;
		}
	}

	protected PartCashBankBonusTAccRepParam _reportRequest { get; set; }

	protected Dictionary<string, PartCashBankBonusTAccRepResult> _reportResult { get; set; }

	public PartCashBankTAccRepViewerBase(CommonConst.AccountingsTypes accType)
	{
		AccType = accType;
		_isByPartner = AccType == CommonConst.AccountingsTypes.PartnerContract;
		_isByBonus = AccType == CommonConst.AccountingsTypes.Bonus;
		DialogSupported = true;
		AddParameter("DateBegin", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("DateEnd", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		if (!_isByPartner && !_isByBonus)
		{
			if (AccType == CommonConst.AccountingsTypes.Cash)
			{
				AddParameter("AccountCode", ArmSoft.SME.Common.Resources.Cash, "", "", !IsForSelectedRows);
			}
			else
			{
				AddParameter("AccountCode", ArmSoft.SME.Common.Resources.Account, "", "", !IsForSelectedRows);
			}
		}
		AddParameter("PartnerGroup", ArmSoft.SME.Common.Resources.Group, "", "", includeCaptionToolTip: false);
		if (_isByPartner || _isByBonus)
		{
			AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, "", "", !IsForSelectedRows);
		}
		AddParameter("ContractCode", ArmSoft.SME.Common.Resources.Contract, "", "", !IsForSelectedRows);
		if (!_isByBonus)
		{
			AddParameter("CurrCode", ArmSoft.SME.Common.Resources.Curr, "", "", !IsForSelectedRows);
			AddParameter("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, ReportCurrTypesSE.NCOnly.Value);
		}
		if (!_isByPartner && !_isByBonus)
		{
			AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, "");
		}
		AddParameter("ShowDailyRems", ArmSoft.SME.Trade.Common.Resources.MTTAccDlgShowDailyRem, false);
		AddParameter("GroupByContract", (_isByBonus || _isByPartner) ? ArmSoft.SME.Trade.Common.Resources.GroupByContract : ArmSoft.SME.Trade.Common.Resources.GroupByPartner, false);
	}

	public override string Caption()
	{
		if (_isByPartner || _isByBonus)
		{
			return InnerCaption + PartnerCode;
		}
		return InnerCaption + AccountCode;
	}

	public override bool BeforeShowReport()
	{
		ReportPanel.ClearAll();
		_listOfRepColumnForAccountLeaf = GetListOfRepColumns();
		ReportPanel.AddTableDefinition("HeadTable", _listOfRepColumnForAccountLeaf);
		if (IsForSelectedRows)
		{
			if (SelectedRows.Count == 0)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.AllAccountIsAccumulate);
			}
			foreach (AccRestRow selectedRow in SelectedRows)
			{
				TAccountForOneAccount(selectedRows: true, selectedRow.PartnerCode, selectedRow.AccCode, selectedRow.CurrCode);
				if (IsCanceled)
				{
					return true;
				}
				if (selectedRow != SelectedRows[SelectedRows.Count - 1])
				{
					ReportPanel.AddString("");
					ReportPanel.AddPageBreak(selectedRow.PartnerCode + "/" + selectedRow.AccCode.Trim() + "/" + selectedRow.CurrCode.Trim());
				}
			}
		}
		else
		{
			TAccountForOneAccount(selectedRows: false, PartnerCode, (_isByPartner || _isByBonus) ? ContractCode : AccountCode, _isByBonus ? "AMD" : CurrCode);
		}
		ReportPanel.EvenOddStyle = !ShowDailyRems && !IsForSelectedRows;
		return true;
	}

	private List<AsRepColumn> GetListOfRepColumns()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		AsRepColumn item;
		if (!GroupByContract)
		{
			item = new AsRepColumn(ArmSoft.SME.Common.Resources.DateWord, 80.0)
			{
				ValuesStyle = AsRepColStyle.DateStyle,
				CaptionAlignment = TextAlignment.Left,
				DataAlignment = TextAlignment.Left
			};
			list.Add(item);
		}
		string caption = (_isByPartner ? (ArmSoft.SME.Trade.Common.Resources.DebtIncrease + Environment.NewLine + ArmSoft.SME.Trade.Common.Resources.WithCurr) : ArmSoft.SME.Trade.Common.Resources.TurnInCurrency);
		string caption2 = (_isByPartner ? (ArmSoft.SME.Trade.Common.Resources.DebtDecrease + Environment.NewLine + ArmSoft.SME.Trade.Common.Resources.WithCurr) : ArmSoft.SME.Trade.Common.Resources.TurnInCurrencyOut);
		item = new AsRepColumn((_isByPartner || _isByBonus) ? ArmSoft.SME.Common.Resources.Contract : ArmSoft.SME.Common.Resources.Partner, ((_isByPartner || _isByBonus) ? 3.0 : 4.2) * 18.0);
		list.Add(item);
		item = new AsRepColumn(ArmSoft.SME.Common.Resources.Name, 200.0);
		list.Add(item);
		if (!_isByBonus && ReportCurrTypes != "1")
		{
			item = new AsRepColumn(caption, 160.0);
			item.ValuesStyle = AsRepColStyle.CurrencyStyle;
			item.CaptionAlignment = TextAlignment.Right;
			item.DataBorders = new ObservableCollection<BorderType>();
			item.DataBorders.Add(BorderType.RightBorder);
			list.Add(item);
			item = new AsRepColumn(caption2, 160.0);
			item.ValuesStyle = AsRepColStyle.CurrencyStyle;
			item.CaptionAlignment = TextAlignment.Right;
			item.DataBorders = new ObservableCollection<BorderType>();
			item.DataBorders.Add(BorderType.LeftBorder);
			list.Add(item);
		}
		if (_isByBonus || ReportCurrTypes != "2")
		{
			object caption3;
			if (_isByPartner)
			{
				caption3 = ArmSoft.SME.Trade.Common.Resources.DebtIncrease + Environment.NewLine + ArmSoft.SME.Trade.Common.Resources.WithAMD;
			}
			else
			{
				caption3 = (_isByBonus ? ArmSoft.SME.Trade.Common.Resources.BonusPointsIn : ArmSoft.SME.Trade.Common.Resources.TurnInAMDIn);
			}
			string caption4;
			if (_isByPartner)
			{
				caption4 = ArmSoft.SME.Trade.Common.Resources.DebtDecrease + Environment.NewLine + ArmSoft.SME.Trade.Common.Resources.WithAMD;
			}
			else
			{
				caption4 = (_isByBonus ? ArmSoft.SME.Trade.Common.Resources.BonusPointsOut : ArmSoft.SME.Trade.Common.Resources.TurnInAMDOut);
			}
			item = new AsRepColumn((string)caption3, 160.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				DataBorders = new ObservableCollection<BorderType>()
			};
			item.DataBorders.Add(BorderType.RightBorder);
			list.Add(item);
			item = new AsRepColumn(caption4, 160.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				DataBorders = new ObservableCollection<BorderType>()
			};
			item.DataBorders.Add(BorderType.LeftBorder);
			list.Add(item);
		}
		if (!GroupByContract)
		{
			item = new AsRepColumn(ArmSoft.SME.Common.Resources.DocNumber, 108.0);
			list.Add(item);
			item = new AsRepColumn(ArmSoft.SME.Common.Resources.Comment, 150.0);
			list.Add(item);
			item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MatRepOperationType, 150.0);
			list.Add(item);
		}
		return list;
	}

	private void TAccountForOneAccount(bool selectedRows, string partCode, string accountCode, string currCode)
	{
		DateOnly? value = null;
		decimal num = 0m;
		decimal num2 = 0m;
		short columnSpan = 1;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string empty = string.Empty;
		empty = ((!_isByBonus && !_isByPartner) ? accountCode : (partCode + accountCode));
		if (_isByBonus || _isByPartner)
		{
			dictionary.Clear();
			foreach (PartnerCacheRow.ContractRow contract in TradeSettings.TradeCacheService.Partner.GetItem(partCode).Contracts)
			{
				dictionary.Add(contract.Contract, contract.Caption);
			}
			ShowStatus(ArmSoft.SME.Common.Resources.Partner + " " + _reportResult[empty].PartnerCode);
		}
		else
		{
			ShowStatus(ArmSoft.SME.Common.Resources.Partner + " " + _reportResult[empty].PartnerCode);
		}
		ReportPanel.UnmovableHeaderLinesCount = ((!selectedRows) ? 2 : 0);
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell asRepRowCell = new AsRepRowCell();
		asRepRowCell.ColumnSpan = columnSpan;
		if (_isByPartner || _isByBonus)
		{
			asRepRowCell.Value = _reportResult[empty].PartnerCaption.Trim() + ArmSoft.SME.Common.Resources.ArmenianBut + _reportResult[empty].PartnerCode.Trim();
		}
		else
		{
			asRepRowCell.Value = _reportResult[empty].CashBankName.Trim() + ArmSoft.SME.Common.Resources.ArmenianBut + _reportResult[empty].CashBankCode.Trim();
		}
		asRepRowCell.Alignment = TextAlignment.Left;
		asRepRowCell.FontWeight = FontWeights.Bold;
		asRepRowCell.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 4;
		list.Add(asRepRowCell);
		ReportPanel.AddCells(list, AsRepRowStyle.CaptionRow);
		ReportPanel.UnmovableHeaderLinesCount += ((!selectedRows) ? 1 : 0);
		if (((_isByPartner || _isByBonus) && !string.IsNullOrWhiteSpace(accountCode)) || (!_isByPartner && !_isByBonus && !string.IsNullOrWhiteSpace(partCode)))
		{
			list = new List<AsRepRowCell>();
			asRepRowCell = new AsRepRowCell();
			asRepRowCell.ColumnSpan = columnSpan;
			if (_isByPartner || _isByBonus)
			{
				asRepRowCell.Value = ArmSoft.SME.Common.Resources.Contract + ArmSoft.SME.Common.Resources.ArmenianBut + accountCode.Trim() + " - " + (dictionary.ContainsKey(accountCode) ? dictionary[accountCode].Trim() : "");
			}
			else
			{
				PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(partCode);
				if (item != null)
				{
					_reportResult[empty].PartnerCaption = item.Caption;
					_reportResult[empty].PartnerCode = item.PartCode;
				}
				asRepRowCell.Value = ArmSoft.SME.Common.Resources.Partner + ArmSoft.SME.Common.Resources.ArmenianBut + _reportResult[empty].PartnerCode.Trim() + " - " + _reportResult[empty].PartnerCaption.Trim();
			}
			asRepRowCell.Alignment = TextAlignment.Left;
			asRepRowCell.FontWeight = FontWeights.Bold;
			asRepRowCell.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 4;
			list.Add(asRepRowCell);
			ReportPanel.AddCells(list, AsRepRowStyle.CaptionRow);
			ReportPanel.UnmovableHeaderLinesCount += ((!selectedRows) ? 1 : 0);
		}
		if (!_isByBonus && !string.IsNullOrWhiteSpace(currCode))
		{
			list = new List<AsRepRowCell>();
			asRepRowCell = new AsRepRowCell();
			asRepRowCell.ColumnSpan = columnSpan;
			_currCodeParameterCaption = Settings.CoreCacheService.Currency.GetItem(currCode).Name;
			asRepRowCell.Value = currCode.Trim() + " (" + (_currenciesKeyValuesDict.ContainsKey(currCode) ? _currenciesKeyValuesDict[currCode] : (_currCodeParameterCaption + ")"));
			asRepRowCell.Alignment = TextAlignment.Left;
			asRepRowCell.FontWeight = FontWeights.Bold;
			asRepRowCell.FontSize = Convert.ToInt32(Settings.GetFontSize()) + 1;
			list.Add(asRepRowCell);
			ReportPanel.AddCells(list, AsRepRowStyle.CaptionRow);
			ReportPanel.UnmovableHeaderLinesCount += ((!selectedRows) ? 1 : 0);
		}
		list = new List<AsRepRowCell>();
		asRepRowCell = new SimpleCurrencyRepRowCell();
		asRepRowCell.ColumnSpan = 2 + ((!GroupByContract) ? 1 : 0);
		asRepRowCell.Value = string.Format(ArmSoft.SME.Common.Resources.RemInStartDay, DateExtension.DateToSymbol(DateBegin.ToDateTime(), shortMode: false, isPrintMode: false));
		asRepRowCell.Alignment = TextAlignment.Right;
		list.Add(asRepRowCell);
		if (!_isByBonus && ReportCurrTypes != "1")
		{
			decimal startRemCurr = _reportResult[empty].StartRemCurr;
			bool flag = true;
			if (startRemCurr < 0m)
			{
				flag = false;
			}
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = (flag ? ((object)Math.Abs(startRemCurr)) : "");
			asRepRowCell.Borders.Add(BorderType.None);
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = (flag ? "" : ((object)Math.Abs(startRemCurr)));
			asRepRowCell.Borders.Add(BorderType.None);
			list.Add(asRepRowCell);
		}
		if ((!_isByBonus && ReportCurrTypes != "2") || _isByBonus)
		{
			decimal startRemCurr = _reportResult[empty].StartRemNCC;
			bool flag = true;
			if (startRemCurr < 0m)
			{
				flag = false;
			}
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = (flag ? ((object)Math.Abs(startRemCurr)) : "");
			asRepRowCell.Borders.Add(BorderType.None);
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = (flag ? "" : ((object)Math.Abs(startRemCurr)));
			asRepRowCell.Borders.Add(BorderType.None);
			list.Add(asRepRowCell);
		}
		ReportPanel.AddTableSpanRow("HeadTable", list);
		ReportPanel.AddTableColumnsHeader("HeadTable");
		if (_reportResult[empty].TAccountRows == null || _reportResult[empty].TAccountRows.Count == 0)
		{
			ReportPanel.AddString("");
		}
		else
		{
			if (!_isByBonus && !_isByPartner)
			{
				foreach (PartCashBankBonusTAccRepResult.PartCashBankBonusTAccRow tAccountRow in _reportResult[empty].TAccountRows)
				{
					if (!string.IsNullOrWhiteSpace(tAccountRow.PartnerCode))
					{
						PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(tAccountRow.PartnerCode);
						if (item != null)
						{
							tAccountRow.PartnerCaption = item.Caption;
						}
					}
				}
			}
			if (ShowDailyRems)
			{
				value = _reportResult[empty].TAccountRows[0].OpDate;
				num = _reportResult[empty].StartRemCurr;
				num2 = _reportResult[empty].StartRemNCC;
			}
			foreach (PartCashBankBonusTAccRepResult.PartCashBankBonusTAccRow tAccountRow2 in _reportResult[empty].TAccountRows)
			{
				if ((!GroupByContract && tAccountRow2.CurrCode != currCode && !string.IsNullOrWhiteSpace(currCode)) || (!_isByBonus && ReportCurrTypes == "2" && tAccountRow2.Op == "RVL" && tAccountRow2.CurrSummIn == 0m && tAccountRow2.CurrSummOut == 0m))
				{
					continue;
				}
				if (ShowDailyRems)
				{
					if (value.HasValue && value.Value < tAccountRow2.OpDate)
					{
						List<AsRepRowCell> list2 = new List<AsRepRowCell>();
						asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
						{
							ColumnSpan = 3,
							Value = string.Format(ArmSoft.SME.Common.Resources.RemInEndDay, value.FormatDDMMYY())
						};
						list2.Add(asRepRowCell);
						if (!_isByBonus && ReportCurrTypes != "1")
						{
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell();
							asRepRowCell.Value = ((num >= 0m) ? ((object)num) : "");
							list2.Add(asRepRowCell);
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell();
							asRepRowCell.Value = ((num >= 0m) ? "" : ((object)Math.Abs(num)));
							list2.Add(asRepRowCell);
						}
						if (!_isByBonus && ReportCurrTypes != "2")
						{
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell();
							asRepRowCell.Value = ((num2 >= 0m) ? ((object)num2) : "");
							list2.Add(asRepRowCell);
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell();
							asRepRowCell.Value = ((num2 >= 0m) ? "" : ((object)Math.Abs(num2)));
							list2.Add(asRepRowCell);
						}
						if (_isByBonus)
						{
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell();
							asRepRowCell.Value = ((num2 >= 0m) ? ((object)num2) : "");
							list2.Add(asRepRowCell);
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell();
							asRepRowCell.Value = ((num2 >= 0m) ? "" : ((object)Math.Abs(num2)));
							list2.Add(asRepRowCell);
						}
						ReportPanel.AddTableSpanRow("HeadTable", list2, AsRepRowStyle.TableCaption);
						value = tAccountRow2.OpDate;
					}
					num += tAccountRow2.CurrSummIn - tAccountRow2.CurrSummOut;
					num2 += tAccountRow2.SummIn - tAccountRow2.SummOut;
				}
				List<object> list3 = new List<object>();
				if (!GroupByContract)
				{
					list3.Add(tAccountRow2.OpDate.FormatDDMMYY());
				}
				if (_isByPartner || _isByBonus)
				{
					list3.Add(tAccountRow2.ContractCode);
					list3.Add(dictionary.ContainsKey(tAccountRow2.ContractCode) ? dictionary[tAccountRow2.ContractCode].Trim() : "");
				}
				else
				{
					list3.Add(tAccountRow2.PartnerCode);
					list3.Add(tAccountRow2.PartnerCaption);
				}
				if (!_isByBonus && ReportCurrTypes != "1")
				{
					if (tAccountRow2.CurrSummOut != 0m)
					{
						list3.Add((tAccountRow2.CurrSummIn == 0m) ? "" : ((object)tAccountRow2.CurrSummIn));
						list3.Add(tAccountRow2.CurrSummOut);
					}
					else
					{
						list3.Add((tAccountRow2.CurrSummIn == 0m) ? "" : ((object)tAccountRow2.CurrSummIn));
						list3.Add("");
					}
				}
				if (_isByBonus || ReportCurrTypes != "2")
				{
					if (tAccountRow2.SummOut != 0m)
					{
						list3.Add((tAccountRow2.SummIn == 0m) ? "" : ((object)tAccountRow2.SummIn));
						list3.Add(tAccountRow2.SummOut);
					}
					else
					{
						list3.Add((tAccountRow2.SummIn == 0m) ? "" : ((object)tAccountRow2.SummIn));
						list3.Add("");
					}
				}
				if (!GroupByContract)
				{
					list3.Add(tAccountRow2.DocNum);
					list3.Add(tAccountRow2.Comment);
					list3.Add(tAccountRow2.DocTypeCaption);
				}
				ReportPanel.AddTableRow("HeadTable", list3, tAccountRow2);
			}
		}
		ReportPanel.UnmovableFooterLinesCount = ((!selectedRows) ? 2 : 0);
		list = new List<AsRepRowCell>();
		asRepRowCell = new SimpleCurrencyRepRowCell();
		asRepRowCell.ColumnSpan = 2 + ((!GroupByContract) ? 1 : 0);
		asRepRowCell.Value = ArmSoft.SME.Common.Resources.TheTurn;
		asRepRowCell.Alignment = TextAlignment.Right;
		list.Add(asRepRowCell);
		if (!_isByBonus && ReportCurrTypes != "1")
		{
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = _reportResult[empty].TurnOverInCurr;
			asRepRowCell.Borders.Add(selectedRows ? BorderType.TopBorder : BorderType.None);
			asRepRowCell.Borders.Add(selectedRows ? BorderType.BottomBorder : BorderType.None);
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = _reportResult[empty].TurnOverOutCurr;
			asRepRowCell.Borders.Add(selectedRows ? BorderType.TopBorder : BorderType.None);
			asRepRowCell.Borders.Add(selectedRows ? BorderType.BottomBorder : BorderType.None);
			list.Add(asRepRowCell);
		}
		if (_isByBonus || ReportCurrTypes != "2")
		{
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = _reportResult[empty].TurnOverInNCC;
			asRepRowCell.Borders.Add(selectedRows ? BorderType.TopBorder : BorderType.None);
			asRepRowCell.Borders.Add(selectedRows ? BorderType.BottomBorder : BorderType.None);
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = _reportResult[empty].TurnOverOutNCC;
			asRepRowCell.Borders.Add(selectedRows ? BorderType.TopBorder : BorderType.None);
			asRepRowCell.Borders.Add(selectedRows ? BorderType.BottomBorder : BorderType.None);
			list.Add(asRepRowCell);
		}
		ReportPanel.AddTableSpanRow("HeadTable", list);
		list = new List<AsRepRowCell>();
		asRepRowCell = new SimpleCurrencyRepRowCell();
		asRepRowCell.ColumnSpan = 2 + ((!GroupByContract) ? 1 : 0);
		asRepRowCell.Value = string.Format(ArmSoft.SME.Common.Resources.RemInEndDay, DateExtension.DateToSymbol(DateEnd.ToDateTime(), shortMode: false, isPrintMode: false));
		asRepRowCell.Alignment = TextAlignment.Right;
		list.Add(asRepRowCell);
		if (!_isByBonus && ReportCurrTypes != "1")
		{
			decimal startRemCurr = _reportResult[empty].EndRemCurr;
			bool flag = true;
			if (startRemCurr < 0m)
			{
				flag = false;
			}
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = (flag ? ((object)Math.Abs(startRemCurr)) : "");
			asRepRowCell.Borders.Add(selectedRows ? BorderType.TopBorder : BorderType.None);
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = (flag ? "" : ((object)Math.Abs(startRemCurr)));
			asRepRowCell.Borders.Add(selectedRows ? BorderType.TopBorder : BorderType.None);
			list.Add(asRepRowCell);
		}
		if (_isByBonus || ReportCurrTypes != "2")
		{
			decimal startRemCurr = _reportResult[empty].EndRemNCC;
			bool flag = true;
			if (startRemCurr < 0m)
			{
				flag = false;
			}
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = (flag ? ((object)Math.Abs(startRemCurr)) : "");
			asRepRowCell.Borders.Add(selectedRows ? BorderType.TopBorder : BorderType.None);
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell();
			asRepRowCell.Value = (flag ? "" : ((object)Math.Abs(startRemCurr)));
			asRepRowCell.Borders.Add(selectedRows ? BorderType.TopBorder : BorderType.None);
			list.Add(asRepRowCell);
		}
		ReportPanel.AddTableSpanRow("HeadTable", list);
	}

	public override bool ShowDialog(bool useParamValues, bool isRefreshMode)
	{
		if (AccType == CommonConst.AccountingsTypes.Cash && !TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableCashDesk())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.AccessToCashesDenied, 48);
		}
		return new TAccountDialog(this).MyShowDialog(useParamValues, isRefreshMode);
	}
}
