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
public class AccountingDocument : SignableData
{
	private string userField;

	private int typeField;

	private bool traceableField;

	private bool traceableFieldSpecified;

	private AccDocGeneralInfo generalInfoField;

	private AccDocSupplierInfo supplierInfoField;

	private AccDocOnBehalfOfSupplierInfo onBehalfOfSupplierInfoField;

	private AccDocBuyerInfo buyerInfoField;

	private AccDocGoodsInfo goodsInfoField;

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

	public AccDocGeneralInfo GeneralInfo
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

	public AccDocSupplierInfo SupplierInfo
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

	public AccDocOnBehalfOfSupplierInfo OnBehalfOfSupplierInfo
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

	public AccDocBuyerInfo BuyerInfo
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

	public AccDocGoodsInfo GoodsInfo
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
