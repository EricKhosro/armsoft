using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.TreeNodes;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.MetaData.Tree;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Report;

namespace ArmSoft.SME.Core.UI.DataViews.TreeNodes;

public class TreeNodes : DataView<TreeNodesDataProvider.DataRow, TreeNodesDataProvider.Param>
{
	private readonly TreeCacheRow _treeCacheRow;

	public override string Caption => _treeCacheRow.Caption;

	public override string HelpId => "Tree_" + _treeCacheRow.Id;

	public override string Name => "Tree_" + _treeCacheRow.Id;

	public override string DataViewGroup => "Tree_" + _treeCacheRow.Id;

	public override bool AllowDocumentHistory => _treeCacheRow.IsLogAvailable;

	public override bool AllowDeleteSelected => AllowDelete;

	public override bool AllowAdd => AllowOperation;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowCreateCopy => AllowOperation;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowExport => true;

	public bool ParamShowAsTree
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowAsTree"].Value);
		}
		set
		{
			Parameters["ShowAsTree"].Value = value;
		}
	}

	public string ParamTreeId
	{
		get
		{
			return Convert.ToString(Parameters["TreeId"].Value);
		}
		set
		{
			Parameters["TreeId"].Value = value;
		}
	}

	public string ParamParent
	{
		get
		{
			return Convert.ToString(Parameters["Parent"].Value);
		}
		set
		{
			Parameters["Parent"].Value = value;
		}
	}

	public bool ParamShowClosed
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowClosed"].Value);
		}
		set
		{
			Parameters["ShowClosed"].Value = value;
		}
	}

	public bool ParamShowOnlyLeafNodes
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowOnlyLeafNodes"].Value);
		}
		set
		{
			Parameters["ShowOnlyLeafNodes"].Value = value;
		}
	}

	public bool ParamShowExpanded
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowExpanded"].Value);
		}
		set
		{
			Parameters["ShowExpanded"].Value = value;
		}
	}

	public TreeNodes(string treeId)
	{
		_treeCacheRow = Settings.CoreCacheService.Tree.GetItem(treeId);
		DataProvider = new TreeNodesDataProvider(treeId, _treeCacheRow.System, Settings.CreateApiClient());
		FillColumnsFromDataProvider();
		if (!string.IsNullOrWhiteSpace(_treeCacheRow.CodeCaption))
		{
			Columns["Code"].Caption = _treeCacheRow.CodeCaption;
		}
		if (!string.IsNullOrWhiteSpace(_treeCacheRow.SpecCaption))
		{
			Columns["Spec"].Caption = _treeCacheRow.SpecCaption;
		}
		Columns["Leaf"].Visible = false;
		if (!string.IsNullOrWhiteSpace(_treeCacheRow.Extension))
		{
			AddDynamicColumns();
		}
		AddParameter("ShowAsTree", "", _treeCacheRow.Multilevel);
		AddParameter("TreeId", "", treeId);
		AddParameter("ShowOnlyLeafNodes", "", false);
		AddParameter("ShowClosed", "", _treeCacheRow.IsClosedAvailable);
		AddParameter("Parent", "", "");
		AddParameter("ShowExpanded", "", _treeCacheRow.Multilevel);
		AddCustomConditionalFormatting();
	}

	public override bool TreeViewAllowed(ref bool setIsDefault)
	{
		setIsDefault = _treeCacheRow.Multilevel && ParamShowAsTree;
		return setIsDefault;
	}

	public override bool ShowExpanded()
	{
		return _treeCacheRow.Multilevel;
	}

	public override string KeyFieldName()
	{
		return "Code";
	}

	public override string ParentFieldName()
	{
		return "Parent";
	}

	public override string IsLeafFieldName()
	{
		return "Leaf";
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("Parent", ParamShowAsTree);
		dvp.Set_ColumnVisible("IsClosed", _treeCacheRow.IsClosedAvailable && ParamShowClosed);
		dvp.Set_ColumnVisible("Spec", !string.IsNullOrWhiteSpace(_treeCacheRow.SpecCaption));
		if (!dvp.IsSorted)
		{
			if (_treeCacheRow.Multilevel)
			{
				dvp.Set_ColumnSortOrder("Parent");
			}
			dvp.Set_ColumnSortOrder("Code");
		}
	}

	private ArmSoft.SME.Common.Models.TreeNodes.TreeNode LoadTreeNode(string code)
	{
		return Settings.TreeService.Load(_treeCacheRow.Id, code);
	}

	public override void Add()
	{
		IPOWindow iPOWindow = Settings.TreeService.CreatePOWindow(_treeCacheRow.Id);
		iPOWindow.DVPID = Settings.HostingEnvironment.CurrentViewPanel.Id;
		iPOWindow.Show();
	}

	public override void Edit()
	{
		TreeNodesDataProvider.DataRow dataRow = (TreeNodesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			Settings.TreeService.ShowWindow(LoadTreeNode(dataRow.Code), WindowViewMode.Edit, Settings.HostingEnvironment.CurrentViewPanel.Id);
		}
	}

	public override void View()
	{
		TreeNodesDataProvider.DataRow dataRow = (TreeNodesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			Settings.TreeService.ShowWindow(LoadTreeNode(dataRow.Code), WindowViewMode.View, Settings.HostingEnvironment.CurrentViewPanel.Id);
		}
	}

	public override void CreateCopy()
	{
		TreeNodesDataProvider.DataRow dataRow = (TreeNodesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			ArmSoft.SME.Common.Models.TreeNodes.TreeNode treeNode = Settings.TreeService.CreateCopy(_treeCacheRow.Id, dataRow.Code);
			Settings.TreeService.ShowWindow(treeNode, WindowViewMode.Copy, Settings.HostingEnvironment.CurrentViewPanel.Id);
		}
	}

	public override void Delete()
	{
		TreeNodesDataProvider.DataRow dataRow = (TreeNodesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		string code = ((IRowWithKey)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow()).GetCode();
		string messageBoxText = ((!string.IsNullOrWhiteSpace(code)) ? string.Format(Resources.ConfDirDelWithCode, _treeCacheRow.NodeCaption, code) : string.Format(Resources.ConfDirDel, _treeCacheRow.Caption));
		if (MessageBox.Show(messageBoxText, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			Mouse.OverrideCursor = Cursors.Wait;
			Settings.TreeService.Delete(_treeCacheRow.Id, code);
			if (string.IsNullOrWhiteSpace(dataRow.Parent))
			{
				Settings.HostingEnvironment.CurrentViewPanel.Update(code);
			}
			else
			{
				Settings.HostingEnvironment.CurrentViewPanel.Refresh();
			}
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
		List<TreeNodesDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<TreeNodesDataProvider.DataRow>();
		TreeNodeDeleteSelectedRequestModel request = new TreeNodeDeleteSelectedRequestModel
		{
			TreeId = _treeCacheRow.Id,
			Codes = (from p in selectedRows
				orderby p.Leaf descending
				select p.Code).ToList()
		};
		TreeNodeDeleteSelectedResponseModel treeNodeDeleteSelectedResponseModel = DataProcessingRequestHelper.Execute<TreeNodeDeleteSelectedRequestModel, TreeNodeDeleteSelectedResponseModel>(DPRType.Other, "DeleteSelectedTreeNodes", request);
		if (treeNodeDeleteSelectedResponseModel.Exceptions != null && treeNodeDeleteSelectedResponseModel.Exceptions.Count != 0)
		{
			foreach (TreeNodeDeleteResponseModel exception in treeNodeDeleteSelectedResponseModel.Exceptions)
			{
				enumerateErrorsReportViewer.AddError(exception.Exception.Message);
			}
		}
		Mouse.OverrideCursor = Cursors.Wait;
		Settings.CoreCacheService.Tree.Refresh();
		Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		Mouse.OverrideCursor = null;
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
		MessageBox.Show(string.Format(Resources.RemovedDirsCount, treeNodeDeleteSelectedResponseModel.SuccessCount), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
	}

	public override void DocumentHistory()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			TreeNodesDataProvider.DataRow dataRow = (TreeNodesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				TreeLog treeLog = new TreeLog();
				treeLog.ParamTreeId = _treeCacheRow.Id;
				treeLog.ParamCode = dataRow.Code;
				treeLog.Show();
			}
		}
	}

	public override void ExportToXML(string fileName, TreeNodesDataProvider.DataRow row)
	{
		base.ExportToXML(fileName, row);
		List<TreeNodesDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<TreeNodesDataProvider.DataRow>();
		List<string> list = new List<string>();
		foreach (TreeNodesDataProvider.DataRow item in selectedOrFocusedRows)
		{
			list.Add(item.GetCode());
		}
		byte[] array = Settings.CreateApiClient().Exchange.ExportTreeNode(_treeCacheRow.Id, list);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		fileStream.Write(array);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.TreeId = ParamTreeId;
		ParametersProvider.Parent = ParamParent;
		ParametersProvider.ShowOnlyLeafNodes = ParamShowOnlyLeafNodes;
		ParametersProvider.ShowAsTree = ParamShowAsTree;
		ParametersProvider.ShowClosed = ParamShowClosed;
		ParametersProvider.ShowOnlyLeafNodes = ParamShowOnlyLeafNodes;
	}

	private void AddDynamicColumns()
	{
		List<ArmSoft.SME.Core.UI.MetaData.UIElement> list = new TreeExtensionDefinitionEx(_treeCacheRow.Extension, new ArmSoft.SME.Core.UI.MetaData.Environment())?.Elements;
		if (list == null)
		{
			return;
		}
		foreach (ArmSoft.SME.Core.UI.MetaData.UIElement item in list)
		{
			ExtensionUIHelper.AddExtensionColumns(Columns, item);
		}
	}

	private void AddCustomConditionalFormatting()
	{
		if (_treeCacheRow.IsClosedAvailable)
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
