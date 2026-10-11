using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(26)]
public class ReceivedTaxInvoiceWindow : TaxDocumentWindow<ReceivedTaxInvoice>
{
	private BarEditItemExt _fillCodes;

	private BarEditItemExt _addCodes;

	private BarEditItemExt _partnerMT;

	protected override bool AllowCreateChildDoc => true;

	public ReceivedTaxInvoiceWindow()
	{
	}

	public ReceivedTaxInvoiceWindow(ReceivedTaxInvoice doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_itemTypeNameCol.Visible = true;
		_itemCodeCol.Visible = true;
		_itemCodeCol.ReadOnly = false;
		_itemNameCol.Visible = true;
		_unitBriefCol.Visible = true;
		_unitBriefCol.ReadOnly = false;
		_cpaCodeFromDirectoryCol.Visible = true;
		_cpaCodeCol.Header = ArmSoft.SME.Common.Resources.External + " " + ArmSoft.SME.Common.Resources.ADGGAAClassification;
		_fillCodes = _grid.AddButton("fill_down_16.svg", ArmSoft.SME.Common.Resources.FillCodes, ArmSoft.SME.Common.Resources.FillCodes, 130.0);
		_addCodes = _grid.AddButton("add_16.svg", ArmSoft.SME.Common.Resources.AddNewCodes, ArmSoft.SME.Common.Resources.AddNewCodes, 150.0);
		_partnerMT = _grid.AddButton("", ArmSoft.SME.Trade.Common.Resources.PartnersMTDirectory, ArmSoft.SME.Trade.Common.Resources.PartnersMTDirectory, 200.0);
	}

	protected override void SetEvents()
	{
		base.SetEvents();
		_grid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(ReceivedTaxInvoiceGrid_ClickDropDown, this).Handler;
		_grid.BeforeAddNewDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(BeforeAddNewDropDown, this).Handler;
		_taxDocumentTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(ReceivedTaxInvoiceTableView_CellValueChanged, this).Handler;
		_taxDocumentTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(TaxDocumentTableView_FocusedRowHandleChangedEventArgs, this).Handler;
		_fillCodes.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FillCodes_Click, this).Handler;
		_addCodes.Click += new SafeDocumentEventHandler<RoutedEventArgs>(AddCodes_Click, this).Handler;
		_partnerMT.Click += new SafeDocumentEventHandler<RoutedEventArgs>(PartnerMT_Click, this).Handler;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (PO.InvoiceDate.HasValue)
		{
			_submissionDate.EditValue = PO.InvoiceDate;
		}
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	private void FillCodes_Click(object sender, RoutedEventArgs e)
	{
		if (!SearchOptionsDialog().Value)
		{
			return;
		}
		IReadOnlyList<PartnerCacheRow> partnersByTaxCode = TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode(_supplierTaxCode.Text.Trim());
		int partID = ((partnersByTaxCode.Count > 0) ? partnersByTaxCode[0].Id : (-1));
		bool isChanged = false;
		foreach (SpecificationRow receivedTaxInvoice in _receivedTaxInvoiceList)
		{
			if (string.IsNullOrWhiteSpace(receivedTaxInvoice.ItemCode))
			{
				GetActiveGoodCodeAndName(partID, receivedTaxInvoice, ref isChanged);
				if (receivedTaxInvoice.ItemType == ItemTypesSE.Goods.Value)
				{
					receivedTaxInvoice.ItemTypeName = ArmSoft.SME.Trade.Common.Resources.MaterialValue;
					isChanged = true;
				}
				else if (receivedTaxInvoice.ItemType == ItemTypesSE.Services.Value)
				{
					receivedTaxInvoice.ItemTypeName = ArmSoft.SME.Common.Resources.Service;
					isChanged = true;
				}
			}
		}
		if (isChanged)
		{
			IsChanged = true;
		}
	}

