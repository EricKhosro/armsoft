using System.Windows;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.TreeNode;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(16)]
public class TreeNodes : DropDownView<TreeNodesDataProvider.DataRow, TreeNodesDataProvider.Param>
{
	private readonly TreeCacheRow _treeCacheRow;

	private readonly bool _paramShowAsTree;

	private readonly bool _paramShowExpanded;

	private bool _paramAllowOperationWithAdditionButtons;

	public string ParamParentCode { get; set; }

	public bool ParamShowOnlyLeafNodes { get; set; }

	public bool ParamShowClosed { get; set; }

	public string ParamTreeId { get; set; }

	public override bool FilteredSearchSupported => false;

	public override string HelpId => "Tree_" + _treeCacheRow.Id;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override bool ClosedFilterSupported => _treeCacheRow?.IsClosedAvailable ?? false;

	public override string ClosedColumnName => "IsClosed";

	public override bool AddNewSupported => true;

	public override bool EditSupported => true;

	public TreeNodes(string treeId)
		: this(treeId, "", true, true, true, false, true)
	{
	}

	public TreeNodes(string treeId, string parentCode = "", bool showAsTree = true, bool showExpanded = true, bool allowOperationWithAdditionButtons = true, bool showOnlyLeafNodes = false, bool showClosed = true)
	{
		_treeCacheRow = Settings.CoreCacheService.Tree.GetItem(treeId, throwExceptionNotExits: true);
		ParamTreeId = treeId;
		ParamParentCode = parentCode;
		ParamShowClosed = showClosed;
		ParamShowOnlyLeafNodes = showOnlyLeafNodes && _treeCacheRow.Multilevel;
		_paramShowAsTree = showAsTree && _treeCacheRow.Multilevel && !showOnlyLeafNodes;
		_paramShowExpanded = showExpanded && _paramShowAsTree && !showOnlyLeafNodes;
		_paramAllowOperationWithAdditionButtons = allowOperationWithAdditionButtons;
		DataProvider = new TreeNodesDataProvider(treeId);
		FillColumnsFromDataProvider();
		if (!string.IsNullOrWhiteSpace(_treeCacheRow.CodeCaption))
		{
			Columns["Code"].Caption = _treeCacheRow.CodeCaption;
		}
		if (!string.IsNullOrWhiteSpace(_treeCacheRow.SpecCaption))
		{
			Columns["Spec"].Caption = _treeCacheRow.SpecCaption;
		}
		Columns.AddSorting("Code");
		AddConditionalFormatting();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ParentCode = ParamParentCode;
		ParametersProvider.ShowOnlyLeafNodes = ParamShowOnlyLeafNodes;
		ParametersProvider.ShowClosed = ParamShowClosed;
	}

	public override short ValueMemberLength()
	{
		return _treeCacheRow.CodeLen;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Name;
	}

	public override bool TreeViewAllowed(ref bool setIsDefault)
	{
		setIsDefault = _paramShowAsTree;
		return setIsDefault;
	}

	public override bool ShowExpanded()
	{
		return _paramShowExpanded;
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
		return "IsLeaf";
	}

	public override bool AddNew(ref string value, ref string comment, Window owner, double top, double left)
	{
		return AddOrEdit(ref value, ref comment, isNew: true, owner, top, left);
	}

	public override bool Edit(ref string value, ref string comment, Window owner, double top, double left, object selectedRow)
	{
		return AddOrEdit(ref value, ref comment, isNew: false, owner, top, left);
	}

	private bool AddOrEdit(ref string value, ref string comment, bool isNew, Window owner, double top, double left)
	{
		ITreeService requiredService = Settings.ServiceProvider.GetRequiredService<ITreeService>();
		TreeNodeWindow treeNodeWindow = ((!isNew) ? ((TreeNodeWindow)requiredService.CreatePOWindow(requiredService.Load(ParamTreeId, value))) : ((TreeNodeWindow)requiredService.CreatePOWindow(ParamTreeId)));
		treeNodeWindow.Owner = owner;
		treeNodeWindow.Top = top;
		treeNodeWindow.Left = left;
		bool result;
		if (treeNodeWindow.ShowDialog().Value)
		{
			DataLoadedAtLeastOnce = false;
			result = true;
			value = treeNodeWindow.PO.Code;
			comment = treeNodeWindow.PO.Caption;
		}
		else
		{
			result = false;
		}
		return result;
	}
}
