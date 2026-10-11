using System;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Common.MetaData.Document;

public class DocumentExtensionKey : DocumentIdentificationModel
{
	public DocumentExtensionKey()
	{
	}

	public DocumentExtensionKey(short poTypeDef, short poType)
	{
		POTypeDef = poTypeDef;
		POType = poType;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(POTypeDef, POType);
	}

	public override bool Equals(object obj)
	{
		if (obj is DocumentExtensionKey documentExtensionKey && documentExtensionKey.POTypeDef == POTypeDef)
		{
			return documentExtensionKey.POType == POType;
		}
		return false;
	}
}
