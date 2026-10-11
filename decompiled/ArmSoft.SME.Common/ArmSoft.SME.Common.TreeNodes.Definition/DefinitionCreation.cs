using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.PersistableObject;

namespace ArmSoft.SME.Common.TreeNodes.Definition;

public static class DefinitionCreation
{
	public static TreeNodeDefinition CreateTreeNode(TreeCacheRow treeCacheRow)
	{
		TreeNodeDefinition treeNodeDefinition = new TreeNodeDefinition
		{
			TreeId = treeCacheRow.Id,
			NextCodeParamId = treeCacheRow.NextCodeParamId,
			AllowLog = treeCacheRow.IsLogAvailable,
			CaptionDelegate = () => treeCacheRow.NodeCaption,
			FullCaptionDelegate = () => treeCacheRow.NodeCaption,
			HelpId = "Tree_" + treeCacheRow.Id,
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase)
		};
		treeNodeDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => (!string.IsNullOrWhiteSpace(treeCacheRow.CodeCaption)) ? treeCacheRow.CodeCaption : Resources.Code, treeCacheRow.CodeLen, isRequired: true));
		treeNodeDefinition.AddFieldDefinition(new StringFieldDefinition("Caption", () => Resources.Name, treeCacheRow.CaptionLen, isRequired: true));
		treeNodeDefinition.AddFieldDefinition(new TreeNodeFieldDefinition("Parent", () => Resources.Parent, treeCacheRow.Id, onlyLeaf: false));
		treeNodeDefinition.AddFieldDefinition(new BoolFieldDefinition("IsClosed", () => Resources.Closed));
		treeNodeDefinition.AddFieldDefinition(new StringFieldDefinition("Spec", () => treeCacheRow.SpecCaption, treeCacheRow.SpecLen));
		return treeNodeDefinition;
	}
}
