using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class BookDeclsListElement
{
	private string dlnField;

	private DateTime createDateField;

	private DateTime submissionDateField;

	private bool submissionDateFieldSpecified;

	private int bookDeclTypeField;

	private string tpNameField;

	private string tinField;

	private string tpAddressField;

	private string distNameField;

	private TpBookType[] tpBookField;

	[XmlElement(Order = 0)]
	public string Dln
	{
		get
		{
			return dlnField;
		}
		set
		{
			dlnField = value;
		}
	}

	[XmlElement(DataType = "date", Order = 1)]
	public DateTime CreateDate
	{
		get
		{
			return createDateField;
		}
		set
		{
			createDateField = value;
		}
	}

	[XmlElement(Order = 2)]
	public DateTime SubmissionDate
	{
		get
		{
			return submissionDateField;
		}
		set
		{
			submissionDateField = value;
		}
	}

	[XmlIgnore]
	public bool SubmissionDateSpecified
	{
		get
		{
			return submissionDateFieldSpecified;
		}
		set
		{
			submissionDateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 3)]
	public int BookDeclType
	{
		get
		{
			return bookDeclTypeField;
		}
		set
		{
			bookDeclTypeField = value;
		}
	}

	[XmlElement(Order = 4)]
	public string TpName
	{
		get
		{
			return tpNameField;
		}
		set
		{
			tpNameField = value;
		}
	}

	[XmlElement(Order = 5)]
	public string Tin
	{
		get
		{
			return tinField;
		}
		set
		{
			tinField = value;
		}
	}

	[XmlElement(Order = 6)]
	public string TpAddress
	{
		get
		{
			return tpAddressField;
		}
		set
		{
			tpAddressField = value;
		}
	}

	[XmlElement(Order = 7)]
	public string DistName
	{
		get
		{
			return distNameField;
		}
		set
		{
			distNameField = value;
		}
	}

	[XmlElement("TpBook", Order = 8)]
	public TpBookType[] TpBook
	{
		get
		{
			return tpBookField;
		}
		set
		{
			tpBookField = value;
		}
	}
}