	private bool? SearchOptionsDialog()
	{
		DialogWindow dialogWindow = new DialogWindow(this)
		{
			Caption = ArmSoft.SME.Common.Resources.SearchOptions,
			Name = "SearchOptions"
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		LayoutGroup container2 = dialogWindow.AddVerticalLayoutGroup();
		dialogWindow.AddStoreValuesButton(container);
		dialogWindow.AddBooleanControl(container, "FullCaption", ArmSoft.SME.Common.Resources.SOFullName);
		dialogWindow.AddBooleanControl(container2, "Unit", ArmSoft.SME.Common.Resources.SOUnit);
		dialogWindow.AddBooleanControl(container2, "IgnoreCase", ArmSoft.SME.Common.Resources.Tar).IsChecked = _ignoreCase;
		if (dialogWindow.ShowDialog().Value)
		{
			_isFullCaption = (bool)dialogWindow["FullCaption"];
			_isUnit = (bool)dialogWindow["Unit"];
			_ignoreCase = (bool)dialogWindow["IgnoreCase"];
			return true;
		}
		return false;
	}

	private void AddCodes_Click(object sender, RoutedEventArgs e)
	{
		if (!AddNewGoodDialog())
		{
			return;
		}
		bool flag = false;
		try
		{
			bool stopAndExit = false;
			bool vat = !Settings.ParametersService.VATType().InList(VatTypesSE.CostWithoutVAT.Value, VatTypesSE.CostZeroVAT.Value);
			foreach (SpecificationRow receivedTaxInvoice in _receivedTaxInvoiceList)
			{
				if (!stopAndExit)
				{
					if (string.IsNullOrWhiteSpace(receivedTaxInvoice.ItemCode))
					{
						AddNewGoodForOneRow(receivedTaxInvoice, vat, ref stopAndExit);
						flag = true;
					}
					continue;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		if (flag)
		{
			IsChanged = true;
		}
	}

	private void PartnerMT_Click(object sender, RoutedEventArgs e)
	{
		PartnersGoods partnersGoods = new PartnersGoods();
		string text = "";
		IReadOnlyList<PartnerCacheRow> partnersByTaxCode = TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode(_supplierTaxCode.Text);
		if (partnersByTaxCode.Count > 1)
		{
			PartnersFilter partnersFilter = new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: false, isFilterByCustomers: false, partnersByTaxCode.Select((PartnerCacheRow r) => r.PartCode).ToList());
			ModalBrowser modalBrowser = new ModalBrowser(partnersFilter)
			{
				Owner = Owner,
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};
			if (modalBrowser.Show())
			{
				text = partnersFilter.ValueMember(modalBrowser.SelectedRowIndex);
			}
		}
		else
		{
			if (partnersByTaxCode.Count != 1)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.PartnerWithTINNotExist, _supplierTaxCode.Text));
				return;
			}
			text = partnersByTaxCode[0].PartCode;
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			partnersGoods.PartCode = text;
			partnersGoods.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}

	private bool AddNewGoodDialog()
	{
		DialogWindow dialogWindow = new DialogWindow(this)
		{
			Caption = ArmSoft.SME.Common.Resources.AddNewCodes
		};
		dialogWindow.AddStringControl("ItemCode", ArmSoft.SME.Common.Resources.CodeInitvalue, 20);
		dialogWindow.AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: true);
		if (dialogWindow.ShowDialog().Value)
		{
			_mtGroup = (string)dialogWindow["MTGroup"];
			_itemCode = (string)dialogWindow["ItemCode"];
			return true;
		}
		return false;
	}

