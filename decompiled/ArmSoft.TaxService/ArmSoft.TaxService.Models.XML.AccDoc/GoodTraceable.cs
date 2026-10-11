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
public class GoodTraceable
{
	private string previousDocumentNumberField;

	private PreviousDocumentType previousDocumentTypeField;

	private bool previousDocumentTypeFieldSpecified;

	private long seqNoByPreviousDocField;

	private bool seqNoByPreviousDocFieldSpecified;

	private string productFEACNCodeField;

	private decimal quantityByFEACNField;

	private bool quantityByFEACNFieldSpecified;

	private string unitByFEACNCodeField;

	public string PreviousDocumentNumber
	{
		get
		{
			return previousDocumentNumberField;
		}
		set
		{
			previousDocumentNumberField = value;
		}
	}

	public PreviousDocumentType PreviousDocumentType
	{
		get
		{
			return previousDocumentTypeField;
		}
		set
		{
			previousDocumentTypeField = value;
		}
	}

	[XmlIgnore]
	public bool PreviousDocumentTypeSpecified
	{
		get
		{
			return previousDocumentTypeFieldSpecified;
		}
		set
		{
			previousDocumentTypeFieldSpecified = value;
		}
	}

	public long SeqNoByPreviousDoc
	{
		get
		{
			return seqNoByPreviousDocField;
		}
		set
		{
			seqNoByPreviousDocField = value;
		}
	}

	[XmlIgnore]
	public bool SeqNoByPreviousDocSpecified
	{
		get
		{
			return seqNoByPreviousDocFieldSpecified;
		}
		set
		{
			seqNoByPreviousDocFieldSpecified = value;
		}
	}

	public string ProductFEACNCode
	{
		get
		{
			return productFEACNCodeField;
		}
		set
		{
			productFEACNCodeField = value;
		}
	}

	public decimal QuantityByFEACN
	{
		get
		{
			return quantityByFEACNField;
		}
		set
		{
			quantityByFEACNField = value;
		}
	}

	[XmlIgnore]
	public bool QuantityByFEACNSpecified
	{
		get
		{
			return quantityByFEACNFieldSpecified;
		}
		set
		{
			quantityByFEACNFieldSpecified = value;
		}
	}

	public string UnitByFEACNCode
	{
		get
		{
			return unitByFEACNCodeField;
		}
		set
		{
			unitByFEACNCodeField = value;
		}
	}
}
