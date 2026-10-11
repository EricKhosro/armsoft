using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.Models.TreeNodes;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.TreeNodes;

namespace ArmSoft.SME.Client.Routes;

public class TreeRoutes
{
	private readonly ApiClient _client;

	private readonly JsonSerializerOptions _options;

	private static string ControllerName => "Tree";

	public TreeRoutes(ApiClient client)
	{
		_client = client;
		_options = JsonSerializerFactory.CloneJsonOptions();
		_options.Converters.Add(new ObjectConverter());
		_options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
	}

	public async Task<TreeNode> CreateAsync(string treeId, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Create/" + treeId);
		return JsonSerializer.Deserialize<TreeNode>(await _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken), _options);
	}

	public TreeNode Create(string treeId)
	{
		return AsyncHelper.RunSync(async () => await CreateAsync(treeId));
	}

	public async Task<TreeNode> LoadAsync(string treeId, string code, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/Load/{treeId}/{code}");
		return JsonSerializer.Deserialize<TreeNode>((await _client.ExecuteAsync<TreeNodeLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, _options);
	}

	public TreeNode Load(string treeId, string code)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync(treeId, code));
	}

	public async Task<TreeNode> CreateCopyAsync(string treeId, string code, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/CreateCopy/{treeId}/{code}");
		return JsonSerializer.Deserialize<TreeNode>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, _options);
	}

	public TreeNode CreateCopy(string treeId, string code)
	{
		return AsyncHelper.RunSync(async () => await CreateCopyAsync(treeId, code));
	}

	public async Task<TreeNodeStoreResponseModelBase> StoreAsync(TreeNodeStoreRequestModelBase requestModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		ArmSoft.SME.Client.Models.TreeNodes.TreeNodeStoreRequestModel requestObject = new ArmSoft.SME.Client.Models.TreeNodes.TreeNodeStoreRequestModel
		{
			StoreRequest = requestModel.StoreRequest,
			JSONDocument = JsonSerializer.Serialize(requestModel.TreeNode, _options)
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Store");
		ArmSoft.SME.Common.Models.TreeNodes.TreeNodeStoreResponseModel treeNodeStoreResponseModel = await _client.ExecuteAsync<ArmSoft.SME.Common.Models.TreeNodes.TreeNodeStoreResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		TreeNodeStoreResponseModelBase treeNodeStoreResponseModelBase = new TreeNodeStoreResponseModelBase
		{
			Exceptions = treeNodeStoreResponseModel.Exceptions
		};
		if (treeNodeStoreResponseModel.JSONDocument != null)
		{
			treeNodeStoreResponseModelBase.TreeNode = JsonSerializer.Deserialize<TreeNode>(treeNodeStoreResponseModel.JSONDocument, _options);
		}
		return treeNodeStoreResponseModelBase;
	}

	public TreeNodeStoreResponseModelBase Store(TreeNodeStoreRequestModelBase requestModel)
	{
		return AsyncHelper.RunSync(async () => await StoreAsync(requestModel));
	}

	public async Task<ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteResponseModel> DeleteAsync(string treeId, ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteRequestModel requestModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteRequestModel requestObject = new ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteRequestModel
		{
			WithoutValidation = requestModel.WithoutValidation,
			Code = requestModel.Code
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Delete/" + treeId);
		DirectoryDeleteResponseModel directoryDeleteResponseModel = await _client.ExecuteAsync<DirectoryDeleteResponseModel>(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, requestObject);
		return new ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteResponseModel
		{
			Code = requestModel.Code,
			Exception = directoryDeleteResponseModel.Exception
		};
	}

	public ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteResponseModel Delete(string treeId, ArmSoft.SME.Client.Models.TreeNodes.TreeNodeDeleteRequestModel requestModel)
	{
		return AsyncHelper.RunSync(async () => await DeleteAsync(treeId, requestModel));
	}
}
