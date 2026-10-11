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
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Trade.Client.Models;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
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

[DocumentWindow(25)]
public class PartnersGroupRevaluationWindow : TradeDocumentWindow<PartnersGroupRevaluation>
{
	public class RevaluationSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		public int partID = -1;

		[ObservableProperty]
		public string partCode = string.Empty;

		[ObservableProperty]
		public string partName = string.Empty;

		[ObservableProperty]
		public string contract = string.Empty;

		[ObservableProperty]
		public string currCode = string.Empty;

		[ObservableProperty]
		public decimal summa;

		[ObservableProperty]
		public short trans;

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
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Summa);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public short Trans
		{
			get
			{
				return trans;
			}
			set
			{
				if (!EqualityComparer<short>.Default.Equals(trans, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Trans);
					trans = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Trans);
				}
			}
		}

		static RevaluationSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(RevaluationSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"PartCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidatePartCode((RevaluationSpecificationRow)row, info);
					}
				},
				{
					"Contract",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateContract((RevaluationSpecificationRow)row, info);
					}
				},
				{
					"CurrCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCurrCode((RevaluationSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidatePartCode(RevaluationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidatePartner(row.PartCode, info, TradeAllDocDefinitions.PartnersGroupRevaluation.Grids["RevaluationList"].Columns["PartCode"]);
		}

		private static void ValidateContract(RevaluationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateContract(row.PartCode, row.Contract, info, TradeAllDocDefinitions.PartnersGroupRevaluation.Grids["RevaluationList"].Columns["Contract"]);
		}

		private static void ValidateCurrCode(RevaluationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (string.IsNullOrWhiteSpace(row.CurrCode) || row.CurrCode == "AMD")
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Common.Resources.RvlAMDisntCurrency, row.CurrCode);
			}
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
		private void OnCurrCodeChanged(string value)
		{
			ResetForceValidationBit("CurrCode");
		}
	}

	private DocumentGridControl _partRevalGrid;

	private TableView _partRevalTableView;

	private ObservableCollection<RevaluationSpecificationRow> _partRevalList;

	private DocumentGridColumn _partCodeCol;

	private DocumentGridColumn _partNameCol;

	private DocumentGridColumn _contractCol;

	private DocumentGridColumn _currCodeCol;

	private DocumentGridColumn _summaCol;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	protected override double WidthValue => 800.0;

	protected override double HeightValue => 500.0;

	protected override double MinWidthValue => 500.0;

	protected override double MinHeightValue => 320.0;

	public PartnersGroupRevaluationWindow()
	{
	}

	public PartnersGroupRevaluationWindow(PartnersGroupRevaluation doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		LayoutGroup container2 = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container2, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container2, Definition.Fields["DocNum"], 1);
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_comment = AddComboBoxControl(container, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_partRevalGrid = AddGrid(container, Definition.Grids["RevaluationList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_partRevalGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(PartRevalGrid_AddingRow, this).Handler;
		_partRevalGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(PartRevalGrid_DuplicatingRow, this).Handler;
		_partRevalGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(PartRevalGrid_ClickDropDown, this).Handler;
		_partRevalGrid.IsRequired = true;
		_partRevalTableView = _partRevalGrid.View;
		_partRevalTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(PartRevalTableView_CellValueChanged, this).Handler;
		_partRevalTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(PartRevalTableView_FocusedRowHandleChangedEventArgs, this).Handler;
		_partRevalTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(PartRevalTableView_ShowingEditor, this).Handler;
		_partCodeCol = _partRevalGrid.AddDropDownColumn(Definition.Grids["RevaluationList"].Columns["PartCode"], new PartnersFilter(), allowAdditionButtons: true);
		_partNameCol = _partRevalGrid.AddStringColumn(Definition.Grids["RevaluationList"].Columns["PartName"]);
		_partNameCol.AllowEditing = DefaultBoolean.False;
		_partNameCol.Width = 225.0;
		_contractCol = _partRevalGrid.AddDropDownColumn(Definition.Grids["RevaluationList"].Columns["Contract"], new Contracts(), allowAdditionButtons: false, isOwnProcessingClickDropDown: true);
		_currCodeCol = _partRevalGrid.AddDropDownColumn(Definition.Grids["RevaluationList"].Columns["CurrCode"], new ArmSoft.SME.Core.UI.DropDowns.Currency(shortMode: true, withOutNCC: true));
		_currCodeCol.AllowEditing = DefaultBoolean.False;
		_summaCol = _partRevalGrid.AddNumberColumn(Definition.Grids["RevaluationList"].Columns["Summa"], showTotal: true);
		_summaCol.AllowEditing = DefaultBoolean.False;
		_partRevalList = new ObservableCollection<RevaluationSpecificationRow>();
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override bool AllowNewAfterSave()
	{
		return false;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_partRevalList.Clear();
		foreach (PartnersGroupRevaluation.RevaluationSpecificationRow revaluation in PO.RevaluationList)
		{
			_partRevalList.Add(new RevaluationSpecificationRow
			{
				PartID = revaluation.PartID,
				PartName = revaluation.PartName,
				PartCode = revaluation.PartCode,
				Contract = revaluation.Contract,
				CurrCode = revaluation.CurrCode,
				Summa = revaluation.Summa,
				Trans = revaluation.Trans
			});
		}
		_partRevalGrid.ItemsSource = _partRevalList;
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

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.RevaluationList.Clear();
		foreach (RevaluationSpecificationRow partReval in _partRevalList)
		{
			PO.RevaluationList.Add(new PartnersGroupRevaluation.RevaluationSpecificationRow
			{
				PartID = partReval.PartID,
				PartName = partReval.PartName,
				PartCode = partReval.PartCode,
				Contract = partReval.Contract,
				CurrCode = partReval.CurrCode,
				Summa = partReval.Summa,
				Trans = partReval.Trans
			});
		}
		PO.CurrCode = _partRevalList.First().CurrCode;
	}

	private void PartRevalGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_partRevalList.Add(new RevaluationSpecificationRow());
		e.RowNumber = _partRevalList.Count - 1;
	}

	private void PartRevalGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		RevaluationSpecificationRow revaluationSpecificationRow = (RevaluationSpecificationRow)_partRevalGrid.Grid.GetRow(e.RowHandle);
		if (revaluationSpecificationRow != null)
		{
			RevaluationSpecificationRow revaluationSpecificationRow2 = new RevaluationSpecificationRow();
			revaluationSpecificationRow.Clone(revaluationSpecificationRow2);
			_partRevalList.Add(revaluationSpecificationRow2);
			e.NewRowNumber = _partRevalList.Count - 1;
		}
	}

	private void PartRevalGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		RevaluationSpecificationRow revaluationSpecificationRow = (RevaluationSpecificationRow)_partRevalGrid.Grid.GetRow(e.RowHandle);
		if (e.Column == _contractCol && revaluationSpecificationRow != null)
		{
			Contracts contracts = new Contracts(revaluationSpecificationRow.PartCode, "", isExceptCurrCodeNCC: true);
			ModalBrowser modalBrowser = new ModalBrowser(contracts)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser.Show())
			{
				e.NewValue = contracts.ValueMember(modalBrowser.SelectedRowIndex);
				if (string.IsNullOrWhiteSpace(revaluationSpecificationRow.PartCode))
				{
					PartnersContract.Key key = (PartnersContract.Key)contracts.KeyMember(modalBrowser.SelectedRowIndex);
					if (key != null)
					{
						PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(key.PartnerCode, key.Contract, out var partnerCacheRow);
						if (contract != null)
						{
							revaluationSpecificationRow.PartID = partnerCacheRow.Id;
							revaluationSpecificationRow.PartCode = partnerCacheRow.PartCode;
							revaluationSpecificationRow.PartName = partnerCacheRow.Caption;
							revaluationSpecificationRow.CurrCode = contract.CurrCode;
						}
					}
				}
			}
		}
		_partRevalGrid.Grid.RefreshData();
	}

	private void PartRevalTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		RevaluationSpecificationRow revaluationSpecificationRow = (RevaluationSpecificationRow)_partRevalGrid.Grid.GetRow(e.RowHandle);
		if (revaluationSpecificationRow == null)
		{
			return;
		}
		if (e.Column == _partCodeCol)
		{
			try
			{
				if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					revaluationSpecificationRow.PartID = -1;
					revaluationSpecificationRow.PartCode = "";
					revaluationSpecificationRow.PartName = "";
					revaluationSpecificationRow.Contract = "";
					revaluationSpecificationRow.CurrCode = "";
					revaluationSpecificationRow.Summa = 0m;
					_contractCol.DataView = new Contracts(revaluationSpecificationRow.PartCode, "", isExceptCurrCodeNCC: true);
				}
				else
				{
					string key = e.Value.ToString();
					PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(key, throwExceptionNotExits: true);
					revaluationSpecificationRow.PartID = item.Id;
					revaluationSpecificationRow.PartCode = item.PartCode;
					revaluationSpecificationRow.PartName = item.Caption;
					PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(revaluationSpecificationRow.PartCode);
					if (defaultContract != null)
					{
						revaluationSpecificationRow.Contract = defaultContract.Contract;
						revaluationSpecificationRow.CurrCode = defaultContract.CurrCode;
					}
					else
					{
						revaluationSpecificationRow.Contract = "";
						revaluationSpecificationRow.CurrCode = "";
					}
					_contractCol.DataView = new Contracts(revaluationSpecificationRow.PartCode, "", isExceptCurrCodeNCC: true);
					revaluationSpecificationRow.Summa = 0m;
				}
			}
			catch (Exception ex)
			{
				revaluationSpecificationRow.PartID = -1;
				revaluationSpecificationRow.PartCode = "";
				revaluationSpecificationRow.PartName = "";
				revaluationSpecificationRow.Contract = "";
				revaluationSpecificationRow.CurrCode = "";
				revaluationSpecificationRow.Summa = 0m;
				_contractCol.DataView = new Contracts(revaluationSpecificationRow.PartCode, "", isExceptCurrCodeNCC: true);
				Settings.ErrorHandler(ex);
			}
		}
		else if (e.Column == _contractCol && e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(revaluationSpecificationRow.PartID, e.Value.ToString());
			revaluationSpecificationRow.CurrCode = ((contract == null) ? "" : contract.CurrCode);
		}
		_partRevalGrid.Grid.RefreshData();
	}

	private void PartRevalTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _partRevalTableView.FocusedRowHandle;
		RevaluationSpecificationRow revaluationSpecificationRow = (RevaluationSpecificationRow)_partRevalGrid.Grid.GetRow(focusedRowHandle);
		if (revaluationSpecificationRow != null)
		{
			ChangeColumnAdditonalView(revaluationSpecificationRow.PartCode);
		}
	}

	private void ChangeColumnAdditonalView(string partnerCode)
	{
		_contractCol.DataView = new Contracts(string.IsNullOrWhiteSpace(partnerCode) ? "" : partnerCode, "", isExceptCurrCodeNCC: true);
	}

	private void PartRevalTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (e.Column == _contractCol && e.Row != null)
		{
			RevaluationSpecificationRow revaluationSpecificationRow = (RevaluationSpecificationRow)e.Row;
			if (string.IsNullOrWhiteSpace(revaluationSpecificationRow.PartCode) || TradeSettings.TradeCacheService.Partner.GetItem(revaluationSpecificationRow.PartCode) == null)
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

	protected override void AddItemsToRibbon()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
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
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span2[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list2;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_partRevalGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_partRevalGrid.RestoreSettings(rk);
	}
}
