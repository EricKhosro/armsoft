using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentDeleteResponseModel
{
	public Guid DocISN { get; set; }

	public List<WarningModel> Warnings { get; set; }

	public ExceptionModel Exception { get; set; }
}
