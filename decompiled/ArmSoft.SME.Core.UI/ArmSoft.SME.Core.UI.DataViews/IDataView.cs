using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.MetaData;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DataViews;

public interface IDataView : IView
{
	DataViewPanel DVP { get; set; }

	DataViewDefinition DVD { get; }

	short ProviderType { get; }

	bool ColHeadersIn2Lines { get; set; }

	List<ViewColumn> PotentiallyColumns { get; }

	bool AllowChangeSorting { get; }

	bool AllowChangeGrouping { get; }

	Type RowType { get; }

	bool IsUpdatable { get; }

	bool AllowAdd { get; }

	bool AllowEdit { get; }

	bool AllowView { get; }

	bool AllowDelete { get; }

	bool AllowEditSelected { get; }

	bool AllowDeleteSelected { get; }

	bool AllowCreateCopy { get; }

	bool AllowPreview { get; }

	bool AllowExport { get; }

	bool ExistExchangeableRow { get; }

	bool AllowDocumentHistory { get; }

	LoadIndication LoadingIndication { get; }

	bool DialogSupported { get; }

	bool RefreshSupported { get; }

	int SearchDelay { get; }

	ImageSource HeaderImage { get; set; }

	List<AsFormatting> ConditionalFormattingList { get; }

	AsRibbon PanelRibbon { get; }

	AsRibbon ContextRibbon { get; }

	bool AllowShowTransactions { get; }

	bool ShowTransactionsOnOpen { get; }

	string TransactionsPanelCaption { get; }

	string TransactionsControlDefaultPosition { get; }

	string ReportTitle();

	string CaptionToolTip();

	ToolTip ToolTipContent(object sender);

	void OnMagneticCardReader(string mcrData);

	void OnScanner(string scannerData);

	void OnF2();

	void ApplyDefaultDataViewDefinition();

	void ApplyDataViewDefinitionParameters();

	bool Show(bool useParamValues = false, bool openAsChild = true, Window owner = null, bool withoutShowDialog = false);

	void BeforeApplyDataViewDefinition(DataViewDefinition dvd);

	void ApplyDataViewDefinition(DataViewDefinition dvd);

	void ApplyDataViewDefinition(string name);

	void ApplySettings(DataViewSetting dvs);

	void BeforeLoadData(DataViewPanel dvp);

	void AddDirectoryLinksToColumns(DataViewPanel dvp);

	void BeforeLoadData(GridControl grid);

	void AfterLoadData(DataViewPanel dvp);

	void AfterLoadData(GridControl grid);

	void FillData(object filledList);

	void LoadReferenceColumns(object key = null);

	List<object> MatchedRows(object key);

	List<int> LoadUpdatedRows(object key, DataViewPanel dvp);

	List<int> LoadUpdatedRows(List<object> keys, DataViewPanel dvp);

	PropertyInfo GetPropertyInfo(string propertyName);

	void Add();

	void Edit();

	void View();

	void Delete();

	void EditSelected();

	void DeleteSelected();

	void CreateCopy();

	void DocumentHistory();

	void PreviewSelected(List<int> rows, PrintMode? mode = null, TemplatesCacheRow template = null, bool insertPageBreaks = true);

	void Preview(int rowHandle, PrintMode mode, TemplatesCacheRow template = null);

	DataViewPrintDescriptor PreviewDescriptor();

	IReadOnlyList<TemplatesCacheRow> PrintTemplates();

	TemplatesCacheRow DefaultPrintTemplate();

	void ExportToXML(string fileName, object rowObject);

	void InitContextFunctions();

	void InitCoreContextFunctions();

	void InitPanelRibbonFunctions();

	string GetDefaultFunction();

	bool ShowDialog(bool useParamValues, bool isRefreshMode, Window owner);

	DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode);

	void ShowProgressWindow(DataViewPanel dvp);

	void ShowProgressWindowInside(DataViewPanel dvp);

	void StopExecution();

	void ShowTransactions(bool showAtRight);

	Control CreateTransactionsControl();

	void SetTransactionsPanelsDefaultSize(ref double totalsPanelHeight, ref double totalsPanelRightWidth);

	void TransactionsPanelRibbonSettings(ref string ribbonCommandCaption, ref ImageSource smallImage, ref ImageSource largeImage);

	bool CardViewAllowed();

	DataTemplate GetCardTemplate();

	DataTemplate GetCardPrintTemplate();

	DataTemplate GetCardHeaderTemplate();

	byte[] GetImageForPrinting(string columnName, object row);
}
