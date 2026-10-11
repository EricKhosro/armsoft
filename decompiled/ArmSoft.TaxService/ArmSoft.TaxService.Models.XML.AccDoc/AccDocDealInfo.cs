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
public class AccDocDealInfo
{
	private DateTime dealDateField;

	private bool dealDateFieldSpecified;

	private string dealNumberField;

	private string dealAdditionalDataField;

	private string dealAdditionalDataRuField;

	[XmlElement(DataType = "date")]
	public DateTime DealDate
	{
		get
		{
			return dealDateField;
		}
		set
		{
			dealDateField = value;
		}
	}

	[XmlIgnore]
	public bool DealDateSpecified
	{
		get
		{
			return dealDateFieldSpecified;
		}
		set
		{
			dealDateFieldSpecified = value;
		}
	}

	public string DealNumber
	{
		get
		{
			return dealNumberField;
		}
		set
		{
			dealNumberField = value;
		}
	}

	public string DealAdditionalData
	{
		get
		{
			return dealAdditionalDataField;
		}
		set
		{
			dealAdditionalDataField = value;
		}
	}

	public string DealAdditionalDataRu
	{
		get
		{
			return dealAdditionalDataRuField;
		}
		set
		{
			dealAdditionalDataRuField = value;
		}
	}
}
