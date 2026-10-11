using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Models.Directories;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Dialog;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.DPR;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(20)]
public class MTIncomeRetailWindow : MTIncomeBaseWindow<MTIncomeRetail>
{
	public class SalePriceSaveOrLoadDialog : DialogWindow
	{
		private DropDownControl _priceType;

		private DropDownControl _roundDgr;

		private CheckEditExt _roundToUpper;

		private MTIncomeRetailWindow _incomeWindow;

		private List<PricesDataProvider.DataRow> _prices;

		private string _priceTypeCurrency;

		private string _priceTypeVATType;

		private readonly bool _docIsVAT;

		private DateEditControl _date;

		private ComboBoxEdit _comment;

		private readonly DateOnly _docDate;

		private bool? _overwritePrices;

		private bool _isLoad;

		private bool OverwritePrices()
		{
			if (!_overwritePrices.HasValue)
			{
				_overwritePrices = ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.OverwritePrices, _date.EditValue), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes;
			}
			return _overwritePrices.Value;
		}

		public SalePriceSaveOrLoadDialog(MTIncomeRetailWindow incomeWindow, DateOnly docDate, bool isLoad)
		{
			_incomeWindow = incomeWindow;
			_docIsVAT = incomeWindow._vatType.Code == "1" || incomeWindow._vatType.Code == "2";
			TryFillDate();
			Name = (isLoad ? "SalePriceLoadDialog" : "SalePriceSaveDialog");
			_docDate = docDate;
			_isLoad = isLoad;
			ConstructUI();
		}

