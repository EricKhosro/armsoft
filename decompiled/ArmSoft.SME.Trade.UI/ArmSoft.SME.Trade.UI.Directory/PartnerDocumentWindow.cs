using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Directories.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.ReportViewers.Partner;
using ArmSoft.SME.Trade.UI.Services;
using ArmSoft.b2b.V2;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Ribbon;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryDocumentWindow(2)]
public class PartnerDocumentWindow : DirectoryDocumentWindow<Partner>
{
	public class PartnerContractRow : DocumentGridRow
	{
		[ObservableProperty]
		private string contract;

		[ObservableProperty]
		private string caption;

		[ObservableProperty]
		private string currCode;

		[ObservableProperty]
		private string priceType;

		[ObservableProperty]
		private decimal limit;

		[ObservableProperty]
		private decimal percent;

		[ObservableProperty]
		private string discountCard;

		[ObservableProperty]
		private bool isBonusCard;

		[ObservableProperty]
		private decimal bonus;

		[ObservableProperty]
		private decimal point;

		[ObservableProperty]
		private bool controlReceivables;

		[ObservableProperty]
		private decimal receivablesLimit;

		[ObservableProperty]
		private bool isDefault;

		[ObservableProperty]
		private DateOnly? contractDate;

		[ObservableProperty]
		private string description;

		[ObservableProperty]
		private DateOnly? bonusCardValidPeriodStart;

		[ObservableProperty]
		private DateOnly? bonusCardValidPeriodEnd;

		[ObservableProperty]
		private bool bonusCardCanceled;

		[ObservableProperty]
		private bool exportedToDiscountServer;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Contract
		{
			get
			{
				return contract;
			}
			[MemberNotNull("contract")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(contract, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Contract);
					contract = value;
					OnContractChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Contract);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Caption
		{
			get
			{
				return caption;
			}
			[MemberNotNull("caption")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(caption, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Caption);
					caption = value;
					OnCaptionChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Caption);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string CurrCode
		{
			get
			{
				return currCode;
			}
			[MemberNotNull("currCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(currCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrCode);
					currCode = value;
					OnCurrCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string PriceType
		{
			get
			{
				return priceType;
			}
			[MemberNotNull("priceType")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(priceType, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PriceType);
					priceType = value;
					OnPriceTypeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PriceType);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Limit
		{
			get
			{
				return limit;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(limit, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Limit);
					limit = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Limit);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Percent
		{
			get
			{
				return percent;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(percent, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Percent);
					percent = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Percent);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string DiscountCard
		{
			get
			{
				return discountCard;
			}
			[MemberNotNull("discountCard")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(discountCard, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DiscountCard);
					discountCard = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DiscountCard);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsBonusCard
		{
			get
			{
				return isBonusCard;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isBonusCard, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsBonusCard);
					isBonusCard = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsBonusCard);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Bonus
		{
			get
			{
				return bonus;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(bonus, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Bonus);
					bonus = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Bonus);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Point
		{
			get
			{
				return point;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(point, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Point);
					point = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Point);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool ControlReceivables
		{
			get
			{
				return controlReceivables;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(controlReceivables, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ControlReceivables);
					controlReceivables = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ControlReceivables);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal ReceivablesLimit
		{
			get
			{
				return receivablesLimit;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(receivablesLimit, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ReceivablesLimit);
					receivablesLimit = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ReceivablesLimit);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsDefault
		{
			get
			{
				return isDefault;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isDefault, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsDefault);
					isDefault = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsDefault);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public DateOnly? ContractDate
		{
			get
			{
				return contractDate;
			}
			set
			{
				if (!EqualityComparer<DateOnly?>.Default.Equals(contractDate, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ContractDate);
					contractDate = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ContractDate);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Description
		{
			get
			{
				return description;
			}
			[MemberNotNull("description")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(description, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Description);
					description = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Description);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public DateOnly? BonusCardValidPeriodStart
		{
			get
			{
				return bonusCardValidPeriodStart;
			}
			set
			{
				if (!EqualityComparer<DateOnly?>.Default.Equals(bonusCardValidPeriodStart, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BonusCardValidPeriodStart);
					bonusCardValidPeriodStart = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BonusCardValidPeriodStart);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public DateOnly? BonusCardValidPeriodEnd
		{
			get
			{
				return bonusCardValidPeriodEnd;
			}
			set
			{
				if (!EqualityComparer<DateOnly?>.Default.Equals(bonusCardValidPeriodEnd, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BonusCardValidPeriodEnd);
					bonusCardValidPeriodEnd = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BonusCardValidPeriodEnd);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool BonusCardCanceled
		{
			get
			{
				return bonusCardCanceled;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(bonusCardCanceled, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BonusCardCanceled);
					bonusCardCanceled = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BonusCardCanceled);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool ExportedToDiscountServer
		{
			get
			{
				return exportedToDiscountServer;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(exportedToDiscountServer, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ExportedToDiscountServer);
					exportedToDiscountServer = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ExportedToDiscountServer);
				}
			}
		}

		static PartnerContractRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(PartnerContractRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"Contract",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateContract((PartnerContractRow)row, info);
					}
				},
				{
					"Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCaption((PartnerContractRow)row, info);
					}
				},
				{
					"CurrCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCurrCode((PartnerContractRow)row, info);
					}
				},
				{
					"PriceType",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidatePriceType((PartnerContractRow)row, info);
					}
				},
				{
					"ReceivablesLimit",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateReceivablesLimit((PartnerContractRow)row, info);
					}
				}
			});
		}

		private static void ValidateContract(PartnerContractRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.Contract, info, TradeAllDirectoryDefinitions.Partner.Grids["PartnerContracts"].Columns["Contract"]);
		}

		private static void ValidateCaption(PartnerContractRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.Caption, info, TradeAllDirectoryDefinitions.Partner.Grids["PartnerContracts"].Columns["Caption"]);
		}

		private static void ValidateCurrCode(PartnerContractRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateCurr(row.CurrCode, info, TradeAllDirectoryDefinitions.Partner.Grids["PartnerContracts"].Columns["CurrCode"]);
		}

		private static void ValidatePriceType(PartnerContractRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.CurrCode))
			{
				TradePropertyValidator.ValidatePriceType(row.PriceType, info, TradeAllDirectoryDefinitions.Partner.Grids["PartnerContracts"].Columns["PriceType"]);
			}
		}

		private static void ValidateReceivablesLimit(PartnerContractRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.ReceivablesLimit, info, ArmSoft.SME.Trade.Common.Resources.ReceivablesLimit);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnContractChanged(string value)
		{
			ResetForceValidationBit("Contract");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCaptionChanged(string value)
		{
			ResetForceValidationBit("Caption");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCurrCodeChanged(string value)
		{
			ResetForceValidationBit("CurrCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPriceTypeChanged(string value)
		{
			ResetForceValidationBit("PriceType");
		}
	}

	public class PartnerSettlementAccount : DocumentGridRow
	{
		[ObservableProperty]
		private string settlAccount;

		[ObservableProperty]
		private string caption;

		[ObservableProperty]
		private bool isDefault;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string SettlAccount
		{
			get
			{
				return settlAccount;
			}
			[MemberNotNull("settlAccount")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(settlAccount, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SettlAccount);
					settlAccount = value;
					OnSettlAccountChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SettlAccount);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Caption
		{
			get
			{
				return caption;
			}
			[MemberNotNull("caption")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(caption, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Caption);
					caption = value;
					OnCaptionChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Caption);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsDefault
		{
			get
			{
				return isDefault;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isDefault, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsDefault);
					isDefault = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsDefault);
				}
			}
		}

		static PartnerSettlementAccount()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(PartnerSettlementAccount), cacheValidationResults: false, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"SettlAccount",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSettlAccount((PartnerSettlementAccount)row, info);
					}
				},
				{
					"Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCaption((PartnerSettlementAccount)row, info);
					}
				}
			});
		}

		private static void ValidateSettlAccount(PartnerSettlementAccount row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.SettlAccount, info, ArmSoft.SME.Common.Resources.Account);
		}

		private static void ValidateCaption(PartnerSettlementAccount row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.Caption, info, ArmSoft.SME.Common.Resources.Name);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSettlAccountChanged(string value)
		{
			ResetForceValidationBit("SettlAccount");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCaptionChanged(string value)
		{
			ResetForceValidationBit("Caption");
		}
	}

	public class PartnerBusinessAddressRow : DocumentGridRow
	{
		[ObservableProperty]
		private string caption;

		[ObservableProperty]
		private bool isDefault;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Caption
		{
			get
			{
				return caption;
			}
			[MemberNotNull("caption")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(caption, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Caption);
					caption = value;
					OnCaptionChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Caption);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsDefault
		{
			get
			{
				return isDefault;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isDefault, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsDefault);
					isDefault = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsDefault);
				}
			}
		}

		static PartnerBusinessAddressRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(PartnerBusinessAddressRow), cacheValidationResults: false, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>> { 
			{
				"Caption",
				(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
				{
					ValidateCaption((PartnerBusinessAddressRow)row, info);
				}
			} });
		}

		private static void ValidateCaption(PartnerBusinessAddressRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.Caption, info, ArmSoft.SME.Common.Resources.BusinessAddresses);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCaptionChanged(string value)
		{
			ResetForceValidationBit("Caption");
		}
	}

