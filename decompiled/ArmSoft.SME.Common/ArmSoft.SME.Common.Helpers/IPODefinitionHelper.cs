using System.Collections.Generic;

namespace ArmSoft.SME.Common.Helpers;

public interface IPODefinitionHelper
{
	List<short> GetExchangableDirectoryTypes();

	List<short> GetLoggablePOTypes();
}
