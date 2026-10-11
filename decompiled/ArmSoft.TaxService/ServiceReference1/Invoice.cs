using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class Invoice : SignableData
{
	private string userField;

	private int typeField;

	private GeneralInfo generalInfoField;

	private SupplierInfo supplierInfoField;

	private OnBehalfOfSupplierInfo onBehalfOfSupplierInfoField;

	private BuyerInfo buyerInfoField;

	private GoodsInfo goodsInfoField;

	private string versionField;

	[XmlElement(Order = 0)]
	public string User
	{
		get
		{
			return userField;
		}
		set
		{
			userField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int Type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}

	[XmlElement(Order = 2)]
	public GeneralInfo GeneralInfo
	{
		get
		{
			return generalInfoField;
		}
		set
		{
			generalInfoField = value;
		}
	}

	[XmlElement(Order = 3)]
	public SupplierInfo SupplierInfo
	{
		get
		{
			return supplierInfoField;
		}
		set
		{
			supplierInfoField = value;
		}
	}

	[XmlElement(Order = 4)]
	public OnBehalfOfSupplierInfo OnBehalfOfSupplierInfo
	{
		get
		{
			return onBehalfOfSupplierInfoField;
		}
		set
		{
			onBehalfOfSupplierInfoField = value;
		}
	}

	[XmlElement(Order = 5)]
	public BuyerInfo BuyerInfo
	{
		get
		{
			return buyerInfoField;
		}
		set
		{
			buyerInfoField = value;
		}
	}

	[XmlElement(Order = 6)]
	public GoodsInfo GoodsInfo
	{
		get
		{
			return goodsInfoField;
		}
		set
		{
			goodsInfoField = value;
		}
	}

	[XmlAttribute]
	public string Version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}
}
