using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.TaxService.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.Documents;

[DocumentWindow(231)]
public class TaxDocumentWindow<T> : DocumentWindow<T> where T : TaxDocument, new()
{
	public class SpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		public string itemType;

		[ObservableProperty]
		public string itemTypeName;

		[ObservableProperty]
		public string itemCode;

		[ObservableProperty]
		public int itemID;

		[ObservableProperty]
		public string unitCodeFromDirectory;

		[ObservableProperty]
		public string itemExternalCode;

		[ObservableProperty]
		public string itemName;

		[ObservableProperty]
		public short unitID;

		[ObservableProperty]
		public string unitBrief;

		[ObservableProperty]
		public decimal volume;

		[ObservableProperty]
		public decimal quantity;

		[ObservableProperty]
		public decimal price;

		[ObservableProperty]
		public decimal discount;

		[ObservableProperty]
		public decimal sum;

		[ObservableProperty]
		public decimal tarePrice;

		[ObservableProperty]
		public decimal exciseRate;

		[ObservableProperty]
		public decimal excise;

		[ObservableProperty]
		public decimal vAT;

		[ObservableProperty]
		public string dealType;

		[ObservableProperty]
		public decimal sumVAT;

		[ObservableProperty]
		public decimal totalSum;

		[ObservableProperty]
		public string cPACode;

		[ObservableProperty]
		public short rowNum;

		[ObservableProperty]
		public string rowISN;

		[ObservableProperty]
		public string itemNameFromDirectory;

		[ObservableProperty]
		public string unitBriefFromDirectory;

		[ObservableProperty]
		public string _cPACodeFromDirectory;

		internal bool isAccDoc;

		public bool CheckItemCode { get; set; } = true;

