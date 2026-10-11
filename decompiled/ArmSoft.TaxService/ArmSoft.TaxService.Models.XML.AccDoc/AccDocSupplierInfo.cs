using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ArmSoft.TaxService.Models.XML.AccDoc;

[Serializable]
[GeneratedCode("xsd", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocSupplierInfo
{
	private string vATNumberField;

	private AccDocTaxpayerInfo taxpayerField;

	private string supplyLocationField;

	private string supplyLocationRuField;

	private string deliveryMethodField;

	private string deliveryAdditionalDataField;

	private string deliveryAdditionalDataRuField;

	private EeuCountry eeuCountryField;

	private bool eeuCountryFieldSpecified;

	private string destinationLocationField;

	private string destinationLocationRuField;

	private string destinationAdditionalDataField;

	private string destinationAdditionalDataRuField;

	private string conveyMethodField;

	private string conveyLocationField;

	private bool sourceIsCarField;

	private bool sourceIsCarFieldSpecified;

	private bool destinationIsCarField;

	private bool destinationIsCarFieldSpecified;

	private CarInfo sourceCarInfoField;

	private CarInfo destinationCarInfoField;

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

	public string SupplyLocationRu
	{
		get
		{
			return supplyLocationRuField;
		}
		set
		{
			supplyLocationRuField = value;
		}
	}

	public string DeliveryMethod
	{
		get
		{
			return deliveryMethodField;
		}
		set
		{
			deliveryMethodField = value;
		}
	}

	public string DeliveryAdditionalData
	{
		get
		{
			return deliveryAdditionalDataField;
		}
		set
		{
			deliveryAdditionalDataField = value;
		}
	}

	public string DeliveryAdditionalDataRu
	{
		get
		{
			return deliveryAdditionalDataRuField;
		}
		set
		{
			deliveryAdditionalDataRuField = value;
		}
	}

	public EeuCountry EeuCountry
	{
		get
		{
			return eeuCountryField;
		}
		set
		{
			eeuCountryField = value;
		}
	}

	[XmlIgnore]
	public bool EeuCountrySpecified
	{
		get
		{
			return eeuCountryFieldSpecified;
		}
		set
		{
			eeuCountryFieldSpecified = value;
		}
	}

	public string DestinationLocation
	{
		get
		{
			return destinationLocationField;
		}
		set
		{
			destinationLocationField = value;
		}
	}

	public string DestinationLocationRu
	{
		get
		{
			return destinationLocationRuField;
		}
		set
		{
			destinationLocationRuField = value;
		}
	}

	public string DestinationAdditionalData
	{
		get
		{
			return destinationAdditionalDataField;
		}
		set
		{
			destinationAdditionalDataField = value;
		}
	}

	public string DestinationAdditionalDataRu
	{
		get
		{
			return destinationAdditionalDataRuField;
		}
		set
		{
			destinationAdditionalDataRuField = value;
		}
	}

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

	public bool DestinationIsCar
	{
		get
		{
			return destinationIsCarField;
		}
		set
		{
			destinationIsCarField = value;
		}
	}

	[XmlIgnore]
	public bool DestinationIsCarSpecified
	{
		get
		{
			return destinationIsCarFieldSpecified;
		}
		set
		{
			destinationIsCarFieldSpecified = value;
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

	public CarInfo DestinationCarInfo
	{
		get
		{
			return destinationCarInfoField;
		}
		set
		{
			destinationCarInfoField = value;
		}
	}
}
