using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocSupplierInfo
{
	private string vATNumberField;

	private AccDocTaxpayerInfo taxpayerField;

	private string supplyLocationField;

	private string conveyMethodField;

	private string conveyLocationField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
	public AccDocTaxpayerInfo Taxpayer
	{
		get
		{
			return taxpayerField;
		}
		set
		{
			taxpayerField = value;
		}
	}

	[XmlElement(Order = 2)]
	public string SupplyLocation
	{
		get
		{
			return supplyLocationField;
		}
		set
		{
			supplyLocationField = value;
		}
	}

	[XmlElement(Order = 3)]
	public string ConveyMethod
	{
		get
		{
			return conveyMethodField;
		}
		set
		{
			conveyMethodField = value;
		}
	}

	[XmlElement(Order = 4)]
	public string ConveyLocation
	{
		get
		{
			return conveyLocationField;
		}
		set
		{
			conveyLocationField = value;
		}
	}
}
