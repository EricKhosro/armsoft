using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10007)]
public class PartnersGoods : TradeDirectoryDataView<PartnersGoodsDataProvider.DataRow, PartnersGoodsDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly PartnersGoods _dataView;

		private readonly DropDownControl _materialCode;

		private readonly DropDownControl _partnerCode;

		public DataViewDialog(PartnersGoods dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.PartnersMTAndServices;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_partnerCode = AddDropDownControl(container, "PartCode", ArmSoft.SME.Common.Resources.Supplier, new PartnersFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_partnerCode.Code = (useParamValues ? _dataView.PartCode : string.Empty);
			_materialCode = AddDropDownControl("MaterialCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, new GoodsFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_materialCode.Code = (useParamValues ? _dataView.MaterialCode : string.Empty);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.PartCode = _partnerCode.Code;
			_dataView.MaterialCode = _materialCode.Code;
		}
	}

	private class InternalCodeAssigningDialog : DialogWindow
	{
		private readonly string _itemType;

		private readonly PartnersGoods _partnerGoodsDataView;

		private DropDownControl _mtOrServiceCode;

		private DropDownControl _mtBarCode;

		public InternalCodeAssigningDialog(string itemType, PartnersGoods dataView)
		{
			_itemType = itemType;
			Caption = ArmSoft.SME.Common.Resources.AssignMTOrServCode;
			Name = "AssignMTCode";
			HelpId = dataView.HelpId;
			_partnerGoodsDataView = dataView;
		}

		public void Invoke()
		{
			if (_itemType == GoodsTypesSE.Good.Value)
			{
				_mtOrServiceCode = AddDropDownControl("MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value));
				_mtBarCode = AddDropDownControl("MTBarCode", ArmSoft.SME.Common.Resources.BarCode + " (F2)", new ArmSoft.SME.Trade.UI.DropDown.MTBarCodes(isShowQntUnitCaption: false, showBarCodeFirst: true));
				WindowValidation += AssignInternalMTCodeValidation;
			}
			else
			{
				_mtOrServiceCode = AddDropDownControl("ServiceCode", ArmSoft.SME.Common.Resources.Service, new GoodsFilter(shortMode: true, ItemTypesSE.Services.Value));
				_mtBarCode = AddDropDownControl("MTBarCode", ArmSoft.SME.Common.Resources.BarCode + " (F2)", new ArmSoft.SME.Trade.UI.DropDown.MTBarCodes(isShowQntUnitCaption: false, showBarCodeFirst: true));
				WindowValidation += AssignInternalServiceCodeValidation;
			}
			_mtBarCode.EditValueChanged += MTBarCode_EditValueChanged;
			_mtOrServiceCode.EditValueChanged += MTOrServiceCode_EditValueChanged;
			ShowDialog();
		}

		private void MTOrServiceCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(_mtOrServiceCode.Code))
			{
				_mtBarCode.IsReadOnly = false;
			}
			else
			{
				_mtBarCode.IsReadOnly = true;
			}
		}

		private void MTBarCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(_mtBarCode.Code))
			{
				_mtOrServiceCode.IsReadOnly = false;
			}
			else
			{
				_mtOrServiceCode.IsReadOnly = true;
			}
		}

		private void AssignInternalMTCodeValidation(DialogWindow dialog, DialogWindowValidationArgs args)
		{
			try
			{
				List<PartnersGoodsDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PartnersGoodsDataProvider.DataRow>();
				Good materialDoc = null;
				string text = (string)dialog["MTCode"];
				string text2 = (string)dialog["MTBarCode"];
				if (string.IsNullOrWhiteSpace(text))
				{
					if (string.IsNullOrWhiteSpace(text2))
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.CodeOrBarCodeMustBeFIlled);
					}
					materialDoc = (Good)Settings.DirectoryService.LoadByBarCode(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value, text2);
				}
				else
				{
					materialDoc = Settings.DirectoryService.Load<Good>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value, text);
				}
				if (materialDoc == null)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
				}
				string text3 = "";
				text3 = ((materialDoc.GoodBarCodes.Count <= 0) ? materialDoc.Unit : materialDoc.GoodBarCodes.FirstOrDefault((Good.GoodBarCode p) => p.BarCode == materialDoc.BarCode).UnitCode);
				foreach (PartnersGoodsDataProvider.DataRow item in selectedOrFocusedRows)
				{
					_partnerGoodsDataView.AssignInternalItemCode(item, materialDoc.Id, text3);
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
				args.SetFocusWhenInvalidSave = dialog.GetUIElement("MTCode");
			}
		}

		private void AssignInternalServiceCodeValidation(DialogWindow dialog, DialogWindowValidationArgs args)
		{
			try
			{
				List<PartnersGoodsDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PartnersGoodsDataProvider.DataRow>();
				string text = (string)dialog["ServiceCode"];
				string text2 = (string)dialog["MTBarCode"];
				Good serviceDoc = null;
				if (string.IsNullOrWhiteSpace(text))
				{
					if (string.IsNullOrWhiteSpace(text2))
					{
						using List<PartnersGoodsDataProvider.DataRow>.Enumerator enumerator = selectedOrFocusedRows.GetEnumerator();
						if (enumerator.MoveNext())
						{
							_ = enumerator.Current;
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.CodeOrBarCodeMustBeFIlled);
						}
					}
					else
					{
						serviceDoc = (Good)Settings.DirectoryService.LoadByBarCode(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value, text2);
					}
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(text2))
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.FillOnlyOneField);
					}
					serviceDoc = Settings.DirectoryService.Load<Good>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value, text);
				}
				if (serviceDoc == null)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
				}
				if (serviceDoc == null)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
				}
				string text3 = "";
				text3 = ((serviceDoc.GoodBarCodes.Count <= 0) ? serviceDoc.Unit : serviceDoc.GoodBarCodes.FirstOrDefault((Good.GoodBarCode p) => p.BarCode == serviceDoc.BarCode).UnitCode);
				foreach (PartnersGoodsDataProvider.DataRow item in selectedOrFocusedRows)
				{
					_partnerGoodsDataView.AssignInternalItemCode(item, serviceDoc.Id, text3);
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
				args.SetFocusWhenInvalidSave = dialog.GetUIElement("ServiceCode");
			}
		}

		public override void OnMagneticCardReader(string mcrData)
		{
			ReadBarCode(mcrData);
		}

		public override void OnScanner(string scannerData)
		{
			ReadBarCode(scannerData);
		}

		private void ReadBarCode(string barCode)
		{
			try
			{
				MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out var _, out var _, out var _);
				if (materialCacheRow == null || _itemType != materialCacheRow.Type)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode));
				}
				_mtBarCode.Code = barCode;
				ControlsHelper.SetFocus(_mtBarCode);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			if ((int)e.Key == 91)
			{
				ControlsHelper.SetFocus(_mtBarCode);
				e.Handled = true;
			}
			base.OnPreviewKeyDown(e);
		}
	}

	public class AddNewMTCodeDialog : DialogWindow
	{
		private readonly PartnersGoods _dataView;

		private readonly TextEditExt _mtCode;

		private readonly DropDownControl _mtGroup;

		public AddNewMTCodeDialog(PartnersGoods dataView)
		{
			_dataView = dataView;
			Name = "AddNewMTCode";
			HelpId = _dataView.HelpId;
			Caption = ArmSoft.SME.Common.Resources.AddAndMap;
			_mtCode = AddStringControl("MTCode", ArmSoft.SME.Common.Resources.CodeInitvalue, 20, isRequired: true);
			_mtGroup = AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: true);
			WindowValidation += AddNewMTCodeDialog_WindowValidation;
		}

		public void AddNewMTCodeDialog_WindowValidation(DialogWindow window, DialogWindowValidationArgs args)
		{
			if (string.IsNullOrWhiteSpace(Convert.ToString(_mtCode.EditValue)) && string.IsNullOrWhiteSpace(Convert.ToString(_mtGroup.Code)))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CodeOrBarCodeMustBeFIlled);
			}
		}
	}

	public override short DirectoryType => ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.PartnersGood.Value;

	public override string Caption
	{
		get
		{
			if (string.IsNullOrWhiteSpace(PartCode))
			{
				return ArmSoft.SME.Trade.Common.Resources.PartnersMTAndServices;
			}
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(PartCode);
			return ArmSoft.SME.Trade.Common.Resources.PartnersMTDirectory + $" ({item.PartCode} - {item.Caption})";
		}
	}

	public override string HelpId => "Purchases_Lists_ProductsAndServicesSubList_SuppliersMaterials";

	public override bool DialogSupported => true;

	public string MaterialCode
	{
		get
		{
			return (string)Parameters["MaterialCode"].Value;
		}
		set
		{
			Parameters["MaterialCode"].Value = value;
		}
	}

	public string PartCode
	{
		get
		{
			return (string)Parameters["PartCode"].Value;
		}
		set
		{
			Parameters["PartCode"].Value = value;
		}
	}

	public PartnersGoods()
	{
		DataProvider = new PartnersGoodsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		AddParameter("PartCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("MaterialCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, string.Empty);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("PartID", value: false);
		dvp.Set_ColumnVisible("PartItemUnit", value: false);
		dvp.Set_ColumnVisible("PartItemUnitID", value: false);
		dvp.Set_ColumnVisible("ItemID", value: false);
		dvp.Set_ColumnVisible("ItemUnit", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("PartCode");
			dvp.Set_ColumnSortOrder("ItemCode");
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "AssignInternalItemCode", ArmSoft.SME.Common.Resources.AssignMTOrServCode, AssignInternalItemCode);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "AddNewMTOrFindMT", ArmSoft.SME.Trade.Common.Resources.AddNewMTOrService, AddNewMTOrFindMT);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextSeparator("ContextFunctions");
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateNewMTPurchaseOrderDoc", ArmSoft.SME.Common.Resources.CreatePurchaseOrder, CreateNewMTPurchaseOrderDoc);
		}
	}

	private void AssignInternalItemCode()
	{
		PartnersGoodsDataProvider.DataRow dataRow = null;
		bool flag = false;
		if (Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
		{
			foreach (PartnersGoodsDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<PartnersGoodsDataProvider.DataRow>())
			{
				if (dataRow != null && dataRow.Type != selectedRow.Type)
				{
					throw new ASException(ArmSoft.SME.Common.Resources.RowsOfDifferentTypesAreSelected);
				}
				if (!flag && selectedRow.ItemID != -1)
				{
					flag = true;
				}
				dataRow = selectedRow;
			}
			if (flag && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ExistsRowsWithMapping, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
			{
				return;
			}
		}
		PartnersGoodsDataProvider.DataRow dataRow2 = (PartnersGoodsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow2 != null)
		{
			if (dataRow2.Type == GoodsTypesSE.Good.Value)
			{
				new InternalCodeAssigningDialog(GoodsTypesSE.Good.Value, this).Invoke();
			}
			else
			{
				new InternalCodeAssigningDialog(GoodsTypesSE.Service.Value, this).Invoke();
			}
		}
	}

	private void AddNewMTOrFindMT()
	{
		List<PartnersGoodsDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PartnersGoodsDataProvider.DataRow>();
		if (selectedRows.Any((PartnersGoodsDataProvider.DataRow p) => p.Type != selectedRows.First().Type))
		{
			throw new ASException(ArmSoft.SME.Common.Resources.RowsOfDifferentTypesAreSelected);
		}
		if (selectedRows.All((PartnersGoodsDataProvider.DataRow p) => !string.IsNullOrWhiteSpace(p.ItemCode)))
		{
			throw new ASException(ArmSoft.SME.Common.Resources.UnnecessaryMakeThisOperation);
		}
		AddNewMTCodeDialog addNewMTCodeDialog = new AddNewMTCodeDialog(this);
		if (!Convert.ToBoolean(addNewMTCodeDialog.ShowDialog()))
		{
			return;
		}
		string mtNextCode = "";
		try
		{
			foreach (PartnersGoodsDataProvider.DataRow item in selectedRows.Where((PartnersGoodsDataProvider.DataRow p) => string.IsNullOrWhiteSpace(p.ItemCode)))
			{
				AddNewMTOrFindMTForOneRow(item, ref mtNextCode, Convert.ToString(addNewMTCodeDialog["MTCode"]), Convert.ToString(addNewMTCodeDialog["MTGroup"]));
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void AddNewMTOrFindMTForOneRow(PartnersGoodsDataProvider.DataRow currentRow, ref string mtNextCode, string mtCode = "", string mtGroup = "")
	{
		TradeSettings.TradeCacheService.QntUnit.GetList();
		MaterialCacheRow materialCacheRow = null;
		IReadOnlyList<MaterialCacheRow> list = TradeSettings.TradeCacheService.Material.GetList();
		if (materialCacheRow != null)
		{
			AssignInternalItemCode(currentRow, materialCacheRow.Id, materialCacheRow.Unit);
			return;
		}
		string unitCode = TradeSettings.TradeCacheService.QntUnit.GetList().FirstOrDefault((QntUnitCacheRow x) => (x.UnitID == currentRow.PartItemUnitID && x.UnitID != 0) || x.Caption == currentRow.PartItemUnitName.Trim() || x.Brief == currentRow.PartItemUnitName.Trim())?.Code ?? "";
		Good good = null;
		int num = -1;
		if (currentRow.PartItemCaption == null || string.IsNullOrWhiteSpace(currentRow.PartItemCaption))
		{
			return;
		}
		try
		{
			List<int> list2 = (from p in list
				where p.Caption.Trim().Equals(currentRow.PartItemCaption, StringComparison.CurrentCultureIgnoreCase) && p.Unit.Trim().Equals(unitCode, StringComparison.CurrentCultureIgnoreCase)
				select p.Id).ToList();
			if (list2.Count != 0)
			{
				num = list2[0];
				if (num != -1)
				{
					good = Settings.DirectoryService.Load<Good>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods, num, withoutAccessCheck: false, throwExceptionIfDoesNotExist: false);
					if (good.Id != -1)
					{
						AssignInternalItemCode(currentRow, good.Id, good.Unit);
					}
					return;
				}
				good = Settings.DirectoryService.Create<Good>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods);
				if (string.IsNullOrWhiteSpace(mtNextCode))
				{
					if (string.IsNullOrWhiteSpace(mtCode))
					{
						good.Code = DocumentHelpers.GetNextMaterialCode();
					}
					else
					{
						good.Code = mtCode;
					}
				}
				else
				{
					good.Code = mtNextCode;
				}
				good.Name = currentRow.PartItemCaption;
				good.FullName = good.Name;
				good.ExpenseMethod = "1";
				good.Unit = unitCode;
				good.AltUnit = unitCode;
				good.Type = currentRow.Type;
				if (!string.IsNullOrWhiteSpace(mtGroup))
				{
					good.Group = mtGroup;
				}
				Settings.DirectoryService.Store(good, new DirStoreRequest
				{
					IsImported = true,
					LogComment = "Mapping"
				});
				Settings.ParametersService.NextMaterial_set(StringExtensions.NextDocNum(good.Code, 20));
				mtNextCode = Settings.ParametersService.NextMaterial();
				AssignInternalItemCode(currentRow, good.Id, good.Unit);
				return;
			}
			good = Settings.DirectoryService.Create<Good>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods);
			if (string.IsNullOrWhiteSpace(mtNextCode))
			{
				if (string.IsNullOrWhiteSpace(mtCode))
				{
					good.Code = DocumentHelpers.GetNextMaterialCode();
				}
				else
				{
					good.Code = mtCode;
				}
			}
			else
			{
				good.Code = mtNextCode;
			}
			good.Name = currentRow.PartItemCaption;
			good.FullName = good.Name;
			good.ExpenseMethod = "1";
			good.Unit = unitCode;
			good.AltUnit = unitCode;
			good.Type = currentRow.Type;
			if (!string.IsNullOrWhiteSpace(mtGroup))
			{
				good.Group = mtGroup;
			}
			good = Settings.DirectoryService.StoreAndLoad(good, new DirStoreRequest
			{
				IsImported = true,
				LogComment = ArmSoft.SME.Trade.Common.Resources.AssignMTCode
			});
			Settings.ParametersService.NextMaterial_set(StringExtensions.NextDocNum(good.Code, 20));
			mtNextCode = Settings.ParametersService.NextMaterial();
			AssignInternalItemCode(currentRow, good.Id, good.Unit);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.MaterialCode = MaterialCode;
		ParametersProvider.PartCode = PartCode;
	}

	private void AssignInternalItemCode(PartnersGoodsDataProvider.DataRow row, int mtId, string mtUnit)
	{
		try
		{
			object key = GetKey(row);
			PartnersGood partnersGood = Settings.DirectoryService.Load<PartnersGood>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.PartnersGood.Value, JsonSerializer.Serialize(key));
			partnersGood.ItemID = mtId;
			partnersGood.ItemCode = TradeSettings.TradeCacheService.Material.GetItemById(mtId).MTCode;
			partnersGood.ItemUnit = mtUnit;
			partnersGood.PartCodeOld = partnersGood.Code;
			partnersGood.PartItemCodeOld = partnersGood.PartItemCode;
			partnersGood.PartItemUnitNameOld = partnersGood.PartItemUnitName;
			partnersGood.PartItemUnitOld = partnersGood.PartItemUnit;
			Settings.DirectoryService.Store(partnersGood, new DirStoreRequest());
			Settings.HostingEnvironment.CurrentViewPanel.Update(key);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewMTPurchaseOrderDoc()
	{
		CreateAndSendPOByEmail();
		if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OpenPOJournal, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			PurchaseOrdersDocumentsJournalDataView purchaseOrdersDocumentsJournalDataView = new PurchaseOrdersDocumentsJournalDataView();
			purchaseOrdersDocumentsJournalDataView.StartDate = Settings.HostingEnvironment.CurrentDate;
			purchaseOrdersDocumentsJournalDataView.EndDate = Settings.HostingEnvironment.CurrentDate;
			purchaseOrdersDocumentsJournalDataView.Show(useParamValues: true, openAsChild: false);
		}
	}

	private static void CreateAndSendPOByEmail()
	{
		Dictionary<int, MTPurchaseOrder> dictionary = new Dictionary<int, MTPurchaseOrder>();
		int num = -1;
		int num2 = -1;
		string text = Settings.ParametersService.MTPurchaseOrderNumber();
		bool flag = false;
		foreach (PartnersGoodsDataProvider.DataRow row in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PartnersGoodsDataProvider.DataRow>())
		{
			IReadOnlyList<PartnerGoodCacheRow> list = TradeSettings.TradeCacheService.PartnerGood.GetList((PartnerGoodCacheRow p) => p.MTID == row.ItemID && row.Type == ItemTypesSE.Goods.Value);
			if (list.Count > 0)
			{
				num2 = list[0].PartID;
			}
			MTPurchaseOrder mTPurchaseOrder;
			MTPurchaseOrder.MTPurchaseOrderSpecificationRow currentMTPurchaseOrderListRow;
			if (dictionary.TryGetValue(num2, out var value))
			{
				mTPurchaseOrder = value;
				currentMTPurchaseOrderListRow = GetCurrentMTPurchaseOrderListRow(row, num);
				if (currentMTPurchaseOrderListRow == null)
				{
					continue;
				}
				bool flag2 = false;
				foreach (MTPurchaseOrder.MTPurchaseOrderSpecificationRow mT in mTPurchaseOrder.MTList)
				{
					if (mT.MTID == currentMTPurchaseOrderListRow.MTID)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					mTPurchaseOrder.MTList.Add(currentMTPurchaseOrderListRow);
					num = (mTPurchaseOrder.MaxRowId = num + 1);
				}
				else if (!flag)
				{
					flag = true;
					if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTCodeRepeatedInList, currentMTPurchaseOrderListRow.MTCode), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
					{
						return;
					}
				}
				continue;
			}
			mTPurchaseOrder = Settings.DocumentService.Create<MTPurchaseOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value);
			if (num2 == -1)
			{
				mTPurchaseOrder.POStates = POStatesSE.EmptySupplier.Value;
			}
			num = 0;
			mTPurchaseOrder.DocDate = Settings.HostingEnvironment.CurrentDate;
			mTPurchaseOrder.DocNum = text;
			text = StringExtensions.NextDocNum(mTPurchaseOrder.DocNum, 12);
			mTPurchaseOrder.PartnerID = num2;
			PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(num2);
			if (itemById != null)
			{
				mTPurchaseOrder.PartnerContractGroup.PartnerCode = itemById.PartCode;
				mTPurchaseOrder.PartnerName = itemById.Caption;
				mTPurchaseOrder.SupplierEMail = itemById.Email;
			}
			currentMTPurchaseOrderListRow = GetCurrentMTPurchaseOrderListRow(row, num);
			if (currentMTPurchaseOrderListRow != null)
			{
				mTPurchaseOrder.MTList.Add(currentMTPurchaseOrderListRow);
				num = (mTPurchaseOrder.MaxRowId = num + 1);
			}
			dictionary.Add(mTPurchaseOrder.PartnerID, mTPurchaseOrder);
			mTPurchaseOrder.Owner = Settings.LoginService.UserName;
		}
		Settings.ParametersService.MTPurchaseOrderNumber_set(text);
		foreach (KeyValuePair<int, MTPurchaseOrder> item in dictionary)
		{
			MTPurchaseOrder mTPurchaseOrder = item.Value;
			if (mTPurchaseOrder.MTList.Count > 0)
			{
				Settings.DocumentService.Store(mTPurchaseOrder, new StoreRequest
				{
					LogComment = ArmSoft.SME.Common.Resources.AutoCreatePurchase
				});
			}
		}
	}

	private static MTPurchaseOrder.MTPurchaseOrderSpecificationRow GetCurrentMTPurchaseOrderListRow(PartnersGoodsDataProvider.DataRow sourceRow, int rowId)
	{
		PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(sourceRow.PartID);
		if (sourceRow.ItemID == -1)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ForPartMTNotExistMap, itemById.PartCode, sourceRow.ItemCode));
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.ItemCode, throwExceptionNotExits: true);
		MTPurchaseOrder.MTPurchaseOrderSpecificationRow mTPurchaseOrderSpecificationRow = new MTPurchaseOrder.MTPurchaseOrderSpecificationRow
		{
			RowID = rowId,
			MTID = sourceRow.ItemID,
			MTCode = item.MTCode,
			MTName = item.Caption,
			UnitCode = sourceRow.ItemUnit,
			UnitBrief = item.UnitRow.Brief
		};
		MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(sourceRow.ItemCode, sourceRow.ItemUnit);
		if (qntUnit != null)
		{
			if (qntUnit.Coef != 1m && qntUnit.Coef != 0m)
			{
				mTPurchaseOrderSpecificationRow.Quantity = (item.MaxQty / qntUnit.Coef).RoundAs(3);
			}
			else
			{
				mTPurchaseOrderSpecificationRow.Quantity = item.MaxQty;
			}
		}
		return mTPurchaseOrderSpecificationRow;
	}

	public override void Add()
	{
		PartnersGoodsDialogWindow partnersGoodsDialogWindow = new PartnersGoodsDialogWindow(DialogWindowMode.NewModeWithNext);
		partnersGoodsDialogWindow.DVPId = ParentViewPanel.Id;
		partnersGoodsDialogWindow.IsFilteredPartner = !string.IsNullOrWhiteSpace(PartCode);
		partnersGoodsDialogWindow.Directory.Code = PartCode;
		partnersGoodsDialogWindow.Directory.PartTIN = (string.IsNullOrWhiteSpace(PartCode) ? "" : (((PartnersGoodsDataProvider.DataRow)ParentViewPanel.FocusedRow())?.PartTIN ?? ""));
		partnersGoodsDialogWindow.ShowDialog();
	}

	public override void Edit()
	{
		object key = GetKey(ParentViewPanel.FocusedRow());
		PartnersGoodsDialogWindow partnersGoodsDialogWindow = new PartnersGoodsDialogWindow(Settings.DirectoryService.Load<PartnersGood>(DirectoryType, JsonSerializer.Serialize(key)), DialogWindowMode.EditMode);
		partnersGoodsDialogWindow.DVPId = ParentViewPanel.Id;
		partnersGoodsDialogWindow.IsFilteredPartner = !string.IsNullOrWhiteSpace(PartCode);
		partnersGoodsDialogWindow.ShowDialog();
	}

	public override void CreateCopy()
	{
		object key = GetKey(ParentViewPanel.FocusedRow());
		PartnersGoodsDialogWindow partnersGoodsDialogWindow = new PartnersGoodsDialogWindow(Settings.DirectoryService.CreateCopy<PartnersGood>(DirectoryType, JsonSerializer.Serialize(key)), DialogWindowMode.CreateCopyModeWithNext);
		partnersGoodsDialogWindow.DVPId = ParentViewPanel.Id;
		partnersGoodsDialogWindow.IsFilteredPartner = !string.IsNullOrWhiteSpace(PartCode);
		partnersGoodsDialogWindow.ShowDialog();
	}
}
