using System;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Client.Data;

public class DynamicRowReader
{
	public string Body { get; set; }

	public DynamicRowReader(string body)
	{
		Body = body;
	}

	public string GetString(string rekvName)
	{
		return GetStringByKey(rekvName);
	}

	public DateTime? GetDateNullable(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return null;
		}
		return stringByKey.YYYYMMDDToDate();
	}

	public DateTime GetDate(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return default;
		}
		return stringByKey.YYYYMMDDToDate();
	}

	public bool GetBoolean(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return false;
		}
		return stringByKey == "1";
	}

	public int GetInteger(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return 0;
		}
		return Convert.ToInt32(stringByKey);
	}

	public decimal GetDecimal(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return 0m;
		}
		return Convert.ToDecimal(stringByKey);
	}

	public short GetShort(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return 0;
		}
		return Convert.ToInt16(stringByKey);
	}

	public long GetLong(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return 0L;
		}
		return Convert.ToInt64(stringByKey);
	}

	public double GetDouble(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return 0.0;
		}
		return Convert.ToDouble(stringByKey);
	}

	public byte GetByte(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return 0;
		}
		return Convert.ToByte(stringByKey);
	}

	public Guid GetGuid(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return Guid.Empty;
		}
		return Guid.Parse(stringByKey);
	}

	public Guid? GetGuidNullable(string rekvName)
	{
		string stringByKey = GetStringByKey(rekvName);
		if (string.IsNullOrEmpty(stringByKey))
		{
			return null;
		}
		return Guid.Parse(stringByKey);
	}

	private string GetStringByKey(string rekvName)
	{
		string text = null;
		int num = 0;
		int num2 = 0;
		text = "\"" + rekvName + "\":";
		num = Body.IndexOf(text);
		num2 = Body.IndexOf(",\"", num);
		if (num2 == -1)
		{
			num2 = Body.IndexOf("}", num);
		}
		int num3 = num2 - num - text.Length;
		if (num3 > 0)
		{
			return Body.Substring(num + text.Length, num3);
		}
		return "";
	}

	internal object GetDataFromReaderByType(Column col)
	{
		Type type = col.Type.CastToType();
		if (type == typeof(string))
		{
			return GetString(col.Name);
		}
		if (type == typeof(decimal) || type == typeof(decimal?))
		{
			return GetDecimal(col.Name);
		}
		if (type == typeof(DateTime))
		{
			return GetDate(col.Name);
		}
		if (type == typeof(DateTime?))
		{
			return GetDateNullable(col.Name);
		}
		if (type == typeof(bool) || type == typeof(bool?))
		{
			return GetBoolean(col.Name);
		}
		if (type == typeof(int) || type == typeof(int?))
		{
			return GetInteger(col.Name);
		}
		if (type == typeof(short) || type == typeof(short?))
		{
			return GetShort(col.Name);
		}
		if (type == typeof(long) || type == typeof(long?))
		{
			return GetLong(col.Name);
		}
		if (type == typeof(double) || type == typeof(double?))
		{
			return GetDouble(col.Name);
		}
		if (type == typeof(byte) || type == typeof(byte?))
		{
			return GetByte(col.Name);
		}
		if (type == typeof(Guid))
		{
			return GetGuid(col.Name);
		}
		if (type == typeof(Guid?))
		{
			return GetGuidNullable(col.Name);
		}
		return null;
	}
}
