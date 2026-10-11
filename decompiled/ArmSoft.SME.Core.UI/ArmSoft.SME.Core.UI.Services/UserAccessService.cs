using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Core.UI.Services;

public class UserAccessService : IUserAccessService
{
	protected Dictionary<NavigatorNodeKey, NavigatorNodePermission> _currentUserAccessTree = new Dictionary<NavigatorNodeKey, NavigatorNodePermission>();

	protected Dictionary<short, ProviderPermission> _providerTypesWithPermissions = new Dictionary<short, ProviderPermission>();

	protected Dictionary<short, POPermission> _docTypesWithPermissions = new Dictionary<short, POPermission>();

	protected Dictionary<short, POPermission> _dirTypesWithPermissions = new Dictionary<short, POPermission>();

	private List<SubsystemAccess> _accessibleSubsystems = new List<SubsystemAccess>();

	protected bool _isLoadedAtLeastOnce;

	public virtual List<SubsystemAccess> GetAccessibleSubsystems()
	{
		List<SubsystemAccess> accessibleSubsystems = _accessibleSubsystems;
		if (!string.IsNullOrWhiteSpace(Settings.ParametersService.UserAWPNavigatorName()))
		{
			accessibleSubsystems.Add(new SubsystemAccess
			{
				SubsystemId = 100,
				IsPartly = true
			});
		}
		return accessibleSubsystems;
	}

	public bool IsSubsystemAccessible(short subsystemId, out bool partlyAccess)
	{
		partlyAccess = false;
		SubsystemAccess subsystemAccess = GetAccessibleSubsystems().Find((SubsystemAccess p) => p.SubsystemId == subsystemId);
		if (subsystemAccess == null)
		{
			return false;
		}
		partlyAccess = subsystemAccess.IsPartly;
		return true;
	}

	public bool IsSubsystemAccessible(short subsystemId)
	{
		bool partlyAccess;
		return IsSubsystemAccessible(subsystemId, out partlyAccess);
	}

	public IReadOnlyDictionary<NavigatorNodeKey, NavigatorNodePermission> GetAccessTree()
	{
		return _currentUserAccessTree;
	}

	public POPermission GetDocumentPermission(short docType)
	{
		return _docTypesWithPermissions[docType];
	}

	public POPermission GetDirectoryPermission(short dirType)
	{
		return _dirTypesWithPermissions[dirType];
	}

	public IReadOnlyDictionary<short, POPermission> GetDocumentsPermissions()
	{
		return _docTypesWithPermissions;
	}

	public IReadOnlyDictionary<short, POPermission> GetDirectoriesPermissions()
	{
		return _dirTypesWithPermissions;
	}

	public ProviderPermission GetProviderPermission(short providerType)
	{
		if (_providerTypesWithPermissions.TryGetValue(providerType, out var value))
		{
			return value;
		}
		return new ProviderPermission(allowView: true, allowOperation: true);
	}

	public List<NavigatorNodeAccess> GetAccessTreeList(int userId)
	{
		return Settings.CreateApiClient().UserAccessRoutes.GetAccessTree(userId);
	}

	public NavigatorNodePermission GetNavigatorNodePermission(short subsystemId, short nodeAccessId)
	{
		if (Settings.LoginService.IsAdmin)
		{
			return new NavigatorNodePermission(allowView: true, allowOperation: true);
		}
		if (!GetAccessTree().TryGetValue(new NavigatorNodeKey(subsystemId, nodeAccessId), out var value))
		{
			return new NavigatorNodePermission(allowView: true, allowOperation: true);
		}
		return value;
	}

