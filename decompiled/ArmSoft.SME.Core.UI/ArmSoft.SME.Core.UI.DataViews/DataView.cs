using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Data;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Common.Models.UnboundColumns;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.ExceptionDispatch;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.Navigation;
using ArmSoft.SME.Core.UI.Progress;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews;

public abstract class DataView<R, P> : View<R, P>, IDataView, IView where R : ExtendableRow, new() where P : class, new()
{
	public class DeleteDialog : DialogWindow
	{
		public DropDownCommentControl Comment;

		public DeleteDialog()
		{
			Name = "DeleteDoc";
			Caption = ArmSoft.SME.Common.Resources.ConfirmDocumentDelete;
			IDropDownService requiredService = Settings.ServiceProvider.GetRequiredService<IDropDownService>();
			Comment = AddDropDownCommentControl("Comment", ArmSoft.SME.Common.Resources.Comment, requiredService.CreateDropDown(DropDownTypeSE.StdComm), isRequired: false, storeValue: false, 255);
			SetFocusOnActivation = _cancelButton;
		}
	}

	private DataProviderProgressWindowInside<R, P> _dataProviderProgressWindowInside;

	private short _providerType;

	private string _dataViewGroup;

	protected DocumentExtensionDefinition _extensionDefinition;

	private LoadIndication _loadingIndication;

	protected bool? _isUpdatable;

	protected AsRibbon _contextRibbon;

	protected AsRibbon _panelRibbon;

	private DataViewDefinition _dvd;

	public DataViewPanel DVP { get; set; }

	public short ProviderType
	{
		get
		{
			if (_providerType == 0)
			{
				DefinitionMappingsService.IsDataView(GetType(), out _providerType);
			}
			return _providerType;
		}
	}

	public override string DataViewGroup
	{
		get
		{
			if (_dataViewGroup == null)
			{
				_dataViewGroup = SmartEnumHelper.GetName(typeof(DataProviderTypeSE), ProviderType);
			}
			return _dataViewGroup;
		}
	}

	public override string Name => DataViewGroup;

	public ImageSource HeaderImage { get; set; }

	public bool ColHeadersIn2Lines { get; set; }

	public List<AsFormatting> ConditionalFormattingList { get; }

	public virtual bool IsUpdatable
	{
		get
		{
			if (!_isUpdatable.HasValue)
			{
				_isUpdatable = typeof(R).GetInterface("IMatchedUpdateKey") != null;
			}
			return _isUpdatable.Value;
		}
	}

	public Type RowType => typeof(R);

	public virtual List<ViewColumn> PotentiallyColumns => Columns.ViewList;

	public virtual bool AllowChangeGrouping => true;

	public virtual bool AllowChangeSorting => true;

	public Dictionary<string, DataViewParameter> Parameters1 => Parameters;

	public bool AllowOperation { get; set; }

	public virtual bool AllowAdd => false;

	public virtual bool AllowEdit => false;

	public virtual bool AllowView => false;

	public virtual bool AllowDelete => false;

	public virtual bool AllowEditSelected => false;

	public virtual bool AllowDeleteSelected => false;

	public virtual bool AllowCreateCopy => false;

	public virtual bool AllowPreview => false;

	public virtual bool AllowExport => false;

	public virtual bool ExistExchangeableRow => AllowExport;

	public virtual bool AllowDocumentHistory => false;

	public virtual bool AllowNotifications => false;

	public virtual bool AllowCreatePOExtensionDefinition => false;

	public virtual int SearchDelay => 1000;

	public AsRibbon ContextRibbon => _contextRibbon;

	public AsRibbon PanelRibbon => _panelRibbon;

	public bool ShowAsTransactionPanel { get; set; }

	public virtual bool AllowShowTransactions => false;

	public virtual bool ShowTransactionsOnOpen => false;

	public virtual string TransactionsControlDefaultPosition => "Bottom";

	public virtual string TransactionsPanelCaption => "";

	public virtual LoadIndication LoadingIndication => _loadingIndication;

	public virtual bool DialogSupported => false;

	public virtual bool RefreshSupported => true;

	public DataViewDefinition DVD => _dvd;

	public virtual string ReportTitle()
	{
		return ComposeTitle();
	}

	public virtual string CaptionToolTip()
	{
		return Caption;
	}

	public string ComposeTitle()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(CaptionToolTip());
		foreach (DataViewParameter value3 in Parameters.Values)
		{
			if (!value3.IncludeCaptionToolTip)
			{
				continue;
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("   ");
			stringBuilder.Append(value3.Caption);
			stringBuilder.Append("  - ");
			object value = value3.Value;
			string s = value as string;
			if (s != null)
			{
				if (string.IsNullOrWhiteSpace(s))
				{
					stringBuilder.Append(Resources.All);
					continue;
				}
				if (value3.DataView == null)
				{
					stringBuilder.Append('"');
					stringBuilder.Append(s);
					stringBuilder.Append('"');
					continue;
				}
				int num = value3.DataView.ValuesList().FindIndex((string p) => p == s);
				if (num >= 0)
				{
					stringBuilder.Append(value3.DataView.CommentMember(num));
					continue;
				}
				stringBuilder.Append('"');
				stringBuilder.Append(s);
				stringBuilder.Append('"');
			}
			else if (value3.Value == null)
			{
				if (value3.Format.InList("dd/MM/yy", "dd/MM/yyyy"))
				{
					stringBuilder.Append(Resources.CompareOpEmpty);
				}
				else
				{
					stringBuilder.Append(Resources.All);
				}
			}
			else if (string.IsNullOrWhiteSpace(value3.Format))
			{
				if (value3.Value is bool value2)
				{
					stringBuilder.Append(value2.ToYesNo());
				}
				else
				{
					stringBuilder.Append(value3.Value.ToString());
				}
			}
			else
			{
				stringBuilder.Append(string.Format(value3.Value.ToString(), value3.Format));
			}
		}
		return stringBuilder.ToString();
	}

	public virtual byte[] GetImageForPrinting(string columnName, object row)
	{
		return null;
	}

	public DataView()
	{
		AllowOperation = Settings.UserAccessService.GetProviderPermission(ProviderType).AllowOperation;
		ConditionalFormattingList = new List<AsFormatting>();
	}

	public virtual void BeforeLoadData(DataViewPanel dvp)
	{
		DVP = dvp;
		if (!dvp.IsSorted && Columns.SortOrder().Count > 0)
		{
			foreach (ViewColumnSorting item in Columns.SortOrder())
			{
				dvp.Set_ColumnSortOrder(item.Column.Name, (!Convert.ToBoolean(item.Descending)) ? ColumnSortOrder.Ascending : ColumnSortOrder.Descending);
			}
		}
		if (!dvp.IsGrouped && Columns.GroupBy().Count > 0)
		{
			foreach (List<ViewColumn> item2 in Columns.GroupBy())
			{
				if (item2 == null)
				{
					continue;
				}
				for (int i = 0; i <= item2.Count - 1; i++)
				{
					if (i == 0)
					{
						dvp.GroupBy(item2[i].Name, merge: false);
					}
					else
					{
						dvp.GroupBy(item2[i].Name, merge: true);
					}
				}
			}
		}
		Columns.ClearDirectoryLinkColumns();
	}

