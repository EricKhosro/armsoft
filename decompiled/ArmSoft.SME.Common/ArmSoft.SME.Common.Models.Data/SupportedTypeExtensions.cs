using System;
using System.ComponentModel;
using System.Data;

namespace ArmSoft.SME.Common.Models.Data;

public static class SupportedTypeExtensions
{
	public static string SQLiteType(this SupportedType supportedType)
	{
		switch (supportedType)
		{
		case SupportedType.String:
		case SupportedType.DateTimeNullable:
		case SupportedType.DecimalNullable:
		case SupportedType.DateOnlyNullable:
			return "text";
		case SupportedType.DateTime:
		case SupportedType.Decimal:
		case SupportedType.DateOnly:
			return "text not null";
		case SupportedType.GuidNullable:
		case SupportedType.ByteArray:
			return "blob";
		case SupportedType.Guid:
			return "blob not null";
		case SupportedType.Boolean:
		case SupportedType.Integer:
		case SupportedType.Short:
		case SupportedType.Byte:
		case SupportedType.Long:
			return "integer not null";
		case SupportedType.BooleanNullable:
		case SupportedType.IntegerNullable:
		case SupportedType.ShortNullable:
		case SupportedType.ByteNullable:
		case SupportedType.LongNullable:
			return "integer";
		case SupportedType.DoubleNullable:
			return "real";
		case SupportedType.Double:
			return "real not null";
		default:
			throw new InvalidEnumArgumentException();
		}
	}

	public static object DefaultValue(this SupportedType supportedType)
	{
		return supportedType switch
		{
			SupportedType.String => (object)string.Empty, 
			SupportedType.ByteArray => null, 
			SupportedType.Decimal => 0m, 
			SupportedType.DecimalNullable => null, 
			SupportedType.DateTime => new DateTime(1, 1, 1), 
			SupportedType.DateTimeNullable => null, 
			SupportedType.Boolean => false, 
			SupportedType.BooleanNullable => null, 
			SupportedType.Integer => 0, 
			SupportedType.IntegerNullable => null, 
			SupportedType.Short => (short)0, 
			SupportedType.ShortNullable => null, 
			SupportedType.Long => 0L, 
			SupportedType.LongNullable => null, 
			SupportedType.Byte => (byte)0, 
			SupportedType.ByteNullable => null, 
			SupportedType.Guid => Guid.Empty, 
			SupportedType.GuidNullable => null, 
			SupportedType.Double => 0.0, 
			SupportedType.DoubleNullable => null, 
			SupportedType.DateOnly => new DateOnly(1, 1, 1), 
			SupportedType.DateOnlyNullable => null, 
			_ => throw new InvalidEnumArgumentException(), 
		};
	}

	public static DbType DbType(this SupportedType supportedType)
	{
		return supportedType switch
		{
			SupportedType.String => System.Data.DbType.String, 
			SupportedType.ByteArray => System.Data.DbType.Binary, 
			SupportedType.Decimal => System.Data.DbType.Decimal, 
			SupportedType.DecimalNullable => System.Data.DbType.Decimal, 
			SupportedType.DateTime => System.Data.DbType.DateTime, 
			SupportedType.DateTimeNullable => System.Data.DbType.DateTime, 
			SupportedType.Boolean => System.Data.DbType.Boolean, 
			SupportedType.BooleanNullable => System.Data.DbType.Boolean, 
			SupportedType.Integer => System.Data.DbType.Int32, 
			SupportedType.IntegerNullable => System.Data.DbType.Int32, 
			SupportedType.Short => System.Data.DbType.Int16, 
			SupportedType.ShortNullable => System.Data.DbType.Int16, 
			SupportedType.Long => System.Data.DbType.Int64, 
			SupportedType.LongNullable => System.Data.DbType.Int64, 
			SupportedType.Byte => System.Data.DbType.Byte, 
			SupportedType.ByteNullable => System.Data.DbType.Byte, 
			SupportedType.Guid => System.Data.DbType.Guid, 
			SupportedType.GuidNullable => System.Data.DbType.Guid, 
			SupportedType.Double => System.Data.DbType.Double, 
			SupportedType.DoubleNullable => System.Data.DbType.Double, 
			SupportedType.DateOnly => System.Data.DbType.DateTime, 
			SupportedType.DateOnlyNullable => System.Data.DbType.DateTime, 
			_ => throw new InvalidEnumArgumentException(), 
		};
	}

