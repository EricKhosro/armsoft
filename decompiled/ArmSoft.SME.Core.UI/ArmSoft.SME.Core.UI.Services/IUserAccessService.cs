using System.Collections.Generic;
using ArmSoft.SME.Common.Models.UserAccess;

namespace ArmSoft.SME.Core.UI.Services;

public interface IUserAccessService
{
	List<SubsystemAccess> GetAccessibleSubsystems();

	bool IsSubsystemAccessible(short subsystemId, out bool partlyAccess);

	bool IsSubsystemAccessible(short subsystemId);

	IReadOnlyDictionary<NavigatorNodeKey, NavigatorNodePermission> GetAccessTree();

	POPermission GetDocumentPermission(short docType);

	POPermission GetDirectoryPermission(short dirType);

	IReadOnlyDictionary<short, POPermission> GetDocumentsPermissions();

	IReadOnlyDictionary<short, POPermission> GetDirectoriesPermissions();

	ProviderPermission GetProviderPermission(short providerType);

	List<NavigatorNodeAccess> GetAccessTreeList(int userId);

	NavigatorNodePermission GetNavigatorNodePermission(short subsystemId, short nodeAccessId);

	bool CanViewNavigatorNode(short nodeAccessId);

	bool ExistUserRole(int userId);

	void LoadAllPermissionsIfNecessary();

	void Clear();
}
