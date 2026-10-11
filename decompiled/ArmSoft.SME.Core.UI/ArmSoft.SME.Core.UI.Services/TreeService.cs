using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Models.TreeNodes;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.TreeNodes;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.TreeNode;

namespace ArmSoft.SME.Core.UI.Services;

public class TreeService : ITreeService
{
	protected virtual ApiClient Client => Settings.CreateApiClient();

	public ArmSoft.SME.Common.Models.TreeNodes.TreeNode Store(ArmSoft.SME.Common.Models.TreeNodes.TreeNode node, POStoreRequest sr)
	{
		TreeNodeStoreRequestModelBase requestModel = new TreeNodeStoreRequestModelBase
		{
			TreeNode = node,
			StoreRequest = sr
		};
		TreeNodeStoreResponseModelBase treeNodeStoreResponseModelBase = Client.TreeRoutes.Store(requestModel);
		if (treeNodeStoreResponseModelBase.Exceptions != null && treeNodeStoreResponseModelBase.Exceptions.Count != 0)
		{
			throw new ASException(treeNodeStoreResponseModelBase.Exceptions[0].Message, treeNodeStoreResponseModelBase.Exceptions[0].FieldName)
			{
				ErrorCode = treeNodeStoreResponseModelBase.Exceptions[0].Code
			};
		}
		Settings.CoreCacheService.Tree.Refresh();
		return treeNodeStoreResponseModelBase.TreeNode;
	}

	public ArmSoft.SME.Common.Models.TreeNodes.TreeNode Create(string treeId)
	{
		return Client.TreeRoutes.Create(treeId);
	}

	public ArmSoft.SME.Common.Models.TreeNodes.TreeNode Load(string treeId, string code)
	{
		return Client.TreeRoutes.Load(treeId, code);
	}

	public void Delete(string treeId, string code, bool withoutValidation = false)
	{
		ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteRequestModel requestModel = new ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteRequestModel
		{
			WithoutValidation = withoutValidation,
			Code = code
		};
		ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteResponseModel treeNodeDeleteResponseModel = Client.TreeRoutes.Delete(treeId, requestModel);
		if (treeNodeDeleteResponseModel.Exception != null)
		{
			throw new ASException(treeNodeDeleteResponseModel.Exception.Message, treeNodeDeleteResponseModel.Exception.FieldName)
			{
				ErrorCode = treeNodeDeleteResponseModel.Exception.Code
			};
		}
		Settings.CoreCacheService.Tree.Refresh();
	}

	public IPOWindow CreatePOWindow(string treeId)
	{
		return new TreeNodeWindow(treeId);
	}

	public IPOWindow CreatePOWindow(ArmSoft.SME.Common.Models.TreeNodes.TreeNode treeNode)
	{
		return new TreeNodeWindow(treeNode);
	}

	public void ShowWindow(string treeId, string code, WindowViewMode viewMode, int dvpId = -1)
	{
		ShowWindow(Load(treeId, code), viewMode, dvpId);
	}

	public void ShowWindow(ArmSoft.SME.Common.Models.TreeNodes.TreeNode treeNode, WindowViewMode viewMode, int dvpId = -1)
	{
		IPOWindow iPOWindow = CreatePOWindow(treeNode);
		if (dvpId != -1)
		{
			iPOWindow.DVPID = dvpId;
		}
		iPOWindow.Show(viewMode);
	}

	public ArmSoft.SME.Common.Models.TreeNodes.TreeNode CreateCopy(string treeId, string code)
	{
		return Client.TreeRoutes.CreateCopy(treeId, code);
	}
}
