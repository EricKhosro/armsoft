using System;

namespace ArmSoft.InternetBank.Web.API.Common;

public class CreateDocumentTemplateParams
{
	public Guid DocId { get; set; }

	public string TemplateName { get; set; }
}
