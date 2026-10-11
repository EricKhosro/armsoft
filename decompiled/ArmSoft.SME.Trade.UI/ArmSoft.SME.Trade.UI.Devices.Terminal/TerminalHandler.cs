using System;
using System.Collections.Generic;
using System.Text;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.Devices.Terminal;

public abstract class TerminalHandler
{
	protected StringBuilder _serializationBuilder;

	protected string _priceType;

	protected DateTime _date;

	protected bool _alternativeUnit;

	public abstract string Brand { get; }

	public virtual bool SerializationImplemented => false;

	public virtual bool DeserializationImplemented => false;

	public virtual string FileExtension => ".dat";

	public virtual string FilesFilter => "Data File (.dat)|*.dat";

	public void Initialize()
	{
	}

	public string Serialize(List<TerminalRow> source, string priceType, DateTime date, bool alternativeUnit)
	{
		_priceType = priceType;
		_date = date;
		_alternativeUnit = alternativeUnit;
		_serializationBuilder = new StringBuilder();
		string value = SerializeHeader();
		if (!string.IsNullOrWhiteSpace(value))
		{
			_serializationBuilder.Append(value);
		}
		string value2 = SerializeBody(source);
		if (!string.IsNullOrWhiteSpace(value2))
		{
			_serializationBuilder.Append(value2);
		}
		string value3 = SerializeFooter();
		if (!string.IsNullOrWhiteSpace(value3))
		{
			_serializationBuilder.Append(value3);
		}
		return _serializationBuilder.ToString();
	}

	protected virtual string SerializeHeader()
	{
		return string.Empty;
	}

	protected virtual string SerializeFooter()
	{
		return string.Empty;
	}

	protected virtual string SerializeBody(List<TerminalRow> source)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (TerminalRow item in source)
		{
			string value = Serialize(item);
			if (!string.IsNullOrWhiteSpace(value))
			{
				stringBuilder.AppendLine(value);
			}
		}
		if (stringBuilder.Length > 2)
		{
			stringBuilder.Length -= 2;
		}
		return stringBuilder.ToString();
	}

	protected virtual string Serialize(TerminalRow sourceRow)
	{
		return string.Empty;
	}

	public virtual IEnumerable<TerminalRow> Deserialize(string fileName)
	{
		return null;
	}

	public void Validate(bool allowZeroQuantity, TerminalRow row, out MaterialCacheRow goodsRow, out MaterialCacheRow.BarCodeRow goodBarCodeRow)
	{
		if (row == null)
		{
			throw new TerminalHandlerException(-1, Resources.ErrWrongLineFormat);
		}
		if (row.BarCode.Length > 20)
		{
			throw new TerminalHandlerException(row.LineNumber, string.Format(Resources.ErrBarCodeLen, (short)20, row.BarCode));
		}
		goodsRow = TradeSettings.TradeCacheService.Material.GetItemByBarCode(row.BarCode);
		if (goodsRow == null)
		{
			throw new TerminalHandlerException(row.LineNumber, string.Format(Resources.BarCodeNotExist, row.BarCode));
		}
		goodBarCodeRow = goodsRow.BarCodes.Find((MaterialCacheRow.BarCodeRow p) => p.BarCode == row.BarCode);
		if (goodsRow.Type == ItemTypesSE.Services.Value)
		{
			throw new TerminalHandlerException(row.LineNumber, string.Format(Resources.ErrorServiceBarCode, row.BarCode));
		}
		if (!row.Quantity.HasValue)
		{
			throw new TerminalHandlerException(row.LineNumber, Resources.ErrWrongLineFormat);
		}
		if (allowZeroQuantity)
		{
			if (row.Quantity.Value < 0m)
			{
				throw new TerminalHandlerException(row.LineNumber, Resources.ErrNegativeQuantity);
			}
		}
		else if (row.Quantity.Value <= 0m)
		{
			throw new TerminalHandlerException(row.LineNumber, Resources.ErrNegOrZeroQuantity);
		}
	}
}
