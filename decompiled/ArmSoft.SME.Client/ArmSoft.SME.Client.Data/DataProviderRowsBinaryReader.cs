using System;
using System.Globalization;
using System.Text;

namespace ArmSoft.SME.Client.Data;

public sealed class DataProviderRowsBinaryReader
{
	private byte[] _data;

	private readonly bool _encodeResultUnicode;

	private int _position;

	private EncodedSupportedType _currentValueType;

	private readonly Encoding _encoding1252;

	private static readonly string[] _separator = new string[1] { "::" };

	public EncodedSupportedType CurrentValueType => _currentValueType;

	internal DataProviderRowsBinaryReader(byte[] data, bool encodeResultUnicode)
	{
		_data = data;
		_encodeResultUnicode = encodeResultUnicode;
		_position = -1;
		_encoding1252 = CodePagesEncodingProvider.Instance.GetEncoding(1252);
		Read();
	}

	internal void NextPart(byte[] part)
	{
		_data = part;
		_position = -1;
		Read();
	}

	public bool Read()
	{
		if (_position == -1)
		{
			_position = 0;
		}
		else
		{
			switch (_currentValueType)
			{
			case EncodedSupportedType.StringNull:
			case EncodedSupportedType.EmptyString:
			case EncodedSupportedType.DateTimeNull:
			case EncodedSupportedType.DateOnlyNull:
			case EncodedSupportedType.DecimalNull:
			case EncodedSupportedType.DecimalZero:
			case EncodedSupportedType.BooleanTrue:
			case EncodedSupportedType.BooleanFalse:
			case EncodedSupportedType.BooleanNull:
			case EncodedSupportedType.IntegerNull:
			case EncodedSupportedType.ShortNull:
			case EncodedSupportedType.ByteNull:
			case EncodedSupportedType.LongNull:
			case EncodedSupportedType.DoubleNull:
			case EncodedSupportedType.FloatNull:
			case EncodedSupportedType.GUIDNull:
			case EncodedSupportedType.ByteArrayNull:
				_position++;
				break;
			case EncodedSupportedType.ANSIStringShort:
			case EncodedSupportedType.UnicodeStringShort:
			case EncodedSupportedType.Decimal:
			{
				int num2 = _data[_position + 1];
				_position += num2 + 2;
				break;
			}
			case EncodedSupportedType.ANSIStringLong:
			case EncodedSupportedType.UnicodeStringLong:
			{
				ushort num = BitConverter.ToUInt16(_data, _position + 1);
				_position += num + 3;
				break;
			}
			case EncodedSupportedType.Date:
			case EncodedSupportedType.DateOnly:
			case EncodedSupportedType.Integer:
			case EncodedSupportedType.Float:
				_position += 5;
				break;
			case EncodedSupportedType.Short:
				_position += 3;
				break;
			case EncodedSupportedType.Byte:
				_position += 2;
				break;
			case EncodedSupportedType.Long:
			case EncodedSupportedType.Double:
				_position += 9;
				break;
			case EncodedSupportedType.DateTime:
				_position += 8;
				break;
			case EncodedSupportedType.GUID:
				_position += 17;
				break;
			case EncodedSupportedType.ByteArray:
			{
				int num2 = BitConverter.ToInt32(_data, _position + 1);
				_position += num2 + 5;
				break;
			}
			case EncodedSupportedType.ListOfString:
			{
				ushort num = BitConverter.ToUInt16(_data, _position + 1);
				_position += num + 5;
				break;
			}
			}
		}
		if (_position >= _data.Length)
		{
			_currentValueType = EncodedSupportedType.EndOfData;
		}
		else
		{
			_currentValueType = _data[_position] switch
			{
				0 => EncodedSupportedType.EndOfData, 
				1 => EncodedSupportedType.StringNull, 
				2 => EncodedSupportedType.EmptyString, 
				3 => EncodedSupportedType.ANSIStringShort, 
				4 => EncodedSupportedType.ANSIStringLong, 
				5 => EncodedSupportedType.UnicodeStringShort, 
				6 => EncodedSupportedType.UnicodeStringLong, 
				10 => EncodedSupportedType.Date, 
				11 => EncodedSupportedType.DateTime, 
				12 => EncodedSupportedType.DateTimeNull, 
				13 => EncodedSupportedType.DateOnly, 
				14 => EncodedSupportedType.DateOnlyNull, 
				20 => EncodedSupportedType.Decimal, 
				21 => EncodedSupportedType.DecimalNull, 
				22 => EncodedSupportedType.DecimalZero, 
				23 => EncodedSupportedType.BooleanTrue, 
				24 => EncodedSupportedType.BooleanFalse, 
				25 => EncodedSupportedType.BooleanNull, 
				30 => EncodedSupportedType.Integer, 
				31 => EncodedSupportedType.IntegerNull, 
				32 => EncodedSupportedType.Short, 
				33 => EncodedSupportedType.ShortNull, 
				34 => EncodedSupportedType.Byte, 
				35 => EncodedSupportedType.ByteNull, 
				36 => EncodedSupportedType.Long, 
				37 => EncodedSupportedType.LongNull, 
				40 => EncodedSupportedType.Double, 
				41 => EncodedSupportedType.DoubleNull, 
				42 => EncodedSupportedType.Float, 
				43 => EncodedSupportedType.FloatNull, 
				44 => EncodedSupportedType.GUID, 
				45 => EncodedSupportedType.GUIDNull, 
				46 => EncodedSupportedType.ByteArray, 
				47 => EncodedSupportedType.ByteArrayNull, 
				50 => EncodedSupportedType.ListOfString, 
				_ => EncodedSupportedType.EndOfData, 
			};
		}
		return _currentValueType != EncodedSupportedType.EndOfData;
	}

