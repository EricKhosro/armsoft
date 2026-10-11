using System;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Services;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI;

public class PartnerContractGroupControls : UserControl
{
	public delegate void PartnerCodeEditValueChangedEventHandler(object sender, EditValueChangedEventArgs e);

	public delegate void PartnerCodeEditAdditionalValueChangedEventHandler(object sender, EventArgs e);

	public delegate void ContractCodeEditValueChangedEventHandler(object sender, EventArgs e);

	private readonly IPOWindow ownerDocWindow;

	private readonly DialogWindow ownerDlgWindow;

	private readonly bool isOwnerDocWindow = true;

	public DropDownControl PartnerGroup;

	public DropDownControl PartnerCode;

	public DropDownControl Contract;

	public PopupCalcEditExt SummaPart;

	public PopupCalcEditExt SummaPartDebt;

	public DropDownControl ContractCurrCode;

	private Button _partDebt;

	private DateEditControl _date;

	private DropDownControl _currCode;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _summaNCC;

	private bool _isFilterBySuppliers;

	private bool _isFilterByCustomers;

	private bool _isVisibleSummaPart;

	private bool _showOnlyBonusContracts;

	private bool _isExistContractCurrCodeControl = true;

	private bool _isShowSummaDebtControl;

	private bool _isShowOnlyCurCodeNCC;

	private bool _isContractCodeRequired = true;

	private bool _connectTob2barmsoftb2bServerforBonusBalance;

	private PartnerCacheRow _partnerCacheRow;

	private PartnersFilter partnersFilter;

	private Contracts contractsDropDown;

	protected internal PopupCalcEditExt DocSummaControl
	{
		set
		{
			_summa = value;
		}
	}

	protected internal DropDownControl DocCurrCodeControl
	{
		set
		{
			_currCode = value;
		}
	}

	public bool IsReadOnly
	{
		set
		{
			PartnerGroup.IsReadOnly = value;
			PartnerCode.IsReadOnly = value;
			Contract.IsReadOnly = value;
			if (ContractCurrCode != null)
			{
				ContractCurrCode.IsReadOnly = true;
			}
		}
	}

	public event PartnerCodeEditValueChangedEventHandler PartnerCodeEditValueChanged;

	public event PartnerCodeEditAdditionalValueChangedEventHandler PartnerCodeEditAdditionalValueChanged;

	public event ContractCodeEditValueChangedEventHandler ContractCodeEditValueChanged;

	protected internal PartnerContractGroupControls(IPOWindow docWindow)
	{
		ownerDocWindow = docWindow;
	}

	protected internal PartnerContractGroupControls(DialogWindow dlgWindow)
	{
		ownerDlgWindow = dlgWindow;
		isOwnerDocWindow = false;
	}