	private ButtonEditExt _taxCode;

	private ButtonInfo _b2bButton;

	private ButtonInfo _b2bTaxCodeButton;

	private ButtonEditExt _name;

	private ButtonInfo _b2bCaptionButton;

	private TextEditExt _fullCaption;

	private DropDownControl _type;

	private CheckEditExt _supplier;

	private CheckEditExt _customer;

	private TextEditExt _address;

	private TextEditExt _businessAddress;

	private TextEditExt _managerFIO;

	private ComboBoxEditExt _managerPost;

	private TextEditExt _chAccFIO;

	private ComboBoxEditExt _chAccPost;

	private TextEditExt _partStateRegister;

	private DropDownControl _idDocType;

	private TextEditExt _passportNum;

	private DropDownControl _sex;

	private DateEditControl _birthDate;

	private TextEditExt _mainAim;

	private TextEditExt _phone;

	private TextEditExt _email;

	private DocumentGridControl _contractsGrid;

	private TableView _contractsTableView;

	private ObservableCollection<PartnerContractRow> _contractsList;

	private DocumentGridControl _settlementAccountsGrid;

	private TableView _settlementAccountsTableView;

	private ObservableCollection<PartnerSettlementAccount> _settlementAccountsList;

	private DocumentGridControl _businessAddressesGrid;

	private TableView _businessAddressesTableView;

	private ObservableCollection<PartnerBusinessAddressRow> _businessAddressesList;

	private DocumentGridColumn _contractCodeColumn;

	private DocumentGridColumn _contractCurrCodeColumn;

	private DocumentGridColumn _contractPriceTypeColumn;

	private DocumentGridColumn _isBonusCardColumn;

	private DocumentGridColumn _bonusColumn;

	private DocumentGridColumn _controlReceivablesColumn;

	private DocumentGridColumn _contractValidPeriodStartColumn;

	private DocumentGridColumn _contractValidPeriodEndColumn;

	private DocumentGridColumn _contractCanceledColumn;

	private DocumentGridColumn _receivablesLimitColumn;

	private DocumentGridColumn _defaultContractColumn;

	private DocumentGridColumn _settlementAccountColumn;

	private DocumentGridColumn _defaultSettlAccColumn;

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

	private readonly bool _useBonus = Settings.ParametersService.UseBonus();

	protected RibbonPageGroup _ribbonPageGroupSubDirectories;

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	protected override double WidthValue => 700.0;

	protected override double HeightValue => 450.0;

	protected override double MinWidthValue => 430.0;

	protected override bool WriteInHistory => true;

	public PartnerDocumentWindow()
	{
	}

