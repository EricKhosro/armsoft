using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Directories;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.ReportViewers.Partner;
using ArmSoft.SME.Trade.UI.Services;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Ribbon;

namespace ArmSoft.SME.Trade.UI.Directories;

public class PartnerDocumentQuickModeWindow : DirectoryDocumentWindow<Partner>
{
	private TextEditExt _name;

	private DropDownControl _type;

	private DropDownControl _sex;

	private DateEditControl _birthDate;

	private TextEditExt _phone;

	private TextEditExt _email;

	private Partner.PartnerContractRow _defaultContract;

	private TextEditExt _discountCardCode;

	private CheckEditExt _isBonusCard;

	private PopupCalcEditExt _bonus;

	private PopupCalcEditExt _discountPercent;

	private BarSubItem _barSubItemSubDirectories = new BarSubItem();

	private BarSubItem _barSubItemReports = new BarSubItem();

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private RibbonPageGroup _ribbonPageGroupJournals;

	private ReportBarButtonItem _barButtonJumpTo;

	private BarButtonItem _barButtonPartnerChangeRequest;

	private ReportBarButtonItem _barButtonSalesAnalysis;

	private BarButtonItem _barButtonPartnerCard;

	private ReportBarButtonItem _barButtonPartnerRemains;

	private ReportBarButtonItem _barButtonPartnerTurn;

	private ReportBarButtonItem _barButtonPartnerOpJournal;

	private ReportBarButtonItem _barButtonDocumentsJournal;

	private ReportBarButtonItem _barButtonPartAccInBank;

	private ReportBarButtonItem _barButtonPartContracts;

	private ReportBarButtonItem _barButtonPartnersMT;

	protected RibbonPageGroup _ribbonPageGroupSubDirectories;

	protected override double WidthValue => 700.0;

	protected override double HeightValue => 490.0;

	protected override double MinWidthValue => 430.0;

	protected override bool WriteInHistory => true;

	public override DirectoryDefinition Definition
	{
		get
		{
			if (_definition == null)
			{
				DefinitionMappingsService.TryResolveDirectoryDefinition(2, out _definition);
			}
			return _definition;
		}
	}

	protected override string Caption => ArmSoft.SME.Common.Resources.Partner;

	public PartnerDocumentQuickModeWindow()
	{
	}

