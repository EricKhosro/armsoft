using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common.Data;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Client.Data;

public class ExtendableRow : IDynamicValue
{
	private class EmittedDelegates
	{
		public object Setter;
	}

	internal delegate void StringSetterDelegate(object obj, string value);

	internal delegate void DecimalSetterDelegate(object obj, decimal value);

	internal delegate void DecimalNullableSetterDelegate(object obj, decimal? value);

	internal delegate void BooleanSetterDelegate(object obj, bool value);

	internal delegate void BooleanNullableSetterDelegate(object obj, bool? value);

	internal delegate void DateTimeSetterDelegate(object obj, DateTime value);

	internal delegate void DateTimeNullableSetterDelegate(object obj, DateTime? value);

	internal delegate void DateOnlySetterDelegate(object obj, DateOnly value);

	internal delegate void DateOnlyNullableSetterDelegate(object obj, DateOnly? value);

	internal delegate void Int32SetterDelegate(object obj, int value);

	internal delegate void Int32NullableSetterDelegate(object obj, int? value);

	internal delegate void Int16SetterDelegate(object obj, short value);

	internal delegate void Int16NullableSetterDelegate(object obj, short? value);

	internal delegate void Int64SetterDelegate(object obj, long value);

	internal delegate void Int64NullableSetterDelegate(object obj, long? value);

	internal delegate void ByteSetterDelegate(object obj, byte value);

	internal delegate void ByteNullableSetterDelegate(object obj, byte? value);

	internal delegate void ByteArraySetterDelegate(object obj, byte[] value);

	internal delegate void ListOfStringSetterDelegate(object obj, List<string> value);

	internal delegate void DoubleSetterDelegate(object obj, double value);

	internal delegate void DoubleNullableSetterDelegate(object obj, double? value);

	internal delegate void FloatSetterDelegate(object obj, float value);

	internal delegate void FloatNullableSetterDelegate(object obj, float? value);

	internal delegate void GuidSetterDelegate(object obj, Guid value);

	internal delegate void GuidNullableSetterDelegate(object obj, Guid? value);

	public class Column
	{
		internal readonly object setter;

		internal readonly SupportedType propertyType;

		public string Name { get; private set; }

		public string Caption { get; private set; }

		public SupportedType Type { get; private set; }

		public string Source { get; private set; }

		public PropertyInfo PI { get; private set; }

		public string Format { get; private set; }

		public string Group { get; private set; }

		public int Index { get; private set; }

		public bool IsReference { get; set; }

		public Column(ArmSoft.SME.Common.Models.Data.Column column, PropertyInfo pi, int index, Type rowType)
		{
			Name = column.Name;
			Caption = column.Caption;
			Type = column.Type;
			PI = pi;
			Group = column.Group;
			Format = column.Format;
			IsReference = column.SourceType == ColumnSourceType.Reference;
			Index = index;
			Source = column.Source;
			if (pi != null)
			{
				propertyType = GetSupportedType(pi).Value;
				setter = CreatePropertySetterDelegate(propertyType, pi, rowType);
			}
		}
	}

	public class RowSchema
	{
		public readonly Type RowType;

		public Dictionary<string, Column> DynamicColumns { get; private set; } = new Dictionary<string, Column>();

		public List<Column> Columns { get; } = new List<Column>();

		public RowSchema(Type rowType, List<ArmSoft.SME.Common.Models.Data.Column> columns, HashSet<string> filteredColumns)
		{
			RowType = rowType;
			DynamicColumns.Clear();
			Columns.Clear();
			if (columns == null)
			{
				return;
			}
			List<ArmSoft.SME.Common.Models.Data.Column> list = columns.Where((ArmSoft.SME.Common.Models.Data.Column p) => p.SourceType == ColumnSourceType.Dynamic).ToList();
			for (int num = 0; num < list.Count; num++)
			{
				DynamicColumns.Add(list[num].Name, new Column(list[num], null, num, rowType));
			}
			if (filteredColumns == null || filteredColumns.Count == 0)
			{
				for (int num2 = 0; num2 < columns.Count; num2++)
				{
					Columns.Add(new Column(columns[num2], rowType.GetProperty(columns[num2].Name), num2, rowType));
				}
				return;
			}
			for (int num3 = 0; num3 < columns.Count; num3++)
			{
				if (filteredColumns.Contains(columns[num3].Name))
				{
					Columns.Add(new Column(columns[num3], rowType.GetProperty(columns[num3].Name), num3, rowType));
				}
			}
		}

