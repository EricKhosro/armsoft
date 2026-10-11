using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ObjectFilter
{
	private int startPositionField;

	private bool startPositionFieldSpecified;

	private int countField;

	private bool countFieldSpecified;

	private int objectTypeField;

	private bool objectTypeFieldSpecified;

	private string tinField;

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

	[XmlElement(Order = 3)]
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
