using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetTaxpayerInfoRequestExtended : GenericAuthenticatedRequest
{
	private string tINField;

	private DateTime checkDateField;

	private bool checkDateFieldSpecified;

	private bool leaseholderField;

	private bool leaseholderFieldSpecified;

	[XmlElement(Order = 0)]
	public string TIN
	{
		get
		{
			return tINField;
		}
		set
		{
			tINField = value;
		}
	}

	[XmlElement(DataType = "date", Order = 1)]
	public DateTime checkDate
	{
		get
		{
			return checkDateField;
		}
		set
		{
			checkDateField = value;
		}
	}

	[XmlIgnore]
	public bool checkDateSpecified
	{
		get
		{
			return checkDateFieldSpecified;
		}
		set
		{
			checkDateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 2)]
	public bool Leaseholder
	{
		get
		{
			return leaseholderField;
		}
		set
		{
			leaseholderField = value;
		}
	}

	[XmlIgnore]
	public bool LeaseholderSpecified
	{
		get
		{
			return leaseholderFieldSpecified;
		}
		set
		{
			leaseholderFieldSpecified = value;
		}
	}
}
