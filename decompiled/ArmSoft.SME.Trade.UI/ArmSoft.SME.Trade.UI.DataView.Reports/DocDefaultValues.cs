using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Parameter;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

public class DocDefaultValues : DialogWindow
{
	private DropDownControl _storage;

	private DropDownControl _supplier;

	private DropDownControl _customer;

	private DropDownControl _buyingPriceType;

	private DropDownControl _salePriceType;

	private DropDownControl _discountPriceType;

	private DropDownControl _cash;

	private DropDownControl _eMarkReceiptDevice;

	private DropDownControl _curr;

	private DropDownControl _salesConsultant;

	private DropDownControl _scannerReadMode;

	private DropDownControl _userAWP;

	private CheckEditExt _showOnlyUserAWP;

	private CheckEditExt _mtBillAllowDifferentStorages;

	private CheckEditExt _showInfoPanel;

	private CheckEditExt _showItemPictureOnInfoPanel;

	private CheckEditExt _showStorageAvailabilityOnInfoPanel;

	private CheckEditExt _showWholeAvailabilityOnInfoPanel;

	private CheckEditExt _showDescriptionOnInfoPanel;

	private CheckEditExt _mtBillGridAutoWidth;

	private CheckEditExt _showPoleDisplay;

	private CheckEditExt _mtBillSimpleMode;

	private CheckEditExt _useCurrentDateInDocs;

	private DateEditControl _dateBegin;

	private DateEditControl _dateEnd;

	private CheckEditExt _ecrCheckPrintDiscount;

	private DropDownControl _ecrDiscountType;

	private CheckEditExt _ecrCheckOnCheckPrinter;

	private DropDownControl _mtCheckPriceType;

	private DropDownControl _mtCheckDiscountPriceType;

	private CheckEditExt _allowChangeMTBillGoodRow;

	private CheckEditExt _mtBillRecordAllEntries;

	private CheckEditExt _openMarkingWindowImmediatly;

	private CheckEditExt _checkMarkingAndBarCode;

	private CheckEditExt _allowToAddNewBuyerWithNotRegisteredBuyerCard;

	private DropDownControl _mtBillRoundDegree;

	private CheckEditExt _mtBillSummRoundUp;

	private CheckEditExt _showASInfoInMTBill;

	private CheckEditExt _printECRCheckAndNew;

	private CheckEditExt _printMTCheckAndNew;

	private CheckEditExt _ecrCheckPrintCreditAsCash;

	private CheckEditExt _ecrCheckPrintCreditAsPartial;

	private DropDownControl _ecrCheckPrintBonusAsCash;

	private CheckEditExt _printPrepaymentCheckSeparately;

	private CheckEditExt _allowChangeECRCheckNum;

	private DropDownControl _mtCheckTemplate;

	private DropDownControl _mtRetCheckTemplate;

	private DropDownControl _asECRCheckItemCode;

	private DropDownControl _asECRCheckItemName;

	private DropDownControl _transitCash;

	private Button _setMTBillDefaultHotKeys;

	private DocumentGridControl _userRightsGrid;

	private DocumentGridControl _userPOSRightsGrid;

	private DocumentGridColumn _paramNameCol;

	private DocumentGridColumn _paramValueCol;

	private TableView _userRightsTableView;

	private TableView _userPOSRightsTableView;

	private ObservableCollection<UserRightsRow> _userRightsRows;

	private ObservableCollection<UserRightsRow> _userPOSRightsRows;

	private DocumentGridControl _mtBillFieldsGrid;

	private DocumentGridControl _mtBillColsGrid;

	private DocumentGridControl _mtBillOperAdjustmentsGrid;

	private TableView _mtBillFieldTableView;

	private TableView _mtBillColsTableView;

	private TableView _mtBillOperAdjustmentsTableView;

	private ObservableCollection<MTFillRowFilds> _mtBillFieldsRows;

	private ObservableCollection<UserRightsRow> _mtBillColsRows;

	private ObservableCollection<MTBillOperAdjustmentsRow> _mtBillOperAdjustmentsRows;

	private DocumentGridColumn _operationCol;

	private DocumentGridColumn _hotKeyCol;

	private DocumentGridColumn _showButtonCol;

	private DocumentGridColumn _rowCol;

	private DocumentGridColumn _showCol;

	private DocumentGridColumn _editCol;

	private readonly string _userName;

	private readonly int _userId;

	private readonly bool _isAdmin;

	private readonly Dictionary<short, string> _paramsBySelectedUser;

	private readonly List<ParameterStoreRequestModel> _editedParameters = new List<ParameterStoreRequestModel>();

	public DocDefaultValues(string userName, int userId, bool isAdmin)
	{
		_userName = userName;
		_userId = userId;
		_isAdmin = isAdmin;
		_paramsBySelectedUser = Settings.ParametersService.GetValuesFromService(_userId);
		Caption = string.Format(ArmSoft.SME.Common.Resources.UserAdditionalOptions, userName);
		HelpId = "AdminTools_Users";
		Height = 670.0;
		Width = 1000.0;
		ResizeMode = ResizeMode.CanResize;
		SizeToContent = SizeToContent.Manual;
		ConstructUI();
	}