		public void AddColumns(List<ArmSoft.SME.Common.Models.Data.Column> dynamicColumns)
		{
			for (int i = 0; i < dynamicColumns.Count; i++)
			{
				if (!DynamicColumns.TryGetValue(dynamicColumns[i].Name, out var _))
				{
					DynamicColumns.Add(dynamicColumns[i].Name, new Column(dynamicColumns[i], null, i, RowType));
				}
			}
		}
	}

	private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<PropertyInfo, EmittedDelegates>> emitted = new ConcurrentDictionary<Type, ConcurrentDictionary<PropertyInfo, EmittedDelegates>>();

	internal static readonly Type stringType = typeof(string);

	internal static readonly Type decimalType = typeof(decimal);

	internal static readonly Type decimalNullableType = typeof(decimal?);

	internal static readonly Type dateTimeType = typeof(DateTime);

	internal static readonly Type dateTimeNullableType = typeof(DateTime?);

	internal static readonly Type booleanType = typeof(bool);

	internal static readonly Type booleanNullableType = typeof(bool?);

	internal static readonly Type integerType = typeof(int);

	internal static readonly Type integerNullableType = typeof(int?);

	internal static readonly Type shortType = typeof(short);

	internal static readonly Type shortNullableType = typeof(short?);

	internal static readonly Type byteType = typeof(byte);

	internal static readonly Type byteNullableType = typeof(byte?);

	internal static readonly Type longType = typeof(long);

	internal static readonly Type longNullableType = typeof(long?);

	internal static readonly Type guidType = typeof(Guid);

	internal static readonly Type guidNullableType = typeof(Guid?);

	internal static readonly Type doubleType = typeof(double);

	internal static readonly Type doubleNullableType = typeof(double?);

	internal static readonly Type dateOnlyType = typeof(DateOnly);

	internal static readonly Type dateOnlyNullableType = typeof(DateOnly?);

	internal static readonly Type byteArrayType = typeof(byte[]);

	internal static readonly Type listOfStringType = typeof(List<string>);

	private RowSchema _schema;

	public string[] Values { get; set; }

	[JsonIgnore]
	public RowSchema Schema
	{
		get
		{
			return _schema;
		}
		set
		{
			_schema = value;
		}
	}

	public object this[string name]
	{
		get
		{
			if (Schema == null)
			{
				throw new InvalidOperationException("Schema is not defined");
			}
			if (Schema.DynamicColumns.TryGetValue(name, out var value))
			{
				if (value.IsReference)
				{
					return ConvertValue(value.Type, GetReferenceOrExtensionValue(value.Name, value.Source, value.Index));
				}
				if (value.Index >= 0)
				{
					return ConvertValue(value.Type, Values[value.Index]);
				}
				return value.PI.GetValue(this);
			}
			throw new InvalidOperationException("Invalid column name '" + name + "'.");
		}
		set
		{
			if (Schema == null)
			{
				throw new InvalidOperationException("Schema is not defined");
			}
			if (Schema.DynamicColumns.TryGetValue(name, out var value2))
			{
				if (value2.Index >= 0)
				{
					if (Values == null)
					{
						string[] array = (Values = new string[Schema.DynamicColumns.Count]);
					}
					Values[value2.Index] = value?.ToString();
				}
				else
				{
					value2.PI.SetValue(this, value);
				}
				return;
			}
			throw new InvalidOperationException("Invalid column name '" + name + "'.");
		}
	}