	public static Type CastToType(this SupportedType supportedType)
	{
		return supportedType switch
		{
			SupportedType.String => typeof(string), 
			SupportedType.ByteArray => typeof(byte[]), 
			SupportedType.Decimal => typeof(decimal), 
			SupportedType.DecimalNullable => typeof(decimal?), 
			SupportedType.DateTime => typeof(DateTime), 
			SupportedType.DateTimeNullable => typeof(DateTime?), 
			SupportedType.Boolean => typeof(bool), 
			SupportedType.BooleanNullable => typeof(bool?), 
			SupportedType.Integer => typeof(int), 
			SupportedType.IntegerNullable => typeof(int?), 
			SupportedType.Short => typeof(short), 
			SupportedType.ShortNullable => typeof(short?), 
			SupportedType.Long => typeof(long), 
			SupportedType.LongNullable => typeof(long?), 
			SupportedType.Byte => typeof(byte), 
			SupportedType.ByteNullable => typeof(byte?), 
			SupportedType.Guid => typeof(Guid), 
			SupportedType.GuidNullable => typeof(Guid?), 
			SupportedType.Double => typeof(double), 
			SupportedType.DoubleNullable => typeof(double?), 
			SupportedType.DateOnly => typeof(DateOnly), 
			SupportedType.DateOnlyNullable => typeof(DateOnly?), 
			_ => throw new InvalidEnumArgumentException(), 
		};
	}

	public static DataColumnType CastToDataColumnType(this SupportedType supportedType)
	{
		return supportedType switch
		{
			SupportedType.String => DataColumnType.String, 
			SupportedType.ByteArray => DataColumnType.Unknown, 
			SupportedType.Decimal => DataColumnType.Decimal, 
			SupportedType.DecimalNullable => DataColumnType.Decimal, 
			SupportedType.DateTime => DataColumnType.DateTime, 
			SupportedType.DateTimeNullable => DataColumnType.DateTime, 
			SupportedType.Boolean => DataColumnType.Boolean, 
			SupportedType.BooleanNullable => DataColumnType.Boolean, 
			SupportedType.Integer => DataColumnType.Integer, 
			SupportedType.IntegerNullable => DataColumnType.Integer, 
			SupportedType.Short => DataColumnType.Integer, 
			SupportedType.ShortNullable => DataColumnType.Integer, 
			SupportedType.Long => DataColumnType.Integer, 
			SupportedType.LongNullable => DataColumnType.Integer, 
			SupportedType.Byte => DataColumnType.Integer, 
			SupportedType.ByteNullable => DataColumnType.Integer, 
			SupportedType.Guid => DataColumnType.String, 
			SupportedType.GuidNullable => DataColumnType.String, 
			SupportedType.Double => DataColumnType.Decimal, 
			SupportedType.DoubleNullable => DataColumnType.Decimal, 
			SupportedType.DateOnly => DataColumnType.DateOnly, 
			SupportedType.DateOnlyNullable => DataColumnType.DateOnly, 
			_ => throw new InvalidEnumArgumentException(), 
		};
	}

	public static SupportedType CastToDataColumnType(this DataColumnType dataColumnType)
	{
		return dataColumnType switch
		{
			DataColumnType.String => SupportedType.String, 
			DataColumnType.DateTime => SupportedType.DateTime, 
			DataColumnType.DateOnly => SupportedType.DateOnly, 
			DataColumnType.Decimal => SupportedType.Decimal, 
			DataColumnType.Integer => SupportedType.Integer, 
			DataColumnType.Boolean => SupportedType.Boolean, 
			_ => throw new InvalidEnumArgumentException(), 
		};
	}
}
