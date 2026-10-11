using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.Services;

public interface ICommonCoreService
{
	void DeleteDbConnection(short id);

	List<int> GetDatabaseUsers();
}
