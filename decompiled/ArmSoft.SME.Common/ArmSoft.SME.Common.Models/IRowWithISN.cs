using System;

namespace ArmSoft.SME.Common.Models;

public interface IRowWithISN
{
	Guid GetDocISN();

	short GetDocType();
}
