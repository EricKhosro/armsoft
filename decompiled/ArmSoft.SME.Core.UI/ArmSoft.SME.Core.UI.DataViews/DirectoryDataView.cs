using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Directories;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.MetaData.Document;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.PrintableElements;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews;

public abstract class DirectoryDataView<R, P> : DataView<R, P> where R : ExtendableRow, IRowWithKey, new() where P : class, new()
{
	public override bool AllowCreatePOExtensionDefinition => DirectoryDefinition.IsExtendable == POExtendableType.Simple;

	protected virtual bool AllowFindRowByMTBarcode { get; }

	protected virtual string MTCodeColumnName { get; }

	public abstract short DirectoryType { get; }

	protected virtual bool IsPOWindow { get; }

	public DirectoryDefinition DirectoryDefinition
	{
		get
		{
			DefinitionMappingsService.TryResolveDirectoryDefinition(DirectoryType, out var definition);
			return definition;
		}
	}

	public override bool AllowAdd => Settings.UserAccessService.GetDirectoryPermission(DirectoryType).AllowAdd;

	public override bool AllowView => Settings.UserAccessService.GetDirectoryPermission(DirectoryType).AllowView;

	public override bool AllowEdit => Settings.UserAccessService.GetDirectoryPermission(DirectoryType).AllowEdit;

	public override bool AllowCreateCopy
	{
		get
		{
			if (AllowAdd)
			{
				if (!AllowEdit)
				{
					return AllowView;
				}
				return true;
			}
			return false;
		}
	}

	public override bool AllowDelete => Settings.UserAccessService.GetDirectoryPermission(DirectoryType).AllowDelete;

	public override bool AllowDeleteSelected => AllowDelete;

	public override bool AllowPreview => false;

	public override bool AllowDocumentHistory => DirectoryDefinition?.AllowLog ?? false;

	public virtual bool AllowEditCodeOfDirectories => false;

	public override bool AllowExport
	{
		get
		{
			if (DirectoryDefinition != null && DirectoryDefinition.IsExchangeable)
			{
				if (!AllowEdit)
				{
					return AllowView;
				}
				return true;
			}
			return false;
		}
	}

	public DataViewPanel ParentViewPanel
	{
		get
		{
			if (!ShowAsTransactionPanel)
			{
				return Settings.HostingEnvironment.CurrentViewPanel;
			}
			return (DataViewPanel)Settings.HostingEnvironment.CurrentViewPanel.TransactionsControl;
		}
	}

	public virtual object GetKey(object row)
	{
		return ((IRowWithKey)row).GetKey();
	}

	public DirectoryDataView()
	{
		if (Settings.CoreCacheService.DocumentExtensions.ItemsWithKeys.TryGetValue(new DocumentExtensionKey(POTypeSE.Directory.Value, DirectoryType), out var value))
		{
			_extensionDefinition = new DocumentExtensionDefinition(value.Xml, new ArmSoft.SME.Core.UI.MetaData.Environment());
		}
		AddConditionalFormatting();
	}

	protected ArmSoft.SME.Common.Models.Directories.Directory LoadDirectory()
	{
		return LoadDirectory(ParentViewPanel.FocusedRow());
	}

	protected ArmSoft.SME.Common.Models.Directories.Directory LoadDirectory(object row)
	{
		object key = GetKey(row);
		if (key is int)
		{
			return Settings.DirectoryService.Load(DirectoryType, Convert.ToInt32(key));
		}
		if (key is short)
		{
			return Settings.DirectoryService.Load(DirectoryType, Convert.ToInt16(key));
		}
		if (key is string)
		{
			return Settings.DirectoryService.Load(DirectoryType, Convert.ToString(key));
		}
		return Settings.DirectoryService.Load(DirectoryType, JsonSerializer.Serialize(key));
	}

	public override void Add()
	{
		if (IsPOWindow)
		{
			IPOWindow iPOWindow = Settings.DirectoryService.CreatePOWindow(DirectoryType);
			iPOWindow.DVPID = ParentViewPanel.Id;
			iPOWindow.Show();
		}
		else
		{
			IDirectoryDialogWindow directoryDialogWindow = Settings.DirectoryService.CreateDialogWindow(DirectoryType, DialogWindowMode.NewMode);
			directoryDialogWindow.DVPId = ParentViewPanel.Id;
			directoryDialogWindow.ShowDialog();
		}
	}

	public override void Edit()
	{
		Settings.DirectoryService.ShowWindow(LoadDirectory(), IsPOWindow, WindowViewMode.Edit, ParentViewPanel.Id);
	}

	public override void View()
	{
		Settings.DirectoryService.ShowWindow(LoadDirectory(), IsPOWindow, WindowViewMode.View, ParentViewPanel.Id);
	}

