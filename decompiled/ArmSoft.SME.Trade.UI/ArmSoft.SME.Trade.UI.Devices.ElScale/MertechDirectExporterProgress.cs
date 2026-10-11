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

public class MertechDirectExporterProgress
{
	private readonly MultilineProgressWindow _progress;

	private readonly List<string> _phases = new List<string>();

	private readonly DateTime _expDate;

	private readonly IReadOnlyList<MaterialCacheRow> _goods;

	private readonly ElScalesAndTerminalsDataProvider.DataRow _row;

	public List<ElScaleGoodsExportDataProvider.DataRow> ResultRows = new List<ElScaleGoodsExportDataProvider.DataRow>();

	public MertechDirectExporterProgress(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
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
		ExportGoodsForMertech(_expDate, _goods, _row);
	}

	public void ExportGoodsForMertech(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		ScalePhase scalePhase = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.Connecting,
			DisableCancel = true
		};
		_progress.ProcessPhase(scalePhase);
		Mertech mertech = new Mertech(row.ExportFile, row.Password, row.DeleteAllItemsBeforeExporting);
		bool flag = mertech.CheckConnection();
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
			mertech.EraseData();
			_progress.ProcessPhase(phase);
		}
		ScalePhase scalePhase2 = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.RowsProcessed,
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
		WeightItemInfo instance = WeightItemInfo.GetInstance();
		PieceItemInfo instance2 = PieceItemInfo.GetInstance();
		Mertech.ProductFileClass productFileClass = new Mertech.ProductFileClass();
		productFileClass.Categories = new List<Mertech.Category>();
		productFileClass.LabelTemplates = new List<Mertech.LabelTemplate>();
		productFileClass.Messages = new List<Mertech.Message>();
		productFileClass.ProductRates = new List<Mertech.ProductRate>();
		productFileClass.Products = new List<Mertech.Product>();
		productFileClass.Categories.Add(new Mertech.Category
		{
			IdCategory = int.Parse(instance.Prefix),
			Name = instance.Prefix
		});
		productFileClass.Categories.Add(new Mertech.Category
		{
			IdCategory = int.Parse(instance2.Prefix),
			Name = instance2.Prefix
		});
		for (int i = 0; i < goods.Count; i++)
		{
			if (_progress.Canceled)
			{
				return;
			}
			MaterialCacheRow item = goods[i];
			int value = 0;
			int code = -1;
			string name = "";
			string text = "";
			double num = 0.0;
			IEnumerable<PricesDataProvider.DataRow> source = rows.Where((PricesDataProvider.DataRow x) => x.MTCode == item.MTCode && x.QntUnit == item.Unit);
			if (row.AsNameChoose == NameInterpretationsSE.InterpretationName)
			{
				name = item.Caption;
			}
			else if (row.AsNameChoose == NameInterpretationsSE.FullName)
			{
				name = item.FullCaption;
			}
			else if (row.AsNameChoose == NameInterpretationsSE.Specification)
			{
				name = item.Description;
			}
			text = item.Description;
			bool flag3 = false;
			string errorComment = "";
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(item.Id, item.Unit);
			if (row.AsCodeExport == "1")
			{
				if (uint.TryParse(item.MTCode, out var result))
				{
					value = (int)result;
					flag3 = true;
				}
				else
				{
					flag3 = false;
					errorComment = string.Format(ArmSoft.SME.Trade.Common.Resources.CodeCanNotBeConvertedToString, item.MTCode);
					value = 0;
				}
			}
			else if (row.AsCodeExport == "2")
			{
				if (item.PLUCode.HasValue)
				{
					value = item.PLUCode.Value;
					flag3 = true;
				}
				else
				{
					flag3 = false;
					errorComment = string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Trade.Common.Resources.PLUCodeInElScale);
					value = 0;
				}
			}
			else if (row.AsCodeExport == ElScaleExportNextCodesSE.BarCode)
			{
				if (barCode != null)
				{
					if (int.TryParse(barCode.BarCode, out var result2))
					{
						value = result2;
						flag3 = true;
					}
					else
					{
						flag3 = false;
						errorComment = string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeCanNotBeConvertedToString, barCode.BarCode);
						value = 0;
					}
				}
				else
				{
					flag3 = false;
					errorComment = string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeWithUnitDoesNotExist, item.MTCode, item.Unit);
					value = 0;
				}
			}
			int num2;
			string prefix;
			int codeLength;
			int labelN;
			int barCodeN;
			if (item.Unit == instance2.Unit)
			{
				num2 = 2;
				prefix = instance2.Prefix;
				codeLength = instance2.CodeLength;
				labelN = row.PieceLabelN;
				barCodeN = row.PieceBarCodeN;
			}
			else
			{
				num2 = 1;
				prefix = instance.Prefix;
				codeLength = instance.CodeLength;
				labelN = row.WeightLabelN;
				barCodeN = row.WeightBarCodeN;
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
				num = (double)source.LastOrDefault().Price;
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
				ValidationService.ValidateString("CODE", ArmSoft.SME.Common.Resources.BarCode, code.ToString(), codeLength, isRequired: true);
			}
			catch (Exception ex)
			{
				flag3 = false;
				errorComment = ex.Message;
			}
			if (flag3)
			{
				Mertech.Product product = new Mertech.Product
				{
					PluNumber = value,
					Code = code,
					Name = name,
					Price = num,
					ProductType = ((num2 == 1) ? ScaleItemType.Weight.ToString().ToUpper() : ScaleItemType.Piece.ToString().ToUpper()),
					ShelfLifeType = "DAYS",
					BarcodePrefixType = "GROUP_CODE",
					BarcodeStructure = $"[{{\"number\":\"1\",\"structure\":\"@{{PREFIX:2}}@{{CODE:{codeLength}}}@{{QUANTITY:{10 - codeLength}}}@{{CHECKSUM:<EAN_13>}}\",\"type\":8}}]",
					Category = int.Parse(prefix)
				};
				if (!string.IsNullOrWhiteSpace(text))
				{
					productFileClass.Messages.Add(new Mertech.Message
					{
						Id = item.Id,
						Value = text
					});
					product.Message = item.Id;
				}
				productFileClass.Products.Add(product);
				ResultRows.Add(new ElScaleGoodsExportDataProvider.DataRow
				{
					CodeInScale = product.Code.ToString(),
					CodeInTrade = item.MTCode,
					Name = name,
					BarCodePrefix = int.Parse(prefix),
					PLU = product.PluNumber,
					Price = (decimal)num,
					IsWeight = (num2 == 1),
					LabelN = labelN,
					Exported = true,
					Ingredients = text
				});
				flag2 = true;
			}
			else
			{
				ResultRows.Add(new ElScaleGoodsExportDataProvider.DataRow
				{
					CodeInTrade = item.MTCode,
					Name = name,
					PLU = null,
					Price = (decimal)num,
					IsWeight = (num2 == 1),
					Ingredients = item.Description,
					LabelN = labelN,
					BarCodeN = barCodeN,
					Exported = false,
					ErrorComment = errorComment
				});
			}
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
		try
		{
			if (!mertech.Upload(productFileClass))
			{
				foreach (ElScaleGoodsExportDataProvider.DataRow resultRow in ResultRows)
				{
					if (resultRow.Exported)
					{
						resultRow.Exported = false;
						resultRow.ErrorComment = "Internal Error";
					}
				}
			}
		}
		catch
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ElScalePasswordIsIncorrect);
		}
		scalePhase3.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase3);
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
