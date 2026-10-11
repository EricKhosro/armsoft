using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Documents;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.MetaData.Document;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.Exchange;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Caches;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.DataViews;

public class DocumentDataView<R, P> : DataView<R, P> where R : DocumentsDataRow, new() where P : class, new()
{
	private readonly DocumentExtension _documentExtension;

	private DocDefinition _docDefinition;

	private bool? _hasDocumentExtensionColumns;

	private IEnumerable<string> _extensionViewColumnsNames;

	public virtual short? DocType { get; set; }

	public string DocTypeCaption
	{
		get
		{
			if (!DocType.HasValue)
			{
				return "";
			}
			return SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), DocType.ToString());
		}
	}

	public virtual bool ShowCreateBasedOnDocFunction { get; set; } = true;

	public virtual bool ShowLinkedDocsFunction { get; set; } = true;

	public DocDefinition RowDocDefinition
	{
		get
		{
			if (DocType.HasValue)
			{
				if (_docDefinition == null)
				{
					DefinitionMappingsService.TryResolveDocumentDefinition(DocType.Value, out _docDefinition);
				}
				return _docDefinition;
			}
			if ((DVP ?? Settings.HostingEnvironment.CurrentViewPanel)?.FocusedRow() is R val)
			{
				DefinitionMappingsService.TryResolveDocumentDefinition(val.GetDocType(), out var definition);
				return definition;
			}
			return null;
		}
	}

	protected bool HasDocumentExtensionColumns
	{
		get
		{
			bool valueOrDefault = _hasDocumentExtensionColumns == true;
			if (!_hasDocumentExtensionColumns.HasValue)
			{
				valueOrDefault = Columns.ReferenceColumns.Any((ViewColumn col) => col.Name.StartsWith("DocumentExtension"));
				_hasDocumentExtensionColumns = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	protected IEnumerable<string> ExtensionViewColumnsNames => _extensionViewColumnsNames ?? (_extensionViewColumnsNames = from col in Columns.ReferenceColumns
		where col.Name.StartsWith("DocumentExtension")
		select col.Name);

	public override bool AllowAdd => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowEditSelected => false;

	public override bool AllowCreateCopy => AllowOperation;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowDeleteSelected => false;

	public override bool AllowPreview => true;

	public override bool AllowDocumentHistory => true;

	public override bool AllowExport
	{
		get
		{
			if (RowDocDefinition != null && RowDocDefinition.IsExchangeable)
			{
				return true;
			}
			return false;
		}
	}

	public override bool ExistExchangeableRow
	{
		get
		{
			if (AllowExport)
			{
				foreach (short item in from p in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<R>()
					select p.DocType)
				{
					if (DefinitionMappingsService.TryResolveDocumentDefinition(item, out var definition) && definition.IsExchangeable)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public DocumentDataView()
	{
		if (DocType.HasValue && Settings.CoreCacheService.DocumentExtensions.ItemsWithKeys.TryGetValue(new DocumentExtensionKey(POTypeSE.Document.Value, DocType.Value), out var value))
		{
			_extensionDefinition = new ArmSoft.SME.Core.UI.MetaData.DocumentExtensionDefinition(value.Xml, new ArmSoft.SME.Core.UI.MetaData.Environment());
			_documentExtension = new DocumentExtension(_extensionDefinition);
		}
	}

	public virtual void AddDocument(short docType)
	{
		IPOWindow iPOWindow = Settings.DocumentService.CreateWindow(docType);
		iPOWindow.DVPID = Settings.HostingEnvironment.CurrentViewPanel.Id;
		iPOWindow.Show();
	}

	public override void Add()
	{
		if (DocType.HasValue)
		{
			AddDocument(DocType.Value);
		}
	}

	public override void DeleteSelected()
	{
		DeleteDialog deleteDialog = new DeleteDialog();
		if (Convert.ToBoolean(deleteDialog.ShowDialog()))
		{
			List<Guid> selectedISNs = (from r in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<R>()
				select r.ISN).ToList();
			DeleteSelected(selectedISNs, deleteDialog.Comment.Text);
		}
	}

	public void DeleteSelected(List<Guid> selectedISNs, string comment = "")
	{
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(Resources.DeleteErrors, "DeleteSelectedErrors");
		DocumentDeleteSelectedRequestModel documentDeleteSelectedRequestModel = new DocumentDeleteSelectedRequestModel
		{
			DocISNs = selectedISNs
		};
		if (!string.IsNullOrWhiteSpace(comment))
		{
			documentDeleteSelectedRequestModel.DeleteRequest = new PODeleteRequest
			{
				LogComment = Convert.ToString(comment)
			};
		}
		DocumentDeleteSelectedResponseModel documentDeleteSelectedResponseModel = DataProcessingRequestHelper.Execute<DocumentDeleteSelectedRequestModel, DocumentDeleteSelectedResponseModel>(DPRType.Other, "DeleteSelectedDocuments", documentDeleteSelectedRequestModel);
		if (documentDeleteSelectedResponseModel.Exceptions != null && documentDeleteSelectedResponseModel.Exceptions.Count != 0)
		{
			foreach (DocumentDeleteResponseModel exception in documentDeleteSelectedResponseModel.Exceptions)
			{
				enumerateErrorsReportViewer.AddError(exception.Exception.Message, exception.DocISN);
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
		MessageBox.Show(string.Format(Resources.RemovedDocsCount, documentDeleteSelectedResponseModel.SuccessCount), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
	}

	protected override void EditSelectedRows(List<PropertyWithValue> properties, bool doRefresh, bool withoutValidation = false)
	{
		StoreSelectedFields(properties, doRefresh, withoutValidation);
	}

	public override void ExportToXML(string fileName, R row)
	{
		base.ExportToXML(fileName, row);
		List<R> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<R>();
		List<DocumentExchangeRow> list = new List<DocumentExchangeRow>();
		foreach (R item in selectedOrFocusedRows)
		{
			list.Add(new DocumentExchangeRow
			{
				DocType = item.GetDocType(),
				ISN = item.ISN
			});
		}
		byte[] array = Settings.CreateApiClient().Exchange.ExportDocument(list);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		fileStream.Write(array);
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		if (ShowLinkedDocsFunction)
		{
			List<SmartEnumDataProvider.DataRow> childDocTypes = Settings.DocumentService.GetChildDocTypes(RowDocDefinition, checkViewAccess: true);
			if (childDocTypes == null || childDocTypes.Count <= 0)
			{
				List<SmartEnumDataProvider.DataRow> baseDocTypes = Settings.DocumentService.GetBaseDocTypes(RowDocDefinition, checkViewAccess: true);
				if (baseDocTypes == null || baseDocTypes.Count <= 0)
				{
					goto IL_00a1;
				}
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "DocumentLinkedList", Resources.LinkedDoc, DocumentLinkedList, "report_link_16.svg");
		}
		goto IL_00a1;
		IL_00a1:
		if (ShowCreateBasedOnDocFunction)
		{
			List<SmartEnumDataProvider.DataRow> childDocTypes2 = Settings.DocumentService.GetChildDocTypes(RowDocDefinition, checkViewAccess: false, checkAddAccess: true);
			if (childDocTypes2 != null && childDocTypes2.Count > 0)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateBasedOnDoc", Resources.CreateBasedOnDoc, CreateBasedOnDoc, "doc_based_on_16.svg");
			}
		}
	}

	protected virtual void DocumentLinkedList()
	{
	}

	protected virtual void CreateBasedOnDoc()
	{
		try
		{
			ContextMenu contextMenu = new ContextMenu
			{
				FontFamily = Settings.FontFamily,
				FontSize = Settings.GetFontSize()
			};
			List<SmartEnumDataProvider.DataRow> childDocTypes = Settings.DocumentService.GetChildDocTypes(RowDocDefinition, checkViewAccess: false, checkAddAccess: true);
			if (childDocTypes == null)
			{
				return;
			}
			if (childDocTypes.Count == 1)
			{
				CreateChildDocument(short.Parse(childDocTypes.First().Code));
				return;
			}
			foreach (SmartEnumDataProvider.DataRow item in childDocTypes)
			{
				MenuItem menuItem = new MenuItem
				{
					Header = item.Caption,
					Tag = item.Code
				};
				menuItem.Click += new SafeEventHandler<RoutedEventArgs>(CreateChildDocument).Handler;
				contextMenu.Items.Add(menuItem);
			}
			contextMenu.ToolTip = Resources.NewDocuments;
			contextMenu.PlacementTarget = Settings.HostingEnvironment.CurrentViewPanel;
			contextMenu.Placement = PlacementMode.Center;
			contextMenu.VerticalOffset = 0.0;
			contextMenu.HorizontalOffset = 0.0;
			contextMenu.IsOpen = true;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected void CreateChildDocument(object sender, RoutedEventArgs e)
	{
		MenuItem menuItem = (MenuItem)sender;
		CreateChildDocument(short.Parse(menuItem.Tag.ToString()));
	}

	protected virtual void CreateChildDocument(short childDocType)
	{
	}

	private protected override void AddExtendedColumns()
	{
		if (this == null || string.IsNullOrWhiteSpace(DataViewGroup) || !DocType.HasValue)
		{
			return;
		}
		foreach (ViewColumn dataViewColumn in DocumentExtensionsCache.GetDataViewColumns(POTypeSE.Document.Value, DocType.Value))
		{
			if (!PotentiallyColumns.Contains(dataViewColumn))
			{
				PotentiallyColumns.Insert(0, dataViewColumn);
			}
		}
	}

	private protected override void CalculateExtendedRow(ExtendableRow row, IDataProviderExtensionHandler handler)
	{
		base.CalculateExtendedRow(row, handler);
		if (!HasDocumentExtensionColumns || _extensionDefinition == null || !(row is DocumentsDataRow documentsDataRow) || string.IsNullOrWhiteSpace(documentsDataRow.ExtensionBody))
		{
			return;
		}
		_documentExtension.LoadFromXML(documentsDataRow.ExtensionBody);
		foreach (string extensionViewColumnsName in ExtensionViewColumnsNames)
		{
			row[extensionViewColumnsName] = GetDataRowDocumentExtensionValue(extensionViewColumnsName);
		}
	}

	protected override void StoreSelectedFields(List<PropertyWithValue> properties, bool doRefresh, bool withoutValidation = false, bool areExtensionFields = false)
	{
		Mouse.OverrideCursor = Cursors.Wait;
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(Resources.EditErrors, "EditSelectedErrors");
		List<Guid> list = new List<Guid>();
		List<R> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<R>();
		foreach (R item in selectedRows)
		{
			list.Add(item.ISN);
		}
		DocumentEditFieldsWithSameValueRequestModel request = new DocumentEditFieldsWithSameValueRequestModel
		{
			DocISNs = list,
			Properties = properties,
			AreExtensionFields = areExtensionFields,
			StoreRequest = new StoreRequest
			{
				WithoutValidation = withoutValidation,
				WithoutGrids = withoutValidation,
				LogComment = (areExtensionFields ? Resources.EditUserDefinedFields : Resources.EditSelected),
				CheckActive = false,
				IsGroupEdited = true,
				IgnoreWarnings = true
			}
		};
		DocumentEditFieldsResponseModel documentEditFieldsResponseModel = DataProcessingRequestHelper.Execute<DocumentEditFieldsWithSameValueRequestModel, DocumentEditFieldsResponseModel>(DPRType.Other, "EditDocumentsFieldsWithSameValue", request);
		if (documentEditFieldsResponseModel.Exceptions != null && documentEditFieldsResponseModel.Exceptions.Count != 0)
		{
			foreach (DocumentEditResponseModel exception in documentEditFieldsResponseModel.Exceptions)
			{
				enumerateErrorsReportViewer.AddError(exception.Exception.Message, exception.DocISN);
			}
		}
		Mouse.OverrideCursor = null;
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		if (doRefresh)
		{
			currentViewPanel.Refresh();
		}
		else
		{
			DataViewPanel dataViewPanel = currentViewPanel;
			List<object> list2 = new List<object>();
			foreach (Guid item2 in selectedRows.Select((R p) => p.ISN))
			{
				list2.Add(item2);
			}
			dataViewPanel.Update(list2);
		}
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
	}

	protected override void EditSelectedRowsExtensionFields()
	{
		DialogWindow dialogWindow = ExtensionUIHelper.CreateDocumentExtensionMultipleFieldsEditDialog(_extensionDefinition);
		dialogWindow.WindowValidation += base.EditSelectedDialogValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			List<PropertyWithValue> properties = DataView<R, P>.ApplyValuesFromEditSelectedDialog(dialogWindow, out var withoutValidation);
			StoreSelectedFields(properties, doRefresh: false, withoutValidation, areExtensionFields: true);
		}
	}

	private object GetDataRowDocumentExtensionValue(string columnName)
	{
		string text = columnName.Substring("DocumentExtension".Length);
		int num = text.IndexOf("_TreeNodeName_");
		object value;
		if (num >= 0)
		{
			string key = text.Substring(0, num);
			if (_documentExtension.Values.TryGetValue(key, out value))
			{
				string text2 = value.ToString();
				if (!string.IsNullOrWhiteSpace(text2))
				{
					return ((TreeNodesCache)Settings.CoreCacheService.Tree.GetItem(text.Substring(num + "_TreeNodeName_".Length), throwExceptionNotExits: true).TreeNodeCache).GetItem(text2)?.Name;
				}
			}
		}
		else if (_documentExtension.Values.TryGetValue(text, out value))
		{
			return value;
		}
		return null;
	}
}
