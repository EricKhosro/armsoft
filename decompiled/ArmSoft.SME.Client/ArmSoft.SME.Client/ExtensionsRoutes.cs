using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.MetaData.Tree;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.StringResources;

namespace ArmSoft.SME.Client;

public class ExtensionsRoutes
{
	private readonly ApiClient _client;

	internal ExtensionsRoutes(ApiClient client)
	{
		_client = client;
	}

	public string LoadAWP(string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/AWP/Load", urlQuery);
		return _client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public void StoreAWP(AWPDefinition definition)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/AWP/Store");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), definition);
	}

	public void DeleteAWP(string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/AWP/Delete", urlQuery);
		_client.Execute(HttpMethod.Delete, stringBuilder.ToString());
	}

	public string LoadUserScript(string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/UserScript/Load", urlQuery);
		return _client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public void StoreUserScript(UserScriptDefinition definition)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/UserScript/Store");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), definition);
	}

	public void DeleteUserScript(string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/UserScript/Delete", urlQuery);
		_client.Execute(HttpMethod.Delete, stringBuilder.ToString());
	}

	public string LoadDVD(string group, string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("group", group);
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/DVD/Load", urlQuery);
		return _client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string LoadDefaultDVD(string group)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("group", group);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/DVD/LoadDefault", urlQuery);
		return _client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public List<string> LoadDVDs(string group)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("group", group);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/DVD/LoadList", urlQuery);
		return _client.Execute<List<string>>(HttpMethod.Get, stringBuilder.ToString());
	}

	public void StoreDVD(DataViewDefinition definition)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/DVD/Store");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), definition);
	}

	public void DeleteDVD(string group, string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("group", group);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/DVD/Delete", urlQuery);
		_client.Execute(HttpMethod.Delete, stringBuilder.ToString());
	}

	public void DVDUpdateLastExecDate(string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/DVD/UpdateLastExecDate", urlQuery);
		_client.Execute(HttpMethod.Get, stringBuilder.ToString());
	}

	public void DVDChangeEnabled(string group, string name, bool value)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("group", group);
		urlQuery.Add("value", value);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/DVD/ChangeEnabled", urlQuery);
		_client.Execute(HttpMethod.Get, stringBuilder.ToString());
	}

	public void DVDChangeDefault(string group, string name, bool value)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("group", group);
		urlQuery.Add("value", value);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/DVD/ChangeDefault", urlQuery);
		_client.Execute(HttpMethod.Get, stringBuilder.ToString());
	}

	public void StoreTree(TreeDefinition definition)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Tree/Store");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), definition);
	}

	public void DeleteTree(string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Tree/Delete", urlQuery);
		_client.Execute(HttpMethod.Delete, stringBuilder.ToString());
	}

	public string LoadTreeXml(string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Tree/LoadXml", urlQuery);
		return _client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public void StoreTreeExtensionXml(TreeExtensionModel model)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Tree/Extension");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), model);
	}

	public TreeDefinition GetTreeDefinition(TreeDefinitionRequestModel request)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Tree");
		return _client.Execute<TreeDefinition>(HttpMethod.Post, stringBuilder.ToString(), request);
	}

	public void DeleteExtension(TreeExtensionModel model)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Tree/Extension");
		_client.Execute(HttpMethod.Delete, stringBuilder.ToString(), model);
	}

	public StringResource LoadResource(StringResourceRequest request)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Resources/Load");
		return _client.Execute<StringResource>(HttpMethod.Post, stringBuilder.ToString(), request);
	}

	public void StoreResources(List<StringResource> resources)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Resources/Store");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), resources);
	}

	public void UpdateResource(StringResource resources)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Resources/Update");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), resources);
	}

	public void DeleteResource(string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/Resources/Delete", urlQuery);
		_client.Execute(HttpMethod.Delete, stringBuilder.ToString());
	}

	public string LoadContextFunction(string group, string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("group", group);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/CF/Load", urlQuery);
		return _client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public List<string> LoadContextFunctions(string group)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("group", group);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/CF/LoadList", urlQuery);
		return _client.Execute<List<string>>(HttpMethod.Get, stringBuilder.ToString());
	}

	public void StoreContextFunction(ContextFunctionDefinition definition)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/CF/Store");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), definition);
	}

	public void DeleteContextFunction(string group, string name)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("group", group);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/CF/Delete", urlQuery);
		_client.Execute(HttpMethod.Delete, stringBuilder.ToString());
	}

	public void ContextFunctionChangeEnabled(string group, string name, bool value)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("group", group);
		urlQuery.Add("value", value);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Extensions/CF/ChangeEnabled", urlQuery);
		_client.Execute(HttpMethod.Get, stringBuilder.ToString());
	}
}
