using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class TpBookType
{
	private string bookNumberField;

	private int bookCreateBaseField;

	private bool bookCreateBaseFieldSpecified;

	private string tpBookNumberField;

	private string tpStumpNumberField;

	private int deliveryTypeField;

	private bool deliveryTypeFieldSpecified;

	private string deliveryTypeNameField;

	private string deliveryAddressField;

	private long idField;

	private bool idFieldSpecified;

	private string dlnField;

	private long oinField;

	private bool oinFieldSpecified;

	private long objTypeIdField;

	private bool objTypeIdFieldSpecified;

	private string objTypeNameField;

	private string objMarzCodeField;

	private string objMarzNameField;

	private string objRegionCodeField;

	private string objRegionNameField;

	private string objCityField;

	private string objStreetField;

	private string objHouseNoField;

	private string objAptNoField;

	private string objTelField;

	private string objTransportNameField;

	private string objTransportNumberField;

	private int statusField;

	private bool statusFieldSpecified;

	private DateTime requestDateField;

	private bool requestDateFieldSpecified;

	private DateTime closeDateField;

	private bool closeDateFieldSpecified;

	private DateTime previousRequestDateField;

	private bool previousRequestDateFieldSpecified;

	private TpBookDetails[] bookDetailsField;

	[XmlElement(Order = 0)]
	public string BookNumber
	{
		get
		{
			return bookNumberField;
		}
		set
		{
			bookNumberField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int BookCreateBase
	{
		get
		{
			return bookCreateBaseField;
		}
		set
		{
			bookCreateBaseField = value;
		}
	}

	[XmlIgnore]
	public bool BookCreateBaseSpecified
	{
		get
		{
			return bookCreateBaseFieldSpecified;
		}
		set
		{
			bookCreateBaseFieldSpecified = value;
		}
	}

	[XmlElement(Order = 2)]
	public string TpBookNumber
	{
		get
		{
			return tpBookNumberField;
		}
		set
		{
			tpBookNumberField = value;
		}
	}

	[XmlElement(Order = 3)]
	public string TpStumpNumber
	{
		get
		{
			return tpStumpNumberField;
		}
		set
		{
			tpStumpNumberField = value;
		}
	}

	[XmlElement(Order = 4)]
	public int DeliveryType
	{
		get
		{
			return deliveryTypeField;
		}
		set
		{
			deliveryTypeField = value;
		}
	}

	[XmlIgnore]
	public bool DeliveryTypeSpecified
	{
		get
		{
			return deliveryTypeFieldSpecified;
		}
		set
		{
			deliveryTypeFieldSpecified = value;
		}
	}

	[XmlElement(Order = 5)]
	public string DeliveryTypeName
	{
		get
		{
			return deliveryTypeNameField;
		}
		set
		{
			deliveryTypeNameField = value;
		}
	}

	[XmlElement(Order = 6)]
	public string DeliveryAddress
	{
		get
		{
			return deliveryAddressField;
		}
		set
		{
			deliveryAddressField = value;
		}
	}

	[XmlElement(Order = 7)]
	public long Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	[XmlIgnore]
	public bool IdSpecified
	{
		get
		{
			return idFieldSpecified;
		}
		set
		{
			idFieldSpecified = value;
		}
	}

	[XmlElement(Order = 8)]
	public string Dln
	{
		get
		{
			return dlnField;
		}
		set
		{
			dlnField = value;
		}
	}

	[XmlElement(Order = 9)]
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

	[XmlIgnore]
	public bool OinSpecified
	{
		get
		{
			return oinFieldSpecified;
		}
		set
		{
			oinFieldSpecified = value;
		}
	}

	[XmlElement(Order = 10)]
	public long ObjTypeId
	{
		get
		{
			return objTypeIdField;
		}
		set
		{
			objTypeIdField = value;
		}
	}

	[XmlIgnore]
	public bool ObjTypeIdSpecified
	{
		get
		{
			return objTypeIdFieldSpecified;
		}
		set
		{
			objTypeIdFieldSpecified = value;
		}
	}

	[XmlElement(Order = 11)]
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

	[XmlElement(Order = 12)]
	public string ObjMarzCode
	{
		get
		{
			return objMarzCodeField;
		}
		set
		{
			objMarzCodeField = value;
		}
	}

	[XmlElement(Order = 13)]
	public string ObjMarzName
	{
		get
		{
			return objMarzNameField;
		}
		set
		{
			objMarzNameField = value;
		}
	}

	[XmlElement(Order = 14)]
	public string ObjRegionCode
	{
		get
		{
			return objRegionCodeField;
		}
		set
		{
			objRegionCodeField = value;
		}
	}

	[XmlElement(Order = 15)]
	public string ObjRegionName
	{
		get
		{
			return objRegionNameField;
		}
		set
		{
			objRegionNameField = value;
		}
	}

	[XmlElement(Order = 16)]
	public string ObjCity
	{
		get
		{
			return objCityField;
		}
		set
		{
			objCityField = value;
		}
	}

	[XmlElement(Order = 17)]
	public string ObjStreet
	{
		get
		{
			return objStreetField;
		}
		set
		{
			objStreetField = value;
		}
	}

	[XmlElement(Order = 18)]
	public string ObjHouseNo
	{
		get
		{
			return objHouseNoField;
		}
		set
		{
			objHouseNoField = value;
		}
	}

	[XmlElement(Order = 19)]
	public string ObjAptNo
	{
		get
		{
			return objAptNoField;
		}
		set
		{
			objAptNoField = value;
		}
	}

	[XmlElement(Order = 20)]
	public string ObjTel
	{
		get
		{
			return objTelField;
		}
		set
		{
			objTelField = value;
		}
	}

	[XmlElement(Order = 21)]
	public string ObjTransportName
	{
		get
		{
			return objTransportNameField;
		}
		set
		{
			objTransportNameField = value;
		}
	}

	[XmlElement(Order = 22)]
	public string ObjTransportNumber
	{
		get
		{
			return objTransportNumberField;
		}
		set
		{
			objTransportNumberField = value;
		}
	}

	[XmlElement(Order = 23)]
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

	[XmlElement(DataType = "date", Order = 24)]
	public DateTime RequestDate
	{
		get
		{
			return requestDateField;
		}
		set
		{
			requestDateField = value;
		}
	}

	[XmlIgnore]
	public bool RequestDateSpecified
	{
		get
		{
			return requestDateFieldSpecified;
		}
		set
		{
			requestDateFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 25)]
	public DateTime CloseDate
	{
		get
		{
			return closeDateField;
		}
		set
		{
			closeDateField = value;
		}
	}

	[XmlIgnore]
	public bool CloseDateSpecified
	{
		get
		{
			return closeDateFieldSpecified;
		}
		set
		{
			closeDateFieldSpecified = value;
		}
	}

	[XmlElement(DataType = "date", Order = 26)]
	public DateTime PreviousRequestDate
	{
		get
		{
			return previousRequestDateField;
		}
		set
		{
			previousRequestDateField = value;
		}
	}

	[XmlIgnore]
	public bool PreviousRequestDateSpecified
	{
		get
		{
			return previousRequestDateFieldSpecified;
		}
		set
		{
			previousRequestDateFieldSpecified = value;
		}
	}

	[XmlElement("BookDetails", Order = 27)]
	public TpBookDetails[] BookDetails
	{
		get
		{
			return bookDetailsField;
		}
		set
		{
			bookDetailsField = value;
		}
	}
}
