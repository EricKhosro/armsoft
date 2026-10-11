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
public class Invoice : SignableData
{
	private string userField;

	private int typeField;

	private bool traceableField;

	private bool traceableFieldSpecified;

	private GeneralInfo generalInfoField;

	private SupplierInfo supplierInfoField;

	private OnBehalfOfSupplierInfo onBehalfOfSupplierInfoField;

	private BuyerInfo buyerInfoField;

	private GoodsInfo goodsInfoField;

	private string versionField;

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

	public bool Traceable
	{
		get
		{
			return traceableField;
		}
		set
		{
			traceableField = value;
		}
	}

	[XmlIgnore]
	public bool TraceableSpecified
	{
		get
		{
			return traceableFieldSpecified;
		}
		set
		{
			traceableFieldSpecified = value;
		}
	}

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
