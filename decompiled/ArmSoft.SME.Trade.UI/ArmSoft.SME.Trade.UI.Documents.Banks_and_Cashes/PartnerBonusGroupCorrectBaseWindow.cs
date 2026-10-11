using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
using ArmSoft.SME.Trade.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Trade.UI.Documents.Banks_and_Cashes;

public abstract class PartnerBonusGroupCorrectBaseWindow<T> : TradeDocumentWindow<T> where T : PartnerBonusRemGroupCorrectBase, new()
{
	public class PartBonusGroupCorrectSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private string partCode = "";

		[ObservableProperty]
		private string partName = "";

		[ObservableProperty]
		private string contract = "";

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private decimal summaNCC;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string PartCode
		{
			get
			{
				return partCode;
			}
			[MemberNotNull("partCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(partCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PartCode);
					partCode = value;
					OnPartCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PartCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string PartName
		{
			get
			{
				return partName;
			}
			[MemberNotNull("partName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(partName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PartName);
					partName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PartName);
				}
			}
		}

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
		public decimal Summa
		{
			get
			{
				return summa;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(summa, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Summa);
					summa = value;
					OnSummaChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Summa);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal SummaNCC
		{
			get
			{
				return summaNCC;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(summaNCC, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SummaNCC);
					summaNCC = value;
					OnSummaNCCChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SummaNCC);
				}
			}
		}

		static PartBonusGroupCorrectSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(PartBonusGroupCorrectSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, ErrorInfo>>
			{
				{
					"PartCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidatePartCode((PartBonusGroupCorrectSpecificationRow)row, info);
					}
				},
				{
					"Contract",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateContract((PartBonusGroupCorrectSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidatePartCode(PartBonusGroupCorrectSpecificationRow row, ErrorInfo info)
		{
			TradePropertyValidator.ValidatePartner(row.PartCode, info, TradeAllDocDefinitions.PartnerGroupCorrect.Grids["PartRemList"].Columns["PartCode"]);
		}

		private static void ValidateContract(PartBonusGroupCorrectSpecificationRow row, ErrorInfo info)
		{
			TradePropertyValidator.ValidateContract(row.PartCode, row.Contract, info, TradeAllDocDefinitions.BonusGroupCorrect.Grids["PartRemList"].Columns["Contract"]);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPartCodeChanged(string value)
		{
			ResetForceValidationBit("PartCode", "Contract");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnContractChanged(string value)
		{
			ResetForceValidationBit("Contract");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSummaChanged(decimal value)
		{
			ResetForceValidationBit("Summa");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSummaNCCChanged(decimal value)
		{
			ResetForceValidationBit("SummaNCC");
		}
	}

	public DropDownControl _currCode;

	public ExchangeRateControl _currExChngRate;

	public DocumentGridControl _partRemGrid;

	protected TableView _partRemTableView;

	public ObservableCollection<PartBonusGroupCorrectSpecificationRow> _partRemList;

	protected DocumentGridColumn _partCodeCol;

	protected DocumentGridColumn _contractCol;

	protected DocumentGridColumn _summaCol;

	protected DocumentGridColumn _summaNCCCol;

	protected ComboBoxEditExt _comment;

	protected DropDownControl _owner;

	protected BarSubItem _barSubItemActions = new BarSubItem();

	protected BarSubItem _barSubItemJournals = new BarSubItem();

	protected ReportBarButtonItem _barButtonJumpTo;

	protected ReportBarButtonItem _barButtonPartnerOperationJournal;

	public PartnerBonusGroupCorrectBaseWindow()
	{
	}

	public PartnerBonusGroupCorrectBaseWindow(T doc)
		: base(doc)
	{
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		_setFocusOnActivation = _date;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_partRemList.Clear();
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_partRemGrid.ItemsSource = _partRemList;
		_partRemList.Clear();
		foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem in PO.PartRemList)
		{
			PartBonusGroupCorrectSpecificationRow item = new PartBonusGroupCorrectSpecificationRow
			{
				PartCode = partRem.PartCode,
				PartName = partRem.PartName,
				Contract = partRem.Contract,
				Summa = partRem.Summa.Value,
				SummaNCC = partRem.SummaNCC.Value
			};
			_partRemList.Add(item);
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.PartRemList.Clear();
		foreach (PartBonusGroupCorrectSpecificationRow partRem in _partRemList)
		{
			PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow item = new PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow
			{
				PartCode = partRem.PartCode,
				PartName = partRem.PartName,
				Contract = partRem.Contract,
				Summa = partRem.Summa,
				SummaNCC = partRem.SummaNCC
			};
			PO.PartRemList.Add(item);
		}
		PO.Summ = Convert.ToDecimal(_summaCol.TotalSummaries[0].Value);
	}

	protected void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	protected void PartRemGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		PartBonusGroupCorrectSpecificationRow partBonusGroupCorrectSpecificationRow = (PartBonusGroupCorrectSpecificationRow)_partRemGrid.Grid.GetRow(e.RowHandle);
		if (partBonusGroupCorrectSpecificationRow != null)
		{
			PartBonusGroupCorrectSpecificationRow partBonusGroupCorrectSpecificationRow2 = new PartBonusGroupCorrectSpecificationRow
			{
				PartCode = partBonusGroupCorrectSpecificationRow.PartCode,
				PartName = partBonusGroupCorrectSpecificationRow.PartName,
				Contract = partBonusGroupCorrectSpecificationRow.Contract,
				Summa = partBonusGroupCorrectSpecificationRow.Summa
			};
			partBonusGroupCorrectSpecificationRow.Clone(partBonusGroupCorrectSpecificationRow2);
			_partRemList.Add(partBonusGroupCorrectSpecificationRow2);
			e.NewRowNumber = _partRemList.Count - 1;
		}
	}

	protected void PartRemTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (e.Column == _contractCol && e.Row != null)
		{
			PartBonusGroupCorrectSpecificationRow partBonusGroupCorrectSpecificationRow = (PartBonusGroupCorrectSpecificationRow)e.Row;
			if (string.IsNullOrWhiteSpace(partBonusGroupCorrectSpecificationRow.PartCode) || TradeSettings.TradeCacheService.Partner.GetItem(partBonusGroupCorrectSpecificationRow.PartCode) == null)
			{
				e.Cancel = true;
			}
		}
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AfterStore(StoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
		if (!Settings.ParametersService.ConnectTob2barmsoftDiscountServer())
		{
			return;
		}
		if (!flag)
		{
			if (!(PO.TransactionISN == Guid.Empty))
			{
				return;
			}
			List<CardTransactionRequest> list = new List<CardTransactionRequest>();
			foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem in PO.PartRemList)
			{
				PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(partRem.PartID.Value, partRem.Contract);
				if (contract != null && contract.IsBonusCard && !string.IsNullOrWhiteSpace(contract.DiscountCard))
				{
					CardTransactionRequest cardTransactionRequest = new CardTransactionRequest();
					cardTransactionRequest.CardNumber = contract.DiscountCard;
					cardTransactionRequest.DocInfo = new CardTransactionRequestDocInfo
					{
						ISN = PO.ISN,
						Type = DocType.BonusCorrection,
						Date = PO.DocDate.ToDateTime(),
						Number = PO.DocNum,
						Amount = PO.Summ.Value,
						CashCode = PO.CashDesk,
						ShiftNumber = PO.CashierShiftNumber,
						User = Settings.LoginService.UserName
					};
					cardTransactionRequest.Rows = new List<CardTransactionRow>
					{
						new CardTransactionRow
						{
							Op = OpType.Correct,
							Amount = partRem.Summa.Value
						}
					};
					list.Add(cardTransactionRequest);
				}
			}
			List<CardTransaction> list2 = DiscountServerService.RecordDiscountServerTransactions(list);
			foreach (CardTransaction item in list2)
			{
				if (item == null)
				{
					continue;
				}
				foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem2 in PO.PartRemList)
				{
					if (partRem2.TransactionISN.HasValue && TradeSettings.TradeCacheService.Partner.GetContract(partRem2.PartID.Value, partRem2.Contract).DiscountCard == item.CardNumber)
					{
						partRem2.TransactionISN = item.ISN;
					}
				}
			}
			if (list2.Count > 0 && list2[0] != null)
			{
				PO.TransactionISN = list2[0].ISN;
				InitContextFunctionsHelper.UpdateDocTransactionISN(PO, list2[0].ISN);
			}
			Dictionary<string, Guid> dictionary = new Dictionary<string, Guid>();
			foreach (CardTransaction item2 in list2)
			{
				dictionary.Add(item2.CardNumber, item2.ISN);
			}
			{
				foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem3 in PO.PartRemList)
				{
					PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(partRem3.PartID.Value, partRem3.Contract);
					if (contract != null && dictionary.ContainsKey(contract.DiscountCard))
					{
						partRem3.TransactionISN = dictionary[contract.DiscountCard];
					}
				}
				return;
			}
		}
		if (State != DocumentStateSE.Processed.Value || !(PO.TransactionISN != Guid.Empty))
		{
			return;
		}
		DiscountServerService.DeleteTransactionsByDocISN(PO.ISN);
		PO.TransactionISN = Guid.Empty;
		foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem4 in PO.PartRemList)
		{
			partRem4.TransactionISN = Guid.Empty;
		}
		InitContextFunctionsHelper.UpdateDocTransactionISN(PO, Guid.Empty);
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	protected void PartRemGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_currCode.Code))
		{
			ControlsHelper.SetFocus(_currCode);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidateEmptyCurrCode);
		}
		if (Settings.CoreCacheService.Currency.GetItem(_currCode.Code) == null)
		{
			ControlsHelper.SetFocus(_currCode);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInCurr, _currCode.Code));
		}
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			PartnerBonusGroupCorrectRowAddEditDialog<T> partnerBonusGroupCorrectRowAddEditDialog = new PartnerBonusGroupCorrectRowAddEditDialog<T>(DialogWindowMode.NewModeWithNext, this);
			partnerBonusGroupCorrectRowAddEditDialog.Invoke();
			if (partnerBonusGroupCorrectRowAddEditDialog.AddedRows != 0)
			{
				_partRemGrid.Grid.RefreshData();
			}
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	protected void PartRemGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			PartBonusGroupCorrectSpecificationRow partBonusGroupCorrectSpecificationRow = (PartBonusGroupCorrectSpecificationRow)_partRemGrid.Grid.GetRow(e.RowHandle);
			if (partBonusGroupCorrectSpecificationRow != null)
			{
				PartnerBonusGroupCorrectRowAddEditDialog<T> partnerBonusGroupCorrectRowAddEditDialog = new PartnerBonusGroupCorrectRowAddEditDialog<T>(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, partBonusGroupCorrectSpecificationRow);
				e.RowEdited = Convert.ToBoolean(partnerBonusGroupCorrectRowAddEditDialog.Invoke());
				if (e.AllowEdit && e.RowEdited)
				{
					_partRemGrid.Grid.RefreshData();
				}
			}
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	protected void PartRemGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			PartBonusGroupCorrectSpecificationRow partBonusGroupCorrectSpecificationRow = (PartBonusGroupCorrectSpecificationRow)_partRemGrid.Grid.GetRow(e.RowHandle);
			if (partBonusGroupCorrectSpecificationRow != null)
			{
				PartnerBonusGroupCorrectRowAddEditDialog<T> partnerBonusGroupCorrectRowAddEditDialog = new PartnerBonusGroupCorrectRowAddEditDialog<T>(DialogWindowMode.NewModeWithNext, this, partBonusGroupCorrectSpecificationRow);
				partnerBonusGroupCorrectRowAddEditDialog.Invoke();
				if (partnerBonusGroupCorrectRowAddEditDialog.AddedRows != 0)
				{
					_partRemGrid.Grid.RefreshData();
				}
			}
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}
}
