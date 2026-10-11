using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class BookDeclFilter
{
	private SortCondition sortConditionField;

	private int startPositionField;

	private bool startPositionFieldSpecified;

	private int countField;

	private bool countFieldSpecified;

	private DateTime submitDateStartField;

	private bool submitDateStartFieldSpecified;

	private DateTime submitDateEndField;

	private bool submitDateEndFieldSpecified;

	private int typeField;

	private bool typeFieldSpecified;

	private string tinField;

	[XmlElement(Order = 0)]
	public SortCondition SortCondition
	{
		get
		{
			return sortConditionField;
		}
		set
		{
			sortConditionField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int StartPosition
	{
		get
		{
			return startPositionField;
		}
		set
		{
			startPositionField = value;
		}
	}

	[XmlIgnore]
	public bool StartPositionSpecified
	{
		get
		{
			return startPositionFieldSpecified;
		}
		set
		{
			startPositionFieldSpecified = value;
		}
	}

	[XmlElement(Order = 2)]
	public int Count
	{
		get
		{
			return countField;
		}
		set
		{
			countField = value;
		}
	}

	[XmlIgnore]
	public bool CountSpecified
	{
		get
		{
			return countFieldSpecified;
		}
		set
		{
			countFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 3)]
	public DateTime SubmitDateStart
	{
		get
		{
			return submitDateStartField;
		}
		set
		{
			submitDateStartField = value;
		}
	}

	[XmlIgnore]
	public bool SubmitDateStartSpecified
	{
		get
		{
			return submitDateStartFieldSpecified;
		}
		set
		{
			submitDateStartFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 4)]
	public DateTime SubmitDateEnd
	{
		get
		{
			return submitDateEndField;
		}
		set
		{
			submitDateEndField = value;
		}
	}

	[XmlIgnore]
	public bool SubmitDateEndSpecified
	{
		get
		{
			return submitDateEndFieldSpecified;
		}
		set
		{
			submitDateEndFieldSpecified = value;
		}
	}

	[XmlElement(Order = 5)]
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

	[XmlIgnore]
	public bool TypeSpecified
	{
		get
		{
			return typeFieldSpecified;
		}
		set
		{
			typeFieldSpecified = value;
		}
	}

	[XmlElement(Order = 6)]
	public string Tin
	{
		get
		{
			return tinField;
		}
		set
		{
			tinField = value;
		}
	}
}