	protected override void SetColumnsVisibleByDataProvider()
	{
		if (ColumnsProvider == null || ColumnsProvider.Count <= 0)
		{
			return;
		}
		foreach (ViewColumn item in Columns.ViewList.Where((ViewColumn p) => !p.IsDynamic && !p.IsReference && !p.IsExtended))
		{
			DVP.Set_ColumnVisible(item.Name, ColumnsProvider.Contains(item.Name));
		}
	}

	public void AddDirectoryLinksToColumns(DataViewPanel dvp)
	{
		if (Columns.DirectoryLinkColumns == null)
		{
			return;
		}
		foreach (DirectoryLinkColumnData linkColumn in Columns.DirectoryLinkColumns)
		{
			if (!Settings.UserAccessService.GetDirectoryPermission(linkColumn.DirectoryType).AllowEdit && !Settings.UserAccessService.GetDirectoryPermission(linkColumn.DirectoryType).AllowView)
			{
				continue;
			}
			GridColumn gridColumn = dvp.InnerGrid().Columns[linkColumn.CodeColumnName];
			if (gridColumn == null)
			{
				continue;
			}
			if (linkColumn.ShowAsLink)
			{
				HyperlinkEditSettings hyperlinkEditSettings = new HyperlinkEditSettings
				{
					TextWrapping = TextWrapping.NoWrap
				};
				hyperlinkEditSettings.RequestNavigation += new SafeEventHandler<HyperlinkEditRequestNavigationEventArgs>((object? s, HyperlinkEditRequestNavigationEventArgs e) =>
				{
					DirectoryHyperlink_RequestNavigation(e, linkColumn.CodeColumnName);
				}).Handler;
				gridColumn.EditSettings = hyperlinkEditSettings;
				if (Settings.IsWin11Theme)
				{
					gridColumn.CellTemplate = null;
				}
			}
			else
			{
				gridColumn.EditSettings = null;
			}
		}
	}

	protected virtual void OpenDirectory(short directoryType, string code)
	{
		Settings.DirectoryService.ShowWindow(directoryType, code, isPOWindow: true, Settings.UserAccessService.GetDirectoryPermission(directoryType).AllowEdit ? WindowViewMode.Edit : WindowViewMode.View, DVP.Id);
	}

	protected virtual void DirectoryHyperlink_RequestNavigation(HyperlinkEditRequestNavigationEventArgs e, string columnName)
	{
		if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
		{
			return;
		}
		DirectoryLinkColumnData directoryLinkColumnData = Columns.DirectoryLinkColumns.FirstOrDefault((DirectoryLinkColumnData p) => p.CodeColumnName == columnName);
		if (directoryLinkColumnData != null)
		{
			if (!Settings.UserAccessService.GetDirectoryPermission(directoryLinkColumnData.DirectoryType).AllowEdit && !Settings.UserAccessService.GetDirectoryPermission(directoryLinkColumnData.DirectoryType).AllowView)
			{
				throw new ASException(string.Format(Resources.DontHavePermissionToViewEditPO, directoryLinkColumnData.DirectoryCaption), 20008);
			}
			Settings.DirectoryService.ShowWindow(directoryLinkColumnData.DirectoryType, e.Value.ToString(), isPOWindow: true, WindowViewMode.Edit, DVP.Id);
		}
	}

	public virtual void BeforeLoadData(GridControl grid)
	{
		if (Columns.SortOrder().Count <= 0)
		{
			return;
		}
		foreach (ViewColumnSorting item in Columns.SortOrder())
		{
			SetColumnSortOrder(grid, item.Column.Name, (!Convert.ToBoolean(item.Descending)) ? ColumnSortOrder.Ascending : ColumnSortOrder.Descending);
		}
	}

