using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ObjectType
{
	private long oinField;

	private int objTypeField;

	private string objTypeNameField;

	private string objNameField;

	private long typeObjIdField;

	private string typeObjNameField;

	private string marzIdField;

	private string marzNameField;

	private string regionIdField;

	private string regionNameField;

	private string cityField;

	private string streetField;

	private string houseNoField;

	private string aptNoField;

	private string telField;

	private string vehicleTypeField;

	private string vehicleNumberField;

	[XmlElement(Order = 0)]
	public long Oin
	{
		get
		{
			return oinField;
		}
		set
		{
			oinField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int ObjType
	{
		get
		{
			return objTypeField;
		}
		set
		{
			objTypeField = value;
		}
	}

	[XmlElement(Order = 2)]
	public string ObjTypeName
	{
		get
		{
			return objTypeNameField;
		}
		set
		{
			objTypeNameField = value;
		}
	}

	[XmlElement(Order = 3)]
	public string ObjName
	{
		get
		{
			return objNameField;
		}
		set
		{
			objNameField = value;
		}
	}

	[XmlElement(Order = 4)]
	public long TypeObjId
	{
		get
		{
			return typeObjIdField;
		}
		set
		{
			typeObjIdField = value;
		}
	}

	[XmlElement(Order = 5)]
	public string TypeObjName
	{
		get
		{
			return typeObjNameField;
		}
		set
		{
			typeObjNameField = value;
		}
	}

	[XmlElement(Order = 6)]
	public string MarzId
	{
		get
		{
			return marzIdField;
		}
		set
		{
			marzIdField = value;
		}
	}

	[XmlElement(Order = 7)]
	public string MarzName
	{
		get
		{
			return marzNameField;
		}
		set
		{
			marzNameField = value;
		}
	}

	[XmlElement(Order = 8)]
	public string RegionId
	{
		get
		{
			return regionIdField;
		}
		set
		{
			regionIdField = value;
		}
	}

	[XmlElement(Order = 9)]
	public string RegionName
	{
		get
		{
			return regionNameField;
		}
		set
		{
			regionNameField = value;
		}
	}

	[XmlElement(Order = 10)]
	public string City
	{
		get
		{
			return cityField;
		}
		set
		{
			cityField = value;
		}
	}

	[XmlElement(Order = 11)]
	public string Street
	{
		get
		{
			return streetField;
		}
		set
		{
			streetField = value;
		}
	}

	[XmlElement(Order = 12)]
	public string HouseNo
	{
		get
		{
			return houseNoField;
		}
		set
		{
			houseNoField = value;
		}
	}

	[XmlElement(Order = 13)]
	public string AptNo
	{
		get
		{
			return aptNoField;
		}
		set
		{
			aptNoField = value;
		}
	}

	[XmlElement(Order = 14)]
	public string Tel
	{
		get
		{
			return telField;
		}
		set
		{
			telField = value;
		}
	}

	[XmlElement(Order = 15)]
	public string VehicleType
	{
		get
		{
			return vehicleTypeField;
		}
		set
		{
			vehicleTypeField = value;
		}
	}

	[XmlElement(Order = 16)]
	public string VehicleNumber
	{
		get
		{
			return vehicleNumberField;
		}
		set
		{
			vehicleNumberField = value;
		}
	}
}