	internal static object CreatePropertySetterDelegate(SupportedType supportedType, PropertyInfo propertyInfo, Type rowType)
	{
		EmittedDelegates emittedDelegates = GetEmittedDelegates(propertyInfo, rowType);
		if (emittedDelegates.Setter == null)
		{
			DynamicMethod dynamicMethod = CreatePropertySetter(propertyInfo, rowType);
			EmittedDelegates emittedDelegates2 = emittedDelegates;
			emittedDelegates2.Setter = supportedType switch
			{
				SupportedType.String => (object)dynamicMethod.CreateDelegate(typeof(StringSetterDelegate)), 
				SupportedType.ByteArray => dynamicMethod.CreateDelegate(typeof(ByteArraySetterDelegate)), 
				SupportedType.Decimal => dynamicMethod.CreateDelegate(typeof(DecimalSetterDelegate)), 
				SupportedType.DecimalNullable => dynamicMethod.CreateDelegate(typeof(DecimalNullableSetterDelegate)), 
				SupportedType.DateTime => dynamicMethod.CreateDelegate(typeof(DateTimeSetterDelegate)), 
				SupportedType.DateTimeNullable => dynamicMethod.CreateDelegate(typeof(DateTimeNullableSetterDelegate)), 
				SupportedType.Boolean => dynamicMethod.CreateDelegate(typeof(BooleanSetterDelegate)), 
				SupportedType.BooleanNullable => dynamicMethod.CreateDelegate(typeof(BooleanNullableSetterDelegate)), 
				SupportedType.Integer => dynamicMethod.CreateDelegate(typeof(Int32SetterDelegate)), 
				SupportedType.IntegerNullable => dynamicMethod.CreateDelegate(typeof(Int32NullableSetterDelegate)), 
				SupportedType.Short => dynamicMethod.CreateDelegate(typeof(Int16SetterDelegate)), 
				SupportedType.ShortNullable => dynamicMethod.CreateDelegate(typeof(Int16NullableSetterDelegate)), 
				SupportedType.Long => dynamicMethod.CreateDelegate(typeof(Int64SetterDelegate)), 
				SupportedType.LongNullable => dynamicMethod.CreateDelegate(typeof(Int64NullableSetterDelegate)), 
				SupportedType.Byte => dynamicMethod.CreateDelegate(typeof(ByteSetterDelegate)), 
				SupportedType.ByteNullable => dynamicMethod.CreateDelegate(typeof(ByteNullableSetterDelegate)), 
				SupportedType.Guid => dynamicMethod.CreateDelegate(typeof(GuidSetterDelegate)), 
				SupportedType.GuidNullable => dynamicMethod.CreateDelegate(typeof(GuidNullableSetterDelegate)), 
				SupportedType.Double => dynamicMethod.CreateDelegate(typeof(DoubleSetterDelegate)), 
				SupportedType.DoubleNullable => dynamicMethod.CreateDelegate(typeof(DoubleNullableSetterDelegate)), 
				SupportedType.DateOnly => dynamicMethod.CreateDelegate(typeof(DateOnlySetterDelegate)), 
				SupportedType.DateOnlyNullable => dynamicMethod.CreateDelegate(typeof(DateOnlyNullableSetterDelegate)), 
				_ => throw new InvalidEnumArgumentException(), 
			};
		}
		return emittedDelegates.Setter;
	}

	private static EmittedDelegates GetEmittedDelegates(PropertyInfo propertyInfo, Type rowType)
	{
		if (!emitted.TryGetValue(rowType, out var value))
		{
			value = new ConcurrentDictionary<PropertyInfo, EmittedDelegates>();
			if (!emitted.TryAdd(rowType, value))
			{
				emitted.TryGetValue(rowType, out value);
			}
		}
		if (!value.TryGetValue(propertyInfo, out var value2))
		{
			value2 = new EmittedDelegates();
			if (!value.TryAdd(propertyInfo, value2))
			{
				value.TryGetValue(propertyInfo, out value2);
			}
		}
		return value2;
	}

