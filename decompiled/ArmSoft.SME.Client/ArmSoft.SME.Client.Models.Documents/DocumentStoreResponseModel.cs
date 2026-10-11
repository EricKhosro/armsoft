using System.Collections.Generic;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Models.Documents;

public class DocumentStoreResponseModel<T>
{
	public List<ExceptionModel> Exceptions { get; set; }

	public List<WarningModel> Warnings { get; set; }

	public T Document { get; set; }
}