	public PartnerDocumentWindow(Partner doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		base.ConstructUI();
		_code = AddButtonEditControl(null, Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_taxCode = AddButtonEditControl(null, Definition.Fields["TaxCode"], 1);
		ImageSource image = ImageService.GetImage("cloud_14_gray.svg");
		DXImage content = new DXImage
		{
			Source = image,
			Width = 14.0,
			Height = 14.0,
			Margin = new Thickness(-2.0, 0.0, -2.0, 0.0)
		};
		_b2bTaxCodeButton = new ButtonInfo
		{
			GlyphKind = GlyphKind.User,
			Content = content,
			ToolTip = ArmSoft.SME.Common.Resources.LoadPartnerFromTIN + " (Ctrl+Shift+M)"
		};
		_taxCode.Buttons.Add(_b2bTaxCodeButton);
		_taxCode.PreviewKeyDown += new SafeDocumentEventHandler<KeyEventArgs>(TaxCodeButton_PreviewKeyDown, this).Handler;
		_b2bTaxCodeButton.Click += new SafeDocumentEventHandler<RoutedEventArgs>(B2bTaxCodeButton_Click, this).Handler;
		_b2bButton = new ButtonInfo
		{
			GlyphKind = GlyphKind.Search,
			ToolTip = ArmSoft.SME.Common.Resources.ViewOrganizationFromB2B
		};
		_taxCode.Buttons.Add(_b2bButton);
		_b2bButton.Click += new SafeDocumentEventHandler<RoutedEventArgs>(B2bButton_Click, this).Handler;
		_name = AddButtonEditControl(null, Definition.Fields["Name"], 1, HorizontalAlignment.Stretch);
		_name.PreviewKeyDown += new SafeDocumentEventHandler<KeyEventArgs>(Caption_PreviewKeyDow, this).Handler;
		_name.EditValueChanging += new SafeDocumentEventHandler<EditValueChangingEventArgs>(Caption_EditValueChanging, this).Handler;
		image = ImageService.GetImage("cloud_14_gray.svg");
		content = new DXImage
		{
			Source = image,
			Width = 14.0,
			Height = 14.0,
			Margin = new Thickness(-2.0, 0.0, -2.0, 0.0)
		};
		_b2bCaptionButton = new ButtonInfo
		{
			GlyphKind = GlyphKind.User,
			Content = content,
			ToolTip = ArmSoft.SME.Common.Resources.LoadPartnerFromName + " (Ctrl+Shift+M)"
		};
		_name.Buttons.Add(_b2bCaptionButton);
		_b2bCaptionButton.Click += new SafeDocumentEventHandler<RoutedEventArgs>(B2bCaptionButton_Click, this).Handler;
		_fullCaption = AddStringControl(null, Definition.Fields["FullCaption"], HorizontalAlignment.Stretch);
		_type = AddDropDownControl(null, Definition.Fields["Type"], new PartnerGroups(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_supplier = AddBooleanControl(container, Definition.Fields["Supplier"]);
		_customer = AddBooleanControl(container, Definition.Fields["Customer"]);
		_address = AddStringControl(container, Definition.Fields["Address"], HorizontalAlignment.Stretch);
		_businessAddress = AddStringControl(container, Definition.Fields["BusinessAddress"], HorizontalAlignment.Stretch);
		_phone = AddStringControl(container, Definition.Fields["Phone"], HorizontalAlignment.Stretch);
		_email = AddStringControl(container, Definition.Fields["Email"], HorizontalAlignment.Stretch);
		_managerFIO = AddStringControl(container, Definition.Fields["ManagerFIO"], HorizontalAlignment.Stretch);
		_managerPost = AddComboBoxControl(container, Definition.Fields["ManagerPost"], SmartEnum<ManagerPositionsSE, string>.List.Select((ManagerPositionsSE p) => p.Caption), HorizontalAlignment.Stretch);
		_chAccFIO = AddStringControl(container, Definition.Fields["ChiefaccFIO"], HorizontalAlignment.Stretch);
		_chAccPost = AddComboBoxControl(container, Definition.Fields["ChiefaccPost"], SmartEnum<ChiefAccPositionsSE, string>.List.Select((ChiefAccPositionsSE p) => p.Caption), HorizontalAlignment.Stretch);
		if (Settings.Localization == Localizations.Armenia)
		{
			_partStateRegister = AddStringControl(container, Definition.Fields["PartStateRegister"], HorizontalAlignment.Stretch);
		}
		_idDocType = AddSmartEnumDropDownControl(container, Definition.Fields["IDDocType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_passportNum = AddStringControl(container, Definition.Fields["Passport"], HorizontalAlignment.Stretch);
		_sex = AddSmartEnumDropDownControl(container, Definition.Fields["Sex"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_birthDate = AddDateControl(container, Definition.Fields["Birthday"], useLongDate: true);
		_mainAim = AddStringControl(container, Definition.Fields["MainAIM"], HorizontalAlignment.Stretch);
		LayoutGroup container2 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.Contracts, "Contracts");
		_contractsGrid = AddGrid(container2, Definition.Grids["PartnerContracts"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_contractsTableView = _contractsGrid.View;
		_contractsTableView.CellValueChanging += new SafeDocumentEventHandler<CellValueChangedEventArgs>(ContractsTableView_CellValueChanging, this).Handler;
		_contractsTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(ContractsTableView_CellValueChanged, this).Handler;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(ContractsTableView_FocusedRowHandleChanged, this);
		_contractsTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_contractCodeColumn = _contractsGrid.AddStringColumn(Definition.Grids["PartnerContracts"].Columns["Contract"]);
		_contractCodeColumn.Width = 50.0;
		_contractsGrid.AddStringColumn(Definition.Grids["PartnerContracts"].Columns["Caption"]).Width = 250.0;
		_contractCurrCodeColumn = _contractsGrid.AddDropDownColumn(Definition.Grids["PartnerContracts"].Columns["CurrCode"], new ArmSoft.SME.Core.UI.DropDowns.Currency(shortMode: true));
		_contractPriceTypeColumn = _contractsGrid.AddDropDownColumn(Definition.Grids["PartnerContracts"].Columns["PriceType"], new PriceTypes());
		_contractsGrid.AddDateColumn(Definition.Grids["PartnerContracts"].Columns["ContractDate"]);
		_contractsGrid.AddStringColumn(Definition.Grids["PartnerContracts"].Columns["Description"]).Width = 200.0;
		_contractsGrid.AddNumberColumn(Definition.Grids["PartnerContracts"].Columns["Percent"]);
		_contractsGrid.AddStringColumn(Definition.Grids["PartnerContracts"].Columns["DiscountCard"]);
		if (_useBonus)
		{
			_isBonusCardColumn = _contractsGrid.AddBooleanColumn(Definition.Grids["PartnerContracts"].Columns["IsBonusCard"]);
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				_bonusColumn = _contractsGrid.AddNumberColumn(Definition.Grids["PartnerContracts"].Columns["Bonus"]);
			}
			else
			{
				_bonusColumn = _contractsGrid.AddNumberColumn(Definition.Grids["PartnerContracts"].Columns["Point"]);
			}
		}
		_contractValidPeriodStartColumn = _contractsGrid.AddDateColumn(Definition.Grids["PartnerContracts"].Columns["BonusCardValidPeriodStart"]);
		_contractValidPeriodEndColumn = _contractsGrid.AddDateColumn(Definition.Grids["PartnerContracts"].Columns["BonusCardValidPeriodEnd"]);
		_contractCanceledColumn = _contractsGrid.AddBooleanColumn(Definition.Grids["PartnerContracts"].Columns["BonusCardCanceled"]);
		_controlReceivablesColumn = _contractsGrid.AddBooleanColumn(Definition.Grids["PartnerContracts"].Columns["ControlReceivables"]);
		_receivablesLimitColumn = _contractsGrid.AddNumberColumn(Definition.Grids["PartnerContracts"].Columns["ReceivablesLimit"]);
		_defaultContractColumn = _contractsGrid.AddBooleanColumn(Definition.Grids["PartnerContracts"].Columns["IsDefault"]);
		_defaultContractColumn.Width = 100.0;
		_contractsGrid.View.AutoWidth = true;
		_contractsGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(ContractsGrid_ClickDropDown, this).Handler;
		_contractsGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(ContractsGrid_AddingRow, this).Handler;
		_contractsGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(ContractsGrid_DuplicatingRow, this).Handler;
		_contractsList = new ObservableCollection<PartnerContractRow>();
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.SettlementAccounts, "SettlementAccounts");
		_settlementAccountsGrid = AddGrid(container3, Definition.Grids["SettlementAccounts"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_settlementAccountsGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(SettlementAccountsGrid_AddingRow, this).Handler;
		_settlementAccountsGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(SettlementAccountsGrid_DuplicatingRow, this).Handler;
		_settlementAccountsTableView = _settlementAccountsGrid.View;
		_settlementAccountsTableView.CellValueChanging += new SafeDocumentEventHandler<CellValueChangedEventArgs>(SettlementAccountsTableView_CellValueChanging, this).Handler;
		_settlementAccountsTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(SettlementAccountsTableView_CellValueChanged, this).Handler;
		_settlementAccountColumn = _settlementAccountsGrid.AddStringColumn(Definition.Grids["SettlementAccounts"].Columns["SettlAccount"]);
		_settlementAccountColumn.Width = 150.0;
		_settlementAccountsGrid.AddStringColumn(Definition.Grids["SettlementAccounts"].Columns["Caption"]).Width = 250.0;
		_defaultSettlAccColumn = _settlementAccountsGrid.AddBooleanColumn(Definition.Grids["SettlementAccounts"].Columns["IsDefault"]);
		_defaultSettlAccColumn.Width = 100.0;
		_settlementAccountsList = new ObservableCollection<PartnerSettlementAccount>();
		LayoutGroup container4 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.AdditionalBusAddrList, "AdditionalBusAddresses");
		_businessAddressesGrid = AddGrid(container4, Definition.Grids["PartnerBusinessAddresses"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_businessAddressesTableView = _businessAddressesGrid.View;
		_businessAddressesGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(BusinessAddressesGrid_AddingRow, this).Handler;
		_businessAddressesGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(BusinessAddressesGrid_DuplicatingRow, this).Handler;
		_businessAddressesGrid.AddStringColumn(Definition.Grids["PartnerBusinessAddresses"].Columns["Caption"]).Width = 500.0;
		_businessAddressesList = new ObservableCollection<PartnerBusinessAddressRow>();
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonPartnerChangeRequest.ConvertForToolbar("t_import_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemReports.ConvertForToolbar("t_report_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_journal_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemSubDirectories.ConvertForToolbar("t_table_16.svg"));
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
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.Partners, showDialog: false, ArmSoft.SME.Common.Resources.JumpToDirectory, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barSubItemReports.Content = "";
		_barSubItemReports.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemReports.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("journal_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("journal_32.svg");
		_barButtonSalesAnalysis = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis.Value, showDialog: true, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
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
		_barButtonPartnerRemains = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartRemains, showDialog: true, ArmSoft.SME.Trade.Common.Resources.PartnerRemainsShort);
		_barButtonPartnerRemains.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemReports);
		_barButtonPartnerTurn = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartnersTurnOver, showDialog: true, ArmSoft.SME.Trade.Common.Resources.PartnerTurnShort);
		_barButtonPartnerTurn.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemReports);
		_barButtonPartnerOpJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, showDialog: true, ArmSoft.SME.Trade.Common.Resources.PartnerOpJournalShort);
		_barButtonPartnerOpJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemReports);
		_barButtonDocumentsJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: true, ArmSoft.SME.Common.Resources.Documents, null, "journal_16.svg", "journal_32.svg");
		_barButtonDocumentsJournal.AddInBarSubItem(ref _ribbonPageGroupJournals, ref _barSubItemJournals);
		_barButtonPartAccInBank = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartnerAccountsInBank, showDialog: false, ArmSoft.SME.Common.Resources.SettlementAccounts, null, "table_16.svg", "table_32.svg");
		_barButtonPartAccInBank.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonPartContracts = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartContracts, showDialog: false, ArmSoft.SME.Common.Resources.Contracts);
		_barButtonPartContracts.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonPartnersMT = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartnersMT, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnersMTDirectory);
		_barButtonPartnersMT.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonPartnerRemains = _barButtonPartnerRemains;
		int num = 1;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonPartnerRemains.PropertyWithValues = list;
		ReportBarButtonItem barButtonPartnerTurn = _barButtonPartnerTurn;
		num = 1;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		CollectionsMarshal.AsSpan(list2)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonPartnerTurn.PropertyWithValues = list2;
		ReportBarButtonItem barButtonPartnerOpJournal = _barButtonPartnerOpJournal;
		num = 3;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list3);
		span[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		span[1] = new PropertyWithValue("StartDate", Settings.HostingEnvironment.DateBegin.ToString());
		span[2] = new PropertyWithValue("EndDate", Settings.HostingEnvironment.DateEnd.ToString());
		barButtonPartnerOpJournal.PropertyWithValues = list3;
		ReportBarButtonItem barButtonDocumentsJournal = _barButtonDocumentsJournal;
		num = 4;
		List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list4);
		span2[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		span2[1] = new PropertyWithValue("StartDate", Settings.HostingEnvironment.DateBegin.ToString());
		span2[2] = new PropertyWithValue("EndDate", Settings.HostingEnvironment.DateEnd.ToString());
		span2[3] = new PropertyWithValue("DocOwner", Settings.CoreCacheService.User.GetNameById(Settings.LoginService.UserId));
		barButtonDocumentsJournal.PropertyWithValues = list4;
		ReportBarButtonItem barButtonPartAccInBank = _barButtonPartAccInBank;
		num = 1;
		List<PropertyWithValue> list5 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list5, num);
		CollectionsMarshal.AsSpan(list5)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonPartAccInBank.PropertyWithValues = list5;
		ReportBarButtonItem barButtonPartContracts = _barButtonPartContracts;
		num = 1;
		List<PropertyWithValue> list6 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list6, num);
		CollectionsMarshal.AsSpan(list6)[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		barButtonPartContracts.PropertyWithValues = list6;
		ReportBarButtonItem barButtonPartnersMT = _barButtonPartnersMT;
		num = 1;
		List<PropertyWithValue> list7 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list7, num);
		CollectionsMarshal.AsSpan(list7)[0] = new PropertyWithValue("PartCode", PO.Code.ToString());
		barButtonPartnersMT.PropertyWithValues = list7;
		ReportBarButtonItem barButtonSalesAnalysis = _barButtonSalesAnalysis;
		num = 3;
		List<PropertyWithValue> list8 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list8, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list8);
		span3[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		span3[1] = new PropertyWithValue("StartDate", Settings.HostingEnvironment.DateBegin.ToString());
		span3[2] = new PropertyWithValue("EndDate", Settings.HostingEnvironment.DateEnd.ToString());
		barButtonSalesAnalysis.PropertyWithValues = list8;
	}

	private void AddRibbonPageGroupJournals()
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

	private void AddRibbonPageGroupSubDirectories()
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
		partnerTAccountReportViewer.DateBegin = Settings.HostingEnvironment.DateBegin;
		partnerTAccountReportViewer.DateEnd = Settings.HostingEnvironment.DateEnd;
		partnerTAccountReportViewer.Show(useParamValues: true, openAsChild: false);
	}

	private void BusinessAddressesGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		PartnerBusinessAddressRow partnerBusinessAddressRow = (PartnerBusinessAddressRow)_businessAddressesGrid.Grid.GetRow(e.RowHandle);
		if (partnerBusinessAddressRow != null)
		{
			PartnerBusinessAddressRow partnerBusinessAddressRow2 = new PartnerBusinessAddressRow();
			partnerBusinessAddressRow.Clone(partnerBusinessAddressRow2);
			_businessAddressesList.Add(partnerBusinessAddressRow2);
			e.NewRowNumber = _businessAddressesList.Count - 1;
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonPartnerChangeRequest.IsEnabled = isEnabled;
		_barButtonSalesAnalysis.IsEnabled = isEnabled;
		_barButtonPartnerCard.IsEnabled = isEnabled;
		_barButtonPartnerRemains.IsEnabled = isEnabled;
		_barButtonPartnerTurn.IsEnabled = isEnabled;
		_barButtonPartnerOpJournal.IsEnabled = isEnabled;
		_barButtonDocumentsJournal.IsEnabled = isEnabled;
		_barButtonPartAccInBank.IsEnabled = isEnabled;
		_barButtonPartContracts.IsEnabled = isEnabled;
		_barButtonPartnersMT.IsEnabled = isEnabled;
	}

	private void BusinessAddressesGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_businessAddressesList.Add(new PartnerBusinessAddressRow());
		e.RowNumber = _businessAddressesList.Count - 1;
	}

	private void SettlementAccountsGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		PartnerSettlementAccount partnerSettlementAccount = (PartnerSettlementAccount)_settlementAccountsGrid.Grid.GetRow(e.RowHandle);
		if (partnerSettlementAccount != null)
		{
			PartnerSettlementAccount partnerSettlementAccount2 = new PartnerSettlementAccount();
			partnerSettlementAccount.Clone(partnerSettlementAccount2);
			partnerSettlementAccount2.IsDefault = false;
			_settlementAccountsList.Add(partnerSettlementAccount2);
			e.NewRowNumber = _settlementAccountsList.Count - 1;
		}
	}

	private void SettlementAccountsGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		PartnerSettlementAccount partnerSettlementAccount = new PartnerSettlementAccount();
		_settlementAccountsList.Add(partnerSettlementAccount);
		if (_settlementAccountsList.Count == 1)
		{
			partnerSettlementAccount.IsDefault = true;
		}
		e.RowNumber = _settlementAccountsList.Count - 1;
	}

	private void SettlementAccountsTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (e.Column != _settlementAccountColumn)
		{
			return;
		}
		PartnerSettlementAccount partnerSettlementAccount = (PartnerSettlementAccount)e.Row;
		string settlAccount = partnerSettlementAccount.SettlAccount;
		if (!string.IsNullOrWhiteSpace(settlAccount))
		{
			string bankCaptionBySettlementAccount = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(settlAccount, out var _);
			if (!string.IsNullOrWhiteSpace(bankCaptionBySettlementAccount))
			{
				partnerSettlementAccount.Caption = bankCaptionBySettlementAccount;
			}
		}
	}

	private void SettlementAccountsTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		if (e.Column != _defaultSettlAccColumn || !Convert.ToBoolean(e.Value))
		{
			return;
		}
		for (int i = 0; i <= _settlementAccountsList.Count - 1; i++)
		{
			int rowHandleByListIndex = _settlementAccountsGrid.Grid.GetRowHandleByListIndex(i);
			if (rowHandleByListIndex != e.RowHandle)
			{
				_settlementAccountsList[i].IsDefault = false;
				_settlementAccountsGrid.Grid.RefreshRow(rowHandleByListIndex);
			}
		}
	}

	private void ContractsGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		PartnerContractRow partnerContractRow = (PartnerContractRow)_contractsGrid.Grid.GetRow(e.RowHandle);
		if (partnerContractRow != null)
		{
			PartnerContractRow partnerContractRow2 = new PartnerContractRow();
			partnerContractRow.Clone(partnerContractRow2);
			partnerContractRow2.IsDefault = false;
			partnerContractRow2.Contract = GetNextContractNumber();
			partnerContractRow2.ExportedToDiscountServer = false;
			_contractsList.Add(partnerContractRow2);
			e.NewRowNumber = _contractsList.Count - 1;
		}
	}

	private void ContractsGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		PartnerContractRow partnerContractRow = new PartnerContractRow();
		_contractsList.Add(partnerContractRow);
		if (_contractsList.Count == 1)
		{
			partnerContractRow.Caption = ArmSoft.SME.Trade.Common.Resources.DefaultContract;
			partnerContractRow.CurrCode = "AMD";
			partnerContractRow.IsDefault = true;
		}
		partnerContractRow.Contract = GetNextContractNumber();
		e.RowNumber = _contractsList.Count - 1;
	}

	private void ContractsGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		if (e.Column != _contractPriceTypeColumn)
		{
			return;
		}
		PartnerContractRow partnerContractRow = (PartnerContractRow)_contractsGrid.Grid.GetRow(e.RowHandle);
		if (partnerContractRow != null)
		{
			PriceTypes priceTypes = new PriceTypes(partnerContractRow.CurrCode);
			ModalBrowser modalBrowser = new ModalBrowser(priceTypes)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser.Show())
			{
				e.NewValue = priceTypes.ValueMember(modalBrowser.SelectedRowIndex);
			}
		}
		_contractsGrid.Grid.RefreshData();
	}

	private void ContractsTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _contractsTableView.FocusedRowHandle;
		PartnerContractRow partnerContractRow = (PartnerContractRow)_contractsGrid.Grid.GetRow(focusedRowHandle);
		if (partnerContractRow == null)
		{
			return;
		}
		_contractPriceTypeColumn.DataView = new PriceTypes(partnerContractRow.CurrCode);
		if (_useBonus)
		{
			bool flag = Convert.ToBoolean(partnerContractRow.CurrCode == "AMD");
			_isBonusCardColumn.ReadOnly = !flag;
			_bonusColumn.ReadOnly = !flag || !partnerContractRow.IsBonusCard;
			if (TradeSettings.CreateTradeClient().CommonServices.CheckEntriesExistenceByBonusContract(partnerContractRow.Contract, PO.Id))
			{
				_isBonusCardColumn.ReadOnly = true;
			}
		}
		_receivablesLimitColumn.ReadOnly = !partnerContractRow.ControlReceivables;
	}

	private void B2bCaptionButton_Click(object sender, RoutedEventArgs e)
	{
		if (!_name.DoValidate() || string.IsNullOrWhiteSpace(_name.Text))
		{
			return;
		}
		try
		{
			OrganizationsDataProvider.Param param = new OrganizationsDataProvider.Param
			{
				OrgName = _name.Text
			};
			List<OrganizationsDataProvider.DataRow> rows = new OrganizationsDataProvider().GetRows(param);
			if (rows == null)
			{
				return;
			}
			if (rows.Count == 0)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.PartnerWithNameDoesNotExistInB2B);
			}
			if (rows.Count == 1)
			{
				FillControlsValuesFromPartner(rows[0]);
			}
			else if (rows.Count > 1)
			{
				Organizations organizations = new Organizations(_name.Text);
				ModalBrowser modalBrowser = new ModalBrowser(organizations)
				{
					Owner = this,
					WindowStartupLocation = WindowStartupLocation.CenterScreen
				};
				if (modalBrowser.Show() && organizations.TryGetRowByIndex(modalBrowser.SelectedRowIndex, out var value))
				{
					FillControlsValuesFromPartner(value);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void B2bButton_Click(object sender, RoutedEventArgs e)
	{
		Showb2bOrganizationCard();
	}

	private void Showb2bOrganizationCard()
	{
		if (!_taxCode.DoValidate() || string.IsNullOrWhiteSpace(_taxCode.Text))
		{
			return;
		}
		try
		{
			if (_taxCode.Text.Length != 8)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldLengthMustBeEqualTo, ArmSoft.SME.Common.Resources.OptionsTaxCode, (short)8) + ": " + _taxCode.Text);
			}
			string editValue = ArmSoft.SME.Common.Resources.Partnerb2bStatusNotFound;
			Organization organization;
			using (B2BClient b2BClient = Settings.B2BClient)
			{
				organization = b2BClient.BusinessUnits.GetOrganization(_taxCode.Text);
				StatisticsRoutes statistics = b2BClient.Statistics;
				int num = 1;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = _taxCode.Text;
				List<OrganizationStatistics> organizationStatistics = statistics.GetOrganizationStatistics(list);
				if (organizationStatistics != null && organizationStatistics.Count > 0)
				{
					editValue = SmartEnum<PartnerStateInB2BSE, byte>.FromValue(Convert.ToByte(organizationStatistics[0].StateExtended)).Caption;
				}
			}
			if (organization != null)
			{
				DialogWindow dialogWindow = new DialogWindow
				{
					Caption = ArmSoft.SME.Trade.Common.Resources.PartnerDataFromB2B,
					Name = "B2B",
					Mode = DialogWindowMode.ViewMode,
					Owner = this,
					HelpId = Definition.HelpId
				};
				TextEditExt textEditExt = dialogWindow.AddStringControl(null, "Code", ArmSoft.SME.Common.Resources.Code, 20);
				textEditExt.EditValue = _code.EditValue;
				textEditExt.IsReadOnly = true;
				TextEditExt textEditExt2 = dialogWindow.AddStringControl(null, "TaxCode", ArmSoft.SME.Common.Resources.OptionsTaxCode, 20);
				textEditExt2.EditValue = _taxCode.Text;
				textEditExt2.IsReadOnly = true;
				LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
				LayoutGroupExt container2 = dialogWindow.AddVerticalLayoutGroup(container, LayoutGroupView.GroupBox, ArmSoft.SME.Common.Resources.DataStoredInDB);
				LayoutGroupExt container3 = dialogWindow.AddVerticalLayoutGroup(container, LayoutGroupView.GroupBox, ArmSoft.SME.Common.Resources.DataStoredInB2B);
				TextEditExt textEditExt3 = dialogWindow.AddStringControl(container2, "Status", ArmSoft.SME.Common.Resources.Partnerb2bStatus, 255);
				TextEditExt textEditExt4 = textEditExt3;
				string editValue2;
				if (PO == null)
				{
					editValue2 = "";
				}
				else
				{
					editValue2 = (SmartEnum<PartnerStatusInB2BSE, byte>.TryFromValue(PO.B2BStatusCode, out var result) ? result.Caption : "");
				}
				textEditExt4.EditValue = editValue2;
				textEditExt3.IsReadOnly = true;
				TextEditExt textEditExt5 = dialogWindow.AddStringControl(container3, "Status_B2B", "", 255);
				textEditExt5.EditValue = editValue;
				textEditExt5.IsReadOnly = true;
				TextEditExt textEditExt6 = dialogWindow.AddStringControl(container2, "FullCaption", ArmSoft.SME.Common.Resources.LegalName, 255);
				textEditExt6.EditValue = (string.IsNullOrWhiteSpace(textEditExt6.Text) ? _name.EditValue : textEditExt6.EditValue);
				textEditExt6.IsReadOnly = true;
				TextEditExt textEditExt7 = dialogWindow.AddStringControl(container3, "FullCaption_B2B", "", 255);
				textEditExt7.EditValue = organization.FullName;
				textEditExt7.IsReadOnly = true;
				TextEditExt textEditExt8 = dialogWindow.AddStringControl(container2, "LegAdd", ArmSoft.SME.Common.Resources.Address, 255);
				textEditExt8.EditValue = _address.EditValue;
				textEditExt8.IsReadOnly = true;
				TextEditExt textEditExt9 = dialogWindow.AddStringControl(container3, "LegAdd_B2B", "", 255);
				textEditExt9.EditValue = organization.LegalAddress.FullPath.ToString();
				textEditExt9.IsReadOnly = true;
				TextEditExt textEditExt10 = dialogWindow.AddStringControl(container2, "BusAdd", ArmSoft.SME.Common.Resources.BusinessAddresses, 255);
				textEditExt10.EditValue = _businessAddress.EditValue;
				textEditExt10.IsReadOnly = true;
				TextEditExt textEditExt11 = dialogWindow.AddStringControl(container3, "BusAdd_B2B", "", 255);
				textEditExt11.EditValue = organization.BusinessAddress.FullPath.ToString();
				textEditExt11.IsReadOnly = true;
				TextEditExt textEditExt12 = dialogWindow.AddStringControl(container2, "MngName", ArmSoft.SME.Common.Resources.ManagerFIO, 50);
				textEditExt12.EditValue = _managerFIO.EditValue;
				textEditExt12.IsReadOnly = true;
				TextEditExt textEditExt13 = dialogWindow.AddStringControl(container3, "MngName_B2B", "", 50);
				textEditExt13.EditValue = organization.Manager.Name;
				textEditExt13.IsReadOnly = true;
				TextEditExt textEditExt14 = dialogWindow.AddStringControl(container2, "MngPost", ArmSoft.SME.Common.Resources.ManagerPost, 50);
				textEditExt14.EditValue = _managerPost.EditValue;
				textEditExt14.IsReadOnly = true;
				TextEditExt textEditExt15 = dialogWindow.AddStringControl(container3, "MngPost_B2B", "", 50);
				textEditExt15.EditValue = organization.Manager.Position;
				textEditExt15.IsReadOnly = true;
				TextEditExt textEditExt16 = dialogWindow.AddStringControl(container2, "ChAccName", ArmSoft.SME.Common.Resources.ChAccFIO, 50);
				textEditExt16.EditValue = _chAccFIO.EditValue;
				textEditExt16.IsReadOnly = true;
				TextEditExt textEditExt17 = dialogWindow.AddStringControl(container3, "ChAccName_B2B", "", 50);
				textEditExt17.EditValue = organization.Accountant.Name;
				textEditExt17.IsReadOnly = true;
				TextEditExt textEditExt18 = dialogWindow.AddStringControl(container2, "ChAccPost", ArmSoft.SME.Common.Resources.AccountantPost, 50);
				textEditExt18.EditValue = _chAccPost.EditValue;
				textEditExt18.IsReadOnly = true;
				TextEditExt textEditExt19 = dialogWindow.AddStringControl(container3, "ChAccPost_B2B", "", 50);
				textEditExt19.EditValue = organization.Accountant.Position;
				textEditExt19.IsReadOnly = true;
				dialogWindow.AddTextBlock(ArmSoft.SME.Common.Resources.SettlementAccountsStoredInDB, TextBlockImage.None);
				DocumentGridControl documentGridControl = dialogWindow.AddGridControl("SettlAcc");
				documentGridControl.Caption = ArmSoft.SME.Common.Resources.SettlementAccountsStoredInDB;
				documentGridControl.AllowAdd = false;
				documentGridControl.AllowDelete = false;
				documentGridControl.AllowDuplicate = false;
				documentGridControl.ShowButtons = false;
				documentGridControl.Height = 80.0;
				DocumentGridColumn documentGridColumn = documentGridControl.AddStringColumn("SettlAccount", ArmSoft.SME.Common.Resources.SettlementAccount, 50);
				documentGridColumn.AllowEditing = DefaultBoolean.False;
				documentGridColumn.AllowFocus = false;
				documentGridColumn.AllowResizing = DefaultBoolean.False;
				documentGridColumn.AllowMoving = DefaultBoolean.False;
				DocumentGridColumn documentGridColumn2 = documentGridControl.AddStringColumn("Caption", ArmSoft.SME.Common.Resources.Name, 255);
				documentGridColumn2.AllowEditing = DefaultBoolean.False;
				documentGridColumn2.AllowFocus = false;
				documentGridColumn2.AllowResizing = DefaultBoolean.False;
				documentGridColumn2.AllowMoving = DefaultBoolean.False;
				DocumentGridColumn documentGridColumn3 = documentGridControl.AddBooleanColumn("IsDefault", ArmSoft.SME.Common.Resources.Default);
				documentGridColumn3.AllowEditing = DefaultBoolean.False;
				documentGridColumn3.AllowFocus = false;
				documentGridColumn3.AllowResizing = DefaultBoolean.False;
				documentGridColumn3.AllowMoving = DefaultBoolean.False;
				documentGridControl.ItemsSource = _settlementAccountsList;
				dialogWindow.ShowDialog();
				return;
			}
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.PartnerNotExistInB2B, _taxCode.Text));
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void TaxCodeButton_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_Shift_M.Matches(this, e))
		{
			B2bTaxCodeButton_Click(sender, e);
		}
	}

	private void B2bTaxCodeButton_Click(object sender, RoutedEventArgs e)
	{
		if (_taxCode.DoValidate() && !string.IsNullOrWhiteSpace(_taxCode.Text))
		{
			FillControlsValuesFromPartner(GetPartnerFromTIN(_taxCode.Text));
		}
	}

	public static Partner GetPartnerFromTIN(string tin)
	{
		try
		{
			if (tin.Length != 8)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldLengthMustBeEqualTo, ArmSoft.SME.Common.Resources.OptionsTaxCode, (short)8) + ": " + tin);
			}
			using B2BClient b2BClient = Settings.B2BClient;
			Organization organization = b2BClient.BusinessUnits.GetOrganization(tin);
			if (organization != null)
			{
				return GetPartnerFromOrganization(organization);
			}
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.PartnerNotExistInB2B, tin));
		}
		catch (EndpointNotFoundException)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.IBEndpointNotFound);
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
		return null;
	}

	private static Partner GetPartnerFromOrganization(Organization org)
	{
		Partner partner = new Partner
		{
			TaxCode = org.TaxCode,
			FullCaption = org.FullName + (string.IsNullOrWhiteSpace(org.LegalStatusShort) ? "" : (" " + org.LegalStatusShort)),
			Address = org.LegalAddress.FullPath,
			BusinessAddress = (string.IsNullOrWhiteSpace(org.BusinessAddress.FullPath) ? org.LegalAddress.FullPath : org.BusinessAddress.FullPath),
			ManagerPost = org.Manager.Position,
			ManagerFIO = org.Manager.Name,
			ChiefaccPost = org.Accountant.Position,
			ChiefaccFIO = org.Accountant.Name
		};
		partner.Name = partner.FullCaption.Left(50);
		return partner;
	}

	private void ContractsTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		PartnerContractRow partnerContractRow = (PartnerContractRow)_contractsGrid.Grid.GetRow(e.RowHandle);
		if (partnerContractRow == null)
		{
			return;
		}
		if (e.Column == _contractCurrCodeColumn)
		{
			if (e.Value != e.OldValue)
			{
				partnerContractRow.PriceType = "";
			}
			if (_useBonus)
			{
				bool flag = Convert.ToBoolean((e.Value == null) ? "" : ((object)(e.Value.ToString() == "AMD")));
				_isBonusCardColumn.ReadOnly = !flag;
				_bonusColumn.ReadOnly = !flag || !partnerContractRow.IsBonusCard;
				if (!flag)
				{
					partnerContractRow.IsBonusCard = false;
					partnerContractRow.Bonus = 0m;
				}
			}
			PartnerContractRow partnerContractRow2 = (PartnerContractRow)e.Row;
			_contractPriceTypeColumn.DataView = new PriceTypes(partnerContractRow2.CurrCode);
			_contractsGrid.Grid.RefreshData();
		}
		else if (e.Column == _isBonusCardColumn)
		{
			_bonusColumn.ReadOnly = !Convert.ToBoolean(e.Value);
			if (!Convert.ToBoolean(e.Value))
			{
				partnerContractRow.Bonus = 0m;
			}
			else if (Owner is MTBillWindow)
			{
				((PartnerContractRow)e.Row).BonusCardValidPeriodStart = DateTime.Now.Date.ToDateOnly();
			}
		}
		else if (e.Column == _controlReceivablesColumn)
		{
			_receivablesLimitColumn.ReadOnly = !Convert.ToBoolean(e.Value);
			if (!Convert.ToBoolean(e.Value))
			{
				partnerContractRow.ReceivablesLimit = 0m;
			}
		}
	}

	private void ContractsTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		if (e.Column != _defaultContractColumn || !Convert.ToBoolean(e.Value))
		{
			return;
		}
		for (int i = 0; i <= _contractsList.Count - 1; i++)
		{
			int rowHandleByListIndex = _contractsGrid.Grid.GetRowHandleByListIndex(i);
			if (rowHandleByListIndex != e.RowHandle)
			{
				_contractsList[i].IsDefault = false;
				_contractsGrid.Grid.RefreshRow(rowHandleByListIndex);
			}
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

	private void Caption_EditValueChanging(object sender, EditValueChangingEventArgs e)
	{
		if (string.IsNullOrWhiteSpace((string)_fullCaption.EditValue))
		{
			_fullCaption.EditValue = _name.EditValue;
		}
	}

	private void Caption_PreviewKeyDow(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_Shift_M.Matches(this, e))
		{
			B2bCaptionButton_Click(sender, e);
			e.Handled = true;
		}
	}

	private void FillControlsValuesFromPartner(Partner partner)
	{
		if (partner == null)
		{
			return;
		}
		_taxCode.Text = partner.TaxCode;
		_name.Text = partner.Name;
		_fullCaption.Text = partner.FullCaption;
		_address.Text = partner.Address;
		_businessAddress.Text = partner.BusinessAddress;
		_managerPost.Text = partner.ManagerPost;
		_managerFIO.Text = partner.ManagerFIO;
		_chAccPost.Text = partner.ChiefaccPost;
		_chAccFIO.Text = partner.ChiefaccFIO;
		_email.Text = partner.Email;
		if (partner.SettlementAccounts == null)
		{
			return;
		}
		foreach (Partner.PartnerSettlementAccount settlementAccount in partner.SettlementAccounts)
		{
			if (!CheckSettlementAccountsRepeatedRows(settlementAccount, out var defaultExist))
			{
				_settlementAccountsList.Add(new PartnerSettlementAccount
				{
					IsDefault = (!(settlementAccount.IsDefault & defaultExist) && settlementAccount.IsDefault),
					SettlAccount = settlementAccount.SettlAccount,
					Caption = settlementAccount.Caption
				});
			}
		}
	}

	private void FillControlsValuesFromPartner(OrganizationsDataProvider.DataRow partner)
	{
		if (partner != null)
		{
			_taxCode.Text = partner.TaxCode;
			_name.Text = partner.Caption;
			_fullCaption.Text = partner.FullCaption;
			_address.Text = partner.Address;
			_businessAddress.Text = partner.BusinessAddress;
			_managerPost.Text = partner.ManagerPost;
			_managerFIO.Text = partner.ManagerFIO;
			_chAccPost.Text = partner.ChiefaccPost;
			_chAccFIO.Text = partner.ChiefaccFIO;
		}
	}

	private bool CheckSettlementAccountsRepeatedRows(Partner.PartnerSettlementAccount settlAccountItem, out bool defaultExist)
	{
		bool result = false;
		defaultExist = false;
		foreach (PartnerSettlementAccount settlementAccounts in _settlementAccountsList)
		{
			if (settlAccountItem.SettlAccount == settlementAccounts.SettlAccount)
			{
				result = true;
			}
			if (settlAccountItem.IsDefault)
			{
				defaultExist = true;
			}
		}
		return result;
	}

	public override void OnMagneticCardReader(string mcrData)
	{
		base.OnMagneticCardReader(mcrData);
		ReadMagneticCard(mcrData);
	}

	public override void OnScanner(string scannerData)
	{
		base.OnScanner(scannerData);
		ReadMagneticCard(scannerData, byMagneticCardPrefixSuffix: false);
	}

	private void ReadMagneticCard(string cardData, bool byMagneticCardPrefixSuffix = true)
	{
		try
		{
			string empty = string.Empty;
			if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
			{
				if (!DocumentHelpers.IsValidPartnerEmployeeCardCode(cardData, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode);
				}
				empty = cardData;
			}
			else if (!byMagneticCardPrefixSuffix)
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
			if (_contractsList.Count == 0)
			{
				_contractsList.Add(new PartnerContractRow
				{
					Caption = ArmSoft.SME.Trade.Common.Resources.DefaultContract,
					CurrCode = "AMD",
					IsDefault = true,
					Contract = GetNextContractNumber(),
					DiscountCard = empty
				});
			}
			else if (_contractsList.Count == 1)
			{
				_contractsList[0].DiscountCard = empty;
			}
			else if (!string.IsNullOrWhiteSpace(_code.Text))
			{
				Contracts contracts = new Contracts(_code.Text);
				ModalBrowser modalBrowser = new ModalBrowser(contracts)
				{
					LookUpValue = _code.Text,
					Owner = this,
					Left = Left + Width / 4.0,
					Top = Top + Height / 4.0
				};
				if (modalBrowser.Show())
				{
					string text = Convert.ToString(contracts.ValueMember(modalBrowser.SelectedRowIndex));
					foreach (PartnerContractRow contracts2 in _contractsList)
					{
						if (contracts2.Contract == text)
						{
							contracts2.DiscountCard = empty;
							break;
						}
					}
				}
			}
			_contractsTableView.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private string GetNextContractNumber()
	{
		if (PO.StoredInDb)
		{
			string text = "";
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(PO.Code);
			if (item != null)
			{
				text = item.NextContractNumber;
			}
			bool flag = string.IsNullOrWhiteSpace(text);
			if (!flag)
			{
				foreach (PartnerContractRow contracts in _contractsList)
				{
					if (contracts.Contract == text)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				if (_contractsList.Count == 1)
				{
					return ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(new string('0', 3), 3);
				}
				ObservableCollection<PartnerContractRow> contractsList = _contractsList;
				return ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(contractsList[contractsList.Count - 2].Contract, 3);
			}
			return text;
		}
		if (_contractsList.Count <= 1)
		{
			return ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(new string('0', 3), 3);
		}
		ObservableCollection<PartnerContractRow> contractsList2 = _contractsList;
		return ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(contractsList2[contractsList2.Count - 2].Contract, 3);
	}

	protected override void OnNew()
	{
		base.OnNew();
		if (string.IsNullOrWhiteSpace(_name.Text))
		{
			_chAccPost.Text = ArmSoft.SME.Common.Resources.ChAccPost1;
		}
		if (_viewMode == WindowViewMode.NewCreate)
		{
			_supplier.IsChecked = true;
			_customer.IsChecked = true;
		}
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _name;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_code.IsReadOnly = StoredInDb;
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_name.Text = "";
		_fullCaption.EditValue = "";
		_taxCode.EditValue = "";
		_address.EditValue = "";
		_businessAddress.EditValue = "";
		_phone.EditValue = "";
		_managerFIO.EditValue = "";
		_managerPost.Text = "";
		_chAccFIO.EditValue = "";
		if (Settings.Localization == Localizations.Armenia)
		{
			_partStateRegister.EditValue = "";
		}
		_passportNum.EditValue = "";
		_birthDate.EditValue = null;
		_email.EditValue = "";
		_mainAim.EditValue = "";
		_contractsList.Clear();
		_settlementAccountsList.Clear();
		_businessAddressesList.Clear();
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_contractsList.Clear();
		foreach (Partner.PartnerContractRow partnerContract in PO.PartnerContracts)
		{
			_contractsList.Add(new PartnerContractRow
			{
				Contract = partnerContract.Contract,
				Caption = partnerContract.Caption,
				CurrCode = partnerContract.CurrCode,
				PriceType = partnerContract.PriceType,
				Limit = partnerContract.Limit,
				Percent = partnerContract.Percent,
				DiscountCard = partnerContract.DiscountCard,
				IsBonusCard = partnerContract.IsBonusCard,
				Bonus = partnerContract.Bonus,
				Point = partnerContract.Point,
				ControlReceivables = partnerContract.ControlReceivables,
				ReceivablesLimit = partnerContract.ReceivablesLimit,
				IsDefault = partnerContract.IsDefault,
				ContractDate = partnerContract.ContractDate,
				Description = partnerContract.Description,
				BonusCardValidPeriodStart = partnerContract.BonusCardValidPeriodStart,
				BonusCardValidPeriodEnd = partnerContract.BonusCardValidPeriodEnd,
				BonusCardCanceled = partnerContract.BonusCardCanceled,
				ExportedToDiscountServer = partnerContract.ExportedToDiscountServer
			});
		}
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		_contractsGrid.ItemsSource = _contractsList;
		_settlementAccountsList.Clear();
		foreach (Partner.PartnerSettlementAccount settlementAccount in PO.SettlementAccounts)
		{
			_settlementAccountsList.Add(new PartnerSettlementAccount
			{
				SettlAccount = settlementAccount.SettlAccount,
				Caption = settlementAccount.Caption,
				IsDefault = settlementAccount.IsDefault
			});
		}
		_settlementAccountsGrid.ItemsSource = _settlementAccountsList;
		_businessAddressesList.Clear();
		foreach (Partner.PartnerBusinessAddressRow partnerBusinessAddress in PO.PartnerBusinessAddresses)
		{
			if (!partnerBusinessAddress.IsDefault)
			{
				_businessAddressesList.Add(new PartnerBusinessAddressRow
				{
					Caption = partnerBusinessAddress.Caption,
					IsDefault = partnerBusinessAddress.IsDefault
				});
			}
		}
		_businessAddressesGrid.ItemsSource = _businessAddressesList;
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 2;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("PartnerCode", PO.Code.ToString());
		span[1] = new PropertyWithValue("ShowExtendedDialog", false.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	protected override void FillDocument(DirStoreRequest sr)
	{
		base.FillDocument(sr);
		PO.Supplier = _supplier.IsChecked.Value;
		PO.Customer = _customer.IsChecked.Value;
		PO.PartnerContracts.Clear();
		foreach (PartnerContractRow contracts in _contractsList)
		{
			PO.PartnerContracts.Add(new Partner.PartnerContractRow
			{
				Contract = contracts.Contract,
				Caption = contracts.Caption,
				CurrCode = contracts.CurrCode,
				PriceType = contracts.PriceType,
				Limit = contracts.Limit,
				Percent = contracts.Percent,
				DiscountCard = contracts.DiscountCard,
				IsBonusCard = contracts.IsBonusCard,
				Bonus = contracts.Bonus,
				Point = contracts.Point,
				ControlReceivables = contracts.ControlReceivables,
				ReceivablesLimit = contracts.ReceivablesLimit,
				IsDefault = contracts.IsDefault,
				ContractDate = contracts.ContractDate,
				Description = contracts.Description,
				BonusCardValidPeriodStart = contracts.BonusCardValidPeriodStart,
				BonusCardValidPeriodEnd = contracts.BonusCardValidPeriodEnd,
				BonusCardCanceled = contracts.BonusCardCanceled,
				ExportedToDiscountServer = contracts.ExportedToDiscountServer
			});
		}
		PO.SettlementAccounts.Clear();
		foreach (PartnerSettlementAccount settlementAccounts in _settlementAccountsList)
		{
			PO.SettlementAccounts.Add(new Partner.PartnerSettlementAccount
			{
				SettlAccount = settlementAccounts.SettlAccount,
				Caption = settlementAccounts.Caption,
				IsDefault = settlementAccounts.IsDefault
			});
		}
		PO.PartnerBusinessAddresses.Clear();
		foreach (PartnerBusinessAddressRow businessAddresses in _businessAddressesList)
		{
			if (!businessAddresses.IsDefault)
			{
				PO.PartnerBusinessAddresses.Add(new Partner.PartnerBusinessAddressRow
				{
					Caption = businessAddresses.Caption,
					IsDefault = businessAddresses.IsDefault
				});
			}
		}
		if (string.IsNullOrWhiteSpace(_taxCode.Text))
		{
			PO.B2BStatusCode = byte.MaxValue;
			return;
		}
		try
		{
			List<OrganizationStatistics> partnerStatisticsFromB2B = GetPartnerStatisticsFromB2B(_taxCode.Text);
			if (partnerStatisticsFromB2B == null || partnerStatisticsFromB2B.Count == 0)
			{
				PO.B2BStatusCode = 254;
			}
			else
			{
				PO.B2BStatusCode = Convert.ToByte(partnerStatisticsFromB2B[0].StateExtended);
			}
		}
		catch (Exception)
		{
		}
	}

	private static List<OrganizationStatistics> GetPartnerStatisticsFromB2B(string taxCode)
	{
		using B2BClient b2BClient = Settings.B2BClient;
		StatisticsRoutes statistics = b2BClient.Statistics;
		int num = 1;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = taxCode;
		return statistics.GetOrganizationStatistics(list);
	}

	protected override bool BeforeStore(DirStoreRequest sr)
	{
		if (!base.BeforeStore(sr))
		{
			return false;
		}
		if (!string.IsNullOrWhiteSpace(_taxCode.Text) || !string.IsNullOrWhiteSpace(_passportNum.Text))
		{
			List<string> list = TradeSettings.CreateTradeClient().Additional.ExistPartnerWithSamePropertiesMsg(_code.Text, _taxCode.Text, _passportNum.Text);
			if (list != null && list.Count > 0)
			{
				string text = "";
				if (list.Count == 1)
				{
					text = text + list[0] + " \r\n";
				}
				else
				{
					for (int i = 0; i < list.Count; i++)
					{
						text = text + (i + 1) + ". " + list[i] + " \r\n";
					}
				}
				text += ArmSoft.SME.Common.Resources.AskContinue;
				if (ArmSoft.SME.Core.UI.MessageBox.Show(text, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
				{
					return false;
				}
			}
		}
		return true;
	}

	protected override void AfterStore(DirStoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		if (!_useBonus || !Settings.ParametersService.ConnectTob2barmsoftDiscountServer())
		{
			return;
		}
		foreach (Partner.PartnerContractRow partnerContract in PO.PartnerContracts)
		{
			if (partnerContract.IsBonusCard && partnerContract.ExportedToDiscountServer && !string.IsNullOrWhiteSpace(partnerContract.DiscountCard))
			{
				try
				{
					DiscountServerService.UpdateCardInfo(new CardUpdateRequest
					{
						CardNumber = partnerContract.DiscountCard,
						BonusPercent = partnerContract.Bonus
					});
				}
				catch
				{
				}
			}
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