	internal static DynamicMethod CreatePropertySetter(PropertyInfo propertyInfo, Type classType)
	{
		MethodInfo setMethod = propertyInfo.SetMethod;
		Type propertyType = propertyInfo.PropertyType;
		DynamicMethod dynamicMethod = new DynamicMethod(setMethod.Name, typeof(void), new Type[2]
		{
			typeof(object),
			propertyType
		}, typeof(ExtendableRow).Module, skipVisibility: true);
		ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
		iLGenerator.Emit(OpCodes.Ldarg_0);
		if (classType.IsValueType)
		{
			iLGenerator.Emit(OpCodes.Unbox, classType);
			iLGenerator.Emit(OpCodes.Ldarg_1);
			iLGenerator.Emit(OpCodes.Call, setMethod);
		}
		else
		{
			iLGenerator.Emit(OpCodes.Castclass, classType);
			iLGenerator.Emit(OpCodes.Ldarg_1);
			iLGenerator.Emit(OpCodes.Callvirt, setMethod);
		}
		iLGenerator.Emit(OpCodes.Ret);
		return dynamicMethod;
	}

	internal static SupportedType? GetSupportedType(PropertyInfo pi)
	{
		if (pi.PropertyType == stringType)
		{
			return SupportedType.String;
		}
		if (pi.PropertyType == decimalType)
		{
			return SupportedType.Decimal;
		}
		if (pi.PropertyType == decimalNullableType)
		{
			return SupportedType.DecimalNullable;
		}
		if (pi.PropertyType == dateTimeType)
		{
			return SupportedType.DateTime;
		}
		if (pi.PropertyType == dateTimeNullableType)
		{
			return SupportedType.DateTimeNullable;
		}
		if (pi.PropertyType == booleanType)
		{
			return SupportedType.Boolean;
		}
		if (pi.PropertyType == booleanNullableType)
		{
			return SupportedType.BooleanNullable;
		}
		if (pi.PropertyType == integerType)
		{
			return SupportedType.Integer;
		}
		if (pi.PropertyType == integerNullableType)
		{
			return SupportedType.IntegerNullable;
		}
		if (pi.PropertyType == shortType)
		{
			return SupportedType.Short;
		}
		if (pi.PropertyType == shortNullableType)
		{
			return SupportedType.ShortNullable;
		}
		if (pi.PropertyType == byteType)
		{
			return SupportedType.Byte;
		}
		if (pi.PropertyType == byteNullableType)
		{
			return SupportedType.ByteNullable;
		}
		if (pi.PropertyType == byteArrayType)
		{
			return SupportedType.ByteArray;
		}
		if (pi.PropertyType == listOfStringType)
		{
			return SupportedType.ListOfString;
		}
		if (pi.PropertyType == guidType)
		{
			return SupportedType.Guid;
		}
		if (pi.PropertyType == guidNullableType)
		{
			return SupportedType.GuidNullable;
		}
		if (pi.PropertyType == doubleType)
		{
			return SupportedType.Double;
		}
		if (pi.PropertyType == doubleNullableType)
		{
			return SupportedType.DoubleNullable;
		}
		if (pi.PropertyType == dateOnlyType)
		{
			return SupportedType.DateOnly;
		}
		if (pi.PropertyType == dateOnlyNullableType)
		{
			return SupportedType.DateOnlyNullable;
		}
		if (pi.PropertyType == longType)
		{
			return SupportedType.Long;
		}
		if (pi.PropertyType == longNullableType)
		{
			return SupportedType.LongNullable;
		}
		return null;
	}

	object IDynamicValue.GetDynamicValue(string name)
	{
		return this[name];
	}

	public virtual object ReferenceValue(string name, string source)
	{
		throw new NotImplementedException();
	}

	private protected virtual object GetExtensionValue(int index)
	{
		return null;
	}

	private object GetReferenceOrExtensionValue(string name, string source, int index)
	{
		object obj = ReferenceValue(name, source);
		if (obj == null && name.StartsWith("DocumentExtension") && index >= 0)
		{
			obj = GetExtensionValue(index);
		}
		return obj;
	}

	public ExtendableRow()
	{
		_schema = null;
	}

