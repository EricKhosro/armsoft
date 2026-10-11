using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class EbookFilter
{
	private int startPositionField;

	private bool startPositionFieldSpecified;

	private int countField;

	private bool countFieldSpecified;

	private string tinField;

	private DateTime requestDateStartField;

	private bool requestDateStartFieldSpecified;

	private DateTime requestDateEndField;

	private bool requestDateEndFieldSpecified;

	private int objectTypeField;

	private bool objectTypeFieldSpecified;

	private int statusField;

	private bool statusFieldSpecified;

	private SortCondition sortConditionField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
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

	[XmlElement(DataType = "date", Order = 3)]
	public DateTime RequestDateStart
	{
		get
		{
			return requestDateStartField;
		}
		set
		{
			requestDateStartField = value;
		}
	}

	[XmlIgnore]
	public bool RequestDateStartSpecified
	{
		get
		{
			return requestDateStartFieldSpecified;
		}
		set
		{
			requestDateStartFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 4)]
	public DateTime RequestDateEnd
	{
		get
		{
			return requestDateEndField;
		}
		set
		{
			requestDateEndField = value;
		}
	}

	[XmlIgnore]
	public bool RequestDateEndSpecified
	{
		get
		{
			return requestDateEndFieldSpecified;
		}
		set
		{
			requestDateEndFieldSpecified = value;
		}
	}

	[XmlElement(Order = 5)]
	public int ObjectType
	{
		get
		{
			return objectTypeField;
		}
		set
		{
			objectTypeField = value;
		}
	}

	[XmlIgnore]
	public bool ObjectTypeSpecified
	{
		get
		{
			return objectTypeFieldSpecified;
		}
		set
		{
			objectTypeFieldSpecified = value;
		}
	}

	[XmlElement(Order = 6)]
	public int Status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
		}
	}

	[XmlIgnore]
	public bool StatusSpecified
	{
		get
		{
			return statusFieldSpecified;
		}
		set
		{
			statusFieldSpecified = value;
		}
	}

	[XmlElement(Order = 7)]
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
}
