using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(35)]
public class MTRevaluationWindow : TradeDocumentWindow<MTRevaluation>
{
	public class MTRevaluationSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private string storage;

		[ObservableProperty]
		private string mTCode;

		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string mTName;

		[ObservableProperty]
		private string unitCode;

		[ObservableProperty]
		private string unitBrief;

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private decimal cost;

		[ObservableProperty]
		private decimal costWithVAT;

		[ObservableProperty]
		private decimal curCostWithVAT;

		[ObservableProperty]
		private decimal newCost;

		[ObservableProperty]
		private decimal newCostWithVAT;

		[ObservableProperty]
		private decimal newCurCostWithVAT;

		[ObservableProperty]
		private bool vAT;

		[ObservableProperty]
		private DateOnly incomeDate;

		[ObservableProperty]
		private string incomeDocDescr;

		[ObservableProperty]
		private int partID;

		[ObservableProperty]
		private string partnerName;

		[ObservableProperty]
		private string contractName;

		[ObservableProperty]
		private string contractCode;

		[ObservableProperty]
		private DateOnly? useLife;

		[ObservableProperty]
		private string countryCode;

		[ObservableProperty]
		private string countryName;

		[ObservableProperty]
		private string producerCode;

		[ObservableProperty]
		private string producerName;

		[ObservableProperty]
		private string property1Code;

		[ObservableProperty]
		private string property1Value;

		[ObservableProperty]
		private string property2Code;

		[ObservableProperty]
		private string property2Value;

		[ObservableProperty]
		private Guid partyBase;