		private void TryFillDate()
		{
			if (_incomeWindow._date.EditValue.HasValue)
			{
				_date = _incomeWindow._date;
				return;
			}
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.DateWord));
		}

		private void ConstructUI()
		{
			Caption = (_isLoad ? ArmSoft.SME.Trade.Common.Resources.SetSalePrices : ArmSoft.SME.Trade.Common.Resources.SaveSalePrices);
			Owner = _incomeWindow;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			AddStoreValuesButton(container);
			_priceType = AddDropDownControl(container, "PriceListType", ArmSoft.SME.Trade.Common.Resources.PriceType, new PriceTypes(), isRequired: true, allowAdditionButtons: false, storeValue: true);
			_roundDgr = AddDropDownControl("RoundDegree", ArmSoft.SME.Common.Resources.DegreeOfRound, new SmartEnum(typeof(RoundDegreesSE)), isRequired: false, allowAdditionButtons: false, storeValue: true);
			_roundToUpper = AddBooleanControl("RoundToUpper", ArmSoft.SME.Trade.Common.Resources.RoundToUpper, storeValue: true);
			_roundToUpper.IsChecked = true;
			if (!_isLoad)
			{
				_comment = AddComboBoxControl("Comment", ArmSoft.SME.Common.Resources.Comment, 255, TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), isRequired: false, storeValue: true);
			}
			_priceType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged).Handler;
			_roundDgr.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(RoundDgr_EditValueChanged).Handler;
		}

		private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			_roundDgr.Code = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_priceType.Code);
			_incomeWindow.SalePriceType = _priceType.Code;
			_incomeWindow.RoundDgr = _roundDgr.Code;
			_incomeWindow.RoundToUpper = _roundToUpper.IsChecked.Value.ToString();
		}

		private void RoundDgr_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			_incomeWindow.RoundDgr = _roundDgr.Code;
		}

		internal void Save()
		{
			if (!ShowDialog().Value)
			{
				return;
			}
			try
			{
				PriceHelper.GetPriceListRowVATType(_priceType.Code, out _priceTypeVATType, out _priceTypeCurrency);
				InitPrices();
				List<PriceFormationRequestProperties> list = new List<PriceFormationRequestProperties>();
				foreach (MTIncomeSpecificationRow mtIncome in _incomeWindow._mtIncomeList)
				{
					if (PriceHelper.PriceAlreadyExists(mtIncome, _prices))
					{
						if (OverwritePrices())
						{
							if (!string.IsNullOrWhiteSpace(_roundDgr.Code))
							{
								mtIncome.SalePrice = Methods.RoundPrice(mtIncome.SalePrice, _roundDgr.Code, _roundToUpper.IsChecked.Value);
								mtIncome.SaleSumma = PriceHelper.CalculateSum(mtIncome.SalePrice, mtIncome.Quantity);
								mtIncome.ExtraPercent = PriceHelper.CalculateExtraPercent(mtIncome.DiscountPrice, mtIncome.SalePrice, mtIncome.VAT, _incomeWindow._vatType.Code, _incomeWindow._currCode.Code, new ExchangeRate(_incomeWindow._currExChngRate.Base, _incomeWindow._currExChngRate.Rate));
							}
							list.Add(new PriceFormationRequestProperties
							{
								MTID = mtIncome.MTID,
								MTCode = mtIncome.MTCode,
								PriceDate = _docDate,
								PriceType = _priceType.Code,
								QntUnit = mtIncome.UnitCode,
								Curr = _priceTypeCurrency,
								ItemPrice = PriceHelper.FormPriceToSave(mtIncome, _priceTypeVATType, _incomeWindow._constVAT, _incomeWindow._vatType.Code),
								Comment = _comment.Text,
								MTCodeOld = mtIncome.MTCode,
								QntUnitOld = mtIncome.UnitCode,
								PriceTypeOld = _priceType.Code,
								PriceDateOld = _docDate
							});
						}
					}
					else
					{
						if (!string.IsNullOrWhiteSpace(_roundDgr.Code))
						{
							mtIncome.SalePrice = Methods.RoundPrice(mtIncome.SalePrice, _roundDgr.Code, _roundToUpper.IsChecked.Value);
							mtIncome.SaleSumma = PriceHelper.CalculateSum(mtIncome.SalePrice, mtIncome.Quantity);
							mtIncome.ExtraPercent = PriceHelper.CalculateExtraPercent(mtIncome.DiscountPrice, mtIncome.SalePrice, mtIncome.VAT, _incomeWindow._vatType.Code, _incomeWindow._currCode.Code, new ExchangeRate(_incomeWindow._currExChngRate.Base, _incomeWindow._currExChngRate.Rate));
						}
						list.Add(new PriceFormationRequestProperties
						{
							MTID = mtIncome.MTID,
							MTCode = mtIncome.MTCode,
							PriceType = _priceType.Code,
							QntUnit = mtIncome.UnitCode,
							PriceDate = _docDate,
							Curr = _priceTypeCurrency,
							ItemPrice = PriceHelper.FormPriceToSave(mtIncome, _priceTypeVATType, _incomeWindow._constVAT, _incomeWindow._vatType.Code),
							Comment = _comment.Text
						});
					}
				}
				if (list.Count != 0)
				{
					PriceFormationRequestModel request = new PriceFormationRequestModel
					{
						Properties = list
					};
					DataProcessingRequestHelper.Execute<PriceFormationRequestModel, DirectoryStoreRequestModel<Price>>(DPRType.Other, "PriceFormation", request);
					TradeSettings.TradeCacheService.CurrentPrices.Refresh();
				}
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}

		private void InitPrices()
		{
			PricesDataProvider.Param param = new PricesDataProvider.Param
			{
				StartDate = _docDate,
				EndDate = _docDate,
				PriceType = _priceType.Code,
				MTCode = string.Join(",", _incomeWindow._mtIncomeList.Select((MTIncomeSpecificationRow x) => x.MTCode))
			};
			_prices = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param, withoutAccessCheck: true);
		}

		private decimal FormPriceToSave(MTIncomeSpecificationRow row)
		{
			if (!_docIsVAT && _priceTypeVATType != "2" && row.VAT)
			{
				decimal num = (row.SalePrice * _incomeWindow._constVAT / (100m + _incomeWindow._constVAT)).RoundAs(2);
				return row.SalePrice - num;
			}
			return row.SalePrice;
		}

		internal void Load()
		{
			if (!ShowDialog().Value)
			{
				return;
			}
			decimal num = 1m;
			decimal num2 = 1m;
			PriceHelper.GetPriceListRowVATType(_priceType.Code, out _priceTypeVATType, out _priceTypeCurrency);
			if (_priceTypeCurrency != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_priceTypeCurrency, _date.EditValue.Value);
				num = exchangeRates.Rate;
				num2 = exchangeRates.Base;
			}
			else
			{
				num = 1m;
				num2 = 1m;
			}
			if (_priceType.Code == "S6")
			{
				GoodsRetailAvailabilityDataProvider goodsRetailAvailabilityDataProvider = new GoodsRetailAvailabilityDataProvider(TradeSettings.CreateTradeClient());
				GoodsRetailAvailabilityDataProvider.Param param = new GoodsRetailAvailabilityDataProvider.Param();
				param.RemDate = _docDate;
				foreach (MTIncomeSpecificationRow mtIncome in _incomeWindow._mtIncomeList)
				{
					param.GoodCode = mtIncome.MTCode;
					List<GoodsRetailAvailabilityDataProvider.DataRow> rows = goodsRetailAvailabilityDataProvider.GetRows(param);
					if (rows.Count > 0)
					{
						mtIncome.SalePrice = Methods.RoundPrice(rows.Min((GoodsRetailAvailabilityDataProvider.DataRow value) => value.Price), _roundDgr.Code);
						mtIncome.SaleSumma = (mtIncome.SalePrice * mtIncome.Quantity).RoundAs(2);
						mtIncome.ExtraPercent = PriceHelper.CalculateExtraPercent(mtIncome.DiscountPrice, mtIncome.SalePrice, mtIncome.VAT, _incomeWindow._vatType.Code, _incomeWindow._currCode.Code, new ExchangeRate(_incomeWindow._currExChngRate.Base, _incomeWindow._currExChngRate.Rate));
					}
					else
					{
						mtIncome.SalePrice = 0m;
						mtIncome.SaleSumma = 0m;
						mtIncome.ExtraPercent = 0m;
					}
				}
			}
			else
			{
				foreach (MTIncomeSpecificationRow mtIncome2 in _incomeWindow._mtIncomeList)
				{
					PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceType.Code, mtIncome2.MTID, mtIncome2.UnitCode, _date.EditValue.Value);
					mtIncome2.SalePrice = (price.Price * num / num2).RoundAs(2);
					if (_priceTypeVATType == "1")
					{
						mtIncome2.SalePrice += (mtIncome2.SalePrice * _incomeWindow._constVAT / 100m).RoundAs(2);
					}
					mtIncome2.SalePrice = Methods.RoundPrice(mtIncome2.SalePrice, _roundDgr.Code);
					mtIncome2.SaleSumma = (mtIncome2.SalePrice * mtIncome2.Quantity).RoundAs(2);
					mtIncome2.ExtraPercent = PriceHelper.CalculateExtraPercent(mtIncome2.DiscountPrice, mtIncome2.SalePrice, mtIncome2.VAT, _incomeWindow._vatType.Code, _incomeWindow._currCode.Code, new ExchangeRate(_incomeWindow._currExChngRate.Base, _incomeWindow._currExChngRate.Rate));
				}
			}
			_incomeWindow._mtIncomeGrid.Grid.RefreshData();
			_incomeWindow.CalcSummAndSummVAT();
		}
	}

	public MTIncomeRetailWindow()
	{
		_isMTIncomeDoc = false;
	}

	public MTIncomeRetailWindow(MTIncomeRetail docWindow)
		: base(docWindow)
	{
		_isMTIncomeDoc = false;
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_fillSalePrices.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FillSalePrices_Click, this).Handler;
		_saveSalePrices.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SaveSalePrices_Click, this).Handler;
		_actualQuantitiesCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ActualQuantitiesCheck_Click, this).Handler;
	}

	protected override void AfterStore(StoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		if (sr.State != DocumentStateSE.Processed.Value)
		{
			return;
		}
		DialogValues values = Settings.DialogService.GetValues("SalePriceSaveDialog");
		if (values?.Values == null || !values.Values.TryGetValue("PriceListType".ToUpper(), out var value))
		{
			return;
		}
		string roundDegree = (values.Values.TryGetValue("RoundDegree".ToUpper(), out var value2) ? value2 : RoundDgr);
		string value3 = (values.Values.TryGetValue("RoundToUpper".ToUpper(), out var value4) ? value4 : RoundToUpper);
		string comment = (values.Values.TryGetValue("Comment".ToUpper(), out var value5) ? value5 : _comment.Text);
		PriceHelper.GetPriceListRowVATType(value, out var priceListRowVATType, out var priceListRowCurr);
		List<PriceFormationRequestProperties> list = new List<PriceFormationRequestProperties>();
		foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
		{
			if (mtIncome.SalePrice != 0m)
			{
				list.Add(new PriceFormationRequestProperties
				{
					MTID = mtIncome.MTID,
					MTCode = mtIncome.MTCode,
					PriceDate = _date.EditValue.Value,
					PriceType = value,
					QntUnit = mtIncome.UnitCode,
					Curr = priceListRowCurr,
					ItemPrice = Methods.RoundPrice(PriceHelper.FormPriceToSave(mtIncome, priceListRowVATType, _constVAT, _vatType.Code), roundDegree, value3.ToBoolean()),
					Comment = comment,
					MTCodeOld = mtIncome.MTCode,
					QntUnitOld = mtIncome.UnitCode,
					PriceTypeOld = value,
					PriceDateOld = _date.EditValue.Value
				});
			}
		}
		if (list.Count != 0)
		{
			PriceFormationRequestModel request = new PriceFormationRequestModel
			{
				Properties = list
			};
			DataProcessingRequestHelper.ExecuteThroughHandler<PriceFormationRequestModel, DirectoryStoreRequestModel<Price>>(DPRType.Other, "PriceFormation", request);
			TradeSettings.TradeCacheService.CurrentPrices.Refresh();
		}
	}

	private void SaveSalePrices_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_mtIncomeList.Count > 0)
			{
				new SalePriceSaveOrLoadDialog(this, _date.EditValue.Value, isLoad: false).Save();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FillSalePrices_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_mtIncomeList.Count > 0)
			{
				new SalePriceSaveOrLoadDialog(this, _date.EditValue.Value, isLoad: true).Load();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ActualQuantitiesCheck_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if ((!StoredInDb || IsChanged) && !SaveDocument(isAsDraft: true))
			{
				return;
			}
			ActualQuantitiesCheckWindow actualQuantitiesCheckWindow = new ActualQuantitiesCheckWindow(this, _mtIncomeList, _date.EditValue.Value, _docNum.EditValue.ToString());
			if (actualQuantitiesCheckWindow.ShowDialog().Value)
			{
				foreach (ActualQuantitiesCheck.ActualQuantitiesCheckRow item4 in actualQuantitiesCheckWindow.QCResult)
				{
					if (item4.VarianceQuantity == 0m)
					{
						continue;
					}
					if (!item4.UserAdd)
					{
						for (int i = 0; i <= _mtIncomeList.Count - 1; i++)
						{
							if (item4.MTID != _mtIncomeList[i].MTID || !(item4.UnitCode == _mtIncomeList[i].UnitCode))
							{
								continue;
							}
							MTIncomeSpecificationRow mTIncomeSpecificationRow = _mtIncomeList[i];
							decimal num = mTIncomeSpecificationRow.Quantity + item4.VarianceQuantity;
							MTIncomeSpecificationRow mTIncomeSpecificationRow2 = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRowByListIndex(i);
							TradeSettings.TradeCacheService.Material.GetItem(item4.MTCode);
							if (num < 0m)
							{
								mTIncomeSpecificationRow.Quantity = 0m;
								mTIncomeSpecificationRow2.Summa = (mTIncomeSpecificationRow.Quantity * mTIncomeSpecificationRow2.DiscountPrice).RoundAs(2);
								if (!_isMTIncomeDoc)
								{
									if (!string.IsNullOrWhiteSpace(_storage.Code))
									{
										StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
										if (item != null && item.Retail)
										{
											UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow2);
											goto IL_01cd;
										}
									}
									FillSalePriceAndSaleSumma(mTIncomeSpecificationRow2);
								}
								goto IL_01cd;
							}
							mTIncomeSpecificationRow.Quantity = item4.ActualQuantity;
							mTIncomeSpecificationRow2.Summa = (mTIncomeSpecificationRow.Quantity * mTIncomeSpecificationRow2.DiscountPrice).RoundAs(2);
							if (!_isMTIncomeDoc)
							{
								if (!string.IsNullOrWhiteSpace(_storage.Code))
								{
									StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
									if (item2 != null && item2.Retail)
									{
										UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow2);
										goto IL_025e;
									}
								}
								FillSalePriceAndSaleSumma(mTIncomeSpecificationRow2);
							}
							goto IL_025e;
							IL_025e:
							item4.VarianceQuantity = 0m;
							break;
							IL_01cd:
							item4.VarianceQuantity = num;
						}
					}
					else
					{
						MaterialCacheRow item3 = TradeSettings.TradeCacheService.Material.GetItem(item4.MTCode);
						AddNewRowOrMergeInner(item3, forFocusedRow: false, item4.VarianceQuantity, item4.UnitCode);
					}
				}
				IsChanged = true;
			}
			_mtIncomeGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void CreateChildDocument(short childDocType, MTIncomeRetail baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if (childDocType == 23)
		{
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}
}
