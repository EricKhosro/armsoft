using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class CheckMobileLoginResponse : GenericResponse
{
	private string transactionIdField;

	private AuthToken authTokenField;

	private TaxpayerInfo taxpayerInfoField;

	private string vATNumberField;

	private bool forEbookField;

	private bool forInvoiceField;

	private bool forAccDocField;

	private bool newVersionField;

	private bool representativeUserField;

	private OnlineUserPermissionDTO permissionField;

	private VatPayerInfo[] vatPayerYearsListField;

	[XmlElement(Order = 0)]
	public string TransactionId
	{
		get
		{
			return transactionIdField;
		}
		set
		{
			transactionIdField = value;
		}
	}

	[XmlElement(Order = 1)]
	public AuthToken AuthToken
	{
		get
		{
			return authTokenField;
		}
		set
		{
			authTokenField = value;
		}
	}

	[XmlElement(Order = 2)]
	public TaxpayerInfo TaxpayerInfo
	{
		get
		{
			return taxpayerInfoField;
		}
		set
		{
			taxpayerInfoField = value;
		}
	}

	[XmlElement(Order = 3)]
	public string VATNumber
	{
		get
		{
			return vATNumberField;
		}
		set
		{
			vATNumberField = value;
		}
	}

	[XmlElement(Order = 4)]
	public bool ForEbook
	{
		get
		{
			return forEbookField;
		}
		set
		{
			forEbookField = value;
		}
	}

	[XmlElement(Order = 5)]
	public bool ForInvoice
	{
		get
		{
			return forInvoiceField;
		}
		set
		{
			forInvoiceField = value;
		}
	}

	[XmlElement(Order = 6)]
	public bool ForAccDoc
	{
		get
		{
			return forAccDocField;
		}
		set
		{
			forAccDocField = value;
		}
	}

	[XmlElement(Order = 7)]
	public bool newVersion
	{
		get
		{
			return newVersionField;
		}
		set
		{
			newVersionField = value;
		}
	}

	[XmlElement(Order = 8)]
	public bool representativeUser
	{
		get
		{
			return representativeUserField;
		}
		set
		{
			representativeUserField = value;
		}
	}

	[XmlElement(Order = 9)]
	public OnlineUserPermissionDTO Permission
	{
		get
		{
			return permissionField;
		}
		set
		{
			permissionField = value;
		}
	}

	[XmlElement("VatPayerYearsList", Order = 10)]
	public VatPayerInfo[] VatPayerYearsList
	{
		get
		{
			return vatPayerYearsListField;
		}
		set
		{
			vatPayerYearsListField = value;
		}
	}
}
