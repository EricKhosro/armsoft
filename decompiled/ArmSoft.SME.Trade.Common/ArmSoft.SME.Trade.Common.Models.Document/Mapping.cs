using System;
using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.Common.Models.Document;

public static class Mapping
{
	public static Type GetDocModel(short docType)
	{
		if (docType == 18)
		{
			return typeof(MTBill);
		}
		throw new ArgumentException("Wrong document type");
	}
}
