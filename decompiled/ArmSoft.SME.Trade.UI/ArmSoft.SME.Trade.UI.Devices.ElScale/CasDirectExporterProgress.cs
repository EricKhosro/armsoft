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

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class CasDirectExporterProgress
{
	private readonly MultilineProgressWindow _progress;

	private readonly List<string> _phases = new List<string>();

	private readonly DateTime _expDate;

	private readonly IReadOnlyList<MaterialCacheRow> _goods;

	private readonly ElScalesAndTerminalsDataProvider.DataRow _row;

	public List<ElScaleGoodsExportDataProvider.DataRow> ResultRows = new List<ElScaleGoodsExportDataProvider.DataRow>();

	public CasDirectExporterProgress(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
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
		_phases.Add(ArmSoft.SME.Common.Resources.Sending);
		_progress.PreparePrimaryPhases(_phases);
		_progress.BackgroundWorkerDoWork += BackgroundWorkerDoWorkEventHandler;
		_progress.ShowDialogWithExceptionHandling();
	}

	private void BackgroundWorkerDoWorkEventHandler(object sender, DoWorkEventArgs e)
	{
		ExportGoodsForCas(_expDate, _goods, _row);
	}

	public void ExportGoodsForCas(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		ScalePhase scalePhase = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.Connecting,
			DisableCancel = true
		};
		_progress.ProcessPhase(scalePhase);
		Cas cas = new Cas(row.ExportFile);
		bool flag = cas.CheckConnection();
		if (!flag)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
		}
		scalePhase.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase);
		if (!flag)
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
			cas.EraseData();
			_progress.ProcessPhase(phase);
		}
		ScalePhase scalePhase2 = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.Sending,
			DisableCancel = false,
			Row = 0,
			Total = goods.Count
		};
		_progress.ProcessPhase(scalePhase2);
		List<PricesDataProvider.DataRow> rows = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(new PricesDataProvider.Param
		{
			EndDate = expDate.ToDateOnly(),
			PriceType = row.PriceList
		}, withoutAccessCheck: true);
		bool flag2 = false;
		for (int i = 0; i < goods.Count; i++)
		{
			if (_progress.Canceled)
			{
				return;
			}
			MaterialCacheRow item = goods[i];
			int num = 0;
			int code = -1;
			int num2 = 0;
			string text = "";
			string name = "";
			string name2 = "";
			string text2 = "";
			int num3 = 0;
			IEnumerable<PricesDataProvider.DataRow> source = rows.Where((PricesDataProvider.DataRow x) => x.MTCode == item.MTCode && x.QntUnit == item.Unit);
			if (row.AsNameChoose == NameInterpretationsSE.InterpretationName)
			{
				text = item.Caption;
			}
			else if (row.AsNameChoose == NameInterpretationsSE.FullName)
			{
				text = item.FullCaption;
			}
			else if (row.AsNameChoose == NameInterpretationsSE.Specification)
			{
				text = item.Description;
			}
			text2 = item.Description;
			bool flag3 = false;
			string errorComment = "";
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(item.Id, item.Unit);
			if (row.AsCodeExport == "1")
			{
				if (uint.TryParse(item.MTCode, out var result))
				{
					num = (int)result;
					flag3 = true;
				}
				else
				{
					flag3 = false;
					errorComment = string.Format(ArmSoft.SME.Trade.Common.Resources.CodeCanNotBeConvertedToString, item.MTCode);
					num = 0;
				}
			}
			else if (row.AsCodeExport == "2")
			{
				if (item.PLUCode.HasValue)
				{
					num = item.PLUCode.Value;
					flag3 = true;
				}
				else
				{
					flag3 = false;
					errorComment = string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Trade.Common.Resources.PLUCodeInElScale);
					num = 0;
				}
			}
			else if (row.AsCodeExport == ElScaleExportNextCodesSE.BarCode)
			{
				if (barCode != null)
				{
					if (int.TryParse(barCode.BarCode, out var result2))
					{
						num = result2;
						flag3 = true;
					}
					else
					{
						flag3 = false;
						errorComment = string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeCanNotBeConvertedToString, barCode.BarCode);
						num = 0;
					}
				}
				else
				{
					flag3 = false;
					errorComment = string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeWithUnitDoesNotExist, item.MTCode, item.Unit);
					num = 0;
				}
			}
			int dep = 1;
			WeightItemInfo instance = WeightItemInfo.GetInstance();
			PieceItemInfo instance2 = PieceItemInfo.GetInstance();
			int num4;
			int num5;
			int num6;
			if (item.Unit == instance2.Unit)
			{
				num4 = 2;
				num2 = int.Parse(instance2.Prefix);
				num5 = row.PieceLabelN;
				num6 = row.PieceBarCodeN;
			}
			else
			{
				num4 = 1;
				num2 = int.Parse(instance.Prefix);
				num5 = row.WeightLabelN;
				num6 = row.WeightBarCodeN;
			}
			if (!source.Any())
			{
				flag3 = false;
				string priceNotFound = ArmSoft.SME.Trade.Common.Resources.PriceNotFound;
				InlineArray4<object> buffer = default;
				buffer[0] = item.MTCode;
				buffer[1] = expDate.ToDateOnly();
				buffer[2] = row.PriceList;
				buffer[3] = item.Unit;
				errorComment = string.Format(priceNotFound, (ReadOnlySpan<object?>)buffer);
			}
			else
			{
				num3 = (int)(source.LastOrDefault().Price * 100m);
			}
			if ((barCode != null) & flag3)
			{
				if (uint.TryParse(barCode.BarCode, out var result3))
				{
					code = (int)result3;
					flag3 = true;
				}
				else
				{
					flag3 = false;
					errorComment = string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeCanNotBeConvertedToString, barCode.BarCode);
					code = 0;
				}
			}
			else if (flag3)
			{
				flag3 = false;
				errorComment = string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeWithUnitDoesNotExist, item.MTCode, item.Unit);
				code = 0;
			}
			try
			{
				ValidationService.ValidateString("PLU", ArmSoft.SME.Trade.Common.Resources.PLUCodeInElScale, num.ToString(), 6, isRequired: true);
				if (flag3 && num == 0)
				{
					flag3 = false;
					errorComment = ArmSoft.SME.Trade.Common.Resources.PLUCanNotBeZeroForCasCLScales;
				}
			}
			catch (Exception ex)
			{
				flag3 = false;
				errorComment = ex.Message;
			}
			if (flag3)
			{
				Cas.CasItem casItem = cas.Upload(new Cas.CasItem(num, code, text, name, name2, text2, num3, num4, dep, num2, num5, num6));
				ResultRows.Add(new ElScaleGoodsExportDataProvider.DataRow
				{
					CodeInScale = casItem.ItemCode.ToString(),
					CodeInTrade = item.MTCode,
					Name = text,
					PLU = (int)casItem.PLU,
					Price = ((decimal)num3 / 100m).RoundAs(2),
					IsWeight = (num4 == 1),
					Ingredients = casItem.DirectMessage,
					Exported = casItem.Exported,
					BarCodePrefix = casItem.Group,
					LabelN = casItem.Label,
					BarCodeN = casItem.Barcode,
					ErrorComment = casItem.ErrorComment
				});
				flag2 = true;
			}
			else
			{
				ResultRows.Add(new ElScaleGoodsExportDataProvider.DataRow
				{
					CodeInTrade = item.MTCode,
					Name = text,
					PLU = null,
					Price = ((decimal)num3 / 100m).RoundAs(2),
					IsWeight = (num4 == 1),
					Ingredients = item.Description,
					BarCodePrefix = num2,
					LabelN = num5,
					BarCodeN = num6,
					Exported = false,
					ErrorComment = errorComment
				});
			}
			scalePhase2.Row = i + 1;
			_progress.ProcessPhase(scalePhase2);
		}
		scalePhase2.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase2);
		if (flag2)
		{
			ElScaleOrTerminal elScaleOrTerminal = (ElScaleOrTerminal)Settings.DirectoryService.Load(DirectoryTypeSE.ElScaleOrTerminal.Value, row.Code);
			elScaleOrTerminal.ExportDate = DateTime.Now;
			Settings.DirectoryService.Store(elScaleOrTerminal, new DirStoreRequest());
		}
	}
}
