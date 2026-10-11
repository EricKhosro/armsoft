using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ArmSoft.ElScaleConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.ExceptionDispatch;
using ArmSoft.SME.Core.UI.Progress;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.DataProviders.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class MassaKDirectExporterProgress
{
	private readonly MultilineProgressWindow _progress;

	private readonly List<string> _phases = new List<string>();

	private readonly DateTime _expDate;

	private readonly IReadOnlyList<MaterialCacheRow> _goods;

	private readonly ElScalesAndTerminalsDataProvider.DataRow _row;

	public List<ElScaleGoodsExportDataProvider.DataRow> ResultRows = new List<ElScaleGoodsExportDataProvider.DataRow>();

	public MassaKDirectExporterProgress(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		_expDate = expDate;
		_goods = goods;
		_row = row;
		_progress = new MultilineProgressWindow
		{
			Title = ArmSoft.SME.Trade.Common.Resources.SendingItemsToElScale
		};
		_phases.Add(ArmSoft.SME.Common.Resources.Connecting);
		if (row.DeleteAllItemsBeforeExporting)
		{
			_phases.Add(ArmSoft.SME.Trade.Common.Resources.ErasingItemsFromScale);
		}
		_phases.Add(ArmSoft.SME.Common.Resources.RowsProcessed);
		_phases.Add(ArmSoft.SME.Common.Resources.Sending);
		_progress.PreparePrimaryPhases(_phases);
		_progress.BackgroundWorkerDoWork += BackgroundWorkerDoWorkEventHandler;
		_progress.ShowDialogWithExceptionHandling();
	}

	private void BackgroundWorkerDoWorkEventHandler(object sender, DoWorkEventArgs e)
	{
		ExportGoodsForMassak(_expDate, _goods, _row);
	}

	public void ExportGoodsForMassak(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		ScalePhase scalePhase = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.Connecting,
			DisableCancel = true
		};
		_progress.ProcessPhase(scalePhase);
		MassaK.MassaKDriverDevices massaKDriverDevices;
		MassaK.MassaKDriverGoods massaKDriverGoods;
		try
		{
			massaKDriverDevices = new MassaK.MassaKDriverDevices();
			massaKDriverGoods = new MassaK.MassaKDriverGoods();
		}
		catch
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.MassakDeviceSettingsAreMissing);
		}
		int device = massaKDriverDevices.GetDevice(row.ExportFile, row.Name);
		if (device != 0)
		{
			throw new ASException(ElScaleAndTerminalHelper.DecodeMassakErrorCode(device));
		}
		scalePhase.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase);
		if (device != 0)
		{
			return;
		}
		if (row.DeleteAllItemsBeforeExporting)
		{
			ScalePhase phase = new ScalePhase
			{
				Name = ArmSoft.SME.Trade.Common.Resources.ErasingItemsFromScale,
				DisableCancel = true
			};
			_progress.ProcessPhase(phase);
			massaKDriverDevices.DeleteGoods();
			_progress.ProcessPhase(phase);
		}
		massaKDriverDevices.WriteType = 1;
		List<PricesDataProvider.DataRow> rows = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(new PricesDataProvider.Param
		{
			EndDate = expDate.ToDateOnly(),
			PriceType = row.PriceList
		}, withoutAccessCheck: true);
		List<ElScaleGoodsExportDataProvider.DataRow> list = new List<ElScaleGoodsExportDataProvider.DataRow>();
		bool flag = false;
		int num = 0;
		ScalePhase scalePhase2 = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.RowsProcessed,
			DisableCancel = false,
			Row = 0,
			Total = goods.Count
		};
		_progress.ProcessPhase(scalePhase2);
		massaKDriverGoods.Clear();
		for (int i = 0; i < goods.Count; i++)
		{
			if (_progress.Canceled)
			{
				return;
			}
			MaterialCacheRow item = goods[i];
			if (row.AsNameChoose == NameInterpretationsSE.InterpretationName)
			{
				massaKDriverGoods.Name = item.Caption;
			}
			else if (row.AsNameChoose == NameInterpretationsSE.FullName)
			{
				massaKDriverGoods.Name = item.FullCaption;
			}
			else if (row.AsNameChoose == NameInterpretationsSE.Specification)
			{
				massaKDriverGoods.Name = item.Description;
			}
			massaKDriverGoods.Ingredients = item.Description;
			massaKDriverGoods.ID = item.Id;
			bool flag2 = false;
			string text = "";
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(item.Id, item.Unit);
			if (row.AsCodeExport == "1")
			{
				if (int.TryParse(item.MTCode, out var result))
				{
					massaKDriverGoods.PLU = result;
					flag2 = true;
				}
				else
				{
					flag2 = false;
					text = string.Format(ArmSoft.SME.Trade.Common.Resources.CodeCanNotBeConvertedToString, item.MTCode);
					massaKDriverGoods.PLU = 0;
				}
			}
			else if (row.AsCodeExport == "2")
			{
				if (item.PLUCode.HasValue)
				{
					massaKDriverGoods.PLU = item.PLUCode.Value;
					flag2 = true;
				}
				else
				{
					flag2 = false;
					text = string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Trade.Common.Resources.PLUCodeInElScale);
					massaKDriverGoods.PLU = 0;
				}
			}
			else if (row.AsCodeExport == ElScaleExportNextCodesSE.BarCode)
			{
				if (barCode != null)
				{
					if (int.TryParse(barCode.BarCode, out var result2))
					{
						massaKDriverGoods.PLU = result2;
						flag2 = true;
					}
					else
					{
						flag2 = false;
						text = string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeCanNotBeConvertedToString, barCode.BarCode);
						massaKDriverGoods.PLU = 0;
					}
				}
				else
				{
					flag2 = false;
					text = string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeWithUnitDoesNotExist, item.MTCode, item.Unit);
					massaKDriverGoods.PLU = 0;
				}
			}
			WeightItemInfo instance = WeightItemInfo.GetInstance();
			PieceItemInfo instance2 = PieceItemInfo.GetInstance();
			if (item.Unit == instance2.Unit)
			{
				massaKDriverGoods.GoodsTypeID = 1;
				massaKDriverGoods.BarcodePrefix = int.Parse(instance2.Prefix);
				massaKDriverGoods.BasicUnit = instance2.Unit;
			}
			else
			{
				massaKDriverGoods.GoodsTypeID = 0;
				massaKDriverGoods.BarcodePrefix = int.Parse(instance.Prefix);
				massaKDriverGoods.BasicUnit = instance.Unit;
			}
			IEnumerable<PricesDataProvider.DataRow> source = rows.Where((PricesDataProvider.DataRow x) => x.MTCode == item.MTCode && x.QntUnit == item.Unit);
			if (!source.Any())
			{
				flag2 = false;
				string priceNotFound = ArmSoft.SME.Trade.Common.Resources.PriceNotFound;
				InlineArray4<object> buffer = default;
				buffer[0] = item.MTCode;
				buffer[1] = expDate.ToDateOnly();
				buffer[2] = row.PriceList;
				buffer[3] = item.Unit;
				text = string.Format(priceNotFound, (ReadOnlySpan<object?>)buffer);
			}
			else
			{
				massaKDriverGoods.Price = (int)(source.LastOrDefault().Price * 100m);
			}
			if ((barCode != null) & flag2)
			{
				massaKDriverGoods.Code = barCode.BarCode;
				flag2 = true;
			}
			else if (flag2)
			{
				flag2 = false;
				text = string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeWithUnitDoesNotExist, item.MTCode, item.Unit);
				massaKDriverGoods.Code = "0";
			}
			try
			{
				ValidationService.ValidateString("PLU", ArmSoft.SME.Trade.Common.Resources.PLUCodeInElScale, massaKDriverGoods.PLU.ToString(), 8, isRequired: true);
			}
			catch (Exception ex)
			{
				flag2 = false;
				text = ex.Message;
			}
			int num2 = -1;
			if (flag2)
			{
				num2 = massaKDriverGoods.Add();
			}
			if (num2 == 0)
			{
				flag = true;
			}
			if (num2 == -10)
			{
				num = massaKDriverDevices.WriteGoods();
				if (num != 0)
				{
					throw new ASException(ElScaleAndTerminalHelper.DecodeMassakErrorCode(num));
				}
				massaKDriverGoods.Clear();
				i--;
				continue;
			}
			list.Add(new ElScaleGoodsExportDataProvider.DataRow
			{
				ID = massaKDriverGoods.ID,
				CodeInScale = ((num2 == 0) ? massaKDriverGoods.Code : ""),
				CodeInTrade = item.MTCode,
				Name = massaKDriverGoods.Name,
				Ingredients = massaKDriverGoods.Ingredients,
				PLU = ((num2 == 0) ? new int?(massaKDriverGoods.PLU) : ((int?)null)),
				IsWeight = (massaKDriverGoods.GoodsTypeID == 0),
				BarCodePrefix = massaKDriverGoods.BarcodePrefix,
				BasicUnit = massaKDriverGoods.BasicUnit,
				Price = DecimalExtension.RoundAs(massaKDriverGoods.Price / 100 + massaKDriverGoods.Price % 100, 2),
				Exported = (num2 == 0),
				ErrorComment = ((!flag2) ? text : ((num2 != 0) ? ElScaleAndTerminalHelper.DecodeMassakErrorCode(num2) : ""))
			});
			scalePhase2.Row = i + 1;
			_progress.ProcessPhase(scalePhase2);
		}
		scalePhase2.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase2);
		ScalePhase scalePhase3 = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.Sending,
			DisableCancel = true
		};
		_progress.ProcessPhase(scalePhase3);
		num = massaKDriverDevices.WriteGoods();
		if (num != 0)
		{
			throw new ASException(ElScaleAndTerminalHelper.DecodeMassakErrorCode(num));
		}
		scalePhase3.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase3);
		if (flag)
		{
			ElScaleOrTerminal elScaleOrTerminal = (ElScaleOrTerminal)Settings.DirectoryService.Load(DirectoryTypeSE.ElScaleOrTerminal.Value, row.Code);
			elScaleOrTerminal.ExportDate = DateTime.Now;
			Settings.DirectoryService.Store(elScaleOrTerminal, new DirStoreRequest());
		}
		ResultRows = list;
	}
}