	public override void CreateCopy()
	{
		object key = GetKey(ParentViewPanel.FocusedRow());
		ArmSoft.SME.Common.Models.Directories.Directory directory;
		if (key is int)
		{
			directory = Settings.DirectoryService.CreateCopy(DirectoryType, Convert.ToInt32(key));
		}
		else if (key is short)
		{
			directory = Settings.DirectoryService.CreateCopy(DirectoryType, Convert.ToInt32(key));
		}
		else
		{
			directory = ((!(key is string)) ? Settings.DirectoryService.CreateCopy(DirectoryType, JsonSerializer.Serialize(key)) : Settings.DirectoryService.CreateCopy(DirectoryType, Convert.ToString(key)));
		}
		Settings.DirectoryService.ShowWindow(directory, IsPOWindow, WindowViewMode.Copy, ParentViewPanel.Id);
	}

	public override void Delete()
	{
		string code = ((IRowWithKey)ParentViewPanel.FocusedRow()).GetCode();
		string messageBoxText = ((!string.IsNullOrWhiteSpace(code)) ? string.Format(Resources.ConfDirDelWithCode, Settings.DirectoryService.GetCaption(DirectoryType), code) : string.Format(Resources.ConfDirDel, Settings.DirectoryService.GetCaption(DirectoryType)));
		if (MessageBox.Show(messageBoxText, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			Mouse.OverrideCursor = Cursors.Wait;
			object key = GetKey(ParentViewPanel.FocusedRow());
			if (key is int)
			{
				Settings.DirectoryService.Delete(DirectoryType, Convert.ToInt32(key));
			}
			else if (key is short)
			{
				Settings.DirectoryService.Delete(DirectoryType, Convert.ToInt16(key));
			}
			else if (key is string)
			{
				Settings.DirectoryService.Delete(DirectoryType, Convert.ToString(key));
			}
			else
			{
				Settings.DirectoryService.Delete(DirectoryType, JsonSerializer.Serialize(key));
			}
			ParentViewPanel.Update(key);
			Mouse.OverrideCursor = null;
		}
	}

	public override void DeleteSelected()
	{
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(Resources.DeleteErrors, "DeleteSelectedErrors");
		if (MessageBox.Show(Resources.ConfirmSelectedRowsDelete, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
		{
			return;
		}
		bool flag = false;
		List<int> list = new List<int>();
		List<string> list2 = new List<string>();
		List<R> selectedRows = ParentViewPanel.GetSelectedRows<R>();
		foreach (R item in selectedRows)
		{
			object key = GetKey(item);
			if (key is int)
			{
				list.Add(Convert.ToInt32(key));
				continue;
			}
			if (key is string)
			{
				list2.Add(Convert.ToString(key));
				continue;
			}
			flag = true;
			list2.Add(JsonSerializer.Serialize(key));
		}
		DirectoryDeleteSelectedRequestModel request = new DirectoryDeleteSelectedRequestModel
		{
			Type = DirectoryType,
			Ids = list,
			Keys = list2
		};
		DirectoryDeleteSelectedResponseModel directoryDeleteSelectedResponseModel = DataProcessingRequestHelper.Execute<DirectoryDeleteSelectedRequestModel, DirectoryDeleteSelectedResponseModel>(DPRType.Other, "DeleteSelectedDirectories", request);
		if (directoryDeleteSelectedResponseModel.Exceptions != null && directoryDeleteSelectedResponseModel.Exceptions.Count != 0)
		{
			foreach (DirectoryDeleteResponseModel exception in directoryDeleteSelectedResponseModel.Exceptions)
			{
				enumerateErrorsReportViewer.AddError(exception.Exception.Message);
			}
		}
		Mouse.OverrideCursor = Cursors.Wait;
		Settings.DirectoryService.RefreshDirectoryUICaches(DirectoryType);
		if (flag)
		{
			ParentViewPanel.Refresh();
		}
		else
		{
			List<object> list3 = new List<object>();
			foreach (R item2 in selectedRows)
			{
				list3.Add(GetKey(item2));
			}
			ParentViewPanel.Update(list3);
		}
		Mouse.OverrideCursor = null;
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
		MessageBox.Show(string.Format(Resources.RemovedDirsCount, directoryDeleteSelectedResponseModel.SuccessCount), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
	}

	protected override void EditSelectedRows(List<PropertyWithValue> properties, bool doRefresh, bool withoutValidation = false)
	{
		StoreSelectedFields(properties, doRefresh, withoutValidation);
	}

	public override void DocumentHistory()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			R val = (R)ParentViewPanel.FocusedRow();
			if (val != null)
			{
				new DirectoryLog(DirectoryType, (int)val.GetKey(), val.GetCode(), val.GetName()).Show();
			}
		}
	}

	public override IReadOnlyList<TemplatesCacheRow> PrintTemplates()
	{
		return Settings.CoreCacheService.Template.GetTemplates(DirectoryType);
	}

	public override TemplatesCacheRow DefaultPrintTemplate()
	{
		return Settings.CoreCacheService.Template.GetDefaultPrintTemplate(DirectoryType);
	}

	public override void Preview(int rowHandle, PrintMode mode, TemplatesCacheRow template = null)
	{
		object row = ((rowHandle != -1) ? Settings.HostingEnvironment.CurrentViewPanel.Row(rowHandle) : Settings.HostingEnvironment.CurrentViewPanel.FocusedRow());
		if (template == null)
		{
			template = DefaultPrintTemplate();
		}
		Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(LoadDirectory(row), mode, template.Name);
	}

	public override void PreviewSelected(List<int> rowsHandle, PrintMode? mode = null, TemplatesCacheRow template = null, bool insertPageBreaks = true)
	{
		List<PrintableElement> list = new List<PrintableElement>();
		foreach (int item in rowsHandle)
		{
			list.Add(LoadDirectory(Settings.HostingEnvironment.CurrentViewPanel.Row(item)));
		}
		PrintMode valueOrDefault = mode.GetValueOrDefault();
		if (!mode.HasValue)
		{
			valueOrDefault = PrintMode.PrintPreview;
			mode = valueOrDefault;
		}
		if (template == null)
		{
			template = DefaultPrintTemplate();
		}
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(Resources.PreviewErrorsReportTitle, "PrintPreview");
		Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(list, mode.Value, template?.Name, insertPageBreaks, null, enumerateErrorsReportViewer);
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
	}

	public override void ExportToXML(string fileName, R row)
	{
		base.ExportToXML(fileName, row);
		List<R> selectedOrFocusedRows = ParentViewPanel.GetSelectedOrFocusedRows<R>();
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		foreach (R item in selectedOrFocusedRows)
		{
			if (string.IsNullOrWhiteSpace(item.GetCode()))
			{
				object key = item.GetKey();
				if (key.GetType().IsValueType)
				{
					list2.Add(Convert.ToInt32(key));
				}
				else
				{
					list.Add(JsonSerializer.Serialize(key));
				}
			}
			else
			{
				list.Add(item.GetCode());
			}
		}
		byte[] array = ((list2.Count <= 0) ? Settings.CreateApiClient().Exchange.ExportDirectory(DirectoryType, list) : Settings.CreateApiClient().Exchange.ExportDirectory(DirectoryType, list2));
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		fileStream.Write(array);
	}

	public override void InitContextFunctions()
	{
		if (ParentViewPanel.FocusedRow() != null && AllowEditCodeOfDirectories && AllowEdit)
		{
			ParentViewPanel.AddContextFunction("ContextFunctions", "EditCode", Resources.EditCodeOfDirectories, EditCode);
			ParentViewPanel.AddContextSeparator("ContextFunctions");
		}
	}

	protected override void CreatePOExtensionDefinition()
	{
		ExtensionUIHelper.CreatePOExtensionBySimpleWindow(POTypeSE.Directory, DirectoryDefinition.POType);
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

	protected override void StoreSelectedFields(List<PropertyWithValue> properties, bool doRefresh, bool withoutValidation = false, bool areExtensionFields = false)
	{
		Mouse.OverrideCursor = Cursors.Wait;
		List<R> selectedRows = ParentViewPanel.GetSelectedRows<R>();
		List<int> list = new List<int>();
		List<string> list2 = new List<string>();
		foreach (R item in selectedRows)
		{
			object key = GetKey(item);
			if (key is int)
			{
				list.Add(Convert.ToInt32(key));
			}
			else if (key is string)
			{
				list2.Add(Convert.ToString(key));
			}
			else
			{
				list2.Add(JsonSerializer.Serialize(key));
			}
		}
		EditSelectedDirectories(properties, list, list2, DirectoryType, out var errorRepViewer, withoutValidation, areExtensionFields);
		if (doRefresh)
		{
			ParentViewPanel.Refresh();
		}
		else
		{
			List<object> list3 = new List<object>();
			foreach (R item2 in selectedRows)
			{
				list3.Add(GetKey(item2));
			}
			ParentViewPanel.Update(list3);
		}
		Mouse.OverrideCursor = null;
		DataViewPanel parentViewPanel = ParentViewPanel;
		if (errorRepViewer.HasErrors)
		{
			errorRepViewer.Show();
		}
		Settings.HostingEnvironment.CurrentViewPanel = parentViewPanel;
	}

	private void EditCode()
	{
		IRowWithKey rowWithKey = (IRowWithKey)ParentViewPanel.FocusedRow();
		DirectoryEditCodeDialog directoryEditCodeDialog = new DirectoryEditCodeDialog(DirectoryDefinition, rowWithKey.GetCode(), rowWithKey.GetName());
		if (directoryEditCodeDialog.ShowDialog().Value)
		{
			object key = rowWithKey.GetKey();
			Settings.DirectoryService.EditCode(DirectoryType, directoryEditCodeDialog.OldCode.Trim(), directoryEditCodeDialog.NewCode.Text.Trim());
			ParentViewPanel.Update(key);
			if (key is string)
			{
				ParentViewPanel.Update(directoryEditCodeDialog.NewCode.Text.Trim());
			}
		}
	}

	private void AddConditionalFormatting()
	{
		if (DirectoryDefinition.IsClosedAvailable)
		{
			ConditionalFormattingList.Add(new AsCondititionFormat
			{
				Expression = "[IsClosed]",
				ApplyToRow = true,
				FormatName = "Custom",
				CustomFormat = new UserDefinedFormat
				{
					ForegroundColor = "#B3B3B3"
				},
				IsFixed = true
			});
		}
	}
}