		public string DealTypeCaption
		{
			get
			{
				if (string.IsNullOrWhiteSpace(DealType))
				{
					return string.Empty;
				}
				if (isAccDoc)
				{
					return SmartEnum<AccDealTypesSE, short>.FromValue(Convert.ToInt16(DealType)).Caption;
				}
				return SmartEnum<InvoiceDealTypesSE, short>.FromValue(Convert.ToInt16(DealType)).Caption;
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ItemType
		{
			get
			{
				return itemType;
			}
			[MemberNotNull("itemType")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(itemType, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemType);
					itemType = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemType);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ItemTypeName
		{
			get
			{
				return itemTypeName;
			}
			[MemberNotNull("itemTypeName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(itemTypeName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemTypeName);
					itemTypeName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemTypeName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ItemCode
		{
			get
			{
				return itemCode;
			}
			[MemberNotNull("itemCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(itemCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemCode);
					itemCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int ItemID
		{
			get
			{
				return itemID;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(itemID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemID);
					itemID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemID);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string UnitCodeFromDirectory
		{
			get
			{
				return unitCodeFromDirectory;
			}
			[MemberNotNull("unitCodeFromDirectory")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(unitCodeFromDirectory, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitCodeFromDirectory);
					unitCodeFromDirectory = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitCodeFromDirectory);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ItemExternalCode
		{
			get
			{
				return itemExternalCode;
			}
			[MemberNotNull("itemExternalCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(itemExternalCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemExternalCode);
					itemExternalCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemExternalCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ItemName
		{
			get
			{
				return itemName;
			}
			[MemberNotNull("itemName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(itemName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemName);
					itemName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public short UnitID
		{
			get
			{
				return unitID;
			}
			set
			{
				if (!EqualityComparer<short>.Default.Equals(unitID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitID);
					unitID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitID);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string UnitBrief
		{
			get
			{
				return unitBrief;
			}
			[MemberNotNull("unitBrief")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(unitBrief, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitBrief);
					unitBrief = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitBrief);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Volume
		{
			get
			{
				return volume;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(volume, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Volume);
					volume = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Volume);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Quantity
		{
			get
			{
				return quantity;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(quantity, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Quantity);
					quantity = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Quantity);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Price
		{
			get
			{
				return price;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(price, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Price);
					price = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Price);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Discount
		{
			get
			{
				return discount;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(discount, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Discount);
					discount = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Discount);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Sum
		{
			get
			{
				return sum;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(sum, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Sum);
					sum = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Sum);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal TarePrice
		{
			get
			{
				return tarePrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(tarePrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.TarePrice);
					tarePrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.TarePrice);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal ExciseRate
		{
			get
			{
				return exciseRate;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(exciseRate, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ExciseRate);
					exciseRate = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ExciseRate);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Excise
		{
			get
			{
				return excise;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(excise, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Excise);
					excise = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Excise);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal VAT
		{
			get
			{
				return vAT;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(vAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VAT);
					vAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VAT);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string DealType
		{
			get
			{
				return dealType;
			}
			[MemberNotNull("dealType")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(dealType, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DealType);
					dealType = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DealType);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal SumVAT
		{
			get
			{
				return sumVAT;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(sumVAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SumVAT);
					sumVAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SumVAT);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal TotalSum
		{
			get
			{
				return totalSum;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(totalSum, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.TotalSum);
					totalSum = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.TotalSum);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string CPACode
		{
			get
			{
				return cPACode;
			}
			[MemberNotNull("cPACode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(cPACode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CPACode);
					cPACode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CPACode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public short RowNum
		{
			get
			{
				return rowNum;
			}
			set
			{
				if (!EqualityComparer<short>.Default.Equals(rowNum, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowNum);
					rowNum = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowNum);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string RowISN
		{
			get
			{
				return rowISN;
			}
			[MemberNotNull("rowISN")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(rowISN, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowISN);
					rowISN = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowISN);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ItemNameFromDirectory
		{
			get
			{
				return itemNameFromDirectory;
			}
			[MemberNotNull("itemNameFromDirectory")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(itemNameFromDirectory, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemNameFromDirectory);
					itemNameFromDirectory = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemNameFromDirectory);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string UnitBriefFromDirectory
		{
			get
			{
				return unitBriefFromDirectory;
			}
			[MemberNotNull("unitBriefFromDirectory")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(unitBriefFromDirectory, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitBriefFromDirectory);
					unitBriefFromDirectory = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitBriefFromDirectory);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string CPACodeFromDirectory
		{
			get
			{
				return _cPACodeFromDirectory;
			}
			[MemberNotNull("_cPACodeFromDirectory")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_cPACodeFromDirectory, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CPACodeFromDirectory);
					_cPACodeFromDirectory = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CPACodeFromDirectory);
				}
			}
		}

		static SpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(SpecificationRow), cacheValidationResults: false, new Dictionary<string, Action<DocumentGridRow, ErrorInfo>>
			{
				{
					"ItemCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateItemCode((SpecificationRow)row, info);
					}
				},
				{
					"CPACode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateCPACode((SpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateItemCode(SpecificationRow row, ErrorInfo info)
		{
			if (string.IsNullOrWhiteSpace(row.ItemCode) && row.CheckItemCode)
			{
				info.ErrorType = ErrorType.Warning;
				info.ErrorText = string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Code);
			}
		}

		private static void ValidateCPACode(SpecificationRow row, ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.CPACode) && Settings.CoreCacheService.CPACode.GetItem(row.CPACode) == null)
			{
				info.ErrorType = ErrorType.Warning;
				info.ErrorText = string.Format(ArmSoft.SME.Common.Resources.CodeNotExistDir, row.CPACode, ArmSoft.SME.Common.Resources.CPAClassifications);
			}
		}
	}

	protected DocumentGridControl _grid;

	protected TableView _taxDocumentTableView;

	protected ObservableCollection<SpecificationRow> _receivedTaxInvoiceList;

	protected DateEditControl _baseInvSubMissionDate;

	protected TextEditExt _baseTaxInvoiceSeries;

	protected TextEditExt _baseTaxInvoiceNumber;

	protected DateEditControl _submissionDate;

	protected PopupCalcEditExt _taxInvForm;

	protected TextEditExt _taxInvFormName;

	protected DateEditControl _suppDate;

	protected TextEditExt _taxInvoiceSeries;

	protected TextEditExt _taxInvoiceNumber;

	protected TextEditExt _comment;

	protected DateEditControl _contractDate;

	protected TextEditExt _contract;

	protected CheckEditExt _signed;

	protected TextEditExt _supplierTaxCode;

	protected TextEditExt _supplierName;

	protected TextEditExt _supplierAddress;

	protected TextEditExt _supplierBusinessAddress;

	protected DropDownControl _supplierSettlAcc;

	protected TextEditExt _supplierBankName;

	protected TextEditExt _supplierAdditionalData;

	protected TextEditExt _buyerTaxCode;

	protected TextEditExt _buyerPassport;

	protected TextEditExt _buyerName;

	protected TextEditExt _buyerAddress;

	protected TextEditExt _buyerBusinessAddress;

	protected DropDownControl _buyerSettlAcc;

	protected TextEditExt _buyerBankName;

	protected TextEditExt _buyerAdditionalData;

	protected TextEditExt _buyerShippingMethod;

	protected TextEditExt _mediator;

	protected TextEditExt _mediatorLicense;

	protected DateEditControl _mediatorLicenseDate;

	protected DocumentGridColumn _itemTypeNameCol;

	protected DocumentGridColumn _itemCodeCol;

	protected DocumentGridColumn _itemNameCol;

	protected DocumentGridColumn _unitBriefCol;

	protected DocumentGridColumn _itemExternalCodeCol;

	protected DocumentGridColumn _itemExternalNameCol;

	protected DocumentGridColumn _itemExternalBriefCol;

	protected DocumentGridColumn _dealTypeCol;

	protected DocumentGridColumn _cpaCodeCol;

	protected DocumentGridColumn _cpaCodeFromDirectoryCol;

	protected PopupCalcEditExt _summaVAT;

	protected PopupCalcEditExt _summa;

	protected bool _isFullCaption;

	protected bool _isUnit;

	protected bool _ignoreCase = true;

	protected string _mtGroup = "";

	protected string _itemCode = "";

	protected string _itemAcc = "";

	protected ReportBarButtonItem _barButtonJumpTo;

	public bool _isFromEInvoicing;

	protected override double WidthValue => 890.0;

	protected override double HeightValue => 480.0;

	protected override double MinWidthValue => 890.0;

	protected override double MinHeightValue => 250.0;

	protected override bool AllowNewAfterSave()
	{
		return false;
	}

	public TaxDocumentWindow()
	{
	}

	public TaxDocumentWindow(T doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		IDropDownService requiredService = Settings.ServiceProvider.GetRequiredService<IDropDownService>();
		_suppDate = AddDateControl(null, ArmSoft.SME.Common.Resources.DeliveryDate, isRequired: false);
		_supplierName = AddStringControl(null, Definition.Fields["SupplierName"], HorizontalAlignment.Stretch);
		_supplierTaxCode = AddStringControl(null, Definition.Fields["SupplierTIN"], HorizontalAlignment.Stretch);
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_baseInvSubMissionDate = AddDateControl(container, Definition.Fields["BaseDate"]);
		LayoutGroup container2 = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Horizontal);
		_baseTaxInvoiceSeries = AddStringControl(container2, Definition.Fields["BaseSeries"]);
		_baseTaxInvoiceNumber = AddStringControl(container2, Definition.Fields["BaseNumber"]);
		_submissionDate = AddDateControl(container, Definition.Fields["SubMissionDate"]);
		_taxInvForm = AddNumericControl(container, Definition.Fields["BehalfOf"]);
		_taxInvForm.ControlVisibility = Visibility.Collapsed;
		_taxInvFormName = AddStringControl(container, ArmSoft.SME.Common.Resources.TaxInvoicesForm, isRequired: false, 19);
		if (!PO.Type.InList(Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocGoods.Value), Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocServices.Value), Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Goods.Value), Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Services.Value)))
		{
			((LayoutItemWithRequired)(object)_taxInvForm.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroup container3 = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Horizontal);
		_taxInvoiceSeries = AddStringControl(container3, Definition.Fields["Series"]);
		_taxInvoiceNumber = AddStringControl(container3, Definition.Fields["TaxNumber"]);
		_comment = AddStringControl(container, Definition.Fields["AdditionalInfo"], HorizontalAlignment.Stretch);
		_contract = AddStringControl(container, Definition.Fields["DealNumber"]);
		_contractDate = AddDateControl(container, Definition.Fields["DealDate"]);
		_signed = AddBooleanControl(container, Definition.Fields["IsSigned"]);
		LayoutGroup container4 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.Supplier, "Supplier");
		_supplierAddress = AddStringControl(container4, Definition.Fields["SupplierAddress"], HorizontalAlignment.Stretch);
		_supplierBusinessAddress = AddStringControl(container4, Definition.Fields["SupplierBusinessAddress"], HorizontalAlignment.Stretch);
		_supplierSettlAcc = AddDropDownControl(container4, Definition.Fields["SupplierBankAccountNumber"], requiredService.CreateDropDown(DropDownTypeSE.PartnerAccountInBanks), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_supplierSettlAcc.CheckValue = false;
		_supplierBankName = AddStringControl(container4, Definition.Fields["SupplierBankName"], HorizontalAlignment.Stretch);
		_supplierAdditionalData = AddStringControl(container4, Definition.Fields["SupplierAdditionalData"], HorizontalAlignment.Stretch);
		LayoutGroup container5 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.Buyer, "Buyer");
		_buyerTaxCode = AddStringControl(container5, Definition.Fields["BuyerTIN"], HorizontalAlignment.Stretch);
		_buyerPassport = AddStringControl(container5, Definition.Fields["BuyerIdentityNo"], HorizontalAlignment.Stretch);
		_buyerName = AddStringControl(container5, Definition.Fields["BuyerName"], HorizontalAlignment.Stretch);
		_buyerAddress = AddStringControl(container5, Definition.Fields["BuyerAddress"], HorizontalAlignment.Stretch);
		_buyerBusinessAddress = AddStringControl(container5, Definition.Fields["BuyerBusinessAddress"], HorizontalAlignment.Stretch);
		_buyerSettlAcc = AddDropDownControl(container5, Definition.Fields["BuyerBankAccountNumber"], requiredService.CreateDropDown(DropDownTypeSE.AccountInBanks), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_buyerSettlAcc.CheckValue = false;
		_buyerBankName = AddStringControl(container5, Definition.Fields["BuyerBankName"], HorizontalAlignment.Stretch);
		_buyerAdditionalData = AddStringControl(container5, Definition.Fields["BuyerAdditionalData"], HorizontalAlignment.Stretch);
		_buyerShippingMethod = AddStringControl(container5, Definition.Fields["DeliveryMethod"], HorizontalAlignment.Stretch);
		_mediator = AddStringControl(container5, Definition.Fields["MediatorName"], HorizontalAlignment.Stretch);
		_mediatorLicense = AddStringControl(container5, Definition.Fields["MediatorLicenseNumber"], HorizontalAlignment.Stretch);
		_mediatorLicenseDate = AddDateControl(container5, Definition.Fields["MediatorLicenseDate"]);
		LayoutGroup container6 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.ItemList, "Pricelist");
		_grid = AddGrid(container6, Definition.Grids["Items"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_grid.Name = "TaxDocumentGrid";
		_grid.AllowAdd = false;
		_grid.AllowDelete = false;
		_grid.AllowDuplicate = false;
		_taxDocumentTableView = _grid.View;
		_grid.ToolTip = ArmSoft.SME.Common.Resources.ItemList;
		DocumentGridColumn documentGridColumn = _grid.AddDropDownColumn(Definition.Grids["Items"].Columns["ItemType"]);
		documentGridColumn.Visible = false;
		documentGridColumn.ShowInColumnChooser = false;
		_itemTypeNameCol = _grid.AddDropDownColumn(Definition.Grids["Items"].Columns["ItemTypeName"], trimValue: false, GlyphKind.Regular);
		_itemTypeNameCol.Width = 55.0;
		_itemTypeNameCol.Visible = false;
		((ButtonEditSettings)_itemTypeNameCol.EditSettings).IsTextEditable = false;
		_itemCodeCol = _grid.AddDropDownColumn(Definition.Grids["Items"].Columns["ItemCode"], trimValue: true, GlyphKind.DropDown, allowAdditionButtons: true);
		_itemCodeCol.Width = 125.0;
		_itemCodeCol.Visible = false;
		_itemCodeCol.ReadOnly = true;
		_itemNameCol = _grid.AddStringColumn(Definition.Grids["Items"].Columns["ItemNameFromDirectory"]);
		_itemNameCol.ReadOnly = true;
		_itemNameCol.Visible = false;
		_unitBriefCol = _grid.AddDropDownColumn(Definition.Grids["Items"].Columns["UnitBriefFromDirectory"]);
		_unitBriefCol.Visible = false;
		_unitBriefCol.ReadOnly = true;
		_itemExternalCodeCol = _grid.AddStringColumn(Definition.Grids["Items"].Columns["ItemExternalCode"]);
		_itemExternalCodeCol.ReadOnly = true;
		_itemExternalCodeCol.Visible = false;
		_itemExternalCodeCol.ShowInColumnChooser = false;
		_itemExternalNameCol = _grid.AddStringColumn(Definition.Grids["Items"].Columns["ItemName"]);
		_itemExternalNameCol.ReadOnly = true;
		_grid.AddStringColumn(Definition.Grids["Items"].Columns["UnitBrief"]).ReadOnly = true;
		bool visible = ((!PO.FromNewInvoicing) ? (PO.Type == Convert.ToInt16(TaxAccAndInvoiceOldDocsTypeSE.Excise.Value)) : (PO.Type == Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Excise.Value)));
		DocumentGridColumn documentGridColumn2 = _grid.AddNumberColumn(Definition.Grids["Items"].Columns["Volume"]);
		documentGridColumn2.Visible = visible;
		documentGridColumn2.ShowInColumnChooser = documentGridColumn2.Visible;
		documentGridColumn2.ReadOnly = true;
		_grid.AddNumberColumn(Definition.Grids["Items"].Columns["Quantity"], showTotal: true).ReadOnly = true;
		_grid.AddNumberColumn(Definition.Grids["Items"].Columns["Price"]).ReadOnly = true;
		_grid.AddNumberColumn(Definition.Grids["Items"].Columns["Discount"]).ReadOnly = true;
		_grid.AddNumberColumn(Definition.Grids["Items"].Columns["Sum"], showTotal: true).ReadOnly = true;
		DocumentGridColumn documentGridColumn3 = _grid.AddNumberColumn(Definition.Grids["Items"].Columns["TarePrice"]);
		documentGridColumn3.Visible = visible;
		documentGridColumn3.ShowInColumnChooser = documentGridColumn3.Visible;
		documentGridColumn3.ReadOnly = true;
		DocumentGridColumn documentGridColumn4 = _grid.AddNumberColumn(Definition.Grids["Items"].Columns["ExciseRate"]);
		documentGridColumn4.Visible = visible;
		documentGridColumn4.ShowInColumnChooser = documentGridColumn4.Visible;
		documentGridColumn4.ReadOnly = true;
		DocumentGridColumn documentGridColumn5 = _grid.AddNumberColumn(Definition.Grids["Items"].Columns["Excise"]);
		documentGridColumn5.Visible = visible;
		documentGridColumn5.ShowInColumnChooser = documentGridColumn5.Visible;
		documentGridColumn5.ReadOnly = true;
		_grid.AddNumberColumn(Definition.Grids["Items"].Columns["VAT"]).ReadOnly = true;
		_grid.AddNumberColumn(Definition.Grids["Items"].Columns["SumVAT"], showTotal: true).ReadOnly = true;
		_dealTypeCol = _grid.AddDropDownColumn("DealTypeCaption", ArmSoft.SME.Common.Resources.DealType, 1, trimValue: false);
		_dealTypeCol.ReadOnly = true;
		_dealTypeCol.ShowInColumnChooser = false;
		_grid.AddNumberColumn(Definition.Grids["Items"].Columns["TotalSum"], showTotal: true).ReadOnly = true;
		DocumentGridColumn documentGridColumn6 = _grid.AddNumberColumn("RowId", "RowId", 16, 0, allowNegativeNumbers: true);
		documentGridColumn6.Visible = false;
		documentGridColumn6.ShowInColumnChooser = false;
		_cpaCodeCol = _grid.AddStringColumn(Definition.Grids["Items"].Columns["CPACode"]);
		_cpaCodeCol.ReadOnly = true;
		_cpaCodeFromDirectoryCol = _grid.AddStringColumn(Definition.Grids["Items"].Columns["CPACodeFromDirectory"]);
		_cpaCodeFromDirectoryCol.ReadOnly = true;
		_cpaCodeFromDirectoryCol.Visible = false;
		_grid.AddStringColumn(Definition.Grids["Items"].Columns["RowISN"]).Visible = PO.SubmittedDoc;
		_receivedTaxInvoiceList = new ObservableCollection<SpecificationRow>();
		_grid.ItemsSource = _receivedTaxInvoiceList;
		_summaVAT = AddNumericControl(container6, Definition.Fields["TotalVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summa = AddNumericControl(container6, Definition.Fields["TotalSum"], useCurrencyFormat: true, HorizontalAlignment.Right);
	}

	protected override void SetControlsReadonly()
	{
		_suppDate.IsReadOnly = true;
		_supplierName.IsReadOnly = true;
		_supplierTaxCode.IsReadOnly = true;
		_baseInvSubMissionDate.IsReadOnly = true;
		_baseTaxInvoiceSeries.IsReadOnly = true;
		_baseTaxInvoiceNumber.IsReadOnly = true;
		_taxInvFormName.IsReadOnly = true;
		_submissionDate.IsReadOnly = true;
		_taxInvForm.IsReadOnly = true;
		_taxInvoiceSeries.IsReadOnly = true;
		_taxInvoiceNumber.IsReadOnly = true;
		_comment.IsReadOnly = true;
		_contract.IsReadOnly = true;
		_contractDate.IsReadOnly = true;
		_signed.IsReadOnly = true;
		_supplierAddress.IsReadOnly = true;
		_supplierBusinessAddress.IsReadOnly = true;
		_supplierSettlAcc.IsReadOnly = true;
		_supplierBankName.IsReadOnly = true;
		_supplierAdditionalData.IsReadOnly = true;
		_buyerTaxCode.IsReadOnly = true;
		_buyerPassport.IsReadOnly = true;
		_buyerName.IsReadOnly = true;
		_buyerAddress.IsReadOnly = true;
		_buyerBusinessAddress.IsReadOnly = true;
		_buyerSettlAcc.IsReadOnly = true;
		_buyerBankName.IsReadOnly = true;
		_buyerAdditionalData.IsReadOnly = true;
		_buyerShippingMethod.IsReadOnly = true;
		_mediator.IsReadOnly = true;
		_mediatorLicense.IsReadOnly = true;
		_mediatorLicenseDate.IsReadOnly = true;
		if (_viewMode == WindowViewMode.View)
		{
			_grid.IsReadOnly = true;
		}
		_summaVAT.IsReadOnly = true;
		_summa.IsReadOnly = true;
		if (string.IsNullOrWhiteSpace(_taxInvFormName.Text))
		{
			_taxInvFormName.ControlVisibility = Visibility.Collapsed;
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_suppDate.EditValue = PO.SupplyDate;
		_taxInvFormName.EditValue = EncodeDecodeTypes.GetBehalfOfCaption((BehalfOfType)PO.BehalfOf);
		_receivedTaxInvoiceList.Clear();
		foreach (TaxDocument.SpecificationRow item in PO.Items)
		{
			_receivedTaxInvoiceList.Add(new SpecificationRow
			{
				ItemType = item.ItemType,
				ItemTypeName = item.ItemTypeName,
				ItemCode = item.ItemCode,
				ItemID = item.ItemID,
				UnitCodeFromDirectory = item.UnitCodeFromDirectory,
				ItemExternalCode = item.ItemExternalCode,
				ItemName = item.ItemName,
				UnitID = item.UnitID,
				UnitBrief = item.UnitBrief,
				Volume = item.Volume,
				Quantity = item.Quantity,
				Price = item.Price,
				Discount = item.Discount,
				Sum = item.Sum,
				TarePrice = item.TarePrice,
				ExciseRate = item.ExciseRate,
				Excise = item.Excise,
				VAT = item.VAT,
				isAccDoc = (PO.TotalVAT == 0m),
				DealType = item.DealType,
				SumVAT = item.SumVAT,
				TotalSum = item.TotalSum,
				CPACode = item.CPACode,
				RowNum = item.RowNum,
				RowISN = Convert.ToString(item.RowISN),
				ItemNameFromDirectory = item.ItemNameFromDirectory,
				UnitBriefFromDirectory = item.UnitBriefFromDirectory,
				CPACodeFromDirectory = item.CPACodeFromDirectory
			});
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_grid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_grid.RestoreSettings(rk);
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		if (_ribbonPageOther != null)
		{
			_ribbonPageOther.IsVisible = false;
		}
		if (_ribbonPageOther != null)
		{
			_ribbonPageOther.IsVisible = false;
		}
	}
}