	private void ConstructUI()
	{
		//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1701: Unknown result type (might be due to invalid IL or missing references)
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
		LayoutGroupExt layoutGroupExt = AddVerticalLayoutGroup(container, LayoutGroupView.GroupBox, ArmSoft.SME.Common.Resources.DefaultValues);
		layoutGroupExt.Orientation = Orientation.Vertical;
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt.IsCollapsible = true;
		layoutGroupExt.IsCollapsed = false;
		Name = "UsersOptions";
		_storage = AddDropDownControl(layoutGroupExt, "Storage", ArmSoft.SME.Common.Resources.Storage, new ArmSoft.SME.Trade.UI.DropDown.Storages());
		_storage.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultStorage.Id];
		_supplier = AddDropDownControl(layoutGroupExt, "Supplier", ArmSoft.SME.Common.Resources.Supplier, new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: true), isRequired: false, allowAdditionButtons: true);
		_supplier.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultSupplier.Id];
		_customer = AddDropDownControl(layoutGroupExt, "Customer", ArmSoft.SME.Common.Resources.Buyer, new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: false, isFilterByCustomers: true), isRequired: false, allowAdditionButtons: true);
		_customer.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultCustomer.Id];
		_buyingPriceType = AddDropDownControl(layoutGroupExt, "BuyingPriceType", ArmSoft.SME.Trade.Common.Resources.BuyingPriceType, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes());
		_buyingPriceType.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultBuyingPrice.Id];
		_salePriceType = AddDropDownControl(layoutGroupExt, "SalePriceType", ArmSoft.SME.Trade.Common.Resources.SalePriceType, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes());
		_salePriceType.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultSalePrice.Id];
		_discountPriceType = AddDropDownControl(layoutGroupExt, "DiscountPriceType", ArmSoft.SME.Trade.Common.Resources.DiscountPriceType, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes());
		_discountPriceType.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultDiscountPrice.Id];
		_cash = AddDropDownControl(layoutGroupExt, "Cash", ArmSoft.SME.Common.Resources.Cash, new ArmSoft.SME.Trade.UI.DropDown.Cashes(withoutCheckAccess: true));
		_cash.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultCash.Id];
		if (Settings.ParametersService.UseMarking())
		{
			_eMarkReceiptDevice = AddDropDownControl(layoutGroupExt, "EMarkReceiptDevice", ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDevice, new EMarkReceipApp());
			_eMarkReceiptDevice.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.EMarkReceiptDevice.Id];
		}
		_curr = AddDropDownControl(layoutGroupExt, "Curr", ArmSoft.SME.Common.Resources.Curr, new Currency(shortMode: false));
		_curr.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultMTBillCurr.Id];
		_salesConsultant = AddDropDownControl(layoutGroupExt, "SalesConsultant", ArmSoft.SME.Trade.Common.Resources.SalesConsultant, new ArmSoft.SME.Trade.UI.DropDown.Employees(), isRequired: false, allowAdditionButtons: true);
		_salesConsultant.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultSalesConsultant.Id];
		_userAWP = AddDropDownControl(layoutGroupExt, "UserAWP", ArmSoft.SME.Common.Resources.UserAWP, new AWPDefinitions());
		_userAWP.Code = _paramsBySelectedUser[ArmSoft.SME.Common.Parameters.Definitions.UserAWPNavigatorName.Id];
		_showOnlyUserAWP = AddBooleanControl(layoutGroupExt, "ShowOnlyUserAWP", ArmSoft.SME.Trade.Common.Resources.ShowOnlyUserAWP);
		_showOnlyUserAWP.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowOnlyUserAWP.Id].ToBoolean();
		_scannerReadMode = AddDropDownControl(container, "ScannerMode", ArmSoft.SME.Trade.Common.Resources.ScannerReadMode, new SmartEnum(typeof(ScanerModesSE)), isRequired: true);
		_scannerReadMode.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ScannerMode.Id];
		LayoutGroup container2 = AddTabedGroup(tc, ArmSoft.SME.Trade.Common.Resources.DocConstraints);
		_useCurrentDateInDocs = AddBooleanControl(container2, "UseCurrDateInDocs", ArmSoft.SME.Trade.Common.Resources.UseCurrentDate);
		LayoutGroupExt container3 = AddHorizontalLayoutGroup(container2, LayoutGroupView.Group, ArmSoft.SME.Trade.Common.Resources.DocConstraints);
		_dateBegin = AddDateControl(container3, "DateBegin", ArmSoft.SME.Common.Resources.OptionWorkPeriod);
		if (!string.IsNullOrWhiteSpace(_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id]))
		{
			_dateBegin.EditValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id].YYYYMMDDToDateOnly();
		}
		_dateEnd = AddDateControl(container3, "DateEnd", "");
		if (!string.IsNullOrWhiteSpace(_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id]))
		{
			_dateEnd.EditValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id].YYYYMMDDToDateOnly();
		}
		AddSpecialDateButton(container3, _dateBegin, _dateEnd);
		_useCurrentDateInDocs.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseCurrentDateInDocs.Id].ToBoolean();
		_useCurrentDateInDocs.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(UseCurrentDateInDocs_EditValueChanged).Handler;
		UseCurrentDateInDocs_EditValueChangedForLoad();
		_userRightsGrid = AddGridControl(container2, "GridUserRights");
		_userRightsGrid.AllowAdd = false;
		_userRightsGrid.AllowDelete = false;
		_userRightsGrid.AllowDuplicate = false;
		_userRightsGrid.ShowButtons = false;
		_userRightsRows = new ObservableCollection<UserRightsRow>
		{
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.DocAllowchangePrices,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeSalePrices.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowChangeDiscountPrice,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDiscountPrice.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowChangePriceType,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangePriceType.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowChangeDiscountPriceType,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDiscountPriceType.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Common.Resources.AutoCalcDiscount,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.CalcDiscountAuto.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AlwaysCalcDiscountAuto,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AlwaysCalcDiscountAuto.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.ShowMTInclInPriceList,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowMTInclInPrList.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.ShowGoodsCostInReportsAndDocs,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowGoodsCostInReportsAndDocs.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowExceedDebtLimit,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowExceedDebtLimit.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Common.Resources.AllowEditCodeOfDirectories,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Common.Parameters.Definitions.AllowEditCodeOfDirectories.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowUse100PercentSale,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowUse100PercentSale.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowPurchaseWith0PriceOr100Percent,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPurchaseWith0PriceOr100Percent.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AddMaterialToPartnersMT,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AddMaterialToPartnersMT.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowGroupDelete,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowGroupDelete.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Common.Resources.AllowOpFromHistoryPanel,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Common.Parameters.Definitions.AllowOpFromHistoryPanel.Id].ToBoolean()
			}
		};
		if (!_isAdmin)
		{
			_userRightsRows.Add(new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.DenyAccessToOtherOwnerDocs,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DenyAccessToOtherOwnerDocs.Id].ToBoolean()
			});
		}
		_userRightsGrid.ItemsSource = _userRightsRows;
		_userRightsTableView = _userRightsGrid.View;
		_userRightsTableView.AllowMoveColumnToDropArea = false;
		_userRightsTableView.IsColumnMenuEnabled = false;
		_userRightsTableView.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(UserRightsTableView_CellValueChanging).Handler;
		_paramNameCol = _userRightsGrid.AddStringColumn("ParamName", "", 250);
		_paramNameCol.Width = 600.0;
		_paramNameCol.AllowEditing = DefaultBoolean.False;
		_paramValueCol = _userRightsGrid.AddBooleanColumn("ParamValue", "");
		LayoutGroup container4 = AddTabedGroup(tc, ArmSoft.SME.Trade.Common.Resources.DocMTBill);
		Label label = new Label();
		TextBlock content = new TextBlock
		{
			Text = ArmSoft.SME.Trade.Common.Resources.MTBillPageInfo,
			Foreground = ColorsAS.Foreground.BlueBrush
		};
		label.Content = content;
		ImageEdit myControl = new ImageEdit
		{
			Source = ImageService.GetImage("information_16.svg"),
			IsEnabled = false,
			ShowBorder = false,
			Stretch = Stretch.Fill,
			Margin = new Thickness(10.0, 3.0, 0.0, 0.0)
		};
		LayoutGroupExt container5 = AddHorizontalLayoutGroup(container4);
		AddControl(container5, "MTBillPageImage", "", myControl);
		AddControl(container5, "MTBillInfoPanel", "", label);
		_mtBillSimpleMode = AddBooleanControl(container4, "MTBillSimpleMode", ArmSoft.SME.Trade.Common.Resources.MTBillSimpleMode);
		_mtBillSimpleMode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTBillSimpleMode_EditValueChanged).Handler;
		Label label2 = new Label();
		TextBlock content2 = new TextBlock
		{
			Text = ArmSoft.SME.Trade.Common.Resources.SimpleModeSettingsInfo,
			TextWrapping = TextWrapping.Wrap,
			Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#f07174" : "#e40000"),
			FontSize = 13.0
		};
		label2.Content = content2;
		label2.Width = SizeCalculator.Height(690.0);
		ImageEdit myControl2 = new ImageEdit
		{
			Source = ImageService.GetImage("warning_circle_16.svg"),
			IsEnabled = false,
			ShowBorder = false,
			Stretch = Stretch.Fill,
			Margin = new Thickness(10.0, 4.0, 0.0, 0.0)
		};
		LayoutGroupExt container6 = AddHorizontalLayoutGroup(container4);
		AddControl(container6, "MTBillSimpleModeImage", "", myControl2);
		AddControl(container6, "MTBillSimpleModeMsg", "", label2);
		_mtBillAllowDifferentStorages = AddBooleanControl(container4, "MTBillAllowDifferentStorages", ArmSoft.SME.Trade.Common.Resources.MTBillAllowDifferentStorages);
		_mtBillAllowDifferentStorages.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillAllowDifferentStorages.Id].ToBoolean();
		_mtBillAllowDifferentStorages.IsReadOnly = !_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeStorageInBill.Id].ToBoolean();
		_showPoleDisplay = AddBooleanControl(container4, "ShowPoleDisplay", ArmSoft.SME.Trade.Common.Resources.PoleDisplay);
		_showPoleDisplay.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPoleDisplay.Id].ToBoolean();
		_mtCheckPriceType = AddDropDownControl(container4, "MTCheckPriceType", ArmSoft.SME.Trade.Common.Resources.PriceType, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes());
		_mtCheckPriceType.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultMTCheckPriceType.Id];
		_mtCheckDiscountPriceType = AddDropDownControl(container4, "MTCheckDiscountPriceType", ArmSoft.SME.Trade.Common.Resources.DiscountPriceType, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes());
		_mtCheckDiscountPriceType.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultMTCheckDiscountPriceType.Id];
		_allowChangeMTBillGoodRow = AddBooleanControl(container4, "AllowChangeMTBillGoodRow", ArmSoft.SME.Trade.Common.Resources.AllowChangeGoodRow);
		_allowChangeMTBillGoodRow.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeMTBillGoodRow.Id].ToBoolean();
		_mtBillRecordAllEntries = AddBooleanControl(container4, "MTBillRecordAllEntries", ArmSoft.SME.Trade.Common.Resources.MTBillRecordAllEntries);
		_mtBillRecordAllEntries.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillRecordAllEntries.Id].ToBoolean();
		_allowToAddNewBuyerWithNotRegisteredBuyerCard = AddBooleanControl(container4, "AllowToAddNewBuyerWithNotRegisteredBuyerCard", ArmSoft.SME.Trade.Common.Resources.AllowToAddNewBuyerWithNotRegisteredBuyerCard);
		_allowToAddNewBuyerWithNotRegisteredBuyerCard.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowToAddNewBuyerWithNotRegisteredBuyerCard.Id].ToBoolean();
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseMarking.Id].ToBoolean())
		{
			_openMarkingWindowImmediatly = AddBooleanControl(container4, "OpenMarkingDocImmediatly", ArmSoft.SME.Common.Resources.OpenMarkingWindow);
			_openMarkingWindowImmediatly.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.OpenMarkingWindowImmediatly.Id].ToBoolean();
			_checkMarkingAndBarCode = AddBooleanControl(container4, "CheckMarkingAndBarCode", ArmSoft.SME.Trade.Common.Resources.CheckMarkingAndBarCode);
			_checkMarkingAndBarCode.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.CheckMarkingAndBarCode.Id].ToBoolean();
		}
		_mtBillRoundDegree = AddDropDownControl(container4, "MTBillRoundDegree", ArmSoft.SME.Trade.Common.Resources.DocRoundDegree, new SmartEnum(typeof(RoundDegreesSE)));
		_mtBillRoundDegree.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillRoundDegree.Id];
		_mtBillSummRoundUp = AddBooleanControl(container4, "MTBillSummRoundUp", ArmSoft.SME.Trade.Common.Resources.AlwaysRoundUp);
		_mtBillSummRoundUp.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSummRoundUp.Id].ToBoolean();
		LayoutGroupExt layoutGroupExt2 = AddVerticalLayoutGroup(container4, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.MTCheckPrintOptions);
		layoutGroupExt2.Orientation = Orientation.Vertical;
		layoutGroupExt2.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt2.IsCollapsible = true;
		layoutGroupExt2.IsCollapsed = true;
		_printMTCheckAndNew = AddBooleanControl(layoutGroupExt2, "PrintCheckAndNew", ArmSoft.SME.Common.Resources.PrintECRCheckAndNew);
		_printMTCheckAndNew.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintMTCheckAndNew.Id].ToBoolean();
		_mtCheckTemplate = AddDropDownControl(layoutGroupExt2, "CheckTemplate", ArmSoft.SME.Common.Resources.PrintTemplate + " " + ArmSoft.SME.Trade.Common.Resources.DocMTBill, new Template(short.Parse(ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTBill)));
		_mtCheckTemplate.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTCheckTemplate.Id];
		_mtRetCheckTemplate = AddDropDownControl(layoutGroupExt2, "RetCheckTemplate", ArmSoft.SME.Common.Resources.PrintTemplate + " " + ArmSoft.SME.Trade.Common.Resources.DocMTBillReturn, new Template(short.Parse(ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTBillReturn)));
		_mtRetCheckTemplate.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.RetCheckTemplate.Id];
		LayoutGroupExt layoutGroupExt3 = AddVerticalLayoutGroup(container4, LayoutGroupView.GroupBox, ArmSoft.SME.Common.Resources.ECRCheckOptions);
		layoutGroupExt3.Orientation = Orientation.Vertical;
		layoutGroupExt3.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt3.IsCollapsible = true;
		layoutGroupExt3.IsCollapsed = true;
		_ecrCheckOnCheckPrinter = AddBooleanControl(layoutGroupExt3, "ECRCheckOnCheckPrinter", ArmSoft.SME.Trade.Common.Resources.PrintOnCheckPrinter);
		_ecrCheckOnCheckPrinter.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintECRCheckOnCheckPrinter.Id].ToBoolean();
		_printECRCheckAndNew = AddBooleanControl(layoutGroupExt3, "PrintECRCheckAndNew", ArmSoft.SME.Common.Resources.PrintECRCheckAndNew);
		_printECRCheckAndNew.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintECRCheckAndNew.Id].ToBoolean();
		_ecrCheckPrintCreditAsCash = AddBooleanControl(layoutGroupExt3, "CheckPrintCreditAsCash", ArmSoft.SME.Trade.Common.Resources.PrintCreditAsCash);
		_ecrCheckPrintCreditAsCash.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRCheckPrintCreditAsCash.Id].ToBoolean();
		_ecrCheckPrintCreditAsCash.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ECRCheckPrintCreditAsCash_EditValueChanged).Handler;
		_ecrCheckPrintCreditAsPartial = AddBooleanControl(layoutGroupExt3, "CheckPrintCreditAsPartial", ArmSoft.SME.Trade.Common.Resources.PrintCreditAsCompensation);
		_ecrCheckPrintCreditAsPartial.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRCheckPrintCreditAsPartial.Id].ToBoolean();
		_ecrCheckPrintCreditAsPartial.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ECRCheckPrintCreditAsPartial_EditValueChanged).Handler;
		_ecrCheckPrintBonusAsCash = AddDropDownControl(layoutGroupExt3, "CheckPrintBonusAsCash", ArmSoft.SME.Trade.Common.Resources.PrintBonusAs, new SmartEnum(typeof(PrintBonusTypesSE)));
		_ecrCheckPrintBonusAsCash.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ECRCheckPrintBonusAsCash_EditValueChanged).Handler;
		_printPrepaymentCheckSeparately = AddBooleanControl(layoutGroupExt3, "PrintPrepaymentCheckSeparately", ArmSoft.SME.Trade.Common.Resources.PrintPrepaymentCheckSeparately);
		_ecrCheckPrintBonusAsCash.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRCheckPrintBonusType.Id];
		_printPrepaymentCheckSeparately.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintPrepaymentCheckSeparately.Id].ToBoolean();
		_printPrepaymentCheckSeparately.IsReadOnly = _ecrCheckPrintBonusAsCash.Code != PrintBonusTypesSE.DiscountAndPrePayment.Value;
		_ecrCheckPrintDiscount = AddBooleanControl(layoutGroupExt3, "ECRCheckPrintDiscount", ArmSoft.SME.Trade.Common.Resources.PrintDiscount);
		_ecrCheckPrintDiscount.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintECRCheckDiscount.Id].ToBoolean();
		_ecrDiscountType = AddDropDownControl(layoutGroupExt3, "ECRDiscountType", ArmSoft.SME.Trade.Common.Resources.DiscountType, new SmartEnum(typeof(ECRRowDiscountTypesSE)), isRequired: true);
		_ecrDiscountType.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRDiscountType.Id];
		_asECRCheckItemCode = AddDropDownControl(layoutGroupExt3, "ASECRCheckItemCode", ArmSoft.SME.Common.Resources.ASECRCheckItemCode, new SmartEnum(typeof(ECRCheckItemCodeSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.Code, CaptionMember.Caption, null, ArmSoft.SME.Common.Resources.Description));
		_asECRCheckItemCode.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ASECRCheckItemCode.Id];
		_asECRCheckItemCode.IsRequired = true;
		_asECRCheckItemName = AddDropDownControl(layoutGroupExt3, "ASECRCheckItemName", ArmSoft.SME.Common.Resources.ASECRCheckItemName, new SmartEnum(typeof(ECRCheckItemNameSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.Code, CaptionMember.Caption, null, ArmSoft.SME.Common.Resources.Description));
		_asECRCheckItemName.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ASECRCheckItemName.Id];
		_asECRCheckItemName.IsRequired = true;
		Label label3 = new Label();
		TextBlock content3 = new TextBlock
		{
			Text = ArmSoft.SME.Trade.Common.Resources.ECRCodeNameWarning,
			TextWrapping = TextWrapping.Wrap,
			Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#f07174" : "#e40000"),
			FontSize = 13.0
		};
		label3.Content = content3;
		label3.Width = SizeCalculator.Height(690.0);
		ImageEdit myControl3 = new ImageEdit
		{
			Source = ImageService.GetImage("warning_circle_16.svg"),
			IsEnabled = false,
			ShowBorder = false,
			Stretch = Stretch.Fill,
			Margin = new Thickness(10.0, 4.0, 0.0, 0.0)
		};
		LayoutGroupExt container7 = AddHorizontalLayoutGroup(layoutGroupExt3);
		AddControl(container7, "MTBillPageEcrInfoImage", "", myControl3);
		AddControl(container7, "MTBillECRInfoPanel", "", label3);
		_allowChangeECRCheckNum = AddBooleanControl(layoutGroupExt3, "ChangeECRCheckNum", ArmSoft.SME.Common.Resources.AllowChangeECRCheckNum);
		_allowChangeECRCheckNum.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeECRCheckNum.Id].ToBoolean();
		LayoutGroupExt layoutGroupExt4 = AddVerticalLayoutGroup(container4, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.MTBillFieldsOptions);
		layoutGroupExt4.Orientation = Orientation.Horizontal;
		layoutGroupExt4.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt4.IsCollapsible = true;
		layoutGroupExt4.IsCollapsed = true;
		_mtBillFieldsGrid = AddGridControl(layoutGroupExt4, "GridMTBillFields");
		_mtBillFieldsGrid.AllowAdd = false;
		_mtBillFieldsGrid.AllowDelete = false;
		_mtBillFieldsGrid.AllowDuplicate = false;
		_mtBillFieldsGrid.ShowButtons = false;
		_mtBillFieldsGrid.Height = 140.0;
		_mtBillFieldsRows = new ObservableCollection<MTFillRowFilds>
		{
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Common.Resources.DateWord,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDateInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDateInBill.Id].ToBoolean()
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Common.Resources.DocNumber,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDocNumInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDocNumberInBill.Id].ToBoolean()
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Trade.Common.Resources.DiscountPercentSymbol,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDiscountInBill.Id].ToBoolean(),
				AllowEditBooleanCell = !_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DiscountUse.Id].ToBoolean()
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Common.Resources.Storage,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowStorageInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeStorageInBill.Id].ToBoolean(),
				AllowEditBooleanCell = true
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Common.Resources.Cash,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCashInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeCashInBill.Id].ToBoolean(),
				AllowEditBooleanCell = true
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Trade.Common.Resources.CashierShiftNum,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCashierShiftNumInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeCashierShiftNumInMTBill.Id].ToBoolean(),
				AllowEditBooleanCell = true,
				AllowEditBooleanCellEdit = false
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Common.Resources.Buyer,
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeBuyerInBill.Id].ToBoolean()
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Common.Resources.PrintExpenseJustifyingCheck,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPrintExpJustifyingCheckInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangePrintExpJustifyingCheckInMTBill.Id].ToBoolean(),
				AllowEditBooleanCell = true
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Trade.Common.Resources.SalesConsultant,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowSalesConsultantInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeSaleConsultantInBill.Id].ToBoolean(),
				AllowEditBooleanCell = true
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Common.Resources.Comment,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCommentInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeCommentInMTBill.Id].ToBoolean(),
				AllowEditBooleanCell = true
			},
			new MTFillRowFilds
			{
				Row = ArmSoft.SME.Common.Resources.DocOwner,
				Show = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowOwnerInMTBill.Id].ToBoolean(),
				Edit = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeOwnerInMTBill.Id].ToBoolean(),
				AllowEditBooleanCell = true
			}
		};
		_mtBillFieldsGrid.ItemsSource = _mtBillFieldsRows;
		_mtBillFieldTableView = _mtBillFieldsGrid.View;
		_mtBillFieldTableView.AllowMoveColumnToDropArea = false;
		_mtBillFieldTableView.IsColumnMenuEnabled = false;
		_mtBillFieldTableView.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(MTBillFieldTableView_CellValueChanged).Handler;
		_mtBillFieldTableView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(MTBillFieldTableView_CellValueChanging).Handler;
		_rowCol = _mtBillFieldsGrid.AddStringColumn("Row", ArmSoft.SME.Common.Resources.Name, 250);
		_rowCol.AllowEditing = DefaultBoolean.False;
		_showCol = _mtBillFieldsGrid.AddBooleanColumn("Show", ArmSoft.SME.Trade.Common.Resources.Showing);
		_editCol = _mtBillFieldsGrid.AddBooleanColumn("Edit", ArmSoft.SME.Trade.Common.Resources.MTBillFields);
		_editCol.AllowEditing = DefaultBoolean.True;
		_showCol.CellTemplate = (DataTemplate)Application.Current.FindResource("ReadOnlyBooleanCellTemplate");
		_editCol.CellTemplate = (DataTemplate)Application.Current.FindResource("ReadOnlyBooleanCellTemplate2");
		LayoutGroupExt layoutGroupExt5 = AddVerticalLayoutGroup(container4, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.MTBillColsOptions);
		layoutGroupExt5.Orientation = Orientation.Vertical;
		layoutGroupExt5.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt5.IsCollapsible = true;
		layoutGroupExt5.IsCollapsed = true;
		_mtBillGridAutoWidth = AddBooleanControl(layoutGroupExt5, "MTBillGridAutoWidth", ArmSoft.SME.Trade.Common.Resources.GridColumnAutoWidth);
		_mtBillGridAutoWidth.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillGridAutoWidth.Id].ToBoolean();
		_mtBillColsGrid = AddGridControl(layoutGroupExt5, "GridMTBillCols");
		_mtBillColsGrid.AllowAdd = false;
		_mtBillColsGrid.AllowDelete = false;
		_mtBillColsGrid.AllowDuplicate = false;
		_mtBillColsGrid.ShowButtons = false;
		_mtBillColsGrid.Height = 170.0;
		_mtBillColsRows = new ObservableCollection<UserRightsRow>
		{
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Common.Resources.Unit,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowUnitCodeColInBill.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Common.Resources.DiscountPercent,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountPercentColInBill.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Common.Resources.DiscountedPrice,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountedPriceColInBill.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.DiscountSumm,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountSummColInBill.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Common.Resources.OptionsVAT,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowVATColInBill.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AccumulateBonus,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowAccumulatedBonusColInBill.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.BonusUse,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowUsedBonusColInBill.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Common.Resources.Party,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyColInBill.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Common.Resources.Description,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowRowDescrColInBill.Id].ToBoolean()
			}
		};
		_mtBillColsGrid.ItemsSource = _mtBillColsRows;
		_mtBillColsTableView = _mtBillColsGrid.View;
		_mtBillColsTableView.AllowMoveColumnToDropArea = false;
		_mtBillColsTableView.IsColumnMenuEnabled = false;
		_mtBillColsTableView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(MTBillColsTableView_CellValueChanging).Handler;
		_paramNameCol = _mtBillColsGrid.AddStringColumn("ParamName", "", 250);
		_paramNameCol.AllowEditing = DefaultBoolean.False;
		_paramValueCol = _mtBillColsGrid.AddBooleanColumn("ParamValue", "");
		_mtBillColsGrid.Grid.Columns["ParamName"].Header = ArmSoft.SME.Common.Resources.Name;
		_mtBillColsGrid.Grid.Columns["ParamValue"].Header = ArmSoft.SME.Trade.Common.Resources.MTBillVisibileCols;
		_mtBillSimpleMode.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSimpleMode.Id].ToBoolean();
		LayoutGroupExt layoutGroupExt6 = AddVerticalLayoutGroup(container4, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.Commands);
		layoutGroupExt6.Orientation = Orientation.Vertical;
		layoutGroupExt6.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt6.IsCollapsible = true;
		layoutGroupExt6.IsCollapsed = true;
		_setMTBillDefaultHotKeys = AddButton(layoutGroupExt6, "SetMTBillDefaultHotKeys", null, ArmSoft.SME.Common.Resources.DefaultValues);
		_setMTBillDefaultHotKeys.Click += new SafeEventHandler<RoutedEventArgs>(SetMTBillDefaultHotKeys_Click).Handler;
		((LayoutItemWithRequired)(object)_setMTBillDefaultHotKeys.Parent).Label = string.Empty;
		_mtBillOperAdjustmentsGrid = AddGridControl(layoutGroupExt6, "MTBillWindowHotKeys");
		_mtBillOperAdjustmentsGrid.AllowDelete = false;
		_mtBillOperAdjustmentsGrid.AllowDuplicate = false;
		_mtBillOperAdjustmentsGrid.ShowButtons = false;
		_mtBillOperAdjustmentsGrid.Height = 170.0;
		_mtBillOperAdjustmentsRows = new ObservableCollection<MTBillOperAdjustmentsRow>
		{
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.InputBoxCaption,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillInputValueHotKey.Id]
			},
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSummaCashHotKey.Id]
			},
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.cmdBarCodeEnter,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillBarCodeEnterHotKey.Id]
			},
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCodeEnterHotKey.Id]
			},
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.cmdNameEnter,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillMTListLikeNameHotKey.Id]
			},
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.cmdQtyEnter,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillQtyEnterHotKey.Id]
			},
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.SalePriceShort,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSalePriceHotKey.Id]
			},
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.UsePrePaymentShort,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillUsePrePaymentHotKey.Id],
				ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillUsePrePaymentShowButton.Id].ToBoolean(),
				AllowEditBooleanCell = true
			},
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.PlasticCartPayment,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPlCardPaymentHotKey.Id],
				ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPlCardPaymentShowButton.Id].ToBoolean(),
				AllowEditBooleanCell = true
			},
			new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.CreditSaleShort,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCreditHotKey.Id],
				ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCreditShowButton.Id].ToBoolean(),
				AllowEditBooleanCell = true
			}
		};
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus.Id].ToBoolean())
		{
			_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.BonusUseShort,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillUseBonusHotKey.Id]
			});
		}
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Common.Resources.ECRC,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintECRCheckHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintECRCheckShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Common.Resources.Print,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintBillHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintBillShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.Weight,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillWeightHotKey.Id]
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Common.Resources.Complect,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillComplectHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillComplectShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.GiveDiscount,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillDiscountHotKey.Id]
		});
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus.Id].ToBoolean())
		{
			_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.CalcBonusesShort,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCalcBonusHotKey.Id]
			});
		}
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Common.Resources.Prices,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPricesHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPricesShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.SetFocusOnGrid,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSetFocusOnGridHotKey.Id]
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.GroupAddition,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFilteredSearchHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFilteredSearchShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Common.Resources.RibbonPageFavorites,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFavoritesHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFavoritesShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.ErasePayments,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillErasePaymentsHotKey.Id]
		});
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus.Id].ToBoolean())
		{
			_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.AddToBonusShort,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillAddToBonusHotKey.Id]
			});
		}
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.Continue,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSContinueHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSContinueShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.Defer,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSDeferSaleHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSDeferSaleShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = $"{ArmSoft.SME.Trade.Common.Resources.Buyers} {ArmSoft.SME.Trade.Common.Resources.Card}",
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSPartnerCardHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSPartnerCardShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.ReturnWord,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSReturnHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillReturnBillShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ActualQuantitiesCheckHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ActualQuantitiesCheckShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.EnableSubstituteItems.Id].ToBoolean())
		{
			_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.SubstituteItems,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSubstituteItemsHotKey.Id],
				ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSubstituteItemsShowButton.Id].ToBoolean(),
				AllowEditBooleanCell = true
			});
		}
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Common.Resources.QRCode,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillQRHotKey.Id],
			ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillQRShowButton.Id].ToBoolean(),
			AllowEditBooleanCell = true
		});
		_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
		{
			Operation = ArmSoft.SME.Trade.Common.Resources.ChangeUnit,
			HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UnitCodeSwitchHotKey.Id],
			ShowButton = false
		});
		if (Settings.ParametersService.UseMarking())
		{
			_mtBillOperAdjustmentsRows.Add(new MTBillOperAdjustmentsRow
			{
				Operation = ArmSoft.SME.Trade.Common.Resources.EMarkReceiptButton,
				HotKey = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.EMarkReceiptHotKey.Id],
				ShowButton = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.EMarkReceiptShowButton.Id].ToBoolean(),
				AllowEditBooleanCell = true
			});
		}
		_mtBillOperAdjustmentsGrid.ItemsSource = _mtBillOperAdjustmentsRows;
		_mtBillOperAdjustmentsTableView = _mtBillOperAdjustmentsGrid.View;
		_mtBillOperAdjustmentsTableView.AllowMoveColumnToDropArea = false;
		_mtBillOperAdjustmentsTableView.IsColumnMenuEnabled = false;
		_operationCol = _mtBillOperAdjustmentsGrid.AddStringColumn("Operation", ArmSoft.SME.Trade.Common.Resources.Command, 50);
		_operationCol.AllowEditing = DefaultBoolean.False;
		_hotKeyCol = _mtBillOperAdjustmentsGrid.AddDropDownColumn("HotKey", ArmSoft.SME.Common.Resources.HotKey, new SmartEnum(typeof(HotKeysSE)));
		_hotKeyCol.Width = 100.0;
		_showButtonCol = _mtBillOperAdjustmentsGrid.AddBooleanColumn("ShowButton", ArmSoft.SME.Trade.Common.Resources.Showing);
		_showButtonCol.CellTemplate = (DataTemplate)Application.Current.FindResource("ReadOnlyBooleanCellTemplate");
		LayoutGroupExt layoutGroupExt7 = AddVerticalLayoutGroup(container4, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.MTBillInfoPanel);
		layoutGroupExt7.Orientation = Orientation.Vertical;
		layoutGroupExt7.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt7.IsCollapsible = true;
		layoutGroupExt7.IsCollapsed = true;
		_showInfoPanel = AddBooleanControl(layoutGroupExt7, "ShowInfoPanel", ArmSoft.SME.Trade.Common.Resources.ShowInfoPanel);
		_showInfoPanel.IsChecked = !Convert.ToBoolean(_showInfoPanel.IsChecked);
		_showItemPictureOnInfoPanel = AddBooleanControl(layoutGroupExt7, "ShowItemPictureOnInfoPanel", ArmSoft.SME.Trade.Common.Resources.ShowItemPictureOnInfoPanel);
		_showItemPictureOnInfoPanel.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowItemPictureOnInfoPanel.Id].ToBoolean();
		_showStorageAvailabilityOnInfoPanel = AddBooleanControl(layoutGroupExt7, "ShowStorageAvailaibilityOnInfoPanel", ArmSoft.SME.Trade.Common.Resources.ShowStorageAvailabilityOnInfoPanel);
		_showStorageAvailabilityOnInfoPanel.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowStorageAvailabilityOnInfoPanel.Id].ToBoolean();
		_showWholeAvailabilityOnInfoPanel = AddBooleanControl(layoutGroupExt7, "ShowWholeAvailabilityOnInfoPanel", ArmSoft.SME.Trade.Common.Resources.ShowWholeAvailabilityOnInfoPanel);
		_showWholeAvailabilityOnInfoPanel.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowWholeAvailabilityOnInfoPanel.Id].ToBoolean();
		_showDescriptionOnInfoPanel = AddBooleanControl(layoutGroupExt7, "ShowDescriptionOnInfoPanel", ArmSoft.SME.Trade.Common.Resources.ShowDescriptionOnInfoPanel);
		_showDescriptionOnInfoPanel.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDescriptionOnInfoPanel.Id].ToBoolean();
		_showInfoPanel.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowInfoPanel_EditValueChanged).Handler;
		_showInfoPanel.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowMTBillInfoPanel.Id].ToBoolean();
		_showASInfoInMTBill = AddBooleanControl(container4, "ShowASInfoInMTBill", ArmSoft.SME.Common.Resources.ShowASInfo);
		_showASInfoInMTBill.IsChecked = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowASInfoInMTBill.Id].ToBoolean();
		AddPOSTabToDialog(tc);
	}

	private void AddPOSTabToDialog(DXTabControl tabControl)
	{
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		LayoutGroup container = AddTabedGroup(tabControl, ArmSoft.SME.Trade.Common.Resources.PointOfSalePOS);
		_transitCash = AddDropDownControl(container, "TransitCash", ArmSoft.SME.Trade.Common.Resources.TransitCashDesk, new ArmSoft.SME.Trade.UI.DropDown.Cashes(withoutCheckAccess: true));
		_transitCash.Code = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultTransitCash.Id];
		_userPOSRightsGrid = AddGridControl(container, "GridUserPOSRights");
		_userPOSRightsGrid.AllowAdd = false;
		_userPOSRightsGrid.AllowDelete = false;
		_userPOSRightsGrid.AllowDuplicate = false;
		_userPOSRightsGrid.ShowButtons = false;
		_userPOSRightsRows = new ObservableCollection<UserRightsRow>
		{
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowMakeReturnDocument,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRecordReturnFromCustomer.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowEditGoodsPrice,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChangePrice.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowDeleteRow,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDeleteRow.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowDeleteFullRow,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSFullDeleteRow.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowDecreaseQuantity,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDecreaseQuantity.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowInputFromKeyboard,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSInputFromKeyboard.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowRemoveCard,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRemoveCard.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowPOSChooseBuyer,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChooseBuyer.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowToRegisterNewBuyer,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowToRegisterNewBuyer.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowCashInOut,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSCashInOut.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowPOSPrePaymentECRCheck,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSPrePaymentECRCheck.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowPrintMTCheck,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPrintMTCheck.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.ShowCashDeskAmount,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCashDeskAmount.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowCloseCashierShiftWhenNotHavePosponedSales,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.NotAllowCloseCashierShiftWhenHavePostponedDocs.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.AllowShowAvailability,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSShowAvailability.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.RecalcQty,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PriceRecalcFromSumm.Id].ToBoolean()
			},
			new UserRightsRow
			{
				ParamName = ArmSoft.SME.Trade.Common.Resources.SaveAndAutoPrintECR,
				ParamValue = _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.SaveAndPrintECR.Id].ToBoolean()
			}
		};
		_userPOSRightsGrid.ItemsSource = _userPOSRightsRows;
		_userPOSRightsTableView = _userPOSRightsGrid.View;
		_userPOSRightsTableView.AllowMoveColumnToDropArea = false;
		_userPOSRightsTableView.IsColumnMenuEnabled = false;
		DocumentGridColumn documentGridColumn = _userPOSRightsGrid.AddStringColumn("ParamName", "", 250);
		documentGridColumn.Width = 600.0;
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		_userPOSRightsGrid.AddBooleanColumn("ParamValue", "");
		_userPOSRightsGrid.Height = 365.0;
		Label label = new Label();
		TextBlock content = new TextBlock
		{
			Text = ArmSoft.SME.Trade.Common.Resources.POSOptionsInfoMessage,
			TextWrapping = TextWrapping.Wrap,
			Foreground = ColorsAS.Foreground.BlueBrush
		};
		label.Content = content;
		label.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
		label.Width = SizeCalculator.Height(700.0);
		ImageEdit myControl = new ImageEdit
		{
			Source = ImageService.GetImage("information_16.svg"),
			Padding = new Thickness(0.0, 0.0, 0.0, 0.0),
			IsEnabled = false,
			ShowBorder = false,
			Stretch = Stretch.Fill,
			Margin = new Thickness(0.0, 8.0, 0.0, 0.0),
			Height = 16.0,
			Width = 16.0
		};
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup(container);
		AddControl(layoutGroupExt, "Image", "", myControl);
		AddControl(layoutGroupExt, "TextBox", "", label);
		layoutGroupExt.VerticalAlignment = VerticalAlignment.Top;
		layoutGroupExt.Padding = new Thickness(12.0);
	}

	protected override void OnValidationWindow(bool okPressed, bool showValidationError = false)
	{
		base.OnValidationWindow(okPressed, showValidationError: true);
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			base.DialogValidation(args);
			if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storage.Code, _userId))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, _userName, _storage.Code));
			}
			if (!TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable(_cash.Code, _userId))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, _userName, _cash.Code));
			}
			if (_dateBegin.EditValue.HasValue && _dateEnd.EditValue.HasValue)
			{
				ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_dateBegin.EditValue, _dateEnd.EditValue);
			}
			else if (_dateBegin.EditValue.HasValue || _dateEnd.EditValue.HasValue)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidUserPeriod);
			}
			if (_mtBillOperAdjustmentsRows.Count((MTBillOperAdjustmentsRow x) => !string.IsNullOrWhiteSpace(x.HotKey)) != (from x in _mtBillOperAdjustmentsRows
				select x.HotKey into hotKey
				where !string.IsNullOrWhiteSpace(hotKey)
				select hotKey).Distinct().Count())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.HotKeysMustDiffer);
			}
			FillUserGeneralParams();
			FillUserRights();
			FillMTBillUserRights();
			FillPOSUserRights();
			if (_editedParameters.Count != 0)
			{
				_editedParameters.ForEach((ParameterStoreRequestModel p) =>
				{
					p.UserId = _userId;
				});
				Settings.ParametersService.SetValues(_editedParameters);
			}
			if (_userId == Settings.LoginService.UserId)
			{
				Settings.HostingEnvironment.RefreshWorkPeriodStatusBar();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}

	private void FillUserGeneralParams()
	{
		if (_storage.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultStorage.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultStorage.Id,
				Value = _storage.Code
			});
		}
		if (_supplier.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultSupplier.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultSupplier.Id,
				Value = _supplier.Code
			});
		}
		if (_customer.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultCustomer.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultCustomer.Id,
				Value = _customer.Code
			});
		}
		if (_buyingPriceType.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultBuyingPrice.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultBuyingPrice.Id,
				Value = _buyingPriceType.Code
			});
		}
		if (_salePriceType.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultSalePrice.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultSalePrice.Id,
				Value = _salePriceType.Code
			});
		}
		if (_discountPriceType.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultDiscountPrice.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultDiscountPrice.Id,
				Value = _discountPriceType.Code
			});
		}
		if (_discountPriceType.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultDiscountPrice.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultDiscountPrice.Id,
				Value = _discountPriceType.Code
			});
		}
		if (_cash.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultCash.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultCash.Id,
				Value = _cash.Code
			});
		}
		if (Settings.ParametersService.UseMarking() && _eMarkReceiptDevice.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.EMarkReceiptDevice.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.EMarkReceiptDevice.Id,
				Value = _eMarkReceiptDevice.Code
			});
		}
		if (_curr.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultMTBillCurr.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultMTBillCurr.Id,
				Value = _curr.Code
			});
		}
		if (_salesConsultant.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultSalesConsultant.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultSalesConsultant.Id,
				Value = _salesConsultant.Code
			});
		}
		if (_userAWP.Code != _paramsBySelectedUser[ArmSoft.SME.Common.Parameters.Definitions.UserAWPNavigatorName.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Common.Parameters.Definitions.UserAWPNavigatorName.Id,
				Value = _userAWP.Code
			});
		}
		if (Convert.ToBoolean(_showOnlyUserAWP.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowOnlyUserAWP.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowOnlyUserAWP.Id,
				Value = Convert.ToString(_showOnlyUserAWP.IsChecked)
			});
		}
		if (_scannerReadMode.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ScannerMode.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ScannerMode.Id,
				Value = _scannerReadMode.Code
			});
		}
		if (Convert.ToBoolean(_useCurrentDateInDocs.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseCurrentDateInDocs.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.UseCurrentDateInDocs.Id,
				Value = Convert.ToString(_useCurrentDateInDocs.IsChecked)
			});
		}
		if (Convert.ToBoolean(_useCurrentDateInDocs.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseCurrentDateInDocs.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.UseCurrentDateInDocs.Id,
				Value = Convert.ToString(_useCurrentDateInDocs.IsChecked)
			});
		}
	}

	private void FillUserRights()
	{
		if (Convert.ToBoolean(_useCurrentDateInDocs.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseCurrentDateInDocs.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.UseCurrentDateInDocs.Id,
				Value = Convert.ToString(_useCurrentDateInDocs.IsChecked)
			});
		}
		if ((!_dateBegin.EditValue.HasValue && !string.IsNullOrWhiteSpace(_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id])) || (_dateBegin.EditValue.HasValue && string.IsNullOrWhiteSpace(_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id])) || (_dateBegin.EditValue.HasValue && !string.IsNullOrWhiteSpace(_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id]) && _dateBegin.EditValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id].YYYYMMDDToDateOnly()))
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id,
				Value = Convertor.ConvertDateOnlyToString(_dateBegin.EditValue),
				IsDefault = (_dateBegin.EditValue == Settings.ParametersService.GetGeneralValue(ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id).YYYYMMDDToDateOnly())
			});
		}
		if ((!_dateEnd.EditValue.HasValue && !string.IsNullOrWhiteSpace(_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id])) || (_dateEnd.EditValue.HasValue && string.IsNullOrWhiteSpace(_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id])) || (_dateEnd.EditValue.HasValue && !string.IsNullOrWhiteSpace(_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id]) && _dateEnd.EditValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id].YYYYMMDDToDateOnly()))
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id,
				Value = Convertor.ConvertDateOnlyToString(_dateEnd.EditValue),
				IsDefault = (_dateEnd.EditValue == Settings.ParametersService.GetGeneralValue(ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id).YYYYMMDDToDateOnly())
			});
		}
		int num = 0;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeSalePrices.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeSalePrices.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDiscountPrice.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDiscountPrice.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangePriceType.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangePriceType.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDiscountPriceType.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDiscountPriceType.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.CalcDiscountAuto.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.CalcDiscountAuto.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AlwaysCalcDiscountAuto.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AlwaysCalcDiscountAuto.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowMTInclInPrList.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowMTInclInPrList.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowGoodsCostInReportsAndDocs.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowGoodsCostInReportsAndDocs.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowExceedDebtLimit.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowExceedDebtLimit.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Common.Parameters.Definitions.AllowEditCodeOfDirectories.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Common.Parameters.Definitions.AllowEditCodeOfDirectories.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowUse100PercentSale.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowUse100PercentSale.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPurchaseWith0PriceOr100Percent.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPurchaseWith0PriceOr100Percent.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AddMaterialToPartnersMT.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AddMaterialToPartnersMT.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowGroupDelete.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowGroupDelete.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Common.Parameters.Definitions.AllowOpFromHistoryPanel.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Common.Parameters.Definitions.AllowOpFromHistoryPanel.Id,
				Value = Convert.ToString(_userRightsRows[num].ParamValue)
			});
		}
		if (!_isAdmin)
		{
			num++;
			if (_userRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DenyAccessToOtherOwnerDocs.Id].ToBoolean())
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DenyAccessToOtherOwnerDocs.Id,
					Value = Convert.ToString(_userRightsRows[num].ParamValue)
				});
			}
		}
	}

	private void FillMTBillUserRights()
	{
		if (Convert.ToBoolean(_mtBillSimpleMode.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSimpleMode.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSimpleMode.Id,
				Value = Convert.ToString(_mtBillSimpleMode.IsChecked)
			});
		}
		if (Convert.ToBoolean(_mtBillAllowDifferentStorages.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillAllowDifferentStorages.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillAllowDifferentStorages.Id,
				Value = Convert.ToString(_mtBillAllowDifferentStorages.IsChecked)
			});
		}
		if (Convert.ToBoolean(_showPoleDisplay.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPoleDisplay.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPoleDisplay.Id,
				Value = Convert.ToString(_showPoleDisplay.IsChecked)
			});
		}
		if (_mtCheckPriceType.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultMTCheckPriceType.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultMTCheckPriceType.Id,
				Value = _mtCheckPriceType.Code
			});
		}
		if (_mtCheckDiscountPriceType.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultMTCheckDiscountPriceType.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultMTCheckDiscountPriceType.Id,
				Value = _mtCheckDiscountPriceType.Code
			});
		}
		if (Convert.ToBoolean(_allowChangeMTBillGoodRow.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeMTBillGoodRow.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeMTBillGoodRow.Id,
				Value = Convert.ToString(_allowChangeMTBillGoodRow.IsChecked)
			});
		}
		if (Convert.ToBoolean(_mtBillRecordAllEntries.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillRecordAllEntries.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillRecordAllEntries.Id,
				Value = Convert.ToString(_mtBillRecordAllEntries.IsChecked)
			});
		}
		if (Convert.ToBoolean(_allowToAddNewBuyerWithNotRegisteredBuyerCard.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowToAddNewBuyerWithNotRegisteredBuyerCard.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowToAddNewBuyerWithNotRegisteredBuyerCard.Id,
				Value = Convert.ToString(_allowToAddNewBuyerWithNotRegisteredBuyerCard.IsChecked)
			});
		}
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseMarking.Id].ToBoolean())
		{
			if (Convert.ToBoolean(_openMarkingWindowImmediatly.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.OpenMarkingWindowImmediatly.Id].ToBoolean())
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.OpenMarkingWindowImmediatly.Id,
					Value = Convert.ToString(_openMarkingWindowImmediatly.IsChecked)
				});
			}
			if (Convert.ToBoolean(_checkMarkingAndBarCode.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.CheckMarkingAndBarCode.Id].ToBoolean())
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.CheckMarkingAndBarCode.Id,
					Value = Convert.ToString(_checkMarkingAndBarCode.IsChecked)
				});
			}
		}
		if (_mtBillRoundDegree.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillRoundDegree.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillRoundDegree.Id,
				Value = _mtBillRoundDegree.Code
			});
		}
		if (Convert.ToBoolean(_mtBillSummRoundUp.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSummRoundUp.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSummRoundUp.Id,
				Value = Convert.ToString(_mtBillSummRoundUp.IsChecked)
			});
		}
		FillMTCheckPrintOptions();
		FillECRCheckOptions();
		FillBillFields();
		FillBillColumns();
		FillBillHotKeys();
		FillInfoPanelRights();
		if (Convert.ToBoolean(_showASInfoInMTBill.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowASInfoInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowASInfoInMTBill.Id,
				Value = Convert.ToString(_showASInfoInMTBill.IsChecked)
			});
		}
	}

	private void FillMTCheckPrintOptions()
	{
		if (Convert.ToBoolean(_printMTCheckAndNew.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintMTCheckAndNew.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintMTCheckAndNew.Id,
				Value = Convert.ToString(_printMTCheckAndNew.IsChecked)
			});
		}
		if (_mtCheckTemplate.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTCheckTemplate.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTCheckTemplate.Id,
				Value = _mtCheckTemplate.Code
			});
		}
		if (_mtRetCheckTemplate.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.RetCheckTemplate.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.RetCheckTemplate.Id,
				Value = _mtRetCheckTemplate.Code
			});
		}
	}

	private void FillECRCheckOptions()
	{
		if (Convert.ToBoolean(_ecrCheckOnCheckPrinter.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintECRCheckOnCheckPrinter.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintECRCheckOnCheckPrinter.Id,
				Value = Convert.ToString(_ecrCheckOnCheckPrinter.IsChecked)
			});
		}
		if (Convert.ToBoolean(_printECRCheckAndNew.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintECRCheckAndNew.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintECRCheckAndNew.Id,
				Value = Convert.ToString(_printECRCheckAndNew.IsChecked)
			});
		}
		if (Convert.ToBoolean(_ecrCheckPrintCreditAsCash.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRCheckPrintCreditAsCash.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRCheckPrintCreditAsCash.Id,
				Value = Convert.ToString(_ecrCheckPrintCreditAsCash.IsChecked)
			});
		}
		if (Convert.ToBoolean(_ecrCheckPrintCreditAsPartial.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRCheckPrintCreditAsPartial.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRCheckPrintCreditAsPartial.Id,
				Value = Convert.ToString(_ecrCheckPrintCreditAsPartial.IsChecked)
			});
		}
		if (_ecrCheckPrintBonusAsCash.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRCheckPrintBonusType.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRCheckPrintBonusType.Id,
				Value = _ecrCheckPrintBonusAsCash.Code
			});
		}
		if (Convert.ToBoolean(_printPrepaymentCheckSeparately.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintPrepaymentCheckSeparately.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintPrepaymentCheckSeparately.Id,
				Value = Convert.ToString(_printPrepaymentCheckSeparately.IsChecked)
			});
		}
		if (Convert.ToBoolean(_ecrCheckPrintDiscount.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintECRCheckDiscount.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.PrintECRCheckDiscount.Id,
				Value = Convert.ToString(_ecrCheckPrintDiscount.IsChecked)
			});
		}
		if (_ecrDiscountType.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRDiscountType.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ECRDiscountType.Id,
				Value = _ecrDiscountType.Code
			});
		}
		if (_asECRCheckItemCode.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ASECRCheckItemCode.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ASECRCheckItemCode.Id,
				Value = _asECRCheckItemCode.Code
			});
		}
		if (_asECRCheckItemName.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ASECRCheckItemName.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ASECRCheckItemName.Id,
				Value = _asECRCheckItemName.Code
			});
		}
		if (Convert.ToBoolean(_allowChangeECRCheckNum.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeECRCheckNum.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeECRCheckNum.Id,
				Value = Convert.ToString(_allowChangeECRCheckNum.IsChecked)
			});
		}
	}

	private void FillBillFields()
	{
		int num = 0;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDateInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDateInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDateInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDateInBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDocNumInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDocNumInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDocNumberInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDocNumberInBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDiscountInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeDiscountInBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowStorageInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowStorageInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeStorageInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeStorageInBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCashInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCashInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeCashInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeCashInBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCashierShiftNumInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCashierShiftNumInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeCashierShiftNumInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeCashierShiftNumInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeBuyerInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeBuyerInBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPrintExpJustifyingCheckInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPrintExpJustifyingCheckInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangePrintExpJustifyingCheckInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangePrintExpJustifyingCheckInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowSalesConsultantInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowSalesConsultantInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeSaleConsultantInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeSaleConsultantInBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCommentInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCommentInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeCommentInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeCommentInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
		num++;
		if (_mtBillFieldsRows[num].Show != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowOwnerInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowOwnerInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Show)
			});
		}
		if (_mtBillFieldsRows[num].Edit != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeOwnerInMTBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowChangeOwnerInMTBill.Id,
				Value = Convert.ToString(_mtBillFieldsRows[num].Edit)
			});
		}
	}

	private void FillBillColumns()
	{
		if (Convert.ToBoolean(_mtBillGridAutoWidth.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillGridAutoWidth.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillGridAutoWidth.Id,
				Value = Convert.ToString(_mtBillGridAutoWidth.IsChecked)
			});
		}
		int num = 0;
		if (_mtBillColsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowUnitCodeColInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowUnitCodeColInBill.Id,
				Value = Convert.ToString(_mtBillColsRows[num].ParamValue)
			});
		}
		num++;
		if (_mtBillColsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountPercentColInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountPercentColInBill.Id,
				Value = Convert.ToString(_mtBillColsRows[num].ParamValue)
			});
		}
		num++;
		if (_mtBillColsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountedPriceColInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountedPriceColInBill.Id,
				Value = Convert.ToString(_mtBillColsRows[num].ParamValue)
			});
		}
		num++;
		if (_mtBillColsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountSummColInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDiscountSummColInBill.Id,
				Value = Convert.ToString(_mtBillColsRows[num].ParamValue)
			});
		}
		num++;
		if (_mtBillColsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowVATColInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowVATColInBill.Id,
				Value = Convert.ToString(_mtBillColsRows[num].ParamValue)
			});
		}
		num++;
		if (_mtBillColsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowAccumulatedBonusColInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowAccumulatedBonusColInBill.Id,
				Value = Convert.ToString(_mtBillColsRows[num].ParamValue)
			});
		}
		num++;
		if (_mtBillColsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowUsedBonusColInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowUsedBonusColInBill.Id,
				Value = Convert.ToString(_mtBillColsRows[num].ParamValue)
			});
		}
		num++;
		if (_mtBillColsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyColInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyColInBill.Id,
				Value = Convert.ToString(_mtBillColsRows[num].ParamValue)
			});
		}
		num++;
		if (_mtBillColsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowRowDescrColInBill.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowRowDescrColInBill.Id,
				Value = Convert.ToString(_mtBillColsRows[num].ParamValue)
			});
		}
	}

	private void FillBillHotKeys()
	{
		int num = 0;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillInputValueHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillInputValueHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSummaCashHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSummaCashHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillBarCodeEnterHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillBarCodeEnterHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCodeEnterHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCodeEnterHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillMTListLikeNameHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillMTListLikeNameHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillQtyEnterHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillQtyEnterHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSalePriceHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSalePriceHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillUsePrePaymentHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillUsePrePaymentHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillUsePrePaymentShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillUsePrePaymentShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPlCardPaymentHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPlCardPaymentHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPlCardPaymentShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPlCardPaymentShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCreditHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCreditHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCreditShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCreditShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus.Id].ToBoolean())
		{
			num++;
			if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillUseBonusHotKey.Id])
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillUseBonusHotKey.Id,
					Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
				});
			}
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintECRCheckHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintECRCheckHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintECRCheckShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintECRCheckShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintBillHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintBillHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintBillShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPrintBillShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillWeightHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillWeightHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillComplectHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillComplectHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillComplectShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillComplectShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillDiscountHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillDiscountHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus.Id].ToBoolean())
		{
			num++;
			if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCalcBonusHotKey.Id])
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillCalcBonusHotKey.Id,
					Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
				});
			}
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPricesHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPricesHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPricesShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillPricesShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSetFocusOnGridHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSetFocusOnGridHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFilteredSearchHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFilteredSearchHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFilteredSearchShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFilteredSearchShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFavoritesHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFavoritesHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFavoritesShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillFavoritesShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillErasePaymentsHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillErasePaymentsHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus.Id].ToBoolean())
		{
			num++;
			if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillAddToBonusHotKey.Id])
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillAddToBonusHotKey.Id,
					Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
				});
			}
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSContinueHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.POSContinueHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSContinueShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.POSContinueShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSDeferSaleHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.POSDeferSaleHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSDeferSaleShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.POSDeferSaleShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSPartnerCardHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.POSPartnerCardHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSPartnerCardShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.POSPartnerCardShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.POSReturnHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.POSReturnHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillReturnBillShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillReturnBillShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ActualQuantitiesCheckHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ActualQuantitiesCheckHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ActualQuantitiesCheckShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ActualQuantitiesCheckShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.EnableSubstituteItems.Id].ToBoolean())
		{
			num++;
			if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSubstituteItemsHotKey.Id])
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSubstituteItemsHotKey.Id,
					Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
				});
			}
			if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSubstituteItemsShowButton.Id].ToBoolean())
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillSubstituteItemsShowButton.Id,
					Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
				});
			}
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillQRHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillQRHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillQRShowButton.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillQRShowButton.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
			});
		}
		num++;
		if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UnitCodeSwitchHotKey.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.UnitCodeSwitchHotKey.Id,
				Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
			});
		}
		if (Settings.ParametersService.UseMarking())
		{
			num++;
			if (_mtBillOperAdjustmentsRows[num].HotKey != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.EMarkReceiptHotKey.Id])
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.EMarkReceiptHotKey.Id,
					Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].HotKey)
				});
			}
			if (_mtBillOperAdjustmentsRows[num].ShowButton != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.EMarkReceiptShowButton.Id].ToBoolean())
			{
				_editedParameters.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.EMarkReceiptShowButton.Id,
					Value = Convert.ToString(_mtBillOperAdjustmentsRows[num].ShowButton)
				});
			}
		}
	}

	private void FillInfoPanelRights()
	{
		if (Convert.ToBoolean(_showInfoPanel.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowMTBillInfoPanel.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowMTBillInfoPanel.Id,
				Value = Convert.ToString(_showInfoPanel.IsChecked)
			});
		}
		if (Convert.ToBoolean(_showItemPictureOnInfoPanel.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowItemPictureOnInfoPanel.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowItemPictureOnInfoPanel.Id,
				Value = Convert.ToString(_showItemPictureOnInfoPanel.IsChecked)
			});
		}
		if (Convert.ToBoolean(_showStorageAvailabilityOnInfoPanel.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowStorageAvailabilityOnInfoPanel.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowStorageAvailabilityOnInfoPanel.Id,
				Value = Convert.ToString(_showStorageAvailabilityOnInfoPanel.IsChecked)
			});
		}
		if (Convert.ToBoolean(_showWholeAvailabilityOnInfoPanel.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowWholeAvailabilityOnInfoPanel.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowWholeAvailabilityOnInfoPanel.Id,
				Value = Convert.ToString(_showWholeAvailabilityOnInfoPanel.IsChecked)
			});
		}
		if (Convert.ToBoolean(_showDescriptionOnInfoPanel.IsChecked) != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDescriptionOnInfoPanel.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowDescriptionOnInfoPanel.Id,
				Value = Convert.ToString(_showDescriptionOnInfoPanel.IsChecked)
			});
		}
	}

	private void FillPOSUserRights()
	{
		if (_transitCash.Code != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultTransitCash.Id])
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DefaultTransitCash.Id,
				Value = _transitCash.Code
			});
		}
		int num = 0;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRecordReturnFromCustomer.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRecordReturnFromCustomer.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChangePrice.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChangePrice.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDeleteRow.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDeleteRow.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSFullDeleteRow.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSFullDeleteRow.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDecreaseQuantity.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDecreaseQuantity.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSInputFromKeyboard.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSInputFromKeyboard.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRemoveCard.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRemoveCard.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChooseBuyer.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChooseBuyer.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowToRegisterNewBuyer.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowToRegisterNewBuyer.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSCashInOut.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSCashInOut.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSPrePaymentECRCheck.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSPrePaymentECRCheck.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPrintMTCheck.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPrintMTCheck.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCashDeskAmount.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowCashDeskAmount.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.NotAllowCloseCashierShiftWhenHavePostponedDocs.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.NotAllowCloseCashierShiftWhenHavePostponedDocs.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSShowAvailability.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSShowAvailability.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.PriceRecalcFromSumm.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.PriceRecalcFromSumm.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
		num++;
		if (_userPOSRightsRows[num].ParamValue != _paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.SaveAndPrintECR.Id].ToBoolean())
		{
			_editedParameters.Add(new ParameterStoreRequestModel
			{
				Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.SaveAndPrintECR.Id,
				Value = Convert.ToString(_userPOSRightsRows[num].ParamValue)
			});
		}
	}

	private void UserRightsTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		UserRightsRow userRightsRow = (UserRightsRow)_userRightsGrid.Grid.GetRow(e.RowHandle);
		if (userRightsRow == null)
		{
			return;
		}
		if (e.RowHandle == 0)
		{
			if (userRightsRow.ParamValue)
			{
				UserRightsRow userRightsRow2 = (UserRightsRow)_userRightsGrid.Grid.GetRow(2);
				if (userRightsRow2 != null)
				{
					userRightsRow2.ParamValue = true;
				}
			}
		}
		else if (e.RowHandle == 1 || e.RowHandle == 3)
		{
			if (userRightsRow.ParamValue)
			{
				UserRightsRow userRightsRow3 = (UserRightsRow)_userRightsGrid.Grid.GetRow(4);
				if (userRightsRow3 != null)
				{
					userRightsRow3.ParamValue = false;
				}
				UserRightsRow userRightsRow4 = (UserRightsRow)_userRightsGrid.Grid.GetRow(5);
				if (userRightsRow4 != null)
				{
					userRightsRow4.ParamValue = false;
				}
				if (e.RowHandle == 1)
				{
					UserRightsRow userRightsRow5 = (UserRightsRow)_userRightsGrid.Grid.GetRow(3);
					if (userRightsRow5 != null)
					{
						userRightsRow5.ParamValue = true;
					}
				}
			}
			else if (e.RowHandle == 3)
			{
				UserRightsRow userRightsRow6 = (UserRightsRow)_userRightsGrid.Grid.GetRow(1);
				if (userRightsRow6 != null)
				{
					userRightsRow6.ParamValue = false;
				}
			}
		}
		else if (e.RowHandle == 2)
		{
			if (!userRightsRow.ParamValue)
			{
				UserRightsRow userRightsRow7 = (UserRightsRow)_userRightsGrid.Grid.GetRow(0);
				if (userRightsRow7 != null)
				{
					userRightsRow7.ParamValue = false;
				}
			}
		}
		else if ((e.RowHandle == 4 || e.RowHandle == 5) && userRightsRow.ParamValue)
		{
			UserRightsRow userRightsRow8 = (UserRightsRow)_userRightsGrid.Grid.GetRow(1);
			if (userRightsRow8 != null)
			{
				userRightsRow8.ParamValue = false;
			}
			UserRightsRow userRightsRow9 = (UserRightsRow)_userRightsGrid.Grid.GetRow(3);
			if (userRightsRow9 != null)
			{
				userRightsRow9.ParamValue = false;
			}
		}
		_userRightsGrid.View.CommitEditing();
		_userRightsGrid.Grid.RefreshData();
	}

	private void MTBillFieldTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTFillRowFilds mTFillRowFilds = (MTFillRowFilds)_mtBillFieldsGrid.Grid.GetRow(e.RowHandle);
		if (mTFillRowFilds != null && mTFillRowFilds.Row == ArmSoft.SME.Common.Resources.Storage)
		{
			if (!mTFillRowFilds.Edit)
			{
				_mtBillAllowDifferentStorages.IsChecked = false;
			}
			_mtBillAllowDifferentStorages.IsReadOnly = !mTFillRowFilds.Edit;
		}
	}

	private void MTBillFieldTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		_mtBillFieldsGrid.View.CommitEditing();
		_mtBillFieldsGrid.Grid.RefreshRow(e.RowHandle);
	}

	private void MTBillColsTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		_mtBillColsGrid.View.CommitEditing();
		_mtBillColsGrid.Grid.RefreshRow(e.RowHandle);
	}

	private void MTBillSimpleMode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = Convert.ToBoolean(_mtBillSimpleMode.IsChecked);
		if (flag)
		{
			_mtBillColsRows[0].ParamValue = false;
			_mtBillColsRows[1].ParamValue = false;
			_mtBillColsRows[2].ParamValue = false;
			_mtBillColsRows[3].ParamValue = false;
			_mtBillColsRows[4].ParamValue = false;
			_mtBillColsRows[5].ParamValue = false;
			_mtBillColsRows[6].ParamValue = false;
			_mtBillColsRows[7].ParamValue = false;
			_mtBillColsGrid.Grid.RefreshData();
			_mtBillAllowDifferentStorages.IsChecked = false;
			_mtBillAllowDifferentStorages.IsReadOnly = true;
		}
		else
		{
			_mtBillAllowDifferentStorages.IsReadOnly = !_mtBillFieldsRows[4].Edit;
		}
		_mtBillFieldsGrid.IsReadOnly = flag;
		_mtBillColsGrid.IsReadOnly = flag;
	}

	private void ShowInfoPanel_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			if (_showInfoPanel.IsChecked.Value)
			{
				_showItemPictureOnInfoPanel.IsReadOnly = false;
				_showStorageAvailabilityOnInfoPanel.IsReadOnly = false;
				_showWholeAvailabilityOnInfoPanel.IsReadOnly = false;
				_showDescriptionOnInfoPanel.IsReadOnly = false;
				return;
			}
			_showItemPictureOnInfoPanel.IsChecked = false;
			_showStorageAvailabilityOnInfoPanel.IsChecked = false;
			_showWholeAvailabilityOnInfoPanel.IsChecked = false;
			_showDescriptionOnInfoPanel.IsChecked = false;
			_showItemPictureOnInfoPanel.IsReadOnly = true;
			_showStorageAvailabilityOnInfoPanel.IsReadOnly = true;
			_showWholeAvailabilityOnInfoPanel.IsReadOnly = true;
			_showDescriptionOnInfoPanel.IsReadOnly = true;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetMTBillDefaultHotKeys_Click(object sender, RoutedEventArgs e)
	{
		int num = 0;
		_mtBillOperAdjustmentsRows[num].HotKey = "F2";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "F6";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "F7";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "F8";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "F9";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "Ctrl+F9";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "Shift+F8";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "F4";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "Ctrl+F11";
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus.Id].ToBoolean())
		{
			num++;
			_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		}
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "F3";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "F11";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "Shift+F9";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus.Id].ToBoolean())
		{
			num++;
			_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		}
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = "Ctrl+1";
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus.Id].ToBoolean())
		{
			num++;
			_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		}
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		if (_paramsBySelectedUser[ArmSoft.SME.Trade.Common.Parameters.Definitions.EnableSubstituteItems.Id].ToBoolean())
		{
			num++;
			_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		}
		num++;
		_mtBillOperAdjustmentsRows[num].HotKey = string.Empty;
		_mtBillOperAdjustmentsGrid.Grid.RefreshData();
	}

	private void UseCurrentDateInDocs_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		UseCurrentDateInDocs_EditValueChangedForLoad();
	}

	private void UseCurrentDateInDocs_EditValueChangedForLoad()
	{
		_dateBegin.IsReadOnly = _useCurrentDateInDocs.IsChecked.Value;
		_dateEnd.IsReadOnly = _useCurrentDateInDocs.IsChecked.Value;
	}

	private void ECRCheckPrintCreditAsCash_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((bool)e.NewValue && _ecrCheckPrintCreditAsPartial != null)
		{
			_ecrCheckPrintCreditAsPartial.IsChecked = false;
		}
	}

	private void ECRCheckPrintCreditAsPartial_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((bool)e.NewValue && _ecrCheckPrintCreditAsCash != null)
		{
			_ecrCheckPrintCreditAsCash.IsChecked = false;
		}
	}

	private void ECRCheckPrintBonusAsCash_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_ecrCheckPrintBonusAsCash.Code == PrintBonusTypesSE.DiscountAndPrePayment.Value)
		{
			_printPrepaymentCheckSeparately.IsReadOnly = false;
			return;
		}
		_printPrepaymentCheckSeparately.IsChecked = true;
		_printPrepaymentCheckSeparately.IsReadOnly = true;
	}
}
