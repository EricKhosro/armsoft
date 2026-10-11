using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetParentDocByInvoiceNumberResponse : GenericResponse
{
	private byte[] parentDocField;

	private int parentStatusField;

	private bool parentStatusFieldSpecified;

	private Child[] childrenDocListField;

	private bool hasWaitingInvalidationInvoiceField;

	private bool hasWaitingInvalidationInvoiceFieldSpecified;

	[XmlElement(DataType = "base64Binary", Order = 0)]
	public byte[] ParentDoc
	{
		get
		{
			return parentDocField;
		}
		set
		{
			parentDocField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int ParentStatus
	{
		get
		{
			return parentStatusField;
		}
		set
		{
			parentStatusField = value;
		}
	}

	[XmlIgnore]
	public bool ParentStatusSpecified
	{
		get
		{
			return parentStatusFieldSpecified;
		}
		set
		{
			parentStatusFieldSpecified = value;
		}
	}

	[XmlArray(Order = 2)]
	[XmlArrayItem(IsNullable = false)]
	public Child[] ChildrenDocList
	{
		get
		{
			return childrenDocListField;
		}
		set
		{
			childrenDocListField = value;
		}
	}

	[XmlElement(Order = 3)]
	public bool hasWaitingInvalidationInvoice
	{
		get
		{
			return hasWaitingInvalidationInvoiceField;
		}
		set
		{
			hasWaitingInvalidationInvoiceField = value;
		}
	}

	[XmlIgnore]
	public bool hasWaitingInvalidationInvoiceSpecified
	{
		get
		{
			return hasWaitingInvalidationInvoiceFieldSpecified;
		}
		set
		{
			hasWaitingInvalidationInvoiceFieldSpecified = value;
		}
	}
}