	public PartnerDocumentQuickModeWindow(Partner doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		if (!PO.StoredInDb)
		{
			_barButtonJumpTo.IsEnabled = false;
			_barButtonPartAccInBank.IsEnabled = false;
			_barButtonPartContracts.IsEnabled = false;
			_barButtonPartnersMT.IsEnabled = false;
			_barButtonPartnerCard.IsEnabled = false;
			_barButtonPartnerRemains.IsEnabled = false;
			_barButtonPartnerTurn.IsEnabled = false;
			_barButtonPartnerOpJournal.IsEnabled = false;
			_barButtonSalesAnalysis.IsEnabled = false;
			_barButtonDocumentsJournal.IsEnabled = false;
			_barButtonPartnerChangeRequest.IsEnabled = false;
		}
		_code = AddButtonEditControl(null, Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_name = AddStringControl(null, Definition.Fields["Name"], HorizontalAlignment.Stretch);
		_phone = AddStringControl(null, Definition.Fields["Phone"], HorizontalAlignment.Stretch);
		_email = AddStringControl(null, Definition.Fields["Email"], HorizontalAlignment.Stretch);
		_discountCardCode = AddStringControl(null, Definition.Grids["PartnerContracts"].Columns["DiscountCard"]);
		_type = AddDropDownControl(null, Definition.Fields["Type"], new PartnerGroups(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_sex = AddSmartEnumDropDownControl(null, Definition.Fields["Sex"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_birthDate = AddDateControl(null, Definition.Fields["Birthday"], useLongDate: true);
		_discountPercent = AddNumericControl(null, Definition.Grids["PartnerContracts"].Columns["Percent"]);
		if (Settings.ParametersService.UseBonus())
		{
			_isBonusCard = AddBooleanControl(null, ArmSoft.SME.Trade.Common.Resources.BonusCard);
			_isBonusCard.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(IsBonusCard_EditValueChanged).Handler;
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				_bonus = AddNumericControl(null, Definition.Grids["PartnerContracts"].Columns["Bonus"]);
			}
			else
			{
				_bonus = AddNumericControl(null, Definition.Grids["PartnerContracts"].Columns["Point"]);
			}
			_bonus.IsReadOnly = true;
		}
		_defaultContract = new Partner.PartnerContractRow();
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _name;
	}

	protected override void AfterStore(POStoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		if (Settings.ParametersService.UseBonus() && Settings.ParametersService.ConnectTob2barmsoftDiscountServer() && _defaultContract.IsBonusCard && _defaultContract.ExportedToDiscountServer && !string.IsNullOrWhiteSpace(_defaultContract.DiscountCard))
		{
			try
			{
				DiscountServerService.UpdateCardInfo(new CardUpdateRequest
				{
					CardNumber = _defaultContract.DiscountCard,
					BonusPercent = _defaultContract.Bonus
				});
			}
			catch
			{
			}
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_name.Text = "";
		_phone.EditValue = "";
		_email.EditValue = "";
		_discountCardCode.EditValue = "";
		_birthDate.EditValue = null;
		_isBonusCard.IsChecked = false;
		_bonus.Value = 0m;
		_discountPercent.Value = 0m;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		_code.IsReadOnly = _viewMode == WindowViewMode.Edit || _viewMode == WindowViewMode.View;
		_barButtonPartnerCard.IsEnabled = false;
		_barButtonPartnerRemains.IsEnabled = false;
		_barButtonPartnerTurn.IsEnabled = false;
		_barButtonPartnerOpJournal.IsEnabled = false;
		_barButtonSalesAnalysis.IsEnabled = false;
		_barButtonDocumentsJournal.IsEnabled = false;
		_barButtonPartnerChangeRequest.IsEnabled = false;
	}

	protected override void FillDocument(DirStoreRequest sr)
	{
		base.FillDocument(sr);
		if (PO.StoredInDb)
		{
			EditDefaultContract();
			return;
		}
		PO.Supplier = false;
		PO.ChiefaccPost = "";
		PO.PartnerContracts.Clear();
		_defaultContract = PartnerContractHelper.AddDefaultContract(_discountCardCode.Text, _discountPercent.Value, _isBonusCard != null && _isBonusCard.IsChecked.Value, (_bonus != null) ? _bonus.Value : 0m);
		PO.PartnerContracts.Add(_defaultContract);
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_defaultContract = new Partner.PartnerContractRow();
		if (PO.PartnerContracts.Count > 0)
		{
			if (PO.StoredInDb)
			{
				_defaultContract = PO.PartnerContracts.First((Partner.PartnerContractRow row) => row.IsDefault);
				_discountCardCode.Text = _defaultContract.DiscountCard;
				_discountPercent.Value = _defaultContract.Percent;
				if (Settings.ParametersService.UseBonus())
				{
					_isBonusCard.IsChecked = _defaultContract.IsBonusCard;
					if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
					{
						_bonus.Value = _defaultContract.Bonus;
					}
					else
					{
						_bonus.Value = _defaultContract.Point;
					}
				}
			}
			else
			{
				_discountCardCode.Text = PO.PartnerContracts.First((Partner.PartnerContractRow row) => row.IsDefault).DiscountCard;
				PO.PartnerContracts.Clear();
			}
		}
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 2;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		span[1] = new PropertyWithValue("ShowExtendedDialog", false.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	public override void OnMagneticCardReader(string cardData)
	{
		ReadMagneticCard(cardData, byMagneticCardPrefixSuffix: true);
	}

	public override void OnScanner(string scannerData)
	{
		ReadMagneticCard(scannerData);
	}

	private void ReadMagneticCard(string cardData, bool byMagneticCardPrefixSuffix = false)
	{
		try
		{
			string empty = string.Empty;
			if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
			{
				if (DocumentHelpers.IsValidPartnerEmployeeCardCode(cardData, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
				{
					_defaultContract.DiscountCard = cardData;
					_discountCardCode.Text = cardData;
					return;
				}
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode);
			}
			if (!byMagneticCardPrefixSuffix)
			{
				empty = cardData;
			}
			else
			{
				if (!MCRInfo.GetInstance().IsValidCard(cardData))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode);
				}
				empty = MCRInfo.GetInstance().GetCardNumber(cardData);
			}
			_defaultContract.DiscountCard = empty;
			_discountCardCode.Text = empty;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private Partner.PartnerContractRow ChangeContractFields(Partner.PartnerContractRow currentContract)
	{
		currentContract.DiscountCard = _discountCardCode.Text;
		currentContract.Percent = _discountPercent.Value;
		if (Settings.ParametersService.UseBonus())
		{
			currentContract.IsBonusCard = Convert.ToBoolean(_isBonusCard.IsChecked);
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				currentContract.Bonus = _bonus.Value;
			}
			else
			{
				currentContract.Point = _bonus.Value;
			}
		}
		return currentContract;
	}

	private void EditDefaultContract()
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(_code.Text))
			{
				_defaultContract = ChangeContractFields(_defaultContract);
				Partner.PartnerContractRow partnerContractRow = PO.PartnerContracts.First((Partner.PartnerContractRow row) => row.IsDefault);
				partnerContractRow.Percent = _defaultContract.Percent;
				partnerContractRow.DiscountCard = _defaultContract.DiscountCard;
				partnerContractRow.IsBonusCard = _defaultContract.IsBonusCard;
				partnerContractRow.Bonus = _defaultContract.Bonus;
			}
		}
		catch
		{
		}
	}

	private void IsBonusCard_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (Convert.ToBoolean(_isBonusCard.IsChecked))
		{
			_bonus.IsReadOnly = false;
			return;
		}
		_bonus.IsReadOnly = true;
		_bonus.Value = 0m;
	}

	protected override Control ControlByName(string name)
	{
		return name switch
		{
			"Code" => (Control)_code, 
			"Type" => _type, 
			"Sex" => _sex, 
			_ => null, 
		};
	}

	protected override void AddItemsToRibbon()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		AddRibbonPageGroupJournals();
		AddRibbonPageGroupSubDirectories();
		_barButtonPartnerChangeRequest = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.SendDataTob2b,
			Glyph = ImageService.GetImage("Import_16.svg"),
			LargeGlyph = ImageService.GetImage("Import_32.svg")
		};
		_barButtonPartnerChangeRequest.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPartnerChangeRequest_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonPartnerChangeRequest);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPartnerChangeRequest.CreateLink());
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.Partners, showDialog: false, ArmSoft.SME.Common.Resources.JumpToDirectory, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barSubItemReports.Content = "";
		_barSubItemReports.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemReports.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("journal_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("journal_32.svg");
		_barButtonSalesAnalysis = new ReportBarButtonItem(DataProviderTypeSE.MatRepSalesAnalysis.Value, showDialog: true, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
		_barButtonSalesAnalysis.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemReports);
		_barButtonPartnerCard = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.PartnerCard,
			Glyph = ImageService.GetImage("report_16.svg"),
			LargeGlyph = ImageService.GetImage("report_32.svg")
		};
		_barButtonPartnerCard.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPartnerCard_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonPartnerCard);
		_ribbonPageGroupReports.ItemLinks.Add(_barButtonPartnerCard.CreateLink());
		BarItemLink barItemLink = _barButtonPartnerCard.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barSubItemReports.ItemLinks.Add(barItemLink);
		_barButtonPartnerRemains = new ReportBarButtonItem(DataProviderTypeSE.PartRemains, showDialog: true, ArmSoft.SME.Trade.Common.Resources.PartnerRemainsShort);
		_barButtonPartnerRemains.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemReports);
		_barButtonPartnerTurn = new ReportBarButtonItem(DataProviderTypeSE.PartnersTurnOver, showDialog: true, ArmSoft.SME.Trade.Common.Resources.PartnerTurnShort);
		_barButtonPartnerTurn.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemReports);
		_barButtonPartnerOpJournal = new ReportBarButtonItem(DataProviderTypeSE.PartOpJournal, showDialog: true, ArmSoft.SME.Trade.Common.Resources.PartnerOpJournalShort);
		_barButtonPartnerOpJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemReports);
		_barButtonDocumentsJournal = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: true, ArmSoft.SME.Common.Resources.Documents, null, "journal_16.svg", "journal_32.svg");
		_barButtonDocumentsJournal.AddInBarSubItem(ref _ribbonPageGroupJournals, ref _barSubItemJournals);
		_barButtonPartAccInBank = new ReportBarButtonItem(DataProviderTypeSE.PartnerAccountsInBank, showDialog: false, ArmSoft.SME.Common.Resources.SettlementAccounts, null, "table_16.svg", "table_32.svg");
		_barButtonPartAccInBank.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonPartContracts = new ReportBarButtonItem(DataProviderTypeSE.PartContracts, showDialog: false, ArmSoft.SME.Common.Resources.Contracts);
		_barButtonPartContracts.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonPartnersMT = new ReportBarButtonItem(DataProviderTypeSE.PartnersMT, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnersMTDirectory);
		_barButtonPartnersMT.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 2;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		span[1] = new PropertyWithValue("ShowExtendedDialog", false.ToString());
		barButtonJumpTo.PropertyWithValues = list;
		ReportBarButtonItem barButtonSalesAnalysis = _barButtonSalesAnalysis;
		num = 3;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		span2[1] = new PropertyWithValue("StartDate", Settings.HostingEnvironment.DateBegin.ToString());
		span2[2] = new PropertyWithValue("EndDate", Settings.HostingEnvironment.DateEnd.ToString());
		barButtonSalesAnalysis.PropertyWithValues = list2;
		ReportBarButtonItem barButtonPartnerRemains = _barButtonPartnerRemains;
		num = 1;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		CollectionsMarshal.AsSpan(list3)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonPartnerRemains.PropertyWithValues = list3;
		ReportBarButtonItem barButtonPartnerTurn = _barButtonPartnerTurn;
		num = 1;
		List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list4, num);
		CollectionsMarshal.AsSpan(list4)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonPartnerTurn.PropertyWithValues = list4;
		ReportBarButtonItem barButtonPartnerOpJournal = _barButtonPartnerOpJournal;
		num = 1;
		List<PropertyWithValue> list5 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list5, num);
		CollectionsMarshal.AsSpan(list5)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonPartnerOpJournal.PropertyWithValues = list5;
		ReportBarButtonItem barButtonDocumentsJournal = _barButtonDocumentsJournal;
		num = 1;
		List<PropertyWithValue> list6 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list6, num);
		CollectionsMarshal.AsSpan(list6)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonDocumentsJournal.PropertyWithValues = list6;
		ReportBarButtonItem barButtonPartAccInBank = _barButtonPartAccInBank;
		num = 1;
		List<PropertyWithValue> list7 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list7, num);
		CollectionsMarshal.AsSpan(list7)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonPartAccInBank.PropertyWithValues = list7;
		ReportBarButtonItem barButtonPartContracts = _barButtonPartContracts;
		num = 1;
		List<PropertyWithValue> list8 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list8, num);
		CollectionsMarshal.AsSpan(list8)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonPartContracts.PropertyWithValues = list8;
		ReportBarButtonItem barButtonPartnersMT = _barButtonPartnersMT;
		num = 1;
		List<PropertyWithValue> list9 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list9, num);
		CollectionsMarshal.AsSpan(list9)[0] = new PropertyWithValue("PartCode", PO.Code.ToString());
		barButtonPartnersMT.PropertyWithValues = list9;
	}

	protected void AddRibbonPageGroupSubDirectories()
	{
		if (_ribbonPageOther == null)
		{
			AddOtherRibbonPage();
		}
		if (_ribbonPageGroupSubDirectories == null)
		{
			_ribbonPageGroupSubDirectories = new RibbonPageGroup
			{
				AllowCollapse = false
			};
			_ribbonPageOther.Groups.Add(_ribbonPageGroupSubDirectories);
			_ribbonPageGroupSubDirectories.Caption = ArmSoft.SME.Trade.Common.Resources.SubDirectories;
		}
	}

	private void BarButtonPartnerCard_ItemClick(object sender, ItemClickEventArgs e)
	{
		PartnerTAccountReportViewer partnerTAccountReportViewer = new PartnerTAccountReportViewer();
		partnerTAccountReportViewer.PartnerCode = _code.Text;
		partnerTAccountReportViewer.Show(useParamValues: true, openAsChild: false);
	}

	protected void AddRibbonPageGroupJournals()
	{
		if (_ribbonPageOther == null)
		{
			AddOtherRibbonPage();
		}
		if (_ribbonPageGroupJournals == null)
		{
			_ribbonPageGroupJournals = new RibbonPageGroup
			{
				AllowCollapse = false
			};
			_ribbonPageOther.Groups.Add(_ribbonPageGroupJournals);
			_ribbonPageGroupJournals.Caption = ArmSoft.SME.Common.Resources.Journals;
		}
	}

	private void BarButtonPartnerChangeRequest_ItemClick(object sender, ItemClickEventArgs e)
	{
		FillDocument(new StoreRequest());
		if (string.IsNullOrWhiteSpace(PO.TaxCode))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.OptionsTaxCode));
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (dvp.IdvName() == "Partners")
		{
			dvp.Update(PO.Id);
		}
		else
		{
			dvp.Refresh();
		}
	}
}
