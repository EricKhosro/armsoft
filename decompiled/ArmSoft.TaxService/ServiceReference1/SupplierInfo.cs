using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class SupplierInfo
{
	private string vATNumberField;

	private TaxpayerInfo taxpayerField;

	private string supplyLocationField;

	private string factualSupplierAddressField;

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
	public TaxpayerInfo Taxpayer
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
	public string FactualSupplierAddress
	{
		get
		{
			return factualSupplierAddressField;
		}
		set
		{
			factualSupplierAddressField = value;
		}
	}
}
