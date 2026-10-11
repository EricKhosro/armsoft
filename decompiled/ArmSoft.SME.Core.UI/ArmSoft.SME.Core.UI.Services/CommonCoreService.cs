using System;
using System.Collections.Generic;
using ArmSoft.SME.Client;

namespace ArmSoft.SME.Core.UI.Services;

public class CommonCoreService : ICommonCoreService
{
	protected virtual ApiClient Client => Settings.CreateApiClient();

	public void DeleteDbConnection(short id)
	{
		Client.CommonCoreRoutes.DeleteDbConnection(id);
	}

	public virtual List<int> GetDatabaseUsers()
	{
		throw new NotImplementedException();
	}
}
