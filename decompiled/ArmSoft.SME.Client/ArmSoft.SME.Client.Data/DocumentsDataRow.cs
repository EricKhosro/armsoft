using System;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data;

public class DocumentsDataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
{
	public Guid ISN { get; set; }

	public string DocNum { get; set; }

	public DateOnly DocDate { get; set; }

	public string Comment { get; set; }

	public short DocType { get; set; }

	public string DocTypeName { get; set; }

	public byte DocState { get; set; }

	public string DocStateName { get; set; }

	public string Creator { get; set; } = "";

	public DateTime CreationDate { get; set; }

	public string LastModifier { get; set; } = "";

	public DateTime LastModifyDate { get; set; }

	public string Body { get; set; } = "";

	public string ExtensionBody { get; set; }

	public Guid GetDocISN()
	{
		return ISN;
	}

	public virtual short GetDocType()
	{
		return DocType;
	}

	public bool Matched(object key)
	{
		return (Guid)key == ISN;
	}

	private protected override object GetExtensionValue(int index)
	{
		string[] values = Values;
		if (values != null && values.Length > index)
		{
			return Values[index];
		}
		return null;
	}
}
