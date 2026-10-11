using System.Collections.Generic;
using ArmSoft.SME.Common.Models.UserAccess;

namespace ArmSoft.SME.Core.UI.Services;

public interface ILocalDBUserAccessService
{
	void StoreAccessTreeInLocalDB(Dictionary<NavigatorNodeKey, NavigatorNodePermission> currentUserAccessTree);

	void StoreDirectoriesPermissionsInLocalDB(Dictionary<short, POPermission> dirTypesWithPermissions);

	void StoreDocumentPermissionsInLocalDB(Dictionary<short, POPermission> docTypesWithPermissions);

	void StoreProviderPermissionsInLocalDB(Dictionary<short, ProviderPermission> providerTypesWithPermissions);

	void StoreAccessibleSubsystemsInLocalDB(List<SubsystemAccess> accessibleSubsystems);

	Dictionary<NavigatorNodeKey, NavigatorNodePermission> LoadAccessTreeFromLocalDB();

	Dictionary<short, POPermission> LoadDirectoriesPermissionsInLocalDB();

	Dictionary<short, POPermission> LoadDocumentPermissionsInLocalDB();

	Dictionary<short, ProviderPermission> LoadProviderPermissionsFromLocalDB();

	List<SubsystemAccess> LoadAccessibleSubsystemsFromLocalDB();
}
