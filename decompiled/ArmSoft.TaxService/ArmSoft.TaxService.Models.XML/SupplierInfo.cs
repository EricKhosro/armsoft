using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class SupplierInfo
{
	private string vATNumberField;

	private TaxpayerInfo taxpayerField;

	private string supplyLocationField;

	private string factualSupplierAddressField;

	private bool sourceIsCarField;

	private bool sourceIsCarFieldSpecified;

	private CarInfo sourceCarInfoField;

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

	public bool SourceIsCar
	{
		get
		{
			return sourceIsCarField;
		}
		set
		{
			sourceIsCarField = value;
		}
	}

	[XmlIgnore]
	public bool SourceIsCarSpecified
	{
		get
		{
			return sourceIsCarFieldSpecified;
		}
		set
		{
			sourceIsCarFieldSpecified = value;
		}
	}

	public CarInfo SourceCarInfo
	{
		get
		{
			return sourceCarInfoField;
		}
		set
		{
			sourceCarInfoField = value;
		}
	}
}