	private void AddNewGoodForOneRow(SpecificationRow row, bool vat, ref bool stopAndExit)
	{
		try
		{
			QntUnitCacheRow qntUnitCacheRow = TradeSettings.TradeCacheService.QntUnit.GetList().FirstOrDefault((QntUnitCacheRow x) => (x.UnitID == row.UnitID && x.UnitID != 0) || string.Equals(x.Caption, row.UnitBrief, StringComparison.OrdinalIgnoreCase) || string.Equals(x.Brief, row.UnitBrief, StringComparison.OrdinalIgnoreCase));
			Good good = Settings.DirectoryService.Create<Good>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value);
			string text = ((qntUnitCacheRow != null) ? qntUnitCacheRow.Code : "");
			if (string.IsNullOrWhiteSpace(_itemCode))
			{
				good.Code = DocumentHelpers.GetNextMaterialCode();
			}
			else
			{
				good.Code = _itemCode;
			}
			good.Type = row.ItemType;
			good.Name = row.ItemName;
			good.FullName = good.Name;
			good.ExpenseMethod = "1";
			good.Unit = text;
			good.AltUnit = text;
			good.Group = _mtGroup;
			good.CPACode = row.CPACode;
			good.VAT = vat;
			try
			{
				Settings.DirectoryService.Store(good, new DirStoreRequest
				{
					IsImported = true,
					LogComment = ArmSoft.SME.Trade.Common.Resources.AssignMTCode
				});
			}
			catch (Exception ex)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ex.Message + ":" + Environment.NewLine + ArmSoft.SME.Common.Resources.AskContinue, ArmSoft.SME.Common.Resources.AddNewCodes, MessageBoxButton.YesNo, MessageBoxImage.Hand, MessageBoxResult.No, this) == MessageBoxResult.No)
				{
					stopAndExit = true;
				}
				return;
			}
			_itemCode = Settings.ParametersService.NextMaterial();
			row.ItemID = good.Id;
			row.ItemCode = good.Code;
			row.ItemNameFromDirectory = good.Name;
			row.UnitCodeFromDirectory = text;
			row.UnitBriefFromDirectory = TradeSettings.TradeCacheService.QntUnit.GetItem(text)?.Brief;
			row.CPACodeFromDirectory = good.CPACode;
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void ReceivedTaxInvoiceTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		SpecificationRow specificationRow = (SpecificationRow)_grid.Grid.GetRow(e.RowHandle);
		if (specificationRow == null)
		{
			return;
		}
		if (e.Column == _itemTypeNameCol)
		{
			specificationRow.ItemID = -1;
			specificationRow.ItemCode = "";
			specificationRow.ItemNameFromDirectory = "";
			specificationRow.UnitCodeFromDirectory = "";
			specificationRow.UnitBriefFromDirectory = "";
			specificationRow.CPACodeFromDirectory = "";
			ChangeItemCodeColumnAdditonalView(specificationRow.ItemType);
		}
		else
		{
			if (e.Column != _itemCodeCol)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(Convert.ToString(e.Value)))
			{
				specificationRow.ItemID = -1;
				specificationRow.ItemCode = "";
				specificationRow.ItemNameFromDirectory = "";
				specificationRow.UnitCodeFromDirectory = "";
				specificationRow.UnitBriefFromDirectory = "";
				specificationRow.CPACodeFromDirectory = "";
				return;
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(Convert.ToString(e.Value));
			if (item == null)
			{
				specificationRow.ItemID = -1;
				specificationRow.ItemNameFromDirectory = "";
				specificationRow.UnitCodeFromDirectory = "";
				specificationRow.UnitBriefFromDirectory = "";
				specificationRow.CPACodeFromDirectory = "";
			}
			else
			{
				specificationRow.ItemType = item.Type;
				specificationRow.ItemTypeName = ((item.Type == ItemTypesSE.Services.Value) ? ArmSoft.SME.Common.Resources.Service : ArmSoft.SME.Trade.Common.Resources.MaterialValue);
				specificationRow.ItemID = item.Id;
				specificationRow.ItemNameFromDirectory = item.Caption;
				specificationRow.UnitCodeFromDirectory = item.Unit;
				specificationRow.UnitBriefFromDirectory = item.UnitRow.Brief;
				specificationRow.CPACodeFromDirectory = item.CPACode;
			}
		}
	}

	private void BeforeAddNewDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		SpecificationRow row = (SpecificationRow)_grid.Grid.GetRow(e.RowHandle);
		if (e.Column == _itemCodeCol)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				ShowAddNewDocumentDialog(row, e);
			}), Array.Empty<object>());
		}
		e.RowHandle = -1;
	}

	private void ShowAddNewDocumentDialog(SpecificationRow row, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			POWindow<Good> pOWindow = ((!Settings.ParametersService.GoodAddSimpleMode()) ? ((DirectoryDocumentWindow<Good>)new GoodDocumentWindow()) : ((DirectoryDocumentWindow<Good>)new GoodDocumentSimpleWindow()));
			Good good = Settings.DirectoryService.Create<Good>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value);
			good.Name = row.ItemName;
			good.Type = row.ItemType;
			good.Code = "";
			good.FullName = good.Name;
			good.ExpenseMethod = "1";
			good.Unit = GetUnitByName(row.UnitBrief);
			good.AltUnit = good.Unit;
			good.CPACode = row.CPACode;
			pOWindow.PO = good;
			if (Convert.ToBoolean(pOWindow.ShowDialog()))
			{
				_taxDocumentTableView.Grid.SetFocusedRowCellValue(e.Column, good.Code);
			}
			_taxDocumentTableView.CommitEditing();
			_taxDocumentTableView.ShowEditor();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void TaxDocumentTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _taxDocumentTableView.FocusedRowHandle;
		SpecificationRow specificationRow = (SpecificationRow)_grid.Grid.GetRow(focusedRowHandle);
		if (specificationRow != null)
		{
			ChangeItemCodeColumnAdditonalView(specificationRow.ItemType);
		}
	}

	private static string GetUnitByName(string unitName)
	{
		return TradeSettings.TradeCacheService.QntUnit.GetList().FirstOrDefault((QntUnitCacheRow x) => x.Brief.Equals(unitName, StringComparison.CurrentCultureIgnoreCase) || x.Caption.Trim().Equals(unitName.Trim(), StringComparison.CurrentCultureIgnoreCase))?.Key ?? string.Empty;
	}

	private void ChangeItemCodeColumnAdditonalView(string typeValue)
	{
		string value = ItemTypesSE.All.Value;
		if (typeValue == ItemTypesSE.Goods.Value)
		{
			value = ItemTypesSE.Goods.Value;
			_cpaCodeCol.DataView = new ArmSoft.SME.Core.UI.DropDowns.CPACodes(CPACodesTypeSE.CNFEA.Value);
		}
		else if (typeValue == ItemTypesSE.Services.Value)
		{
			value = ItemTypesSE.Services.Value;
			_cpaCodeCol.DataView = new ArmSoft.SME.Core.UI.DropDowns.CPACodes(CPACodesTypeSE.CPA.Value);
		}
		_itemCodeCol.DataView = new GoodsFilter(shortMode: true, value);
	}

	private void GetActiveGoodCodeAndName(int partID, SpecificationRow row, ref bool isChanged)
	{
		if (partID != -1)
		{
			PartnersGoodsDataProvider.DataRow mTFromMapping = TradeSettings.CreateTradeClient().CommonServices.GetMTFromMapping(partID, row.ItemExternalCode, row.UnitBrief);
			if (mTFromMapping != null && mTFromMapping.ItemID != -1)
			{
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mTFromMapping.ItemID);
				if (itemById != null)
				{
					row.ItemType = itemById.Type;
					row.ItemID = itemById.Id;
					row.ItemCode = itemById.MTCode;
					row.ItemNameFromDirectory = itemById.Caption;
					row.UnitCodeFromDirectory = itemById.Unit;
					row.UnitBriefFromDirectory = itemById.UnitRow.Brief;
					row.CPACodeFromDirectory = itemById.CPACode;
					isChanged = true;
					return;
				}
			}
		}
		IReadOnlyList<MaterialCacheRow> list = TradeSettings.TradeCacheService.Material.GetList();
		IReadOnlyList<QntUnitCacheRow> list2 = TradeSettings.TradeCacheService.QntUnit.GetList();
		string itemName = row.ItemName.Trim();
		string unitBrief = row.UnitBrief.Trim();
		if (_isFullCaption)
		{
			if (_isUnit)
			{
				if (_ignoreCase)
				{
					var anon = (from good in list
						from materialUnit in good.QntUnits
						join qntUnit in list2 on materialUnit.QntUnit equals qntUnit.Code
						where good.FullCaption.Trim().Equals(itemName, StringComparison.CurrentCultureIgnoreCase) && qntUnit.Brief.Trim().Equals(unitBrief, StringComparison.CurrentCultureIgnoreCase)
						select new { good.Id, good.Type, good.MTCode, good.Caption, materialUnit.QntUnit, qntUnit.Brief, good.CPACode }).FirstOrDefault();
					if (anon != null)
					{
						row.ItemID = anon.Id;
						row.ItemType = anon.Type;
						row.ItemCode = anon.MTCode.Trim();
						row.ItemNameFromDirectory = anon.Caption.Trim();
						row.UnitCodeFromDirectory = anon.QntUnit;
						row.UnitBriefFromDirectory = anon.Brief.Trim();
						row.CPACodeFromDirectory = anon.CPACode;
					}
				}
				else
				{
					var anon2 = (from good in list
						from materialUnit in good.QntUnits
						join qntUnit in list2 on materialUnit.QntUnit equals qntUnit.Code
						where good.FullCaption.Trim().Equals(itemName) && qntUnit.Brief.Trim().Equals(unitBrief)
						select new { good.Id, good.Type, good.MTCode, good.Caption, materialUnit.QntUnit, qntUnit.Brief, good.CPACode }).FirstOrDefault();
					if (anon2 != null)
					{
						row.ItemID = anon2.Id;
						row.ItemType = anon2.Type;
						row.ItemCode = anon2.MTCode.Trim();
						row.ItemNameFromDirectory = anon2.Caption.Trim();
						row.UnitCodeFromDirectory = anon2.QntUnit;
						row.UnitBriefFromDirectory = anon2.Brief.Trim();
						row.CPACodeFromDirectory = anon2.CPACode;
					}
				}
			}
			else if (_ignoreCase)
			{
				MaterialCacheRow materialCacheRow = list.FirstOrDefault((MaterialCacheRow p) => p.FullCaption.Trim().Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
				if (materialCacheRow != null)
				{
					row.ItemID = materialCacheRow.Id;
					row.ItemType = materialCacheRow.Type;
					row.ItemCode = materialCacheRow.MTCode.Trim();
					row.ItemNameFromDirectory = materialCacheRow.Caption.Trim();
					row.UnitCodeFromDirectory = materialCacheRow.Unit;
					row.UnitBriefFromDirectory = materialCacheRow.UnitRow.Brief.Trim();
					row.CPACodeFromDirectory = materialCacheRow.CPACode;
				}
			}
			else
			{
				MaterialCacheRow materialCacheRow2 = list.FirstOrDefault((MaterialCacheRow p) => p.FullCaption.Trim().Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
				if (materialCacheRow2 != null)
				{
					row.ItemID = materialCacheRow2.Id;
					row.ItemType = materialCacheRow2.Type;
					row.ItemCode = materialCacheRow2.MTCode.Trim();
					row.ItemNameFromDirectory = materialCacheRow2.Caption.Trim();
					row.UnitCodeFromDirectory = materialCacheRow2.Unit;
					row.UnitBriefFromDirectory = materialCacheRow2.UnitRow.Brief.Trim();
					row.CPACodeFromDirectory = materialCacheRow2.CPACode;
				}
			}
		}
		else if (_isUnit)
		{
			if (_ignoreCase)
			{
				var anon3 = (from good in list
					from materialUnit in good.QntUnits
					join qntUnit in list2 on materialUnit.QntUnit equals qntUnit.Code
					where good.Caption.Trim().Equals(itemName, StringComparison.CurrentCultureIgnoreCase) && qntUnit.Brief.Trim().Equals(unitBrief, StringComparison.CurrentCultureIgnoreCase)
					select new { good.Id, good.Type, good.MTCode, good.Caption, materialUnit.QntUnit, qntUnit.Brief, good.CPACode }).FirstOrDefault();
				if (anon3 != null)
				{
					row.ItemID = anon3.Id;
					row.ItemType = anon3.Type;
					row.ItemCode = anon3.MTCode.Trim();
					row.ItemNameFromDirectory = anon3.Caption.Trim();
					row.UnitCodeFromDirectory = anon3.QntUnit;
					row.UnitBriefFromDirectory = anon3.Brief.Trim();
					row.CPACodeFromDirectory = anon3.CPACode;
				}
			}
			else
			{
				var anon4 = (from good in list
					from materialUnit in good.QntUnits
					join qntUnit in list2 on materialUnit.QntUnit equals qntUnit.Code
					where good.Caption.Trim().Equals(itemName) && qntUnit.Brief.Trim().Equals(unitBrief)
					select new { good.Id, good.Type, good.MTCode, good.Caption, materialUnit.QntUnit, qntUnit.Brief, good.CPACode }).FirstOrDefault();
				if (anon4 != null)
				{
					row.ItemID = anon4.Id;
					row.ItemType = anon4.Type;
					row.ItemCode = anon4.MTCode.Trim();
					row.ItemNameFromDirectory = anon4.Caption.Trim();
					row.UnitCodeFromDirectory = anon4.QntUnit;
					row.UnitBriefFromDirectory = anon4.Brief.Trim();
					row.CPACodeFromDirectory = anon4.CPACode;
				}
			}
		}
		else if (_ignoreCase)
		{
			MaterialCacheRow materialCacheRow3 = list.FirstOrDefault((MaterialCacheRow p) => p.Caption.Trim().Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
			if (materialCacheRow3 != null)
			{
				row.ItemID = materialCacheRow3.Id;
				row.ItemType = materialCacheRow3.Type;
				row.ItemCode = materialCacheRow3.MTCode.Trim();
				row.ItemNameFromDirectory = materialCacheRow3.Caption.Trim();
				row.UnitCodeFromDirectory = materialCacheRow3.Unit;
				row.UnitBriefFromDirectory = materialCacheRow3.UnitRow.Brief.Trim();
				row.CPACodeFromDirectory = materialCacheRow3.CPACode;
			}
		}
		else
		{
			MaterialCacheRow materialCacheRow4 = list.FirstOrDefault((MaterialCacheRow p) => p.Caption.Trim().Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
			if (materialCacheRow4 != null)
			{
				row.ItemID = materialCacheRow4.Id;
				row.ItemType = materialCacheRow4.Type;
				row.ItemCode = materialCacheRow4.MTCode.Trim();
				row.ItemNameFromDirectory = materialCacheRow4.Caption.Trim();
				row.UnitCodeFromDirectory = materialCacheRow4.Unit;
				row.UnitBriefFromDirectory = materialCacheRow4.UnitRow.Brief.Trim();
				row.CPACodeFromDirectory = materialCacheRow4.CPACode;
			}
		}
	}

	private void ReceivedTaxInvoiceGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		SpecificationRow specificationRow = (SpecificationRow)_grid.Grid.GetRow(e.RowHandle);
		if (e.Column == _itemCodeCol)
		{
			GoodsFilter goodsFilter = ((!(specificationRow.ItemType == ItemTypesSE.Services.Value)) ? new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value) : new GoodsFilter(shortMode: true, ItemTypesSE.Services.Value));
			ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser.Show())
			{
				e.NewValue = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(e.NewValue);
				if (item == null)
				{
					specificationRow.ItemID = -1;
					specificationRow.ItemNameFromDirectory = "";
					specificationRow.UnitCodeFromDirectory = "";
					specificationRow.UnitBriefFromDirectory = "";
					specificationRow.CPACodeFromDirectory = "";
				}
				else
				{
					specificationRow.ItemID = item.Id;
					specificationRow.ItemNameFromDirectory = item.Caption;
					specificationRow.UnitCodeFromDirectory = item.Unit;
					specificationRow.UnitBriefFromDirectory = item.UnitRow.Brief;
					specificationRow.CPACodeFromDirectory = item.CPACode;
				}
			}
		}
		else if (e.Column == _itemTypeNameCol)
		{
			SmartEnum smartEnum = new SmartEnum(typeof(GoodsTypesSE));
			ModalBrowser modalBrowser2 = new ModalBrowser(smartEnum)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (!modalBrowser2.Show())
			{
				return;
			}
			e.NewValue = Convert.ToString((smartEnum.ValueMember(modalBrowser2.SelectedRowIndex) == ItemTypesSE.Services.Value) ? ArmSoft.SME.Common.Resources.Service : ArmSoft.SME.Trade.Common.Resources.MaterialValue);
			specificationRow.ItemType = smartEnum.ValueMember(modalBrowser2.SelectedRowIndex);
			if (!string.IsNullOrWhiteSpace(specificationRow.ItemCode))
			{
				MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(specificationRow.ItemCode);
				if (item2 != null)
				{
					specificationRow.ItemID = item2.Id;
					specificationRow.ItemNameFromDirectory = item2.Caption;
					specificationRow.UnitCodeFromDirectory = item2.Unit;
					specificationRow.UnitBriefFromDirectory = item2.UnitRow.Brief;
					specificationRow.CPACodeFromDirectory = item2.CPACode;
				}
			}
		}
		else
		{
			if (e.Column != _unitBriefCol)
			{
				return;
			}
			ArmSoft.SME.Trade.UI.DropDown.MTQntUnits mTQntUnits = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(specificationRow.ItemCode);
			ModalBrowser modalBrowser3 = new ModalBrowser(mTQntUnits)
			{
				LookUpValue = specificationRow.UnitCodeFromDirectory,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser3.Show())
			{
				string key = mTQntUnits.ValueMember(modalBrowser3.SelectedRowIndex).Trim();
				QntUnitCacheRow item3 = TradeSettings.TradeCacheService.QntUnit.GetItem(key);
				if (item3 != null)
				{
					specificationRow.UnitCodeFromDirectory = item3.Code;
					e.NewValue = item3.Brief;
				}
			}
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.Items.Clear();
		foreach (SpecificationRow receivedTaxInvoice in _receivedTaxInvoiceList)
		{
			PO.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = receivedTaxInvoice.ItemType,
				ItemTypeName = receivedTaxInvoice.ItemTypeName,
				ItemCode = receivedTaxInvoice.ItemCode,
				ItemID = (TradeSettings.TradeCacheService.Material.GetItem(receivedTaxInvoice.ItemCode)?.Id ?? (-1)),
				UnitCodeFromDirectory = receivedTaxInvoice.UnitCodeFromDirectory,
				ItemExternalCode = receivedTaxInvoice.ItemExternalCode,
				ItemName = receivedTaxInvoice.ItemName,
				UnitID = receivedTaxInvoice.UnitID,
				UnitBrief = receivedTaxInvoice.UnitBrief,
				Volume = receivedTaxInvoice.Volume,
				Quantity = receivedTaxInvoice.Quantity,
				Price = receivedTaxInvoice.Price,
				Discount = receivedTaxInvoice.Discount,
				Sum = receivedTaxInvoice.Sum,
				TarePrice = receivedTaxInvoice.TarePrice,
				ExciseRate = receivedTaxInvoice.ExciseRate,
				Excise = receivedTaxInvoice.Excise,
				VAT = receivedTaxInvoice.VAT,
				SumVAT = receivedTaxInvoice.SumVAT,
				TotalSum = receivedTaxInvoice.TotalSum,
				RowNum = receivedTaxInvoice.RowNum,
				DealType = receivedTaxInvoice.DealType,
				CPACode = receivedTaxInvoice.CPACode,
				ItemNameFromDirectory = receivedTaxInvoice.ItemNameFromDirectory,
				UnitBriefFromDirectory = receivedTaxInvoice.UnitBriefFromDirectory,
				CPACodeFromDirectory = receivedTaxInvoice.CPACodeFromDirectory
			});
		}
	}

	protected override void CreateChildDocument(short childDocType, ReceivedTaxInvoice baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		try
		{
			if (Settings.DocumentService.GetChildrenDocsCount(PO.ISN) != 0 && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.AlreadyDocBased, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
			{
				return;
			}
			foreach (SpecificationRow receivedTaxInvoice in _receivedTaxInvoiceList)
			{
				if (string.IsNullOrWhiteSpace(receivedTaxInvoice.ItemCode))
				{
					if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ExistsRowsWithoutCodeMapping, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
					{
						return;
					}
					break;
				}
			}
			if (PO.IsCorrector)
			{
				if (childDocType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreateRetToSuppForCorrInv);
				}
			}
			else if (childDocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ReturnToSuppvalidate);
			}
			CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void BarButtonDocumentLinkedList_ItemClick(object sender, ItemClickEventArgs e)
	{
		base.BarButtonDocumentLinkedList_ItemClick(sender, e);
		string documentTypeName = "";
		if (SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.TryFromValue(PO.POType, out var result))
		{
			documentTypeName = result.Caption;
		}
		DocumentLinkedList documentLinkedList = new DocumentLinkedList();
		documentLinkedList.DocISN = PO.ISN;
		documentLinkedList.DocumentTypeName = documentTypeName;
		documentLinkedList.BaseDocISN = PO.ParentDocs.FirstOrDefault().Key;
		documentLinkedList.DocDate = Convert.ToString(PO.DocDate);
		documentLinkedList.DocOwner = (Settings.ParametersService.DenyAccessToOtherOwnerDocs() ? Settings.LoginService.UserName : null);
		documentLinkedList.Show(useParamValues: true, openAsChild: false);
		Keyboard.Focus(documentLinkedList.DVP);
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		_ribbonPageOther.IsVisible = true;
		_ribbonPageForOther.IsVisible = true;
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		BarSubItem barSubItem = new BarSubItem();
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref barSubItem);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
	}

	protected override void AfterStore(StoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		Settings.HostingEnvironment.FindDataViewPanel(DVPID)?.Update(PO.ISN);
		Settings.DirectoryService.RefreshDirectoryUICaches(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.PartnersGood.Value);
	}

	public override void OnScanner(string scannerData)
	{
		if (!_grid.IsReadOnly)
		{
			ReadBarcode(scannerData);
		}
	}

	private void ReadBarcode(string scannerData)
	{
		try
		{
			object row = _grid.Grid.GetRow(_taxDocumentTableView.FocusedRowHandle);
			SpecificationRow row2 = row as SpecificationRow;
			if (row2 == null)
			{
				return;
			}
			if (_taxDocumentTableView.IsEditing)
			{
				_taxDocumentTableView.CommitEditing();
			}
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				try
				{
					string barCode = scannerData;
					MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out var _, out var unitCode, out var unitBrief);
					if (materialCacheRow == null)
					{
						throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, barCode));
					}
					row2.ItemType = materialCacheRow.Type;
					row2.ItemID = materialCacheRow.Id;
					row2.ItemCode = materialCacheRow.MTCode;
					row2.ItemNameFromDirectory = materialCacheRow.Caption;
					row2.UnitCodeFromDirectory = unitCode;
					row2.UnitBriefFromDirectory = unitBrief;
					row2.CPACodeFromDirectory = materialCacheRow.CPACode;
					ChangeItemCodeColumnAdditonalView(row2.ItemType);
					_grid.View.CommitEditing();
				}
				catch (Exception ex2)
				{
					Settings.ErrorHandler(ex2);
				}
			}), Array.Empty<object>());
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
