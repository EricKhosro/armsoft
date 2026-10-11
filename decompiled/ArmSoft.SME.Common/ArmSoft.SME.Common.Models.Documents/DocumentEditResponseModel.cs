using System;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentEditResponseModel
{
	public Guid DocISN { get; set; }

	public ExceptionModel Exception { get; set; }
}