	public ExtendableRow(RowSchema schema)
	{
		_schema = schema;
	}

	private static object ConvertValue(SupportedType columnType, string value)
	{
		switch (columnType)
		{
		case SupportedType.Boolean:
		case SupportedType.BooleanNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)value.ToBoolean());
		case SupportedType.Byte:
		case SupportedType.ByteNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)Convert.ToByte(value));
		case SupportedType.Decimal:
		case SupportedType.DecimalNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)Convert.ToDecimal(value));
		case SupportedType.Double:
		case SupportedType.DoubleNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)Convert.ToDouble(value));
		case SupportedType.Integer:
		case SupportedType.IntegerNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)Convert.ToInt32(value));
		case SupportedType.DateTime:
		case SupportedType.DateTimeNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)Convert.ToDateTime(value));
		case SupportedType.DateOnly:
		case SupportedType.DateOnlyNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)Convert.ToDateTime(value).ToDateOnly());
		case SupportedType.Short:
		case SupportedType.ShortNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)Convert.ToInt16(value));
		case SupportedType.Guid:
		case SupportedType.GuidNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)new Guid(value));
		case SupportedType.Long:
		case SupportedType.LongNullable:
			return string.IsNullOrWhiteSpace(value) ? null : ((object)(long)Convert.ToDecimal(value));
		default:
			return value;
		}
	}

	private static object ConvertValue(SupportedType columnType, object value)
	{
		switch (columnType)
		{
		case SupportedType.Boolean:
		case SupportedType.BooleanNullable:
			return Convert.ToString(value).ToBoolean();
		case SupportedType.Byte:
		case SupportedType.ByteNullable:
			return Convert.ToByte(value);
		case SupportedType.Decimal:
		case SupportedType.DecimalNullable:
			return Convert.ToDecimal(value);
		case SupportedType.Double:
		case SupportedType.DoubleNullable:
			return Convert.ToDouble(value);
		case SupportedType.Integer:
		case SupportedType.IntegerNullable:
			return Convert.ToInt32(value);
		case SupportedType.Short:
		case SupportedType.ShortNullable:
			return Convert.ToInt16(value);
		case SupportedType.Long:
		case SupportedType.LongNullable:
			return (long)Convert.ToDecimal(value);
		case SupportedType.DateOnly:
			return (value != null) ? ((object)Convert.ToDateTime(value).ToDateOnly()) : null;
		default:
			return value;
		}
	}

	internal void Load(DataProviderRowsBinaryReader reader)
	{
		for (int i = 0; i < _schema.Columns.Count; i++)
		{
			Column column = _schema.Columns[i];
			if (_schema.DynamicColumns.TryGetValue(column.Name, out var _))
			{
				if (Values == null)
				{
					Values = reader.GetListOfString();
					reader.Read();
					break;
				}
			}
			else
			{
				switch (column.Type)
				{
				case SupportedType.String:
				{
					string value6 = reader.GetString();
					((StringSetterDelegate)column.setter)(this, value6);
					break;
				}
				case SupportedType.ByteArray:
				{
					byte[] byteArray = reader.GetByteArray();
					((ByteArraySetterDelegate)column.setter)(this, byteArray);
					break;
				}
				case SupportedType.ListOfString:
				{
					List<string> value8 = new List<string>();
					((ListOfStringSetterDelegate)column.setter)(this, value8);
					break;
				}
				case SupportedType.Decimal:
				{
					decimal value4 = reader.GetDecimal();
					((DecimalSetterDelegate)column.setter)(this, value4);
					break;
				}
				case SupportedType.DecimalNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.DecimalNull)
					{
						((DecimalNullableSetterDelegate)column.setter)(this, null);
						break;
					}
					decimal value4 = reader.GetDecimal();
					((DecimalNullableSetterDelegate)column.setter)(this, value4);
					break;
				}
				case SupportedType.DateTime:
				{
					DateTime dateTime = reader.GetDateTime();
					((DateTimeSetterDelegate)column.setter)(this, dateTime);
					break;
				}
				case SupportedType.DateTimeNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.DateTimeNull)
					{
						((DateTimeNullableSetterDelegate)column.setter)(this, null);
						break;
					}
					DateTime dateTime = reader.GetDateTime();
					((DateTimeNullableSetterDelegate)column.setter)(this, dateTime);
					break;
				}
				case SupportedType.DateOnly:
				{
					DateOnly dateOnly = reader.GetDateOnly();
					((DateOnlySetterDelegate)column.setter)(this, dateOnly);
					break;
				}
				case SupportedType.DateOnlyNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.DateOnlyNull)
					{
						((DateOnlyNullableSetterDelegate)column.setter)(this, null);
						break;
					}
					DateOnly dateOnly = reader.GetDateOnly();
					((DateOnlyNullableSetterDelegate)column.setter)(this, dateOnly);
					break;
				}
				case SupportedType.Boolean:
				{
					bool boolean = reader.GetBoolean();
					((BooleanSetterDelegate)column.setter)(this, boolean);
					break;
				}
				case SupportedType.BooleanNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.BooleanNull)
					{
						((BooleanNullableSetterDelegate)column.setter)(this, null);
						break;
					}
					bool boolean = reader.GetBoolean();
					((BooleanNullableSetterDelegate)column.setter)(this, boolean);
					break;
				}
				case SupportedType.Integer:
				{
					int integer = reader.GetInteger();
					((Int32SetterDelegate)column.setter)(this, integer);
					break;
				}
				case SupportedType.IntegerNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.IntegerNull)
					{
						((Int32NullableSetterDelegate)column.setter)(this, null);
						break;
					}
					int integer = reader.GetInteger();
					((Int32NullableSetterDelegate)column.setter)(this, integer);
					break;
				}
				case SupportedType.Short:
				{
					short value7 = reader.GetShort();
					((Int16SetterDelegate)column.setter)(this, value7);
					break;
				}
				case SupportedType.ShortNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.ShortNull)
					{
						((Int16NullableSetterDelegate)column.setter)(this, null);
						break;
					}
					short value7 = reader.GetShort();
					((Int16NullableSetterDelegate)column.setter)(this, value7);
					break;
				}
				case SupportedType.Long:
				{
					long value5 = reader.GetLong();
					((Int64SetterDelegate)column.setter)(this, value5);
					break;
				}
				case SupportedType.LongNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.LongNull)
					{
						((Int64NullableSetterDelegate)column.setter)(this, null);
						break;
					}
					long value5 = reader.GetLong();
					((Int64NullableSetterDelegate)column.setter)(this, value5);
					break;
				}
				case SupportedType.Guid:
				{
					Guid guid = reader.GetGuid();
					((GuidSetterDelegate)column.setter)(this, guid);
					break;
				}
				case SupportedType.GuidNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.GUIDNull)
					{
						((GuidNullableSetterDelegate)column.setter)(this, null);
						break;
					}
					Guid guid = reader.GetGuid();
					((GuidNullableSetterDelegate)column.setter)(this, guid);
					break;
				}
				case SupportedType.Byte:
				{
					byte value3 = reader.GetByte();
					((ByteSetterDelegate)column.setter)(this, value3);
					break;
				}
				case SupportedType.ByteNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.ByteNull)
					{
						((ByteNullableSetterDelegate)column.setter)(this, null);
						break;
					}
					byte value3 = reader.GetByte();
					((ByteNullableSetterDelegate)column.setter)(this, value3);
					break;
				}
				case SupportedType.Double:
				{
					double value2 = reader.GetDouble();
					((DoubleSetterDelegate)column.setter)(this, value2);
					break;
				}
				case SupportedType.DoubleNullable:
				{
					if (reader.CurrentValueType == EncodedSupportedType.DoubleNull)
					{
						((DoubleNullableSetterDelegate)column.setter)(this, null);
						break;
					}
					double value2 = reader.GetDouble();
					((DoubleNullableSetterDelegate)column.setter)(this, value2);
					break;
				}
				}
			}
			reader.Read();
		}
	}
}