	protected internal void ConstructUI(LayoutGroup lgPartCont, DateEditControl docDateControl, DropDownControl docCurrCodeControl, PopupCalcEditExt docSummaControl, PopupCalcEditExt docSummaNCCControl = null, string partnerCaption = "", bool isFilterBySuppliers = false, bool isFilterByCustomers = false, bool isVisibleSummaPart = true, bool showOnlyBonusContracts = false, string contractCaption = "", bool isShowContractCurrCode = true, bool isShowSummaDebt = true, bool showOnlyCurCodeNCC = false, bool isContractRequired = true, bool allowPartnerAdditionalButtons = true, string dataItemName = "")
	{
		_date = docDateControl;
		_currCode = docCurrCodeControl;
		_summa = docSummaControl;
		_summaNCC = docSummaNCCControl;
		_isFilterBySuppliers = isFilterBySuppliers;
		_isFilterByCustomers = isFilterByCustomers;
		_isVisibleSummaPart = isVisibleSummaPart;
		_isExistContractCurrCodeControl = isShowContractCurrCode;
		_isShowSummaDebtControl = isShowSummaDebt;
		_showOnlyBonusContracts = showOnlyBonusContracts;
		_isShowOnlyCurCodeNCC = showOnlyCurCodeNCC;
		_isContractCodeRequired = isContractRequired;
		PartnerGroup = ownerDocWindow.AddDropDownControl(lgPartCont, ArmSoft.SME.Common.Resources.Group, isRequired: false, new PartnerGroups(), allowAdditionButtons: false, HorizontalAlignment.Stretch, "", dataItemName + "PartnerGroup");
		PartnerGroup.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PartnerGroup_EditValueChanged, ownerDocWindow).Handler;
		partnersFilter = new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", _isFilterBySuppliers, _isFilterByCustomers);
		partnerCaption = (string.IsNullOrWhiteSpace(partnerCaption) ? ArmSoft.SME.Common.Resources.Partner : partnerCaption);
		PartnerCode = ownerDocWindow.AddDropDownControl(lgPartCont, partnerCaption, isRequired: false, partnersFilter, allowPartnerAdditionalButtons, HorizontalAlignment.Stretch, "", dataItemName + "PartnerCode");
		PartnerCode.EditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerCode_EditAdditionalValueChanged, ownerDocWindow).Handler;
		PartnerCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PartnerCode_EditValueChanged, ownerDocWindow).Handler;
		PartnerCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(PartnerCode_ClickDropDown).Handler;
		contractCaption = (string.IsNullOrWhiteSpace(contractCaption) ? ArmSoft.SME.Common.Resources.Contract : contractCaption);
		contractsDropDown = new Contracts("", _isShowOnlyCurCodeNCC ? "AMD" : "", isExceptCurrCodeNCC: false, _isFilterBySuppliers, _isFilterByCustomers, isValueMemberPartner: false, _showOnlyBonusContracts);
		Contract = ownerDocWindow.AddDropDownControl(lgPartCont, contractCaption, isRequired: false, contractsDropDown, allowAdditionButtons: false, HorizontalAlignment.Stretch, "", dataItemName + "Contract");
		Contract.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Contract_EditValueChanged, ownerDocWindow).Handler;
		Contract.IsReadOnly = true;
		Contract.IsEnabled = false;
		if (_isVisibleSummaPart)
		{
			SummaPart = ownerDocWindow.AddNumericControl(lgPartCont, ArmSoft.SME.Common.Resources.Summa, isRequired: false, 16, 2, allowNegativeNumbers: true, useCurrencyFormat: true, HorizontalAlignment.Left, "", null, dataItemName + "SummaPart");
			SummaPart.IsReadOnly = true;
			SummaPart.IsTabStop = false;
		}
		LayoutGroup layoutGroup = null;
		if (_isShowSummaDebtControl)
		{
			layoutGroup = ownerDocWindow.AddLayoutGroup(lgPartCont, LayoutGroupView.Group, Orientation.Horizontal);
			layoutGroup.HorizontalAlignment = HorizontalAlignment.Stretch;
			SummaPartDebt = ownerDocWindow.AddNumericControl(layoutGroup, ArmSoft.SME.Trade.Common.Resources.PartnerRem, isRequired: false, 16, 2, allowNegativeNumbers: true, useCurrencyFormat: true, HorizontalAlignment.Left, "", null, dataItemName + "SummaPartDebt");
			SummaPartDebt.IsReadOnly = true;
			SummaPartDebt.IsTabStop = false;
			_partDebt = ownerDocWindow.AddButton(layoutGroup, "refresh_32.svg", null, null, ArmSoft.SME.Common.Resources.Refresh, isTabStop: false, HorizontalAlignment.Left, dataItemName + "PartDebtRefresh");
			_partDebt.IsEnabled = false;
			_partDebt.IsTabStop = false;
			_partDebt.Click += new SafeDocumentEventHandler<RoutedEventArgs>(PartDebt_Click, ownerDocWindow).Handler;
		}
		if (_isExistContractCurrCodeControl)
		{
			ContractCurrCode = ownerDocWindow.AddDropDownControl((_currCode == null) ? lgPartCont : layoutGroup, ArmSoft.SME.Common.Resources.Curr, isRequired: false, new Currency(shortMode: true), allowAdditionButtons: false, HorizontalAlignment.Left, "", dataItemName + "ContractCurrCode");
			ContractCurrCode.IsReadOnly = true;
			ContractCurrCode.IsTabStop = false;
			ContractCurrCode.IsEnabled = false;
		}
	}

	public void SetVisibility(Visibility visibility)
	{
		((LayoutItemWithRequired)(object)PartnerGroup.Parent).Visibility = visibility;
		((LayoutItemWithRequired)(object)PartnerCode.Parent).Visibility = visibility;
		((LayoutItemWithRequired)(object)Contract.Parent).Visibility = visibility;
		((LayoutItemWithRequired)(object)SummaPart.Parent).Visibility = visibility;
		((LayoutItemWithRequired)(object)SummaPartDebt.Parent).Visibility = visibility;
		((LayoutItemWithRequired)(object)_partDebt.Parent).Visibility = visibility;
		((LayoutItemWithRequired)(object)ContractCurrCode.Parent).Visibility = visibility;
	}

	protected internal void ConstructDlgUI(LayoutGroup lgPartCont, bool storeValue = true, string partnerCaption = "", bool isFilterBySuppliers = false, bool isFilterByCustomers = false, bool isShowContractCurrCode = true, bool isShowSummaDebt = false, string contractCaption = "", string summaDebtCaption = "", bool showOnlyBonusContracts = false, bool showOnlyCurCodeNCC = false, bool connectTob2bArmsoftDiscountServerforBonusSum = false, string namePrefix = "", bool showRefreshButtonCaption = false, bool allowadditionalButtons = false)
	{
		_isFilterBySuppliers = isFilterBySuppliers;
		_isFilterByCustomers = isFilterByCustomers;
		_isShowSummaDebtControl = isShowSummaDebt;
		_showOnlyBonusContracts = showOnlyBonusContracts;
		_isShowOnlyCurCodeNCC = showOnlyCurCodeNCC;
		_connectTob2barmsoftb2bServerforBonusBalance = connectTob2bArmsoftDiscountServerforBonusSum;
		partnerCaption = (string.IsNullOrWhiteSpace(partnerCaption) ? ArmSoft.SME.Common.Resources.Partner : partnerCaption);
		contractCaption = (string.IsNullOrWhiteSpace(contractCaption) ? ArmSoft.SME.Common.Resources.Contract : contractCaption);
		summaDebtCaption = (string.IsNullOrWhiteSpace(summaDebtCaption) ? ArmSoft.SME.Trade.Common.Resources.PartnerRem : summaDebtCaption);
		PartnerGroup = ownerDlgWindow.AddDropDownControl(lgPartCont, namePrefix + "PartnerGroup", ArmSoft.SME.Common.Resources.Group, new PartnerGroups(), isRequired: false, allowAdditionButtons: false, storeValue);
		PartnerGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PartnerGroup_EditValueChanged).Handler;
		partnersFilter = new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", _isFilterBySuppliers, _isFilterByCustomers);
		PartnerCode = ownerDlgWindow.AddDropDownControl(lgPartCont, namePrefix + "PartnerCode", partnerCaption, partnersFilter, isRequired: false, allowadditionalButtons, storeValue);
		PartnerCode.EditAdditionalValueChanged += new SafeEventHandler<EventArgs>(PartnerCode_EditAdditionalValueChanged).Handler;
		PartnerCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PartnerCode_EditValueChanged).Handler;
		PartnerCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(PartnerCode_ClickDropDown).Handler;
		contractsDropDown = new Contracts("", _isShowOnlyCurCodeNCC ? "AMD" : "", isExceptCurrCodeNCC: false, _isFilterBySuppliers, _isFilterByCustomers, isValueMemberPartner: false, _showOnlyBonusContracts);
		Contract = ownerDlgWindow.AddDropDownControl(lgPartCont, namePrefix + "Contract", contractCaption, contractsDropDown, isRequired: false, allowAdditionButtons: false, storeValue);
		Contract.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Contract_EditValueChanged).Handler;
		Contract.IsReadOnly = true;
		Contract.IsEnabled = false;
		if (isShowContractCurrCode)
		{
			ContractCurrCode = ownerDlgWindow.AddDropDownControl(lgPartCont, namePrefix + "CurrCode", ArmSoft.SME.Common.Resources.Curr, new Currency(shortMode: true), isRequired: false, allowAdditionButtons: false, storeValue);
		}
		else
		{
			_isExistContractCurrCodeControl = false;
		}
		if (_isShowSummaDebtControl)
		{
			LayoutGroupExt layoutGroupExt = ownerDlgWindow.AddHorizontalLayoutGroup(lgPartCont);
			layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
			SummaPartDebt = ownerDlgWindow.AddNumericControl(layoutGroupExt, namePrefix + "SummaDebt", summaDebtCaption, 16, 2, isRequired: false, allowNegativeNumbers: true);
			SummaPartDebt.IsReadOnly = true;
			SummaPartDebt.IsTabStop = false;
			_partDebt = ownerDlgWindow.AddButton(layoutGroupExt, namePrefix + "Refresh", "refresh_32.svg", showRefreshButtonCaption ? ArmSoft.SME.Common.Resources.Refresh : "", "", ArmSoft.SME.Common.Resources.Refresh);
			_partDebt.IsEnabled = true;
			_partDebt.IsTabStop = true;
			_partDebt.Click += new SafeEventHandler<RoutedEventArgs>(PartDebt_Click).Handler;
			if (_connectTob2barmsoftb2bServerforBonusBalance)
			{
				Label label = new Label();
				TextBlock content = new TextBlock
				{
					Text = ArmSoft.SME.Trade.Common.Resources.IsConnectedTob2barmsoftDiscountServer,
					TextWrapping = TextWrapping.Wrap,
					Foreground = ColorsAS.Foreground.BlueBrush
				};
				label.Content = content;
				ownerDlgWindow.AddControl(layoutGroupExt, "ConnectTob2barmsoftDiscountServer", "", label);
			}
		}
	}

	private void PartnerGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		partnersFilter.PartType = (string)e.NewValue;
	}

	private void PartnerCode_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		PartnersFilter partnersFilter = new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, PartnerGroup.DoValidate() ? PartnerGroup.Code : "", _isFilterBySuppliers, _isFilterByCustomers);
		ModalBrowser modalBrowser = new ModalBrowser(partnersFilter)
		{
			LookUpValue = PartnerCode.Code,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			PartnerCode.Code = partnersFilter.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void PartnerCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		ContractEditValueChanged();
		PartnerCodeEditAdditionalValueChanged?.Invoke(sender, e);
	}

	private void PartnerCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		PartnerEditValueChanged();
		PartnerCodeEditValueChanged?.Invoke(this, e);
	}

	private void PartnerEditValueChanged()
	{
		bool flag = true;
		if (!string.IsNullOrWhiteSpace(PartnerCode.Code) && PartnerCode.DoValidate())
		{
			Contract.IsReadOnly = false;
			_partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(PartnerCode.Code);
			if (_partnerCacheRow != null)
			{
				Contract.DataView = new Contracts(_partnerCacheRow.PartCode, _isShowOnlyCurCodeNCC ? "AMD" : "", isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: false, isValueMemberPartner: false, _showOnlyBonusContracts);
				if (isOwnerDocWindow)
				{
					Contract.IsRequired = _isContractCodeRequired;
					PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(_partnerCacheRow);
					if (defaultContract != null)
					{
						if (Contract.Code.Trim() == defaultContract.Contract.Trim())
						{
							flag = false;
						}
						Contract.Code = defaultContract.Contract;
						Contract.IsEnabled = true;
					}
				}
				else
				{
					Contract.IsEnabled = true;
				}
			}
		}
		else
		{
			Contract.DataView = new Contracts(string.Empty, _isShowOnlyCurCodeNCC ? "AMD" : "", isExceptCurrCodeNCC: false, _isFilterBySuppliers, _isFilterByCustomers);
			Contract.IsRequired = false;
			Contract.IsReadOnly = true;
			Contract.Code = "";
			Contract.IsEnabled = false;
			_partnerCacheRow = null;
		}
		Contract.DoValidate();
		if (!flag)
		{
			ContractEditValueChanged();
		}
	}

	private void Contract_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (((e.NewValue != null) & (e.OldValue != null)) && e.NewValue.ToString().Trim() != e.OldValue.ToString().Trim())
		{
			ContractEditValueChanged();
		}
	}

	private void ContractEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(Contract.Code) && _partnerCacheRow != null)
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(PartnerCode.Code, Contract.Code);
			if (contract != null && _isExistContractCurrCodeControl)
			{
				ContractCurrCode.Code = contract.CurrCode;
				if (!isOwnerDocWindow)
				{
					ContractCurrCode.IsReadOnly = true;
					ContractCurrCode.IsEnabled = false;
				}
			}
			if (isOwnerDocWindow && _isShowSummaDebtControl)
			{
				_partDebt.IsEnabled = true;
			}
		}
		else
		{
			if (_isExistContractCurrCodeControl)
			{
				ContractCurrCode.Code = "";
			}
			if (!isOwnerDocWindow)
			{
				if (_isExistContractCurrCodeControl)
				{
					ContractCurrCode.IsReadOnly = false;
					ContractCurrCode.IsEnabled = true;
				}
			}
			else if (_isShowSummaDebtControl)
			{
				_partDebt.IsEnabled = false;
			}
		}
		if (_isExistContractCurrCodeControl)
		{
			ContractCurrCode.DoValidate();
		}
		if (isOwnerDocWindow)
		{
			CalculatePartSumma();
		}
		if (_isShowSummaDebtControl)
		{
			CalculatePartDebtRem(_showOnlyBonusContracts);
		}
		ContractCodeEditValueChanged?.Invoke(this, new EventArgs());
	}

	private void PartDebt_Click(object sender, RoutedEventArgs e)
	{
		CalculatePartDebtRem(_showOnlyBonusContracts);
	}

	protected internal void CalculatePartSumma(decimal SummaNCC = 0m)
	{
		if (!_isVisibleSummaPart)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(ContractCurrCode.Code) && ContractCurrCode.DoValidate())
		{
			DropDownControl dropDownControl = ((_currCode == null) ? ContractCurrCode : _currCode);
			if (!string.IsNullOrWhiteSpace(dropDownControl.Code) && dropDownControl.DoValidate() && _date.DoValidate())
			{
				try
				{
					decimal num = 0m;
					num = ((_summaNCC == null) ? SummaNCC : _summaNCC.Value);
					if (_summa != null)
					{
						SummaPart.EditValue = TradeSettings.CreateTradeClient().CommonServices.GetPartSummaByDocSumma(dropDownControl.Code, _summa.Value, num, _date.EditValue.Value, ContractCurrCode.Code);
					}
					else
					{
						SummaPart.EditValue = num;
					}
					return;
				}
				catch (Exception ex)
				{
					SummaPart.EditValue = 0m;
					Settings.ErrorHandler(ex);
					return;
				}
			}
			decimal num2 = ((_summaNCC != null) ? _summaNCC.Value : SummaNCC);
			SummaPart.EditValue = num2;
		}
		else
		{
			SummaPart.EditValue = 0m;
		}
	}

	protected internal void CalculatePartDebtRem(bool bBonusContract = false)
	{
		if (_partDebt.IsEnabled && _partnerCacheRow != null)
		{
			decimal num = 0m;
			if (bBonusContract)
			{
				if (_connectTob2barmsoftb2bServerforBonusBalance)
				{
					PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partnerCacheRow, Contract.Code);
					if (contract != null)
					{
						num = DiscountServerService.GetCardBalance(contract.DiscountCard);
					}
				}
				else
				{
					num = TradeSettings.CreateTradeClient().CommonServices.GetBonusRemByPartnerAndContract(_partnerCacheRow.Id, Contract.Code);
				}
			}
			else if (!string.IsNullOrWhiteSpace(Contract.Code))
			{
				num = TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(_partnerCacheRow.Id, Contract.Code, "").SummCurr;
			}
			SummaPartDebt.EditValue = num;
		}
		else
		{
			SummaPartDebt.EditValue = 0m;
		}
	}

	protected internal void ValueChangedForLoadDoc()
	{
		_partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(PartnerCode.Code);
		if (_partnerCacheRow != null)
		{
			Contract.IsReadOnly = false;
			Contract.IsEnabled = true;
			Contract.IsRequired = _isContractCodeRequired;
			Contract.DataView = new Contracts(_partnerCacheRow.PartCode, _isShowOnlyCurCodeNCC ? "AMD" : "", isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: false, isValueMemberPartner: false, _showOnlyBonusContracts);
		}
		ContractEditValueChanged();
	}
}
