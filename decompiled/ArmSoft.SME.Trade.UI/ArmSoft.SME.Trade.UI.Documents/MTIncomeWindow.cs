using System;
using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(7)]
public class MTIncomeWindow : MTIncomeBaseWindow<MTIncome>
{
	public MTIncomeWindow()
	{
		_isMTIncomeDoc = true;
	}

	public MTIncomeWindow(MTIncome doc)
		: base(doc)
	{
		_isMTIncomeDoc = true;
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_actualQuantitiesCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ActualQuantitiesCheck_Click, this).Handler;
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
								if (!_isMTIncomeDoc && !string.IsNullOrWhiteSpace(_storage.Code))
								{
									StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
									if (item != null && item.Retail)
									{
										UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow2);
									}
								}
								item4.VarianceQuantity = num;
								continue;
							}
							mTIncomeSpecificationRow.Quantity = item4.ActualQuantity;
							mTIncomeSpecificationRow2.Summa = (mTIncomeSpecificationRow.Quantity * mTIncomeSpecificationRow2.DiscountPrice).RoundAs(2);
							if (!_isMTIncomeDoc && !string.IsNullOrWhiteSpace(_storage.Code))
							{
								StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
								if (item2 != null && item2.Retail)
								{
									UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow2);
								}
							}
							item4.VarianceQuantity = 0m;
							break;
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

	protected override void CreateChildDocument(short childDocType, MTIncome baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if (childDocType == 26 || childDocType == 32)
		{
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}
}