	public string GetString()
	{
		switch (_currentValueType)
		{
		case EncodedSupportedType.StringNull:
			return null;
		case EncodedSupportedType.EmptyString:
			return string.Empty;
		case EncodedSupportedType.ANSIStringShort:
		{
			int count3 = _data[_position + 1];
			return _encoding1252.GetString(_data, _position + 2, count3);
		}
		case EncodedSupportedType.ANSIStringLong:
		{
			ushort count2 = BitConverter.ToUInt16(_data, _position + 1);
			return _encoding1252.GetString(_data, _position + 3, count2);
		}
		case EncodedSupportedType.UnicodeStringShort:
		{
			int count = _data[_position + 1];
			return Encoding.UTF8.GetString(_data, _position + 2, count);
		}
		case EncodedSupportedType.UnicodeStringLong:
		{
			int count = BitConverter.ToUInt16(_data, _position + 1);
			return Encoding.UTF8.GetString(_data, _position + 3, count);
		}
		default:
			throw new InvalidOperationException("Value not string");
		}
	}

	public DateTime GetDateTime()
	{
		switch (_currentValueType)
		{
		case EncodedSupportedType.Date:
		{
			short year2 = BitConverter.ToInt16(_data, _position + 1);
			byte month = _data[_position + 3];
			byte day = _data[_position + 4];
			return new DateTime(year2, month, day);
		}
		case EncodedSupportedType.DateTime:
		{
			short year = BitConverter.ToInt16(_data, _position + 1);
			byte month = _data[_position + 3];
			byte day = _data[_position + 4];
			byte hour = _data[_position + 5];
			byte minute = _data[_position + 6];
			byte second = _data[_position + 7];
			return new DateTime(year, month, day, hour, minute, second);
		}
		default:
			throw new InvalidOperationException("Value not date");
		}
	}

	public DateOnly GetDateOnly()
	{
		if (_currentValueType == EncodedSupportedType.DateOnly)
		{
			short year = BitConverter.ToInt16(_data, _position + 1);
			byte month = _data[_position + 3];
			byte day = _data[_position + 4];
			return new DateOnly(year, month, day);
		}
		throw new InvalidOperationException("Value not date");
	}

	public decimal GetDecimal()
	{
		switch (_currentValueType)
		{
		case EncodedSupportedType.DecimalZero:
			return 0m;
		case EncodedSupportedType.Decimal:
		{
			int count = _data[_position + 1];
			return decimal.Parse(_encoding1252.GetString(_data, _position + 2, count), CultureInfo.InvariantCulture);
		}
		default:
			throw new InvalidOperationException("Value not decimal");
		}
	}

	public bool GetBoolean()
	{
		return _currentValueType switch
		{
			EncodedSupportedType.BooleanTrue => true, 
			EncodedSupportedType.BooleanFalse => false, 
			_ => throw new InvalidOperationException("Value not boolean"), 
		};
	}

	public int GetInteger()
	{
		if (_currentValueType == EncodedSupportedType.Integer)
		{
			return BitConverter.ToInt32(_data, _position + 1);
		}
		throw new InvalidOperationException("ValueNotInteger");
	}

	public short GetShort()
	{
		if (_currentValueType == EncodedSupportedType.Short)
		{
			return BitConverter.ToInt16(_data, _position + 1);
		}
		throw new InvalidOperationException("Current value not short.");
	}

	public byte GetByte()
	{
		if (_currentValueType == EncodedSupportedType.Byte)
		{
			return _data[_position + 1];
		}
		throw new InvalidOperationException("ValueNotByte");
	}

	public long GetLong()
	{
		if (_currentValueType == EncodedSupportedType.Long)
		{
			return BitConverter.ToInt64(_data, _position + 1);
		}
		throw new InvalidOperationException("ValueNotLong");
	}

	public double GetDouble()
	{
		if (_currentValueType == EncodedSupportedType.Double)
		{
			return BitConverter.ToDouble(_data, _position + 1);
		}
		throw new InvalidOperationException("ValueNotDouble");
	}

	public float GetFloat()
	{
		if (_currentValueType == EncodedSupportedType.Float)
		{
			return BitConverter.ToSingle(_data, _position + 1);
		}
		throw new InvalidOperationException("ValueNotFloat");
	}

	public Guid GetGuid()
	{
		if (_currentValueType == EncodedSupportedType.GUID)
		{
			byte[] array = new byte[16];
			Array.Copy(_data, _position + 1, array, 0, 16);
			return new Guid(array);
		}
		throw new InvalidOperationException("ValueNotGuid");
	}

	public byte[] GetByteArray()
	{
		if (_currentValueType == EncodedSupportedType.ByteArray)
		{
			int num = BitConverter.ToInt32(_data, _position + 1);
			byte[] array = new byte[num];
			Array.Copy(_data, _position + 5, array, 0, num);
			return array;
		}
		if (_currentValueType == EncodedSupportedType.ByteArrayNull)
		{
			return null;
		}
		throw new InvalidOperationException("ValueNotByteArray");
	}

	public string[] GetListOfString()
	{
		if (_currentValueType == EncodedSupportedType.ListOfString)
		{
			int count = BitConverter.ToUInt16(_data, _position + 1);
			return Encoding.UTF8.GetString(_data, _position + 5, count).Split(_separator, StringSplitOptions.None);
		}
		throw new InvalidOperationException("ValueNotListOfString");
	}
}
