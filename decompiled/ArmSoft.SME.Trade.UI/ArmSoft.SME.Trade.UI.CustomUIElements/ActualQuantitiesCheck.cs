using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class ActualQuantitiesCheck
{
	public class ActualQuantitiesCheckRow : DocumentGridRow
	{
		[ObservableProperty]
		public int rowID;

		[ObservableProperty]
		public int mTID = -1;

		[ObservableProperty]
		private string mTCode = string.Empty;

		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private decimal baseQuantity;

		[ObservableProperty]
		private decimal actualQuantity;

		[ObservableProperty]
		private decimal varianceQuantity;

		[ObservableProperty]
		private string varianceState = string.Empty;

		public string MTName { get; set; }

		public string UnitBrief { get; set; }

		public bool UserAdd { get; set; } = true;

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
					OnMTCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTCode);
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
					OnUnitCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal BaseQuantity
		{
			get
			{
				return baseQuantity;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(baseQuantity, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BaseQuantity);
					baseQuantity = value;
					OnBaseQuantityChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BaseQuantity);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal ActualQuantity
		{
			get
			{
				return actualQuantity;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(actualQuantity, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ActualQuantity);
					actualQuantity = value;
					OnActualQuantityChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ActualQuantity);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal VarianceQuantity
		{
			get
			{
				return varianceQuantity;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(varianceQuantity, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VarianceQuantity);
					varianceQuantity = value;
					OnVarianceQuantityChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VarianceQuantity);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string VarianceState
		{
			get
			{
				return varianceState;
			}
			[MemberNotNull("varianceState")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(varianceState, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VarianceState);
					varianceState = value;
					OnVarianceStateChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VarianceState);
				}
			}
		}

		static ActualQuantitiesCheckRow()
		{
			Dictionary<string, Action<DocumentGridRow, ErrorInfo>> validators = new Dictionary<string, Action<DocumentGridRow, ErrorInfo>>
			{
				{
					"MTCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateMTCode((ActualQuantitiesCheckRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateUnitCode((ActualQuantitiesCheckRow)row, info);
					}
				},
				{
					"ActualQuantity",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateQuantity((ActualQuantitiesCheckRow)row, info);
					}
				}
			};
			DocumentGridRow.RegisterPropertyValidators(typeof(ActualQuantitiesCheckRow), cacheValidationResults: true, validators);
		}

		private static void ValidateMTCode(ActualQuantitiesCheckRow row, ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.MTCode, info, Resources.Code);
		}

		private static void ValidateUnitCode(ActualQuantitiesCheckRow row, ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.UnitCode, info, Resources.Unit);
		}

		private static void ValidateQuantity(ActualQuantitiesCheckRow row, ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.ActualQuantity, info, Resources.ActualQTY);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTCodeChanged(string value)
		{
			ResetForceValidationBit("MTCode", "MTCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode", "UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnBaseQuantityChanged(decimal value)
		{
			ResetForceValidationBit("BaseQuantity", "BaseQuantity");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnActualQuantityChanged(decimal value)
		{
			ResetForceValidationBit("ActualQuantity", "ActualQuantity");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnVarianceQuantityChanged(decimal value)
		{
			ResetForceValidationBit("VarianceQuantity", "VarianceQuantity");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnVarianceStateChanged(string value)
		{
			ResetForceValidationBit("VarianceState", "VarianceState");
		}
	}
}