	protected void SetColumnSortOrder(GridControl grid, string columnName, ColumnSortOrder sortOrder)
	{
		foreach (GridColumn column in grid.Columns)
		{
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				column.SortOrder = sortOrder;
				break;
			}
		}
	}

	protected bool Get_ColumnVisible(GridControl grid, string columnName)
	{
		foreach (GridColumn column in grid.Columns)
		{
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				return Convert.ToBoolean(column.Visible);
			}
		}
		return false;
	}

	protected void Set_ColumnVisible(GridControl grid, string columnName, bool value)
	{
		int num = 0;
		if (!(grid.View is TableView tableView))
		{
			return;
		}
		foreach (GridColumn column in grid.Columns)
		{
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				if (Convert.ToBoolean((!column.Visible & value) && column.ShowInColumnChooser))
				{
					column.Visible = false;
				}
				else
				{
					column.Visible = value;
				}
				column.ShowInColumnChooser = value;
				if (value)
				{
					column.VisibleIndex = num;
					num++;
				}
			}
			else if (tableView.VisibleColumns.Contains(column))
			{
				column.VisibleIndex = num;
				num++;
			}
		}
	}

	public virtual void AfterLoadData(DataViewPanel dvp)
	{
		DVP = dvp;
		if (dvp.InRefresh == DataViewPanel.RefreshType.None)
		{
			DVP.AddConditionalFormattingToView();
		}
	}

	public void LoadReferenceColumns(object key = null)
	{
		IEnumerable<ViewColumn> referenceColumns = Columns.ReferenceColumns;
		IEnumerable<ViewColumn> extendedColumns = Columns.ExtendedColumns;
		if ((!referenceColumns.Any() && !extendedColumns.Any()) || Data.Count <= 0)
		{
			return;
		}
		List<Column> list = new List<Column>();
		ExtendableRow.RowSchema rowSchema = new ExtendableRow.RowSchema(Data[0].GetType(), SchemaProvider.Columns.ToList(), ColumnsProvider);
		IDataProviderExtensionHandler dataProviderExtensionHandler = null;
		if (_dvd != null)
		{
			Type type = DefinitionMappingsService.ResolveDVD(DataViewGroup, _dvd.Name);
			if (type != null)
			{
				dataProviderExtensionHandler = (IDataProviderExtensionHandler)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, type);
				if (dataProviderExtensionHandler != null)
				{
					Dictionary<string, object> extendedParameters = null;
					if (_dvd.DialogExtension != null)
					{
						extendedParameters = _dvd.DialogExtension.Values;
					}
					dataProviderExtensionHandler.SetParameters(ApplyDataViewDefinitionParametersInner(extendedParameters));
					dataProviderExtensionHandler.BeforeLoad(updateRows: false);
				}
			}
		}
		foreach (R datum in Data)
		{
			if (list.Count == 0)
			{
				foreach (ViewColumn item in referenceColumns)
				{
					list.Add(new Column
					{
						Type = item.Type.CastToDataColumnType(),
						Name = item.Name,
						Caption = item.Caption,
						Format = item.Format,
						FooterSum = item.FooterSum,
						Group = item.Group,
						Source = item.Source,
						SourceType = ColumnSourceType.Reference,
						After = item.After,
						Before = item.Before
					});
				}
				foreach (ViewColumn item2 in extendedColumns)
				{
					list.Add(new Column
					{
						Type = item2.Type.CastToDataColumnType(),
						Name = item2.Name,
						Caption = item2.Caption,
						Format = item2.Format,
						FooterSum = item2.FooterSum,
						Group = item2.Group,
						Source = item2.Source,
						SourceType = ColumnSourceType.Extender,
						After = item2.After,
						Before = item2.Before
					});
				}
			}
			if (key == null)
			{
				if (datum.Schema != null)
				{
					datum.Schema.AddColumns(list);
				}
				else
				{
					rowSchema.AddColumns(list);
					datum.Schema = rowSchema;
				}
			}
			else if (((ExtendableRow)key).Schema != null)
			{
				((ExtendableRow)key).Schema.AddColumns(list);
			}
			else
			{
				rowSchema.AddColumns(list);
				((ExtendableRow)key).Schema = rowSchema;
			}
			SetCacheRow(datum);
			CalculateExtendedRow(datum, dataProviderExtensionHandler);
		}
		dataProviderExtensionHandler?.EditRows(Data);
	}

	private protected virtual void CalculateExtendedRow(ExtendableRow row, IDataProviderExtensionHandler handler)
	{
		if (_dvd == null || handler == null)
		{
			return;
		}
		foreach (ViewColumn extendedColumn in Columns.ExtendedColumns)
		{
			handler.SetRow(row);
			if (handler.OnEachRow())
			{
				object innerHandler = handler.InnerHandler;
				MethodInfo method = innerHandler.GetType().GetMethod(extendedColumn.MethodInfoSource);
				if (method == null)
				{
					throw new InvalidOperationException($"Invalid method name {extendedColumn.Source} in type {innerHandler.GetType().Name}");
				}
				row[extendedColumn.Name] = method.Invoke(innerHandler, null);
			}
			handler.SetRow(null);
		}
	}

	protected virtual void SetCacheRow(ExtendableRow row)
	{
	}

	public virtual void AfterLoadData(GridControl grid)
	{
	}

	public void FillData(object filledList)
	{
		if (Data.Count > 0)
		{
			Data.Clear();
		}
		foreach (R item in (List<R>)filledList)
		{
			Data.Add(item);
		}
		DataLoadedAtLeastOnce = true;
	}

	public List<object> MatchedRows(object key)
	{
		List<object> list = new List<object>();
		foreach (R datum in Data)
		{
			if (((IMatchedUpdateKey)datum).Matched(key))
			{
				list.Add(datum);
			}
		}
		return list;
	}

	public List<int> LoadUpdatedRows(object key, DataViewPanel dvp)
	{
		List<int> list = new List<int>();
		SetDataProviderProperties();
		DataProvider<R, P> dataProvider = DataProvider;
		P parametersProvider = ParametersProvider;
		ExecutionSettings settings = new ExecutionSettings();
		int num = 1;
		List<object> list2 = new List<object>(num);
		CollectionsMarshal.SetCount(list2, num);
		CollectionsMarshal.AsSpan(list2)[0] = key;
		List<R> rows = dataProvider.Execute(parametersProvider, settings, list2, withoutAccessCheck: false, ColumnsProvider).Rows;
		List<int> list3 = new List<int>();
		for (int i = 0; i <= Data.Count - 1; i++)
		{
			if (((IMatchedUpdateKey)Data[i]).Matched(key))
			{
				list3.Add(i);
			}
		}
		if (list3.Count == 0)
		{
			foreach (R item in rows)
			{
				Data.Add(item);
			}
			list.Add(Data.Count - 1);
		}
		else if ((list3.Count == 1) & (rows.Count == 1))
		{
			Data[list3[0]] = rows[0];
			list.Add(list3[0]);
		}
		else
		{
			for (int num2 = list3.Count - 1; num2 >= 0; num2--)
			{
				Data.RemoveAt(list3[num2]);
				dvp.RowDeleted(list3[num2]);
			}
			foreach (R item2 in rows)
			{
				Data.Add(item2);
				list.Add(Data.Count - 1);
			}
		}
		return list;
	}

	public List<int> LoadUpdatedRows(List<object> keys, DataViewPanel dvp)
	{
		List<int> list = new List<int>();
		SetDataProviderProperties();
		List<R> rows = DataProvider.Execute(ParametersProvider, new ExecutionSettings(), keys, withoutAccessCheck: false, ColumnsProvider).Rows;
		Dictionary<object, List<int>> dictionary = new Dictionary<object, List<int>>();
		for (int i = 0; i <= Data.Count - 1; i++)
		{
			IMatchedUpdateKey matchedUpdateKey = (IMatchedUpdateKey)Data[i];
			foreach (object key in keys)
			{
				if (matchedUpdateKey.Matched(key))
				{
					if (dictionary.TryGetValue(key, out var value))
					{
						value.Add(i);
						break;
					}
					int num = 1;
					List<int> list2 = new List<int>(num);
					CollectionsMarshal.SetCount(list2, num);
					CollectionsMarshal.AsSpan(list2)[0] = i;
					dictionary.Add(key, list2);
					break;
				}
			}
		}
		int count = Data.Count;
		for (int j = 0; j <= rows.Count - 1; j++)
		{
			IMatchedUpdateKey matchedUpdateKey = (IMatchedUpdateKey)rows[j];
			foreach (object key2 in keys)
			{
				if (matchedUpdateKey.Matched(key2))
				{
					if (dictionary.TryGetValue(key2, out var value2))
					{
						Data[value2[0]] = rows[j];
						list.Add(value2[0]);
						value2.RemoveAt(0);
						break;
					}
					int num = 1;
					List<int> list3 = new List<int>(num);
					CollectionsMarshal.SetCount(list3, num);
					CollectionsMarshal.AsSpan(list3)[0] = count + j;
					dictionary.Add(key2, list3);
					Data.Add(rows[j]);
					list.Add(Data.Count - 1);
					break;
				}
			}
		}
		List<int> list4 = new List<int>();
		foreach (List<int> value3 in dictionary.Values)
		{
			list4.AddRange(value3);
		}
		list4 = list4.Order().ToList();
		for (int num2 = list4.Count - 1; num2 >= 0; num2--)
		{
			int num3 = list4[num2];
			if (num3 < count)
			{
				Data.RemoveAt(num3);
				dvp.RowDeleted(num3);
			}
		}
		return list;
	}

	public object ExecuteDataProvider(RowProcessedCallback callback)
	{
		SetDataProviderProperties();
		return DataProvider.Execute(ParametersProvider, null, null, withoutAccessCheck: false, ColumnsProvider);
	}

	public PropertyInfo GetPropertyInfo(string propertyName)
	{
		return typeof(R).GetProperty(propertyName);
	}

	protected void AddParameter(string name, string caption, object defaultValue, string format = "", bool includeCaptionToolTip = true, IDropDownView dropDownView = null)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new InvalidOperationException("Parameter name is empty.");
		}
		DataViewParameter dataViewParameter = new DataViewParameter
		{
			Name = name.Trim(),
			Caption = caption,
			Value = defaultValue,
			Format = format,
			IncludeCaptionToolTip = includeCaptionToolTip,
			DataView = dropDownView
		};
		Parameters.Add(dataViewParameter.Name, dataViewParameter);
	}

	public virtual void Add()
	{
		throw new NotImplementedException();
	}

	public virtual void Edit()
	{
		IRowWithISN rowWithISN = (IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Settings.DocumentService.ShowWindow(rowWithISN.GetDocISN(), WindowViewMode.Edit, Settings.HostingEnvironment.CurrentViewPanel.Id);
	}

	public virtual void View()
	{
		IRowWithISN rowWithISN = (IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Settings.DocumentService.ShowWindow(rowWithISN.GetDocISN(), WindowViewMode.View, Settings.HostingEnvironment.CurrentViewPanel.Id);
	}

	public virtual void Delete()
	{
		IRowWithISN rowWithISN = (IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		DeleteDialog deleteDialog = new DeleteDialog();
		if (Convert.ToBoolean(deleteDialog.ShowDialog()))
		{
			Settings.DocumentService.Delete(rowWithISN.GetDocISN(), new PODeleteRequest
			{
				LogComment = deleteDialog.Comment.Text
			});
			Settings.HostingEnvironment.CurrentViewPanel.Update(rowWithISN.GetDocISN());
		}
	}

	public virtual void CreateCopy()
	{
		IRowWithISN rowWithISN = (IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Settings.DocumentService.ShowWindow(Settings.DocumentService.CreateCopy(rowWithISN.GetDocISN()), WindowViewMode.Copy, Settings.HostingEnvironment.CurrentViewPanel.Id);
	}

	public virtual void DeleteSelected()
	{
		throw new NotImplementedException();
	}

	public virtual void DocumentHistory()
	{
		IRowWithISN rowWithISN = (IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (rowWithISN != null)
		{
			new DocumentLog(rowWithISN.GetDocISN()).Show();
		}
	}

	public virtual void Preview(int rowHandle, PrintMode mode, TemplatesCacheRow template = null)
	{
		Guid docIsn = ((rowHandle != -1) ? ((IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.Row(rowHandle)).GetDocISN() : ((IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow()).GetDocISN());
		Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(docIsn, mode, template.Name);
	}

	public virtual void PreviewSelected(List<int> rowsHandle, PrintMode? mode = null, TemplatesCacheRow template = null, bool insertPageBreaks = true)
	{
		List<Guid> list = new List<Guid>();
		foreach (int item in rowsHandle)
		{
			list.Add(((IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.Row(item)).GetDocISN());
		}
		PrintMode valueOrDefault = mode.GetValueOrDefault();
		if (!mode.HasValue)
		{
			valueOrDefault = PrintMode.PrintPreview;
			mode = valueOrDefault;
		}
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(Resources.PreviewErrorsReportTitle, "PrintPreview");
		Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(list, mode.Value, template?.Name, insertPageBreaks, null, enumerateErrorsReportViewer);
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
	}

	public virtual IReadOnlyList<TemplatesCacheRow> PrintTemplates()
	{
		return Settings.CoreCacheService.Template.GetTemplates(((IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow()).GetDocType());
	}

	public virtual TemplatesCacheRow DefaultPrintTemplate()
	{
		return Settings.CoreCacheService.Template.GetDefaultPrintTemplate(((IRowWithISN)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow()).GetDocType());
	}

	public virtual DataViewPrintDescriptor PreviewDescriptor()
	{
		return null;
	}

	public void ExportToXML(string fileName, object rowObject)
	{
		ExportToXML(fileName, (R)rowObject);
	}

	public virtual void ExportToXML(string fileName, R row)
	{
	}

	public virtual void InitContextFunctions()
	{
	}

	public void InitCoreContextFunctions()
	{
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		if (AllowCreatePOExtensionDefinition)
		{
			currentViewPanel.AddContextSeparator("ContextFunctions");
			currentViewPanel.AddContextFunction("ContextFunctions", "CreatePOExtensionDefinition", Resources.CreateUserDefinedFields, CreatePOExtensionDefinition, "layout_add16.svg", "", storeOpContext: false, isEnabled: true, null, isNew: true);
		}
		if (currentViewPanel.GetSelectedAny())
		{
			if (AllowEditSelected && _extensionDefinition != null)
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "EditSelectedRowsExtensionFields", Resources.EditUserDefinedFields, EditSelectedRowsExtensionFields, "", "", storeOpContext: false, isEnabled: true, null, isNew: true);
			}
			if (AllowEditSelected || AllowDeleteSelected)
			{
				currentViewPanel.AddContextSeparator("ContextFunctions");
			}
			if (AllowEditSelected)
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "EditSelected", Resources.EditSelected, EditSelected, "groupEdit_16.svg", null, storeOpContext: true);
			}
			if (AllowDeleteSelected)
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "DeleteSelected", Resources.DeleteSelected, DeleteSelected, "groupDelete_16.svg", null, storeOpContext: true);
			}
		}
		AddDirectoryLinkFunctions();
	}

	private void AddDirectoryLinkFunctions()
	{
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		object obj = currentViewPanel.FocusedRow();
		if (obj == null || ShowAsTransactionPanel)
		{
			return;
		}
		IEnumerable<DirectoryLinkColumnData> enumerable = Columns.DirectoryLinkColumns?.Where((DirectoryLinkColumnData p) => p.ShowInContextMenu);
		if (enumerable == null)
		{
			return;
		}
		DataTemplate contentTemplate = (DataTemplate)Settings.GenericResources["LinkFunctionTemplate"];
		Type type = obj.GetType();
		currentViewPanel.AddContextSeparator("OpenDirectoryLink");
		foreach (DirectoryLinkColumnData linkColumn in enumerable)
		{
			string text;
			PropertyInfo property;
			if (string.IsNullOrWhiteSpace(linkColumn.DirectoryCaptionColumnName))
			{
				text = (string.IsNullOrWhiteSpace(linkColumn.DirectoryCaption) ? Settings.DirectoryService.GetCaption(linkColumn.DirectoryType) : linkColumn.DirectoryCaption);
			}
			else
			{
				property = type.GetProperty(linkColumn.DirectoryCaptionColumnName);
				text = property.GetValue(obj, null)?.ToString();
			}
			property = type.GetProperty(linkColumn.CodeColumnName);
			string codeValue = property.GetValue(obj, null)?.ToString();
			text = text + " : " + codeValue;
			if (!string.IsNullOrWhiteSpace(linkColumn.CaptionColumnName))
			{
				property = type.GetProperty(linkColumn.CaptionColumnName);
				text = text + " - " + property.GetValue(obj, null);
			}
			if (!string.IsNullOrWhiteSpace(codeValue) && (Settings.UserAccessService.GetDirectoryPermission(linkColumn.DirectoryType).AllowEdit || Settings.UserAccessService.GetDirectoryPermission(linkColumn.DirectoryType).AllowView))
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "OpenDirectory" + linkColumn.CodeColumnName, text, () =>
				{
					OpenDirectory(linkColumn.DirectoryType, codeValue);
				}, "open_link_16.svg", null, storeOpContext: true, isEnabled: true, contentTemplate);
			}
		}
	}

	public virtual void InitPanelRibbonFunctions()
	{
	}

	public virtual string GetDefaultFunction()
	{
		return "";
	}

	public virtual void SetTransactionsPanelsDefaultSize(ref double totalsPanelHeight, ref double totalsPanelRightWidth)
	{
		totalsPanelHeight = 0.0;
		totalsPanelRightWidth = 0.0;
	}

	public virtual void TransactionsPanelRibbonSettings(ref string ribbonCommandCaption, ref ImageSource smallImage, ref ImageSource largeImage)
	{
		ribbonCommandCaption = "";
		smallImage = null;
		largeImage = null;
	}

	public virtual void ShowTransactions(bool showAtRight)
	{
	}

	public virtual System.Windows.Controls.Control CreateTransactionsControl()
	{
		return null;
	}

	public void ShowProgressWindow(DataViewPanel dvp)
	{
		DVP = dvp;
		Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
		SetDataProviderProperties();
		DataProvider.Settings = new ExecutionSettings
		{
			FetchSize = FetchSize,
			FirstFetchSize = FirstFetchSize,
			ShowProgress = true,
			UseRowsBinarySerializer = true
		};
		if (dvp.InRefresh == DataViewPanel.RefreshType.None)
		{
			JobEnqueueWithSchemaResponse schemaAndEnqueue = DataProvider.GetSchemaAndEnqueue(ParametersProvider, DataProvider.Settings, ColumnsProvider);
			DataProvider.Schema = schemaAndEnqueue.Schema;
			DataProvider.JobId = schemaAndEnqueue.Id;
			if (DataProvider.Schema.Columns.Any((Column p) => p.SourceType == ColumnSourceType.Dynamic))
			{
				FillDynamicColumnsFromDataProvider(DataProvider.Schema);
				dvp.PopulateDynamicColumns();
			}
		}
		Mouse.OverrideCursor = null;
		DataProviderProgressWindow<R, P> dataProviderProgressWindow = new DataProviderProgressWindow<R, P>(DataProvider);
		dataProviderProgressWindow.Parameters = ParametersProvider;
		dataProviderProgressWindow.Columns = ColumnsProvider;
		dataProviderProgressWindow.ProcessingDataPhaseName = Resources.PrepareDataView;
		dataProviderProgressWindow.ProcessRows += new SafeEventHandler<ProcessDataProviderEventArgs<R>>(DataViewProgressWindow_ProcessRows).Handler;
		dataProviderProgressWindow.ShowDialogWithExceptionHandling();
		Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
		DVP.PanelIsLoaded = true;
		DVP.FitColumns();
		if (DVP.Grid.VisibleRowCount > 0)
		{
			FocusManager.SetFocusedElement((DependencyObject)(object)DVP, DVP.Grid);
			if (ShowExpanded() && DVP.Grid.View is IPanelView panelView)
			{
				panelView.ExpandGroups();
			}
		}
		Mouse.OverrideCursor = null;
		DVP.InRefresh = DataViewPanel.RefreshType.None;
	}

	public void ShowProgressWindowInside(DataViewPanel dvp)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		DVP = dvp;
		HeaderImage = ImageService.GetImage("download_16.svg");
		dvp.HeaderImage = HeaderImage;
		Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
		dvp.SetBarStatesDisabled();
		SetDataProviderProperties();
		DataProvider.Settings = new ExecutionSettings
		{
			FetchSize = FetchSize,
			FirstFetchSize = FirstFetchSize,
			ShowProgress = true,
			UseRowsBinarySerializer = true
		};
		if (dvp.InRefresh == DataViewPanel.RefreshType.None)
		{
			JobEnqueueWithSchemaResponse schemaAndEnqueue = DataProvider.GetSchemaAndEnqueue(ParametersProvider, DataProvider.Settings, ColumnsProvider);
			DataProvider.Schema = schemaAndEnqueue.Schema;
			DataProvider.JobId = schemaAndEnqueue.Id;
			if (DataProvider.Schema.Columns.Any((Column p) => p.SourceType == ColumnSourceType.Dynamic))
			{
				FillDynamicColumnsFromDataProvider(DataProvider.Schema);
				dvp.PopulateDynamicColumns();
			}
		}
		Mouse.OverrideCursor = null;
		_dataProviderProgressWindowInside = new DataProviderProgressWindowInside<R, P>(DataProvider)
		{
			Parameters = ParametersProvider,
			Columns = ColumnsProvider,
			ProcessingDataPhaseName = Resources.PrepareDataView
		};
		dvp.LoadingDecoratorGrid.Children.Add(_dataProviderProgressWindowInside);
		_dataProviderProgressWindowInside.ProcessRows += new SafeEventHandler<ProcessDataProviderEventArgs<R>>(DataViewProgressWindow_ProcessRows).Handler;
		_dataProviderProgressWindowInside.BackgroundWorkerDoWorkCompleted += new SafeEventHandler<EventArgs>(DataViewProgressWindow_BackgroundWorkerDoWorkCompleted).Handler;
	}

	public void StopExecution()
	{
		if (DataProvider != null && DataProvider.JobId != Guid.Empty)
		{
			JobStopRequest jobStopRequest = new JobStopRequest
			{
				Id = DataProvider.JobId
			};
			DataProvider.Stop(jobStopRequest);
		}
	}

	private void DataViewProgressWindow_ProcessRows(object sender, ProcessDataProviderEventArgs<R> e)
	{
		try
		{
			DVP.Grid.BeginDataUpdate();
			FillData(e.Result.Rows);
			AfterLoadData(DVP);
			LoadReferenceColumns();
			DVP.ClearSelection();
		}
		finally
		{
			DVP.Grid.EndDataUpdate();
		}
	}

	private void DataViewProgressWindow_BackgroundWorkerDoWorkCompleted(object sender, EventArgs e)
	{
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
		DVP.PanelIsLoaded = true;
		DVP.FitColumns();
		if (DVP.Grid.VisibleRowCount > 0)
		{
			FocusManager.SetFocusedElement((DependencyObject)(object)DVP, DVP.Grid);
			if (ShowExpanded() && DVP.Grid.View is IPanelView panelView)
			{
				panelView.ExpandGroups();
			}
			FocusManager.SetFocusedElement((DependencyObject)(object)DVP, DVP.Grid);
			DVP.Grid.CurrentItem = DVP.Grid.GetRow(DVP.Grid.GetRowHandleByVisibleIndex(0));
			DVP.Grid.SelectedItem = null;
		}
		HeaderImage = null;
		DVP.HeaderImage = HeaderImage;
		Mouse.OverrideCursor = null;
		if (_dataProviderProgressWindowInside.IfErrorExist())
		{
			DVP.CaptionImage = ImageService.GetImage("error_16.svg");
		}
		else
		{
			_dataProviderProgressWindowInside.Visibility = Visibility.Collapsed;
		}
		DVP.SetBarStatesEnabled();
		DVP.SetBarStates();
		DVP.InRefresh = DataViewPanel.RefreshType.None;
		CallsRecordingHelper.RecordCallStep(StatisticTypeSE.DataView, GetType().Name);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDataView))
		{
			CallsRecordingHelper.ExportDataView(GetType().Name, ProviderType, DVP);
		}
	}

	public bool Show(bool useParamValues = false, bool openAsChild = true, Window owner = null, bool withoutShowDialog = false)
	{
		if (AllowNotifications)
		{
			NavigationBarsControllerBase.UpdateNavigatorPanelNotificationState(SmartEnumHelper.GetDataProviderUserAccessId(ProviderType), hasNotification: false);
		}
		if (ProviderType > 0 && !Settings.UserAccessService.GetProviderPermission(ProviderType).AllowView)
		{
			throw new ASException(string.Format(Resources.DontHavePermissionToExecuteData, SmartEnumHelper.GetPartialSmartEnumCaption(typeof(DataProviderTypeSE), ProviderType.ToString())), 20008);
		}
		bool result = false;
		AddUnboundColumns();
		AddExtendedColumns();
		if (!withoutShowDialog && DialogSupported)
		{
			if (ShowDialog(useParamValues, isRefreshMode: false, owner))
			{
				ShowBrowser(openAsChild);
				result = true;
			}
		}
		else
		{
			ShowBrowser(openAsChild);
			result = true;
		}
		return result;
	}

	public bool ShowDialog(bool useParamValues, bool isRefreshMode, Window owner = null)
	{
		DataViewParametersDialogWindow dataViewParametersDialogWindow = ((owner == null) ? CreateDialog(useParamValues, isRefreshMode) : CreateDialog(useParamValues, isRefreshMode, owner));
		bool flag = dataViewParametersDialogWindow?.ShowDialog(useParamValues, isRefreshMode) ?? true;
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile(GetType().Name + " data view dialog execute start");
		}
		if ((Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDataView) && dataViewParametersDialogWindow != null) & flag)
		{
			CallsRecordingHelper.ExportDataViewDialogResult(GetType().Name, ProviderType, dataViewParametersDialogWindow.Name, dataViewParametersDialogWindow.Caption, dataViewParametersDialogWindow.GetControlsInfoForRecord());
		}
		return flag;
	}

	public virtual DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return null;
	}

	public virtual DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode, Window owner)
	{
		return null;
	}

	private void ShowBrowser(bool bOpenAsChild = false)
	{
		Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
		DataViewPanel panel = new DataViewPanel(this);
		BeforeLoadData(panel);
		AddDirectoryLinksToColumns(panel);
		if (LoadingIndication == LoadIndication.None)
		{
			LoadData();
			if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDataView))
			{
				CallsRecordingHelper.ExportDataProviderResult(DataProvider.GetType().Name, ParametersProvider, Data.ToList());
			}
			AfterLoadData(panel);
			LoadReferenceColumns();
			panel.PanelIsLoaded = true;
		}
		panel.SetCaption();
		panel.BindData();
		panel.Grid.BeginDataUpdate();
		panel.SetBarStates();
		panel.AddToolbarItems();
		Settings.HostingEnvironment.AddPanelToDocumentGroup(panel, bOpenAsChild);
		Settings.HostingEnvironment.LayoutManager.Activate(panel);
		if (LoadingIndication == LoadIndication.None)
		{
			panel.FitColumns();
		}
		Settings.HostingEnvironment.LayoutManager.Activate(panel);
		try
		{
			if (LoadingIndication == LoadIndication.None)
			{
				((DispatcherObject)Settings.HostingEnvironment).Dispatcher.Invoke((Action)(() =>
				{
					Mouse.OverrideCursor = null;
				}), (DispatcherPriority)6, default(CancellationToken));
			}
			else if (LoadingIndication == LoadIndication.AlwaysInside)
			{
				((DispatcherObject)Settings.HostingEnvironment).Dispatcher.Invoke((Action)(() =>
				{
					ShowProgressWindowInside(panel);
				}), (DispatcherPriority)6);
			}
			else
			{
				((DispatcherObject)Settings.HostingEnvironment).Dispatcher.Invoke((Action)(() =>
				{
					ShowProgressWindow(panel);
				}), (DispatcherPriority)6);
			}
		}
		finally
		{
			Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
			panel.Grid.UnselectAll();
			panel.Grid.EndDataUpdate();
		}
		if (LoadingIndication == LoadIndication.AlwaysInside)
		{
			return;
		}
		if (panel.Grid.VisibleRowCount > 0)
		{
			SelectByCondition(panel);
			if (ShowExpanded())
			{
				panel.ExpandGroups();
			}
			FocusManager.SetFocusedElement((DependencyObject)(object)panel, panel.Grid);
			panel.Grid.CurrentItem = panel.Grid.GetRow(panel.Grid.GetRowHandleByVisibleIndex(0));
			panel.Grid.SelectedItem = null;
		}
		CallsRecordingHelper.RecordCallStep(StatisticTypeSE.DataView, GetType().Name);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDataView))
		{
			CallsRecordingHelper.ExportDataView(GetType().Name, ProviderType, panel);
		}
		Mouse.OverrideCursor = null;
	}

	protected virtual void SelectByCondition(DataViewPanel dvp)
	{
	}

	public void AddUnboundColumns()
	{
		if (this == null || string.IsNullOrWhiteSpace(DataViewGroup))
		{
			return;
		}
		foreach (UnboundColumn item in Settings.UnboundColumnsService.Get(DataViewGroup))
		{
			string text = item.Caption;
			if (LanguageService.IsEnglish)
			{
				text = item.CaptionEn;
			}
			else if (LanguageService.IsRussian)
			{
				text = item.CaptionRu;
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = item.Caption;
			}
			ViewColumn viewColumn = new ViewColumn(item.Name, text, item.Name)
			{
				FooterSum = (item.Type == (short)UnboundColumnTypeSE.Decimal),
				Visible = true,
				IsUnbound = true,
				Group = Resources.UnboundColumns,
				IsDynamic = true,
				Type = (DataColumnType)item.Type,
				UnboundExpression = item.Expression
			};
			if (item.Type == (short)UnboundColumnTypeSE.Decimal)
			{
				viewColumn.Format = DecimalExtension.GetCurrencyFormat(16, item.Precision);
			}
			Columns.Add(viewColumn);
			if (!PotentiallyColumns.Contains(viewColumn))
			{
				PotentiallyColumns.Insert(0, viewColumn);
			}
		}
	}

	private protected virtual void AddExtendedColumns()
	{
	}

	public void ApplyDataViewDefinition(string name)
	{
		DataViewDefinition dvd = null;
		if (name != DataViewGroup)
		{
			dvd = Settings.ServiceProvider.GetRequiredService<IDVDService>().Load(DataViewGroup, name);
		}
		ApplyDataViewDefinition(dvd);
	}

	public void ApplyDataViewDefinition(DataViewDefinition dvd)
	{
		BeforeApplyDataViewDefinition(dvd);
		ExtendableRow.RowSchema rowSchema = new ExtendableRow.RowSchema(typeof(R), SchemaProvider.Columns, ColumnsProvider);
		if (rowSchema == null)
		{
			_dvd = null;
			return;
		}
		if (dvd == null)
		{
			_dvd = null;
			return;
		}
		ViewColumnsCollection viewColumnsCollection = Columns.Clone();
		if (!dvd.PreserveColumns)
		{
			Columns.Clear();
		}
		DataColumnType type = DataColumnType.String;
		foreach (ArmSoft.SME.Core.UI.MetaData.ViewColumn viewColumn in dvd.ViewColumns)
		{
			if (viewColumn.Action == ViewColumnAction.Add)
			{
				if (Columns.Contains(viewColumn.Name))
				{
					throw new ASException(string.Format(Resources.ColumnAlreadyExists, viewColumn.Name));
				}
				if (viewColumnsCollection.Contains(viewColumn.Name))
				{
					Columns.Add(viewColumnsCollection[viewColumn.Name]);
				}
				else
				{
					ExtendableRow.Column column = null;
					if (rowSchema.DynamicColumns.TryGetValue(viewColumn.Name, out var value))
					{
						column = value;
					}
					else
					{
						if (!dvd.CalculatedColumns.TryGetValue(viewColumn.Name, out var value2))
						{
							throw new ASException(string.Format(Resources.InvalidColumnName, viewColumn.Name));
						}
						column = new ExtendableRow.Column(new Column
						{
							Name = value2.Name,
							Caption = value2.Caption,
							Source = value2.Source,
							SourceType = ColumnSourceType.Extender
						}, null, 0, rowSchema.RowType);
					}
					switch (column.Type)
					{
					case SupportedType.String:
						type = DataColumnType.String;
						break;
					case SupportedType.Boolean:
						type = DataColumnType.Boolean;
						break;
					case SupportedType.DateTime:
						type = DataColumnType.DateTime;
						break;
					case SupportedType.Decimal:
						type = DataColumnType.Decimal;
						break;
					case SupportedType.Integer:
						type = DataColumnType.Integer;
						break;
					}
					Columns.AddDynamic(column.Name, column.Caption, column.Name, type, footerSum: false, "", "", isExtended: true).MethodInfoSource = column.Source;
					Columns[column.Name].Format = column.Format;
				}
			}
			else if (!Columns.Contains(viewColumn.Name))
			{
				throw new ASException(string.Format(Resources.InvalidColumnName, viewColumn.Name));
			}
			if (viewColumn.Action == ViewColumnAction.Remove)
			{
				Columns.Remove(viewColumn.Name);
				continue;
			}
			Columns[viewColumn.Name].Visible = viewColumn.Visible;
			Columns[viewColumn.Name].FooterSum = viewColumn.FooterSum;
			if (!string.IsNullOrWhiteSpace(viewColumn.CaptionId) || !string.IsNullOrWhiteSpace(viewColumn.Caption))
			{
				Columns[viewColumn.Name].Caption = viewColumn.Caption;
			}
			if (!string.IsNullOrWhiteSpace(viewColumn.Format))
			{
				Columns[viewColumn.Name].Format = viewColumn.Format;
			}
			if (!string.IsNullOrWhiteSpace(viewColumn.After))
			{
				Columns.Move(viewColumn.Name, viewColumn.After, after: true);
			}
			else if (!string.IsNullOrWhiteSpace(viewColumn.Before))
			{
				Columns.Move(viewColumn.Name, viewColumn.Before, after: false);
			}
		}
		Dictionary<string, object> extendedParameters = null;
		if (dvd.DialogExtension != null)
		{
			extendedParameters = dvd.DialogExtension.Values;
		}
		_dvd = dvd;
		Type type2 = DefinitionMappingsService.ResolveDVD(DataViewGroup, _dvd.Name);
		if (type2 != null)
		{
			IDataProviderExtensionHandler dataProviderExtensionHandler = (IDataProviderExtensionHandler)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, type2);
			if (dataProviderExtensionHandler != null)
			{
				dataProviderExtensionHandler.SetParameters(ApplyDataViewDefinitionParametersInner(extendedParameters));
				string text = dataProviderExtensionHandler.Caption();
				if (!string.IsNullOrWhiteSpace(text))
				{
					Caption = text;
				}
				else
				{
					Caption = dvd.Caption;
				}
				ConditionalFormattingList.AddRange(dataProviderExtensionHandler.ConditionalFormatting());
			}
		}
		if (dvd.SortOrder.Count > 0)
		{
			Columns.ClearSorting();
			foreach (ArmSoft.SME.Core.UI.MetaData.ColumnOrder item in dvd.SortOrder)
			{
				Columns.AddSorting(item.Name, item.Descending);
			}
		}
		switch (dvd.Indication)
		{
		case ViewLoadIndication.Always:
			_loadingIndication = LoadIndication.Always;
			break;
		case ViewLoadIndication.None:
			_loadingIndication = LoadIndication.None;
			break;
		}
	}

	public void ApplySettings(DataViewSetting dvs)
	{
		int num = 0;
		for (int i = 0; i <= dvs.Columns.Count - 1; i++)
		{
			string text = dvs.Columns[i];
			if (Columns.Contains(text))
			{
				Columns.Move(text, num);
				num++;
				continue;
			}
			ViewColumn col = null;
			foreach (ViewColumn potentiallyColumn in PotentiallyColumns)
			{
				if (potentiallyColumn.Name == text)
				{
					col = potentiallyColumn;
					break;
				}
			}
			if (col != null)
			{
				Columns.Insert(num, col);
				Schema schema = DataProvider.GetSchema(ParametersProvider, DataProvider.Settings, ColumnsProvider);
				if (!schema.Columns.Any((Column p) => p.Name == col.Name))
				{
					schema.Columns.Add(new Column
					{
						Name = col.Name,
						Format = col.Format,
						Group = col.Format,
						Source = col.Source,
						FooterSum = col.FooterSum,
						SourceType = ColumnSourceType.Reference,
						Type = col.Type.CastToDataColumnType(),
						Caption = col.Caption
					});
				}
				num++;
			}
		}
		for (int num2 = Columns.ViewList.Count - 1; num2 >= num; num2--)
		{
			Columns.Remove(Columns.ViewList[num2].Name);
		}
		if (AllowChangeSorting && dvs.SortOrder.Count > 0)
		{
			Columns.ClearSorting();
			foreach (ArmSoft.SME.Common.Models.Data.ColumnOrder item in dvs.SortOrder)
			{
				Columns.AddSorting(item.Name, item.Descending);
			}
		}
		if (AllowChangeGrouping && dvs.GroupBy.Count > 0)
		{
			Columns.ClearGroupBy();
			foreach (List<string> item2 in dvs.GroupBy)
			{
				if (item2.Count <= 0)
				{
					continue;
				}
				Columns.AddGroup();
				foreach (string item3 in item2)
				{
					Columns.AddToLastGroup(item3);
				}
			}
		}
		FilterString = dvs.FilterString;
	}

	public virtual void BeforeApplyDataViewDefinition(DataViewDefinition dvd)
	{
	}

	public void ApplyDefaultDataViewDefinition()
	{
		if (!string.IsNullOrWhiteSpace(DataViewGroup))
		{
			DataViewDefinition dataViewDefinition = Settings.ServiceProvider.GetRequiredService<IDVDService>().LoadDefault(DataViewGroup);
			if (dataViewDefinition != null)
			{
				ApplyDataViewDefinition(dataViewDefinition);
			}
		}
	}

	public void ApplyDataViewDefinitionParameters()
	{
	}

	private Dictionary<string, object> ApplyDataViewDefinitionParametersInner(Dictionary<string, object> extendedParameters)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (KeyValuePair<string, DataViewParameter> parameter in Parameters)
		{
			dictionary.Add(parameter.Value.Name, parameter.Value.Value);
		}
		if (extendedParameters != null)
		{
			foreach (KeyValuePair<string, object> extendedParameter in extendedParameters)
			{
				if (!dictionary.ContainsKey(extendedParameter.Key))
				{
					dictionary.Add(extendedParameter.Key, extendedParameter.Value);
				}
			}
		}
		return dictionary;
	}

	public virtual bool CardViewAllowed()
	{
		return true;
	}

	public virtual DataTemplate GetCardHeaderTemplate()
	{
		return null;
	}

	public virtual DataTemplate GetCardTemplate()
	{
		return null;
	}

	public virtual DataTemplate GetCardPrintTemplate()
	{
		return null;
	}

	public virtual void OnMagneticCardReader(string mcrData)
	{
	}

	public virtual void OnScanner(string scannerData)
	{
	}

	public virtual void OnF2()
	{
	}

	public virtual System.Windows.Controls.ToolTip ToolTipContent(object sender)
	{
		return null;
	}

	public virtual void EditSelected()
	{
		ValidateSelectedRowsForEditing();
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = Resources.EditSelected,
			Name = "EditSelected",
			MaxHeight = Screen.PrimaryScreen.WorkingArea.Height
		};
		AddControlsInEditSelectedDialog(dialogWindow, out var doRefresh);
		dialogWindow.WindowValidation += EditSelectedDialogValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			List<PropertyWithValue> properties = ApplyValuesFromEditSelectedDialog(dialogWindow, out var withoutValidation);
			EditSelectedRows(properties, doRefresh, withoutValidation);
		}
	}

	public virtual void EditSelectedDialogValidation(DialogWindow window, DialogWindowValidationArgs args)
	{
		foreach (string item in window.Controls.Keys.Where((string p) => !p.EndsWith("_CHB") && !p.EndsWith("_TIMEOFDATE")))
		{
			if (DialogChangeDemandControls.IsFieldChanged(window, item))
			{
				return;
			}
		}
		throw new ASException(Resources.NoFieldEditDemand);
	}

	protected virtual void EditSelectedRows(List<PropertyWithValue> properties, bool doRefresh, bool withoutValidation = false)
	{
	}

	protected virtual void CreatePOExtensionDefinition()
	{
	}

	protected virtual void EditSelectedRowsExtensionFields()
	{
	}

	protected void EditSelectedDirectories(List<PropertyWithValue> properties, List<int> selectedIdKeys, List<string> selectedCodeKeys, short directoryType, out EnumerateErrorsReportViewer errorRepViewer, bool withoutValidation = false, bool areExtensionFields = false)
	{
		if ((selectedIdKeys == null && selectedCodeKeys == null) || directoryType == 0)
		{
			throw new ArgumentException("SelectedIdKeys/SelectedCodeKeys and DirectoryType must be given.");
		}
		errorRepViewer = new EnumerateErrorsReportViewer(Resources.EditErrors, "EditSelectedErrors");
		DirectoryEditFieldsWithSameValueRequestModel request = new DirectoryEditFieldsWithSameValueRequestModel
		{
			WithoutValidation = withoutValidation,
			Type = directoryType,
			Ids = selectedIdKeys,
			Keys = selectedCodeKeys,
			Properties = properties,
			AreExtensionFields = areExtensionFields
		};
		DirectoryEditFieldsWithSameValueResponseModel directoryEditFieldsWithSameValueResponseModel = DataProcessingRequestHelper.Execute<DirectoryEditFieldsWithSameValueRequestModel, DirectoryEditFieldsWithSameValueResponseModel>(DPRType.Other, "EditDirectoriesFieldsWithSameValue", request);
		Settings.DirectoryService.RefreshDirectoryUICaches(directoryType, ignoreDeleted: true);
		if (directoryEditFieldsWithSameValueResponseModel.Exceptions == null || directoryEditFieldsWithSameValueResponseModel.Exceptions.Count == 0)
		{
			return;
		}
		foreach (DirectoryEditResponseModel exception in directoryEditFieldsWithSameValueResponseModel.Exceptions)
		{
			errorRepViewer.AddError(exception.Exception.Message);
		}
	}

	protected virtual void StoreSelectedFields(List<PropertyWithValue> properties, bool doRefresh, bool withoutValidation = false, bool areExtensionFields = false)
	{
	}

	protected virtual List<string> ValidateSelectedRowsForEditing()
	{
		return null;
	}

	protected virtual void AddControlsInEditSelectedDialog(DialogWindow dialog, out bool doRefresh)
	{
		doRefresh = false;
	}

	protected static List<PropertyWithValue> ApplyValuesFromEditSelectedDialog(DialogWindow dialog, out bool withoutValidation)
	{
		withoutValidation = true;
		List<PropertyWithValue> list = new List<PropertyWithValue>();
		foreach (DialogWindow.ControlDescriptor item in dialog.Controls.Values.Where((DialogWindow.ControlDescriptor p) => !p.Name.EndsWith("_CHB") && !p.Name.EndsWith("_TIMEOFDATE")))
		{
			if (!DialogChangeDemandControls.IsFieldChanged(dialog, item.DataItemName))
			{
				continue;
			}
			if (item.SpecData == "DateTimeEditControl")
			{
				string text = item.Name + "_TIMEOFDATE";
				list.Add(new PropertyWithValue(item.DataItemName, Convert.ToString(Convert.ToDateTime(dialog[item.Name].ToString()).Add(Convert.ToDateTime(dialog[text]).TimeOfDay))));
				if (!Convert.ToBoolean(dialog.Controls[text].SpecData))
				{
					withoutValidation = false;
				}
			}
			else
			{
				list.Add(new PropertyWithValue(item.DataItemName, Convert.ToString(dialog[item.Name])));
				if (!Convert.ToBoolean(item.SpecData))
				{
					withoutValidation = false;
				}
			}
		}
		return list;
	}

	public GridControlBand CreateGridBand(string header, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Center)
	{
		return new GridControlBand
		{
			Header = header,
			HorizontalHeaderContentAlignment = horizontalAlignment,
			HeaderTemplate = new DataTemplate
			{
				VisualTree = CreateBoldHeaderFactory()
			},
			AllowMoving = DefaultBoolean.True,
			AllowResizing = DefaultBoolean.True
		};
	}

	private static FrameworkElementFactory CreateBoldHeaderFactory()
	{
		FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(TextBlock));
		frameworkElementFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
		frameworkElementFactory.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding());
		return frameworkElementFactory;
	}
}
