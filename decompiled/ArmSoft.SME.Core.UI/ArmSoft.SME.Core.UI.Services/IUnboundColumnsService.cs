using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models.UnboundColumns;

namespace ArmSoft.SME.Core.UI.Services;

public interface IUnboundColumnsService
{
	ApiClient Client { get; }

	List<UnboundColumn> Get(string group);

	UnboundColumn Get(string group, string name);

	bool Store(UnboundColumn column);

	bool Delete(UnboundColumn column);
}
