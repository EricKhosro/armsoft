using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.TreeNodes;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.PersistableObject;

namespace ArmSoft.SME.Core.UI.Services;

public interface ITreeService
{
	ArmSoft.SME.Common.Models.TreeNodes.TreeNode Store(ArmSoft.SME.Common.Models.TreeNodes.TreeNode node, POStoreRequest sr);

	ArmSoft.SME.Common.Models.TreeNodes.TreeNode Create(string treeId);

	ArmSoft.SME.Common.Models.TreeNodes.TreeNode Load(string treeId, string code);

	void Delete(string treeId, string code, bool withoutValidation = false);

	IPOWindow CreatePOWindow(string treeId);

	IPOWindow CreatePOWindow(ArmSoft.SME.Common.Models.TreeNodes.TreeNode treeNode);

	void ShowWindow(string treeId, string code, WindowViewMode viewMode, int dvpId = -1);

	void ShowWindow(ArmSoft.SME.Common.Models.TreeNodes.TreeNode treeNode, WindowViewMode viewMode, int dvpId = -1);

	ArmSoft.SME.Common.Models.TreeNodes.TreeNode CreateCopy(string treeId, string code);
}