	public bool CanViewNavigatorNode(short nodeAccessId)
	{
		if (Settings.LoginService.IsAdmin)
		{
			return true;
		}
		IEnumerable<NavigatorNodeKey> enumerable = GetAccessTree().Keys.Where((NavigatorNodeKey p) => p.NodeAccessId == nodeAccessId);
		if (enumerable.Any())
		{
			foreach (NavigatorNodeKey item in enumerable)
			{
				if (GetAccessTree().TryGetValue(item, out var value) && value.AllowView)
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public void LoadAllPermissionsIfNecessary()
	{
		if (_isLoadedAtLeastOnce)
		{
			return;
		}
		if (Settings.IsOffline)
		{
			LoadAllPermissionsFromLocalDB();
		}
		else
		{
			LoadAllPermissionsFromService();
			if (Settings.OfflineModeIsAvailable)
			{
				StoreAllPermissionsInLocalDB();
			}
		}
		_isLoadedAtLeastOnce = true;
	}

	protected virtual void LoadAllPermissionsFromService()
	{
		foreach (NavigatorNodeAccess item in Settings.CreateApiClient().UserAccessRoutes.GetAccessTree())
		{
			_currentUserAccessTree.Add(new NavigatorNodeKey(item.SubsystemId, item.NodeAccessId), new NavigatorNodePermission(item.AllowView, item.AllowOperation));
		}
		foreach (POPermissionWithType documentsPermission in Settings.CreateApiClient().UserAccessRoutes.GetDocumentsPermissions())
		{
			_docTypesWithPermissions.Add(documentsPermission.POType, new POPermission(documentsPermission.AllowAdd, documentsPermission.AllowEdit, documentsPermission.AllowView, documentsPermission.AllowDelete));
		}
		foreach (POPermissionWithType directoriesPermission in Settings.CreateApiClient().UserAccessRoutes.GetDirectoriesPermissions())
		{
			_dirTypesWithPermissions.Add(directoriesPermission.POType, new POPermission(directoriesPermission.AllowAdd, directoriesPermission.AllowEdit, directoriesPermission.AllowView, directoriesPermission.AllowDelete));
		}
		foreach (ProviderPermissionWithType providersPermission in Settings.CreateApiClient().UserAccessRoutes.GetProvidersPermissions())
		{
			_providerTypesWithPermissions.Add(providersPermission.ProviderType, new ProviderPermission(providersPermission.AllowView, providersPermission.AllowOperation));
		}
		foreach (SubsystemAccess accessibleSubsystem in Settings.CreateApiClient().UserAccessRoutes.GetAccessibleSubsystems())
		{
			_accessibleSubsystems.Add(new SubsystemAccess
			{
				SubsystemId = accessibleSubsystem.SubsystemId,
				IsPartly = accessibleSubsystem.IsPartly
			});
		}
	}

	private void StoreAllPermissionsInLocalDB()
	{
		Settings.LocalDBUserAccessService.StoreAccessTreeInLocalDB(_currentUserAccessTree);
		Settings.LocalDBUserAccessService.StoreDocumentPermissionsInLocalDB(_docTypesWithPermissions);
		Settings.LocalDBUserAccessService.StoreDirectoriesPermissionsInLocalDB(_dirTypesWithPermissions);
		Settings.LocalDBUserAccessService.StoreProviderPermissionsInLocalDB(_providerTypesWithPermissions);
		Settings.LocalDBUserAccessService.StoreAccessibleSubsystemsInLocalDB(_accessibleSubsystems);
	}

	public bool ExistUserRole(int userId)
	{
		return Settings.CreateApiClient().UserAccessRoutes.ExistUserRole(userId);
	}

	private void LoadAllPermissionsFromLocalDB()
	{
		_currentUserAccessTree = Settings.LocalDBUserAccessService.LoadAccessTreeFromLocalDB();
		_docTypesWithPermissions = Settings.LocalDBUserAccessService.LoadDocumentPermissionsInLocalDB();
		_dirTypesWithPermissions = Settings.LocalDBUserAccessService.LoadDirectoriesPermissionsInLocalDB();
		_providerTypesWithPermissions = Settings.LocalDBUserAccessService.LoadProviderPermissionsFromLocalDB();
		_accessibleSubsystems = Settings.LocalDBUserAccessService.LoadAccessibleSubsystemsFromLocalDB();
	}

	public void Clear()
	{
		_currentUserAccessTree.Clear();
		_docTypesWithPermissions.Clear();
		_dirTypesWithPermissions.Clear();
		_providerTypesWithPermissions.Clear();
		_accessibleSubsystems.Clear();
		_isLoadedAtLeastOnce = false;
	}
}