		[ObservableProperty]
		private int partyRow;

		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private short rowNum;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Storage
		{
			get
			{
				return storage;
			}
			[MemberNotNull("storage")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(storage, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Storage);
					storage = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Storage);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string MTCode
		{
			get
			{
				return mTCode;
			}
			[MemberNotNull("mTCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(mTCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTCode);
					mTCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int MTID
		{
			get
			{
				return mTID;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(mTID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTID);
					mTID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTID);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string MTName
		{
			get
			{
				return mTName;
			}
			[MemberNotNull("mTName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(mTName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTName);
					mTName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string UnitCode
		{
			get
			{
				return unitCode;
			}
			[MemberNotNull("unitCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(unitCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitCode);
					unitCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitCode);
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
					OnQuantityChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Quantity);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Cost
		{
			get
			{
				return cost;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(cost, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Cost);
					cost = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Cost);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal CostWithVAT
		{
			get
			{
				return costWithVAT;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(costWithVAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CostWithVAT);
					costWithVAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CostWithVAT);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal CurCostWithVAT
		{
			get
			{
				return curCostWithVAT;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(curCostWithVAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurCostWithVAT);
					curCostWithVAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurCostWithVAT);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal NewCost
		{
			get
			{
				return newCost;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(newCost, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.NewCost);
					newCost = value;
					OnNewCostChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.NewCost);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal NewCostWithVAT
		{
			get
			{
				return newCostWithVAT;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(newCostWithVAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.NewCostWithVAT);
					newCostWithVAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.NewCostWithVAT);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal NewCurCostWithVAT
		{
			get
			{
				return newCurCostWithVAT;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(newCurCostWithVAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.NewCurCostWithVAT);
					newCurCostWithVAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.NewCurCostWithVAT);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool VAT
		{
			get
			{
				return vAT;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(vAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VAT);
					vAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VAT);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public DateOnly IncomeDate
		{
			get
			{
				return incomeDate;
			}
			set
			{
				if (!EqualityComparer<DateOnly>.Default.Equals(incomeDate, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IncomeDate);
					incomeDate = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IncomeDate);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string IncomeDocDescr
		{
			get
			{
				return incomeDocDescr;
			}
			[MemberNotNull("incomeDocDescr")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(incomeDocDescr, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IncomeDocDescr);
					incomeDocDescr = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IncomeDocDescr);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int PartID
		{
			get
			{
				return partID;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(partID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PartID);
					partID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PartID);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string PartnerName
		{
			get
			{
				return partnerName;
			}
			[MemberNotNull("partnerName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(partnerName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PartnerName);
					partnerName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PartnerName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ContractName
		{
			get
			{
				return contractName;
			}
			[MemberNotNull("contractName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(contractName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ContractName);
					contractName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ContractName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ContractCode
		{
			get
			{
				return contractCode;
			}
			[MemberNotNull("contractCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(contractCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ContractCode);
					contractCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ContractCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public DateOnly? UseLife
		{
			get
			{
				return useLife;
			}
			set
			{
				if (!EqualityComparer<DateOnly?>.Default.Equals(useLife, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UseLife);
					useLife = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UseLife);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string CountryCode
		{
			get
			{
				return countryCode;
			}
			[MemberNotNull("countryCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(countryCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CountryCode);
					countryCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CountryCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string CountryName
		{
			get
			{
				return countryName;
			}
			[MemberNotNull("countryName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(countryName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CountryName);
					countryName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CountryName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ProducerCode
		{
			get
			{
				return producerCode;
			}
			[MemberNotNull("producerCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(producerCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ProducerCode);
					producerCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ProducerCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ProducerName
		{
			get
			{
				return producerName;
			}
			[MemberNotNull("producerName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(producerName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ProducerName);
					producerName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ProducerName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Property1Code
		{
			get
			{
				return property1Code;
			}
			[MemberNotNull("property1Code")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(property1Code, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Property1Code);
					property1Code = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Property1Code);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Property1Value
		{
			get
			{
				return property1Value;
			}
			[MemberNotNull("property1Value")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(property1Value, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Property1Value);
					property1Value = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Property1Value);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Property2Code
		{
			get
			{
				return property2Code;
			}
			[MemberNotNull("property2Code")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(property2Code, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Property2Code);
					property2Code = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Property2Code);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Property2Value
		{
			get
			{
				return property2Value;
			}
			[MemberNotNull("property2Value")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(property2Value, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Property2Value);
					property2Value = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Property2Value);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public Guid PartyBase
		{
			get
			{
				return partyBase;
			}
			set
			{
				if (!EqualityComparer<Guid>.Default.Equals(partyBase, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PartyBase);
					partyBase = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PartyBase);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int PartyRow
		{
			get
			{
				return partyRow;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(partyRow, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PartyRow);
					partyRow = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PartyRow);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int RowID
		{
			get
			{
				return rowID;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(rowID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowID);
					rowID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowID);
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

		public MTRevaluationSpecificationRow()
		{
		}

		public MTRevaluationSpecificationRow(MTRevaluationWindow docWindow)
			: base(docWindow)
		{
		}

		static MTRevaluationSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTRevaluationSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTRevaluationSpecificationRow)row, info);
					}
				},
				{
					"NewCost",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateNewCost((MTRevaluationSpecificationRow)row, info);
					}
				},
				{
					"MTCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateGoodCode((MTRevaluationSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTRevaluationSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateQuantity(MTRevaluationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		private static void ValidateNewCost(MTRevaluationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.NewCost, info, ArmSoft.SME.Common.Resources.NewPrice);
		}

		private static void ValidateGoodCode(MTRevaluationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTRevaluation.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTRevaluationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTRevaluation.Grids["MTList"].Columns["UnitCode"]);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnQuantityChanged(decimal value)
		{
			ResetForceValidationBit("Quantity", "Quantity");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnNewCostChanged(decimal value)
		{
			ResetForceValidationBit("NewCost");
		}
	}

	private DropDownCommentControl _baseDocumentControl;

	private LayoutGroup _lgCoefficent;

	private LayoutGroup _lgPriceType;

	private LayoutGroup _lgDistrSum;

	private LayoutGroup _lgPrice;

	private DropDownControl _newPriceFormationType;

	private PopupCalcEditExt _coeff;

	private PopupCalcEditExt _distrSum;

	private PopupCalcEditExt _price;

	private DropDownControl _distrSumBy;

	private DropDownControl _colsVewType;

	private DropDownControl _priceType;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private BarEditItemExt _fillNewPrices;

	private DocumentGridControl _mtRevaluationGrid;

	private TableView _mtRevaluationTableView;

	private ObservableCollection<MTRevaluationSpecificationRow> _mtRevaluationList = new ObservableCollection<MTRevaluationSpecificationRow>();

	private DocumentGridColumn _storageCol;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _mtNameCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _unitBriefCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _costCol;

	private DocumentGridColumn _costWithVATCol;

	private DocumentGridColumn _curCostWithVATCol;

	private DocumentGridColumn _newCostCol;

	private DocumentGridColumn _newCostWithVATCol;

	private DocumentGridColumn _newCurCostWithVATCol;

	private DocumentGridColumn _incomeDateCol;

	private DocumentGridColumn _incomeDocDescrCol;

	private DocumentGridColumn _partnerCol;

	private DocumentGridColumn _contractCol;

	private DocumentGridColumn _useLifeCol;

	private DocumentGridColumn _countryCol;

	private DocumentGridColumn _producerCol;

	private DocumentGridColumn _property1Col;

	private DocumentGridColumn _property2Col;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private Guid _baseISN = Guid.Empty;

	private TradeDocument _baseDocument;

	private ExchangeRate _exchangeRate;

	private decimal _tradeConstVAT;

	private static readonly string _mtCurr = Settings.ParametersService.MTCurr();

	protected override double WidthValue => 890.0;

	protected override double HeightValue => 480.0;

	protected override double MinWidthValue => 890.0;

	protected override double MinHeightValue => 250.0;

	private TradeDocument BaseDocument
	{
		get
		{
			return _baseDocument;
		}
		set
		{
			_baseDocument = value;
			if (_baseDocument == null)
			{
				_baseDocumentControl.Text = "";
			}
			else
			{
				_baseDocumentControl.Text = DocumentHelpers.GetDocumentDescription(_baseDocument, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
			}
		}
	}

	private Guid BaseISN
	{
		get
		{
			return _baseISN;
		}
		set
		{
			if (((IComparable)value).CompareTo((object?)_baseISN) == 0)
			{
				return;
			}
			_baseISN = value;
			if (_baseISN == Guid.Empty)
			{
				BaseDocument = null;
				return;
			}
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillUsingBaseQ, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
			{
				CreateBasedOnDoc();
			}
		}
	}

	public MTRevaluationWindow()
	{
	}

	public MTRevaluationWindow(MTRevaluation doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_tradeConstVAT = Settings.ParametersService.VAT();
		LayoutGroup container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString(),
			DocState = DocumentStateSE.DraftOrProcessed.ValueUI
		};
		_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch, "", "BaseDocumentControl");
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocumentControl_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocument_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container2 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		LayoutGroup container3 = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Horizontal);
		_newPriceFormationType = AddSmartEnumDropDownControl(container3, Definition.Fields["PriceFormationOpType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_newPriceFormationType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(OpType_EditValueChanged, this).Handler;
		_colsVewType = AddSmartEnumDropDownControl(container3, Definition.Fields["GridViewColsType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_colsVewType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(GridCols_EditValueChanged, this).Handler;
		_lgCoefficent = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Horizontal);
		_coeff = AddNumericControl(_lgCoefficent, Definition.Fields["Coefficient"]);
		_lgPriceType = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Vertical);
		_priceType = AddDropDownControl(_lgPriceType, Definition.Fields["PriceType"], new PriceTypes());
		_lgDistrSum = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Vertical);
		_distrSum = AddNumericControl(_lgDistrSum, Definition.Fields["DistrSum"]);
		_distrSumBy = AddSmartEnumDropDownControl(_lgDistrSum, Definition.Fields["DistrSumBy"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_lgPrice = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Horizontal);
		_price = AddNumericControl(_lgPrice, Definition.Fields["Price"]);
		AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_comment = AddComboBoxControl(container2, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container2, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_mtRevaluationGrid = AddGrid(container2, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_mtRevaluationGrid.AllowAdd = false;
		_mtRevaluationGrid.AllowMoveUp = false;
		_mtRevaluationGrid.AllowMoveDown = false;
		_mtRevaluationGrid.AllowDuplicate = false;
		_mtRevaluationGrid.IsVisibleViewEditButton = false;
		_mtRevaluationGrid.AllowDelete = true;
		_mtRevaluationGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTRevaluationGrid_DeletedRow, this).Handler;
		_fillNewPrices = _mtRevaluationGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Trade.Common.Resources.FillNewPrice, "", 150.0);
		_fillNewPrices.Click += new SafeDocumentEventHandler<RoutedEventArgs>(NewPriceFormation_Click, this).Handler;
		_mtRevaluationGrid.IsRequired = true;
		_mtRevaluationTableView = _mtRevaluationGrid.View;
		_mtRevaluationGrid.AddRowIndicator(35);
		_storageCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["Storage"]);
		_storageCol.AllowEditing = DefaultBoolean.False;
		_storageCol.Width = 60.0;
		_mtCodeCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTCode"]);
		_mtCodeCol.Width = 125.0;
		_mtCodeCol.AllowEditing = DefaultBoolean.False;
		_mtNameCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		_mtNameCol.AllowEditing = DefaultBoolean.False;
		_unitCodeCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitCode"]);
		_unitCodeCol.AllowEditing = DefaultBoolean.False;
		_unitBriefCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]);
		_unitBriefCol.AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtRevaluationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_costCol = _mtRevaluationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Cost"]);
		_costCol.AllowEditing = DefaultBoolean.False;
		_costWithVATCol = _mtRevaluationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CostWithVAT"]);
		_costWithVATCol.AllowEditing = DefaultBoolean.False;
		_curCostWithVATCol = _mtRevaluationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CurCostWithVAT"]);
		_curCostWithVATCol.AllowEditing = DefaultBoolean.False;
		_curCostWithVATCol.Header = string.Format(ArmSoft.SME.Trade.Common.Resources.PriceCurr, _mtCurr);
		_newCostCol = _mtRevaluationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["NewCost"]);
		_newCostWithVATCol = _mtRevaluationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["NewCostWithVAT"]);
		_newCurCostWithVATCol = _mtRevaluationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["NewCurCostWithVAT"]);
		_newCurCostWithVATCol.Header = string.Format(ArmSoft.SME.Trade.Common.Resources.NewPriceCurr, _mtCurr);
		_incomeDateCol = _mtRevaluationGrid.AddDateColumn(Definition.Grids["MTList"].Columns["IncomeDate"]);
		_incomeDateCol.AllowEditing = DefaultBoolean.False;
		_incomeDateCol.Visible = Settings.ParametersService.ShowPartyIncomeDate();
		_incomeDateCol.ShowInColumnChooser = _incomeDateCol.Visible;
		_incomeDocDescrCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["IncomeDocDescr"]);
		_incomeDocDescrCol.AllowEditing = DefaultBoolean.False;
		_incomeDocDescrCol.Visible = Settings.ParametersService.ShowPartyDocType();
		_incomeDocDescrCol.ShowInColumnChooser = _incomeDocDescrCol.Visible;
		_partnerCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["PartnerName"]);
		_partnerCol.AllowEditing = DefaultBoolean.False;
		_partnerCol.Visible = Settings.ParametersService.ShowPartyProducer();
		_partnerCol.ShowInColumnChooser = _partnerCol.Visible;
		_contractCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["ContractName"]);
		_contractCol.AllowEditing = DefaultBoolean.False;
		_contractCol.Visible = Settings.ParametersService.ShowPartyCONTRACT();
		_contractCol.ShowInColumnChooser = _contractCol.Visible;
		_useLifeCol = _mtRevaluationGrid.AddDateColumn(Definition.Grids["MTList"].Columns["UseLife"]);
		_useLifeCol.AllowEditing = DefaultBoolean.False;
		_useLifeCol.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
		_useLifeCol.ShowInColumnChooser = _useLifeCol.Visible;
		_countryCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["CountryName"]);
		_countryCol.AllowEditing = DefaultBoolean.False;
		_countryCol.Visible = Settings.ParametersService.ShowPartyCountry();
		_countryCol.ShowInColumnChooser = _countryCol.Visible;
		_producerCol = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["ProducerName"]);
		_producerCol.Width = SizeCalculator.WidthForString(5);
		_producerCol.AllowEditing = DefaultBoolean.False;
		_producerCol.Visible = Settings.ParametersService.ShowPartyProducer();
		_producerCol.ShowInColumnChooser = _producerCol.Visible;
		_property1Col = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["Property1Value"]);
		_property1Col.Width = SizeCalculator.WidthForString(Convert.ToInt32(25.0));
		_property1Col.AllowEditing = DefaultBoolean.False;
		_property1Col.Visible = Settings.ParametersService.ShowPartyProperty1();
		_property1Col.ShowInColumnChooser = _property1Col.Visible;
		_property2Col = _mtRevaluationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["Property2Value"]);
		_property2Col.Width = SizeCalculator.WidthForString(Convert.ToInt32(25.0));
		_property2Col.AllowEditing = DefaultBoolean.False;
		_property2Col.Visible = Settings.ParametersService.ShowPartyProperty2();
		_property2Col.ShowInColumnChooser = _property2Col.Visible;
		foreach (GridColumn column in _mtRevaluationGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_mtRevaluationList = new ObservableCollection<MTRevaluationSpecificationRow>();
		foreach (GridColumn column2 in _mtRevaluationGrid.Grid.Columns)
		{
			column2.AllowSorting = DefaultBoolean.True;
			column2.AllowColumnFiltering = DefaultBoolean.True;
			column2.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_mtRevaluationTableView = _mtRevaluationGrid.View;
		_mtRevaluationTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTRevaluationTableView_CellValueChanged, this).Handler;
	}

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}

	protected override bool AllowNewAfterSave()
	{
		return false;
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
		ReportBarButtonItem barButtonGoodsOperationsJournal = _barButtonGoodsOperationsJournal;
		num = 2;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsOperationsJournal.PropertyWithValues = list2;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
	}

	protected override void OnNew()
	{
		base.OnNew();
		if ((PO.MTList == null || PO.MTList.Count == 0) && _baseDocumentControl == null)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.RevMaterialFromRemains, 64);
		}
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		_exchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(Settings.ParametersService.MTCurr(), _date.EditValue.Value);
		_docNum.EditValue = Settings.ParametersService.MTRevaluationNumber();
		_colsVewType.Code = MTRvlGridColsViewMethodsSE.NCCSummWithVAT;
		_newPriceFormationType.Code = MTRvlNewPriceFormationTypesSE.Coefficent;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_exchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(Settings.ParametersService.MTCurr(), _date.EditValue.Value);
		_setFocusOnActivation = _docNum;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
			bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
			_date.IsReadOnly = flag2;
			_baseDocumentControl.IsReadOnly = flag2;
			_distrSum.IsReadOnly = flag2;
			_distrSumBy.IsEnabled = !flag2;
			_priceType.IsReadOnly = flag2;
			_price.IsReadOnly = flag2;
			_coeff.IsReadOnly = flag2;
			_colsVewType.IsReadOnly = flag2;
			_newPriceFormationType.IsReadOnly = flag2;
			_mtRevaluationGrid.IsReadOnly = flag2;
		}
	}

	private void ChangeColumnsVisibilityForAverageCost()
	{
		if (_mtRevaluationList.Count != 0 && PO.MTList[0].ExpenseMethod == "3")
		{
			_quantityCol.AllowEditing = DefaultBoolean.False;
			_incomeDateCol.Visible = false;
			_incomeDateCol.ShowInColumnChooser = false;
			_incomeDocDescrCol.Visible = false;
			_incomeDocDescrCol.ShowInColumnChooser = false;
			_partnerCol.Visible = false;
			_partnerCol.ShowInColumnChooser = false;
			_contractCol.Visible = false;
			_contractCol.ShowInColumnChooser = false;
			_useLifeCol.Visible = false;
			_useLifeCol.ShowInColumnChooser = false;
			_countryCol.Visible = false;
			_countryCol.ShowInColumnChooser = false;
			_producerCol.Visible = false;
			_producerCol.ShowInColumnChooser = false;
			_property1Col.Visible = false;
			_property1Col.ShowInColumnChooser = false;
			_property2Col.Visible = false;
			_property2Col.ShowInColumnChooser = false;
			_costCol.Header = ArmSoft.SME.Trade.Common.Resources.Currentprice;
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		NewPriceFormationOpType_ValueChanged();
		GridViewCols_EditValueChanged();
		ChangeColumnsVisibilityForAverageCost();
		_baseISN = PO.BaseISN;
		if (_baseISN == Guid.Empty)
		{
			BaseDocument = null;
		}
		else
		{
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
		}
		_mtRevaluationList.Clear();
		foreach (MTRevaluation.MTRevaluationSpecificationRow mT in PO.MTList)
		{
			MTRevaluationSpecificationRow mTRevaluationSpecificationRow = new MTRevaluationSpecificationRow();
			mTRevaluationSpecificationRow.Storage = mT.Storage;
			mTRevaluationSpecificationRow.MTCode = mT.MTCode;
			mTRevaluationSpecificationRow.MTID = mT.MTID;
			mTRevaluationSpecificationRow.MTName = mT.MTName;
			mTRevaluationSpecificationRow.UnitCode = mT.UnitCode;
			mTRevaluationSpecificationRow.UnitBrief = mT.UnitBrief;
			mTRevaluationSpecificationRow.Quantity = mT.Quantity;
			mTRevaluationSpecificationRow.Cost = mT.Cost;
			mTRevaluationSpecificationRow.CostWithVAT = mT.CostWithVAT;
			mTRevaluationSpecificationRow.CurCostWithVAT = mT.CurCostWithVAT;
			mTRevaluationSpecificationRow.NewCost = mT.NewCost;
			mTRevaluationSpecificationRow.NewCostWithVAT = mT.NewCostWithVAT;
			mTRevaluationSpecificationRow.NewCurCostWithVAT = mT.NewCurCostWithVAT;
			mTRevaluationSpecificationRow.RowID = mT.RowID;
			mTRevaluationSpecificationRow.RowNum = mT.RowNum;
			mTRevaluationSpecificationRow.IncomeDocDescr = mT.IncomeDocDescr;
			mTRevaluationSpecificationRow.IncomeDate = mT.IncomeDate;
			mTRevaluationSpecificationRow.PartID = mT.PartID;
			mTRevaluationSpecificationRow.PartnerName = mT.PartnerName;
			mTRevaluationSpecificationRow.ContractCode = mT.ContractCode;
			mTRevaluationSpecificationRow.ContractName = mT.ContractName;
			mTRevaluationSpecificationRow.UseLife = mT.UseLife;
			mTRevaluationSpecificationRow.CountryCode = mT.CountryCode;
			mTRevaluationSpecificationRow.CountryName = mT.CountryName;
			mTRevaluationSpecificationRow.ProducerCode = mT.ProducerCode;
			mTRevaluationSpecificationRow.ProducerName = mT.ProducerName;
			mTRevaluationSpecificationRow.Property1Code = mT.Property1Code;
			mTRevaluationSpecificationRow.Property1Value = mT.Property1Value;
			mTRevaluationSpecificationRow.Property2Code = mT.Property2Code;
			mTRevaluationSpecificationRow.Property2Value = mT.Property2Value;
			mTRevaluationSpecificationRow.PartyBase = mT.PartyBase;
			mTRevaluationSpecificationRow.PartyRow = mT.PartyRow;
			_mtRevaluationList.Add(mTRevaluationSpecificationRow);
		}
		_mtRevaluationGrid.ItemsSource = _mtRevaluationList;
		if (!(_baseISN == Guid.Empty) && !PO.StoredInDb)
		{
			CreateBasedOnDoc();
		}
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	private void OpType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_newPriceFormationType.DoValidate() && e.NewValue != null && !string.IsNullOrWhiteSpace((string)e.NewValue))
		{
			NewPriceFormationOpType_ValueChanged();
		}
	}

	private void NewPriceFormationOpType_ValueChanged()
	{
		switch (_newPriceFormationType.Code)
		{
		case "1":
			_lgCoefficent.Visibility = Visibility.Visible;
			_lgPriceType.Visibility = Visibility.Collapsed;
			_lgDistrSum.Visibility = Visibility.Collapsed;
			_lgPrice.Visibility = Visibility.Collapsed;
			break;
		case "4":
			_lgCoefficent.Visibility = Visibility.Collapsed;
			_lgPriceType.Visibility = Visibility.Visible;
			_lgDistrSum.Visibility = Visibility.Collapsed;
			_lgPrice.Visibility = Visibility.Collapsed;
			break;
		case "2":
			_lgCoefficent.Visibility = Visibility.Collapsed;
			_lgPriceType.Visibility = Visibility.Collapsed;
			_lgDistrSum.Visibility = Visibility.Visible;
			_lgPrice.Visibility = Visibility.Collapsed;
			break;
		case "3":
			_lgCoefficent.Visibility = Visibility.Collapsed;
			_lgPriceType.Visibility = Visibility.Collapsed;
			_lgDistrSum.Visibility = Visibility.Collapsed;
			_lgPrice.Visibility = Visibility.Visible;
			break;
		}
		_coeff.IsRequired = _newPriceFormationType.Code == "1";
		_coeff.DoValidate();
		_priceType.IsRequired = _newPriceFormationType.Code == "4";
		_priceType.DoValidate();
		_distrSum.IsRequired = _newPriceFormationType.Code == "2";
		_distrSum.DoValidate();
		_distrSumBy.IsRequired = _distrSum.IsRequired;
		_distrSumBy.DoValidate();
		_price.IsRequired = _newPriceFormationType.Code == "3";
		_price.DoValidate();
	}

	private void NewPriceFormation_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_mtRevaluationList.Count <= 0 || !_newPriceFormationType.DoValidate() || string.IsNullOrWhiteSpace(_newPriceFormationType.Code))
			{
				return;
			}
			switch (_newPriceFormationType.Code)
			{
			case "1":
				FillPriceByCoefficent();
				break;
			case "4":
				if (string.IsNullOrWhiteSpace(_priceType.Code))
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Trade.Common.Resources.PriceType));
				}
				FillPriceByPriceType();
				break;
			case "2":
				FillPriceByDistrSum();
				break;
			case "3":
				FillPrice();
				break;
			}
		}
		catch (ASException ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FillPrice()
	{
		if (!_price.DoValidate())
		{
			return;
		}
		decimal value = _price.Value;
		foreach (MTRevaluationSpecificationRow mtRevaluation in _mtRevaluationList)
		{
			switch (_colsVewType.Code)
			{
			case "1":
			case "4":
				mtRevaluation.NewCost = value;
				if (mtRevaluation.Cost != 0m)
				{
					mtRevaluation.NewCostWithVAT = (mtRevaluation.NewCost * (mtRevaluation.CostWithVAT / mtRevaluation.Cost)).RoundAs(2);
				}
				else
				{
					mtRevaluation.NewCostWithVAT = mtRevaluation.NewCost + PriceHelper.CalcVATAmountByType(mtRevaluation.NewCost, "1", mtRevaluation.VAT);
				}
				if (mtRevaluation.CostWithVAT != 0m)
				{
					mtRevaluation.NewCurCostWithVAT = (mtRevaluation.NewCostWithVAT * (mtRevaluation.CurCostWithVAT / mtRevaluation.CostWithVAT)).RoundAs(2);
				}
				else if (_exchangeRate.Rate != 0m)
				{
					mtRevaluation.NewCurCostWithVAT = (mtRevaluation.NewCurCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
				}
				break;
			case "2":
				mtRevaluation.NewCostWithVAT = value;
				if (mtRevaluation.CostWithVAT != 0m)
				{
					mtRevaluation.NewCost = (mtRevaluation.NewCostWithVAT * (mtRevaluation.Cost / mtRevaluation.CostWithVAT)).RoundAs(2);
				}
				else
				{
					mtRevaluation.NewCost = mtRevaluation.NewCostWithVAT - PriceHelper.CalcVATAmountByType(mtRevaluation.NewCostWithVAT, "1", mtRevaluation.VAT);
				}
				if (mtRevaluation.CostWithVAT != 0m)
				{
					mtRevaluation.NewCurCostWithVAT = (mtRevaluation.NewCostWithVAT * (mtRevaluation.CurCostWithVAT / mtRevaluation.CostWithVAT)).RoundAs(2);
				}
				else if (_exchangeRate.Rate != 0m)
				{
					mtRevaluation.NewCurCostWithVAT = (mtRevaluation.NewCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
				}
				break;
			case "3":
				mtRevaluation.NewCurCostWithVAT = value;
				if (mtRevaluation.CurCostWithVAT != 0m)
				{
					mtRevaluation.NewCostWithVAT = (mtRevaluation.NewCurCostWithVAT * (mtRevaluation.CostWithVAT / mtRevaluation.CurCostWithVAT)).RoundAs(2);
				}
				else if (_exchangeRate.Rate != 0m)
				{
					mtRevaluation.NewCostWithVAT = (mtRevaluation.NewCurCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
				}
				if (mtRevaluation.CostWithVAT != 0m)
				{
					mtRevaluation.NewCost = (mtRevaluation.NewCostWithVAT * (mtRevaluation.Cost / mtRevaluation.CostWithVAT)).RoundAs(2);
				}
				else
				{
					mtRevaluation.NewCost = mtRevaluation.NewCostWithVAT - PriceHelper.CalcVATAmountByType(mtRevaluation.NewCostWithVAT, "1", mtRevaluation.VAT);
				}
				break;
			}
		}
		_mtRevaluationGrid.Grid.RefreshData();
	}

	private void FillPriceByCoefficent()
	{
		if (!_coeff.DoValidate())
		{
			return;
		}
		decimal value = _coeff.Value;
		foreach (MTRevaluationSpecificationRow mtRevaluation in _mtRevaluationList)
		{
			mtRevaluation.NewCost = (mtRevaluation.Cost * value).RoundAs(2);
			mtRevaluation.NewCostWithVAT = (mtRevaluation.CostWithVAT * value).RoundAs(2);
			mtRevaluation.NewCurCostWithVAT = (mtRevaluation.CurCostWithVAT * value).RoundAs(2);
		}
		_mtRevaluationGrid.Grid.RefreshData();
	}

	private void FillPriceByPriceType()
	{
		if (!_date.DoValidate() || !_priceType.DoValidate())
		{
			return;
		}
		string priceListRowVATType = "";
		PriceHelper.GetPriceListRowVATType(_priceType.Code, out priceListRowVATType, out var _);
		foreach (MTRevaluationSpecificationRow mtRevaluation in _mtRevaluationList)
		{
			mtRevaluation.NewCost = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceType.Code, "3", mtRevaluation.MTID, mtRevaluation.UnitCode, _date.EditValue.Value, "AMD");
			if (priceListRowVATType == "2")
			{
				mtRevaluation.NewCostWithVAT = mtRevaluation.NewCost + PriceHelper.CalcVATAmountByType(mtRevaluation.NewCost, "1", mtRevaluation.VAT);
			}
			else
			{
				mtRevaluation.NewCostWithVAT = mtRevaluation.NewCost;
			}
			if (_exchangeRate.Rate != 0m)
			{
				mtRevaluation.NewCurCostWithVAT = (mtRevaluation.NewCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
			}
		}
		_mtRevaluationGrid.Grid.RefreshData();
	}

	private void FillPriceByDistrSum()
	{
		if (!_distrSum.DoValidate() || !_distrSumBy.DoValidate())
		{
			return;
		}
		decimal value = _distrSum.Value;
		decimal num = 0m;
		decimal num2 = 0m;
		if (_distrSumBy.Code == "2")
		{
			decimal num3 = _mtRevaluationList.Sum((MTRevaluationSpecificationRow row) => row.Quantity);
			if (num3 != 0m)
			{
				foreach (MTRevaluationSpecificationRow mtRevaluation in _mtRevaluationList)
				{
					num2 = mtRevaluation.Quantity / num3 * value;
					if (!(mtRevaluation.Quantity != 0m))
					{
						continue;
					}
					switch (_colsVewType.Code)
					{
					case "1":
					case "4":
						mtRevaluation.NewCost = (mtRevaluation.Cost + num2 / mtRevaluation.Quantity).RoundAs(2);
						if (mtRevaluation.Cost != 0m)
						{
							mtRevaluation.NewCostWithVAT = (mtRevaluation.NewCost * (mtRevaluation.CostWithVAT / mtRevaluation.Cost)).RoundAs(2);
						}
						else
						{
							mtRevaluation.NewCostWithVAT = mtRevaluation.NewCost + PriceHelper.CalcVATAmountByType(mtRevaluation.NewCost, "1", mtRevaluation.VAT);
						}
						if (mtRevaluation.CostWithVAT != 0m)
						{
							mtRevaluation.NewCurCostWithVAT = (mtRevaluation.NewCostWithVAT * (mtRevaluation.CurCostWithVAT / mtRevaluation.CostWithVAT)).RoundAs(2);
						}
						else if (_exchangeRate.Rate != 0m)
						{
							mtRevaluation.NewCurCostWithVAT = (mtRevaluation.NewCurCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
						}
						break;
					case "2":
						mtRevaluation.NewCostWithVAT = (mtRevaluation.CostWithVAT + num2 / mtRevaluation.Quantity).RoundAs(2);
						if (mtRevaluation.CostWithVAT != 0m)
						{
							mtRevaluation.NewCost = (mtRevaluation.NewCostWithVAT * (mtRevaluation.Cost / mtRevaluation.CostWithVAT)).RoundAs(2);
						}
						else
						{
							mtRevaluation.NewCost = mtRevaluation.NewCostWithVAT - PriceHelper.CalcVATAmountByType(mtRevaluation.NewCostWithVAT, "1", mtRevaluation.VAT);
						}
						if (mtRevaluation.CostWithVAT != 0m)
						{
							mtRevaluation.NewCurCostWithVAT = (mtRevaluation.NewCostWithVAT * (mtRevaluation.CurCostWithVAT / mtRevaluation.CostWithVAT)).RoundAs(2);
						}
						else if (_exchangeRate.Rate != 0m)
						{
							mtRevaluation.NewCurCostWithVAT = (mtRevaluation.NewCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
						}
						break;
					case "3":
						mtRevaluation.NewCurCostWithVAT = (mtRevaluation.CurCostWithVAT + num2 / mtRevaluation.Quantity).RoundAs(2);
						if (mtRevaluation.CostWithVAT != 0m)
						{
							mtRevaluation.NewCostWithVAT = (mtRevaluation.NewCurCostWithVAT * mtRevaluation.CurCostWithVAT / mtRevaluation.CostWithVAT).RoundAs(2);
						}
						else if (_exchangeRate.Rate != 0m)
						{
							mtRevaluation.NewCostWithVAT = (mtRevaluation.NewCurCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
						}
						if (mtRevaluation.CostWithVAT != 0m)
						{
							mtRevaluation.NewCost = (mtRevaluation.NewCostWithVAT * (mtRevaluation.Cost / mtRevaluation.CostWithVAT)).RoundAs(2);
						}
						else
						{
							mtRevaluation.NewCost = mtRevaluation.NewCostWithVAT - PriceHelper.CalcVATAmountByType(mtRevaluation.NewCostWithVAT, "1", mtRevaluation.VAT);
						}
						break;
					}
				}
				num += num2;
			}
		}
		else if (_distrSumBy.Code == "1")
		{
			decimal num4 = _mtRevaluationList.Sum((MTRevaluationSpecificationRow row) => row.Quantity * row.Cost);
			if (num4 != 0m)
			{
				foreach (MTRevaluationSpecificationRow mtRevaluation2 in _mtRevaluationList)
				{
					num2 = mtRevaluation2.Cost * mtRevaluation2.Quantity / num4 * value;
					if (mtRevaluation2.Quantity != 0m)
					{
						switch (_colsVewType.Code)
						{
						case "1":
						case "4":
							mtRevaluation2.NewCost = (mtRevaluation2.Cost + num2 / mtRevaluation2.Quantity).RoundAs(2);
							if (mtRevaluation2.Cost != 0m)
							{
								mtRevaluation2.NewCostWithVAT = (mtRevaluation2.NewCost * (mtRevaluation2.CostWithVAT / mtRevaluation2.Cost)).RoundAs(2);
							}
							else
							{
								mtRevaluation2.NewCostWithVAT = mtRevaluation2.NewCost + PriceHelper.CalcVATAmountByType(mtRevaluation2.NewCost, "1", mtRevaluation2.VAT);
							}
							if (mtRevaluation2.CostWithVAT != 0m)
							{
								mtRevaluation2.NewCurCostWithVAT = (mtRevaluation2.NewCostWithVAT * (mtRevaluation2.CurCostWithVAT / mtRevaluation2.CostWithVAT)).RoundAs(2);
							}
							else if (_exchangeRate.Rate != 0m)
							{
								mtRevaluation2.NewCurCostWithVAT = (mtRevaluation2.NewCurCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
							}
							break;
						case "2":
							mtRevaluation2.NewCostWithVAT = (mtRevaluation2.CostWithVAT + num2 / mtRevaluation2.Quantity).RoundAs(2);
							if (mtRevaluation2.CostWithVAT != 0m)
							{
								mtRevaluation2.NewCost = (mtRevaluation2.NewCostWithVAT * (mtRevaluation2.Cost / mtRevaluation2.CostWithVAT)).RoundAs(2);
							}
							else
							{
								mtRevaluation2.NewCost = mtRevaluation2.NewCostWithVAT - PriceHelper.CalcVATAmountByType(mtRevaluation2.NewCostWithVAT, "1", mtRevaluation2.VAT);
							}
							if (mtRevaluation2.CostWithVAT != 0m)
							{
								mtRevaluation2.NewCurCostWithVAT = (mtRevaluation2.NewCostWithVAT * (mtRevaluation2.CurCostWithVAT / mtRevaluation2.CostWithVAT)).RoundAs(2);
							}
							else if (_exchangeRate.Rate != 0m)
							{
								mtRevaluation2.NewCurCostWithVAT = (mtRevaluation2.NewCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
							}
							break;
						case "3":
							mtRevaluation2.NewCurCostWithVAT = (mtRevaluation2.CurCostWithVAT + num2 / mtRevaluation2.Quantity).RoundAs(2);
							if (mtRevaluation2.CostWithVAT != 0m)
							{
								mtRevaluation2.NewCostWithVAT = (mtRevaluation2.NewCurCostWithVAT * mtRevaluation2.CurCostWithVAT / mtRevaluation2.CostWithVAT).RoundAs(2);
							}
							else if (_exchangeRate.Rate != 0m)
							{
								mtRevaluation2.NewCostWithVAT = (mtRevaluation2.NewCurCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
							}
							if (mtRevaluation2.CostWithVAT != 0m)
							{
								mtRevaluation2.NewCost = (mtRevaluation2.NewCostWithVAT * (mtRevaluation2.Cost / mtRevaluation2.CostWithVAT)).RoundAs(2);
							}
							else
							{
								mtRevaluation2.NewCost = mtRevaluation2.NewCostWithVAT - PriceHelper.CalcVATAmountByType(mtRevaluation2.NewCostWithVAT, "1", mtRevaluation2.VAT);
							}
							break;
						}
					}
					num += num2;
				}
			}
		}
		if (num != value)
		{
			ObservableCollection<MTRevaluationSpecificationRow> mtRevaluationList = _mtRevaluationList;
			if (mtRevaluationList[mtRevaluationList.Count - 1].Quantity != 0m)
			{
				ObservableCollection<MTRevaluationSpecificationRow> mtRevaluationList2 = _mtRevaluationList;
				MTRevaluationSpecificationRow mTRevaluationSpecificationRow = mtRevaluationList2[mtRevaluationList2.Count - 1];
				ObservableCollection<MTRevaluationSpecificationRow> mtRevaluationList3 = _mtRevaluationList;
				decimal newCost = mtRevaluationList3[mtRevaluationList3.Count - 1].NewCost;
				ObservableCollection<MTRevaluationSpecificationRow> mtRevaluationList4 = _mtRevaluationList;
				decimal num5 = newCost * mtRevaluationList4[mtRevaluationList4.Count - 1].Quantity + Math.Abs(value - num);
				ObservableCollection<MTRevaluationSpecificationRow> mtRevaluationList5 = _mtRevaluationList;
				mTRevaluationSpecificationRow.NewCost = (num5 / mtRevaluationList5[mtRevaluationList5.Count - 1].Quantity).RoundAs(2);
			}
		}
		_mtRevaluationGrid.Grid.RefreshData();
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	private void MTRevaluationGrid_DeletedRow(object sender, EventArgs e)
	{
		_mtRevaluationGrid.Grid.RefreshData();
	}

	private void MTRevaluationTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTRevaluationSpecificationRow mTRevaluationSpecificationRow = (MTRevaluationSpecificationRow)_mtRevaluationGrid.Grid.GetRow(e.RowHandle);
		if (mTRevaluationSpecificationRow != null)
		{
			if (e.Column == _newCostCol)
			{
				UpdatePrices(CommonConst.SummaTypes.NCCSummWithoutVAT, mTRevaluationSpecificationRow);
			}
			else if (e.Column == _newCostWithVATCol)
			{
				UpdatePrices(CommonConst.SummaTypes.NCCSummWithVAT, mTRevaluationSpecificationRow);
			}
			else if (e.Column == _newCurCostWithVATCol)
			{
				UpdatePrices(CommonConst.SummaTypes.CurrSummWithVAT, mTRevaluationSpecificationRow);
			}
			_mtRevaluationGrid.Grid.RefreshData();
		}
	}

	private void UpdatePrices(CommonConst.SummaTypes costType, MTRevaluationSpecificationRow mtRevaluationlGridRow)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtRevaluationlGridRow.MTCode);
		switch (costType)
		{
		case CommonConst.SummaTypes.NCCSummWithoutVAT:
			if (item != null)
			{
				if (item.VAT)
				{
					mtRevaluationlGridRow.NewCostWithVAT = (mtRevaluationlGridRow.NewCost * (100m + _tradeConstVAT) / 100m).RoundAs(2);
				}
				else
				{
					mtRevaluationlGridRow.NewCostWithVAT = mtRevaluationlGridRow.NewCost;
				}
			}
			if (_exchangeRate.Rate != 0m)
			{
				mtRevaluationlGridRow.NewCurCostWithVAT = (mtRevaluationlGridRow.NewCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
			}
			break;
		case CommonConst.SummaTypes.NCCSummWithVAT:
			if (item != null)
			{
				if (item.VAT)
				{
					mtRevaluationlGridRow.NewCost = (mtRevaluationlGridRow.NewCostWithVAT * 100m / (100m + _tradeConstVAT)).RoundAs(2);
				}
				else
				{
					mtRevaluationlGridRow.NewCost = mtRevaluationlGridRow.NewCostWithVAT;
				}
			}
			if (_exchangeRate.Rate != 0m)
			{
				mtRevaluationlGridRow.NewCurCostWithVAT = (mtRevaluationlGridRow.NewCostWithVAT * _exchangeRate.Base / _exchangeRate.Rate).RoundAs(2);
			}
			break;
		case CommonConst.SummaTypes.CurrSummWithVAT:
			if (_exchangeRate.Base != 0m)
			{
				mtRevaluationlGridRow.NewCostWithVAT = (mtRevaluationlGridRow.NewCurCostWithVAT * _exchangeRate.Rate / _exchangeRate.Base).RoundAs(2);
			}
			if (item != null)
			{
				if (item.VAT)
				{
					mtRevaluationlGridRow.NewCost = (mtRevaluationlGridRow.NewCostWithVAT * 100m / (100m + _tradeConstVAT)).RoundAs(2);
				}
				else
				{
					mtRevaluationlGridRow.NewCost = mtRevaluationlGridRow.NewCostWithVAT;
				}
			}
			break;
		}
	}

	private void GridCols_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		GridViewCols_EditValueChanged();
	}

	private void GridViewCols_EditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_colsVewType.Code))
		{
			switch (_colsVewType.Code)
			{
			case "1":
				_mtRevaluationGrid.Grid.Columns["Cost"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["CostWithVAT"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["CurCostWithVAT"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["NewCost"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["NewCostWithVAT"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["NewCurCostWithVAT"].Visible = false;
				break;
			case "2":
				_mtRevaluationGrid.Grid.Columns["Cost"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["CostWithVAT"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["CurCostWithVAT"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["NewCost"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["NewCostWithVAT"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["NewCurCostWithVAT"].Visible = false;
				break;
			case "3":
				_mtRevaluationGrid.Grid.Columns["Cost"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["CostWithVAT"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["CurCostWithVAT"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["NewCost"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["NewCostWithVAT"].Visible = false;
				_mtRevaluationGrid.Grid.Columns["NewCurCostWithVAT"].Visible = true;
				break;
			case "4":
				_mtRevaluationGrid.Grid.Columns["Cost"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["CostWithVAT"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["CurCostWithVAT"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["NewCost"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["NewCostWithVAT"].Visible = true;
				_mtRevaluationGrid.Grid.Columns["NewCurCostWithVAT"].Visible = true;
				break;
			}
		}
		int num = 0;
		foreach (GridColumn column in _mtRevaluationGrid.Grid.Columns)
		{
			if (column.Visible)
			{
				column.VisibleIndex = num;
				num++;
			}
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.BaseISN = BaseISN;
		PO.MTList.Clear();
		foreach (MTRevaluationSpecificationRow mtRevaluation in _mtRevaluationList)
		{
			MTRevaluation.MTRevaluationSpecificationRow mTRevaluationSpecificationRow = new MTRevaluation.MTRevaluationSpecificationRow();
			mTRevaluationSpecificationRow.Storage = mtRevaluation.Storage;
			mTRevaluationSpecificationRow.MTCode = mtRevaluation.MTCode;
			mTRevaluationSpecificationRow.MTID = mtRevaluation.MTID;
			mTRevaluationSpecificationRow.UnitCode = mtRevaluation.UnitCode;
			mTRevaluationSpecificationRow.Quantity = mtRevaluation.Quantity;
			mTRevaluationSpecificationRow.Cost = mtRevaluation.Cost;
			mTRevaluationSpecificationRow.CostWithVAT = mtRevaluation.CostWithVAT;
			mTRevaluationSpecificationRow.CurCostWithVAT = mtRevaluation.CurCostWithVAT;
			mTRevaluationSpecificationRow.NewCost = mtRevaluation.NewCost;
			mTRevaluationSpecificationRow.NewCostWithVAT = mtRevaluation.NewCostWithVAT;
			mTRevaluationSpecificationRow.NewCurCostWithVAT = mtRevaluation.NewCurCostWithVAT;
			mTRevaluationSpecificationRow.RowID = mtRevaluation.RowID;
			mTRevaluationSpecificationRow.RowNum = mtRevaluation.RowNum;
			mTRevaluationSpecificationRow.MTName = mtRevaluation.MTName;
			mTRevaluationSpecificationRow.UnitBrief = mtRevaluation.UnitBrief;
			mTRevaluationSpecificationRow.IncomeDate = mtRevaluation.IncomeDate;
			mTRevaluationSpecificationRow.PartID = mtRevaluation.PartID;
			mTRevaluationSpecificationRow.ContractCode = mtRevaluation.ContractCode;
			mTRevaluationSpecificationRow.UseLife = mtRevaluation.UseLife;
			mTRevaluationSpecificationRow.CountryCode = mtRevaluation.CountryCode;
			mTRevaluationSpecificationRow.ProducerCode = mtRevaluation.ProducerCode;
			mTRevaluationSpecificationRow.Property1Code = mtRevaluation.Property1Code;
			mTRevaluationSpecificationRow.Property2Code = mtRevaluation.Property2Code;
			mTRevaluationSpecificationRow.PartyBase = mtRevaluation.PartyBase;
			mTRevaluationSpecificationRow.PartyRow = mtRevaluation.PartyRow;
			PO.MTList.Add(mTRevaluationSpecificationRow);
		}
	}

	protected override void AfterStore(StoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		_barButtonJumpTo.IsEnabled = true;
		_barButtonGoodsOperationsJournal.IsEnabled = true;
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_mtRevaluationGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double DefaultWidth, double DefaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(DefaultWidth, DefaultHeight, rk);
		_mtRevaluationGrid.RestoreSettings(rk);
	}

	private void BaseDocumentControl_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		DocumentsJournal documentsJournal = (DocumentsJournal)e.DataView;
		documentsJournal.DialogTop = e.Top;
		documentsJournal.DialogLeft = e.Left;
		if (!e.DataView.ShowDialog())
		{
			e.Cancel = true;
		}
	}

	private void BaseDocumentDelete_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		_baseDocumentControl.KeyValue = Guid.Empty;
		_baseDocumentControl.DoValidate();
	}

	private void BaseDocument_EditKeyValueChanged(object sender, EditKeyValueChangedEventArgs e)
	{
		if (e.KeyValue != null)
		{
			BaseISN = (Guid)e.KeyValue;
		}
		else
		{
			BaseISN = Guid.Empty;
		}
	}

	private void BaseDocumentSearch_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		if (_baseISN != Guid.Empty)
		{
			Settings.DocumentService.ShowWindow(BaseISN, WindowViewMode.Edit);
		}
	}

	private void CreateBasedOnDoc()
	{
		_mtRevaluationList.Clear();
		if (BaseDocument.POType != DocumentTypeSE.MTMove.Value && BaseDocument.POType != DocumentTypeSE.MTMoveWithConfirmation.Value)
		{
			return;
		}
		MTMoveWithConfirmationBase mTMoveWithConfirmationBase = (MTMoveWithConfirmationBase)BaseDocument;
		int num = 0;
		if (mTMoveWithConfirmationBase == null)
		{
			return;
		}
		List<PartyRow> mTExpDocSelectedPartiesList = mTMoveWithConfirmationBase.MTExpDocSelectedPartiesList;
		bool flag = false;
		bool flag2 = false;
		foreach (PartyRow row in mTExpDocSelectedPartiesList)
		{
			MTMoveBase.MTMoveSpecificationRow mTMoveSpecificationRow = mTMoveWithConfirmationBase.MTList.Where((MTMoveBase.MTMoveSpecificationRow grow) => grow.RowID == row.GridRow).FirstOrDefault();
			MTRevaluationSpecificationRow mTRevaluationSpecificationRow = new MTRevaluationSpecificationRow();
			decimal num2 = 1m;
			TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(Settings.ParametersService.MTCurr(), mTMoveWithConfirmationBase.DocDate);
			mTRevaluationSpecificationRow.MTCode = mTMoveSpecificationRow.MTCode;
			mTRevaluationSpecificationRow.MTID = mTMoveSpecificationRow.MTID;
			mTRevaluationSpecificationRow.Storage = mTMoveWithConfirmationBase.StorageIncome;
			mTRevaluationSpecificationRow.MTName = mTMoveSpecificationRow.MTName;
			if (row.ExpMethod == "3")
			{
				mTRevaluationSpecificationRow.Quantity = 0m;
				flag = true;
			}
			else
			{
				flag2 = true;
				mTRevaluationSpecificationRow.Quantity = row.ExpQty;
			}
			mTRevaluationSpecificationRow.UnitCode = mTMoveSpecificationRow.UnitCode;
			mTRevaluationSpecificationRow.UnitBrief = mTMoveSpecificationRow.UnitBrief;
			mTRevaluationSpecificationRow.Cost = row.Cost;
			mTRevaluationSpecificationRow.CostWithVAT = (mTRevaluationSpecificationRow.Cost * Settings.ParametersService.VAT() / 100m).RoundAs(2) + mTRevaluationSpecificationRow.Cost;
			mTRevaluationSpecificationRow.CurCostWithVAT = mTRevaluationSpecificationRow.Cost / num2;
			mTRevaluationSpecificationRow.NewCost = mTRevaluationSpecificationRow.Cost;
			mTRevaluationSpecificationRow.NewCostWithVAT = mTRevaluationSpecificationRow.CostWithVAT;
			mTRevaluationSpecificationRow.NewCurCostWithVAT = mTRevaluationSpecificationRow.CurCostWithVAT;
			mTRevaluationSpecificationRow.IncomeDate = row.IncomeDate;
			mTRevaluationSpecificationRow.IncomeDocDescr = row.Comment;
			mTRevaluationSpecificationRow.PartyBase = row.Base;
			mTRevaluationSpecificationRow.PartyRow = row.Row;
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(row.PartnerCode);
			PartnerCacheRow.ContractRow contractRow = null;
			if (item != null)
			{
				mTRevaluationSpecificationRow.PartID = item.Id;
				mTRevaluationSpecificationRow.PartnerName = item.Caption;
				contractRow = TradeSettings.TradeCacheService.Partner.GetContract(item, row.Contract);
			}
			else
			{
				mTRevaluationSpecificationRow.PartID = -1;
				mTRevaluationSpecificationRow.PartnerName = "";
			}
			if (contractRow != null)
			{
				mTRevaluationSpecificationRow.ContractCode = contractRow.Contract;
				mTRevaluationSpecificationRow.ContractName = contractRow.Caption;
			}
			else
			{
				mTRevaluationSpecificationRow.ContractCode = "";
				mTRevaluationSpecificationRow.ContractName = "";
			}
			mTRevaluationSpecificationRow.ProducerName = row.Producer;
			mTRevaluationSpecificationRow.CountryCode = ((row.Country == null) ? "" : row.Country);
			mTRevaluationSpecificationRow.UseLife = row.UseableDate;
			mTRevaluationSpecificationRow.Property1Code = ((row.Property1Code == null) ? "" : row.Property1Code);
			mTRevaluationSpecificationRow.Property1Value = ((row.Property1 == null) ? "" : row.Property1);
			mTRevaluationSpecificationRow.Property2Code = ((row.Property2Code == null) ? "" : row.Property2Code);
			mTRevaluationSpecificationRow.Property2Value = ((row.Property2 == null) ? "" : row.Property2);
			mTRevaluationSpecificationRow.RowID = num;
			num++;
			_mtRevaluationList.Add(mTRevaluationSpecificationRow);
		}
		if (flag & flag2)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.AvgWeigtItemsRevaluatedSeparately, ArmSoft.SME.Trade.Common.Resources.MaterialRevaluation);
		}
	}
}
