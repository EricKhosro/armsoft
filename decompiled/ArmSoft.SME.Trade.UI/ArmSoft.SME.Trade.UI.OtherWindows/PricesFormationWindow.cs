using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Models.Directories;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Other;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class PricesFormationWindow : DialogWindow
{
	public class GoodsRow
	{
		public int Id { get; set; }

		public string Code { get; set; }

		public string Unit { get; set; }

		public string AltUnit { get; set; }

		public decimal NewPrice { get; set; }

		public GoodsRow()
		{
		}

		public GoodsRow(int id, string code, string unit, string altUnit, decimal basePrice = 0m)
		{
			Id = id;
			Code = code;
			Unit = unit;
			AltUnit = altUnit;
			NewPrice = basePrice;
		}
	}

	private readonly DateEditControl _dateControl;

	private readonly DropDownControl _mtGroup;

	private readonly DropDownControl _mtCode;

	private readonly DropDownControl _operType;

	private readonly DropDownControl _priceTypeControl;

	private readonly DropDownControl _basePriceTypeControl;

	private readonly ComboBoxEditExt _operationControl;

	private readonly PopupCalcEditExt _value;

	private readonly ComboBoxEditExt _prcentUnitControl;

	private readonly DropDownControl _roundDegreeControl;

	private readonly CheckEditExt _roundToUpperControl;

	private readonly CheckEditExt _forAltUnitControl;

	private readonly TextBlock _equal;

	private readonly ComboBoxEditExt _commentControl;

	private string _vat = "";

	private string _currCode = "";

	private string _basePriceVAT = "";

	private string _basePriceCurrCode = "";

	private bool _forming;

	private string _operation = "";

	private string _percentUnit = "";

	private string _group = "";

	private string _groupCaption = "";

	private DateOnly _date;

	private decimal _numValue;

	private string _roundDegree = "";

	private bool _roundToUpper = true;

	private bool _forAltUnit = true;

	private string _priceType = "";

	private string _basePriceType = "";

	private int _mtID = -1;

	private readonly List<GoodsRow> _selectedGoods = new List<GoodsRow>();

	private readonly bool _showPurchasePriceRow;

	private readonly bool _showSalePriceRow;

	private readonly bool _showRetailAvailabilityPriceRow;

	private readonly bool _showDiscountPriceRow;

	private readonly List<SelectedMaterialsRow> _docMTList;

	private string _comment;

	private readonly bool _isAveragePrice;

	private PricesDataProvider _pricesProvider;

	private List<PricesDataProvider.DataRow> _pricesRows;

	public PricesFormationWindow(List<GoodsRow> selectedGoods, string dialogName, bool showPurchasePriceRow = false, bool showSalePriceRow = false, bool showRetailAvailabilityPriceRow = false, string currCode = "", List<SelectedMaterialsRow> docMTList = null, Window ownWindow = null, bool showDiscountPriceRow = false, bool isAveragePrice = false)
	{
		if (isAveragePrice)
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.AvgPricesFormation;
		}
		else
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.PricesFormation;
		}
		_selectedGoods = selectedGoods;
		_showPurchasePriceRow = showPurchasePriceRow;
		_showSalePriceRow = showSalePriceRow;
		_showRetailAvailabilityPriceRow = showRetailAvailabilityPriceRow;
		_showDiscountPriceRow = showDiscountPriceRow;
		_basePriceCurrCode = currCode;
		_docMTList = docMTList;
		Owner = ownWindow ?? Settings.HostingEnvironment;
		Name = dialogName;
		HelpId = "PricesFormat";
		_isAveragePrice = isAveragePrice;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		AddStoreValuesButton(container);
		_dateControl = AddDateControl(container, "Date", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true);
		_mtGroup = AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups());
		_mtGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTGroup_EditValueChanged).Handler;
		_mtGroup.IsReadOnly = _isAveragePrice;
		_mtCode = AddDropDownControl("Code", ArmSoft.SME.Common.Resources.Code, new GoodsFilter());
		_mtCode.IsReadOnly = _isAveragePrice;
		if (_selectedGoods.Count > 1)
		{
			((LayoutItemWithRequired)(object)_mtGroup.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_mtCode.Parent).Visibility = Visibility.Collapsed;
		}
		else if (_selectedGoods.Count != 0)
		{
			_mtCode.Code = _selectedGoods[0].Code;
		}
		_operType = AddDropDownControl("OperType", ArmSoft.SME.Common.Resources.Operation, new SmartEnum(typeof(PriceFormationOpTypeSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
		_operType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(OperType_EditValueChanged).Handler;
		_operType.IsReadOnly = _isAveragePrice;
		container = AddHorizontalLayoutGroup();
		container.HorizontalAlignment = HorizontalAlignment.Left;
		_priceTypeControl = AddDropDownControl(container, "PriceType", ArmSoft.SME.Trade.Common.Resources.PriceType, new PriceTypes(), isRequired: true, allowAdditionButtons: false, storeValue: true);
		_priceTypeControl.ShowComment = false;
		_priceTypeControl.Width = 80.0;
		_equal = AddTextBlock(container, "=", TextBlockImage.None);
		_equal.FontSize = Settings.GetFontSize() + 4.0;
		_equal.VerticalAlignment = VerticalAlignment.Center;
		_equal.HorizontalAlignment = HorizontalAlignment.Center;
		_basePriceTypeControl = AddDropDownControl(container, "BasePriceType", " ", new PriceTypesExtended(showPrimeCostRows: true, _showPurchasePriceRow, _showSalePriceRow, _showRetailAvailabilityPriceRow, _showDiscountPriceRow), isRequired: true, allowAdditionButtons: false, storeValue: true);
		_basePriceTypeControl.ShowComment = false;
		_basePriceTypeControl.Width = 80.0;
		IEnumerable<string> itemsSource = SmartEnum<PriceFormationOpSE, string>.List.Select((PriceFormationOpSE p) => p.Value);
		_operationControl = AddComboBoxControl(container, "Operation", " ", 1, itemsSource, isRequired: false, storeValue: true);
		_operationControl.IsTextEditable = false;
		_value = AddNumericControl(container, "NumValue", "", 13, 3, isRequired: false, allowNegativeNumbers: false, useCurrencyFormat: true, storeValue: true);
		_value.IsReadOnly = _isAveragePrice;
		IEnumerable<string> itemsSource2 = SmartEnum<PriceFormationPercentUnitSE, string>.List.Select((PriceFormationPercentUnitSE p) => p.Caption);
		_prcentUnitControl = AddComboBoxControl(container, "PercentUnit", " ", 7, itemsSource2, isRequired: false, storeValue: true);
		_prcentUnitControl.IsTextEditable = false;
		_roundDegreeControl = AddDropDownControl("RoundDegree", ArmSoft.SME.Trade.Common.Resources.PriceRoundDegree, new SmartEnum(typeof(RoundDegreesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.Code, CaptionMember.Caption, null, null, orderByCode: false), isRequired: false, allowAdditionButtons: false, storeValue: true);
		_commentControl = AddComboBoxControl("Comment", ArmSoft.SME.Common.Resources.Comment, 255, TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), isRequired: false, storeValue: true);
		_roundToUpperControl = AddBooleanControl("RoundToUpper", ArmSoft.SME.Trade.Common.Resources.RoundToUpper, storeValue: true);
		_forAltUnitControl = AddBooleanControl("ForAltUnit", ArmSoft.SME.Trade.Common.Resources.FormingForAltUnit, storeValue: true);
		_prcentUnitControl.Text = PriceFormationPercentUnitSE.Percent.Caption;
		_operType.Code = PriceFormationOpTypeSE.PricesInputing.Value;
		_operationControl.Text = PriceFormationOpSE.Plus.Value;
		_dateControl.EditValue = Settings.HostingEnvironment.CurrentDate;
		_roundToUpperControl.IsChecked = true;
		WindowValidation += PricesFormationWindow_Validation;
	}

	internal void Invoke()
	{
		if (!ShowDialog().Value)
		{
			return;
		}
		List<GoodsRow> selectedGoods = _selectedGoods;
		if (!_isAveragePrice && _selectedGoods.Count == 1 && _selectedGoods[0].Code != _mtCode.Code)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item != null)
			{
				int num = 1;
				List<GoodsRow> list = new List<GoodsRow>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = new GoodsRow
				{
					Id = item.Id,
					Code = item.MTCode,
					Unit = item.Unit
				};
				selectedGoods = list;
			}
		}
		new PricesFormationWindow(selectedGoods, Name, _showPurchasePriceRow, _showSalePriceRow, _showRetailAvailabilityPriceRow, _basePriceCurrCode, _docMTList, Owner, _showDiscountPriceRow, _isAveragePrice).Invoke();
	}

	private void PricesFormationWindow_Validation(DialogWindow window, DialogWindowValidationArgs args)
	{
		try
		{
			args.isValid = PricesFormationWindowValidation();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.SetFocusWhenInvalidSave = _dateControl;
			args.isValid = false;
		}
	}

	private bool PricesFormationWindowValidation()
	{
		_date = _dateControl.EditValue.Value;
		Settings.CommonCoreHelper.CheckWorkingPeriod(_dateControl.EditValue.Value, "Date");
		string text = "";
		if (_selectedGoods.Count == 1)
		{
			text = _mtCode.Code.Trim();
			_group = _mtGroup.Code.Trim();
			_groupCaption = _mtGroup.Comment.Trim();
		}
		string text2 = _operType.Code.Trim();
		string text3 = _priceTypeControl.Comment.Trim();
		_priceType = _priceTypeControl.Code.Trim();
		_basePriceType = _basePriceTypeControl.Code.Trim();
		_operation = _operationControl.Text;
		_numValue = _value.Value;
		_percentUnit = _prcentUnitControl.Text;
		_roundDegree = _roundDegreeControl.Code.Trim();
		_roundToUpper = _roundToUpperControl.IsChecked.Value;
		_forAltUnit = _forAltUnitControl.IsChecked.Value;
		_mtID = -1;
		_forming = false;
		_basePriceVAT = "";
		_comment = _commentControl.Text;
		if (!string.IsNullOrWhiteSpace(text))
		{
			_mtID = TradeSettings.TradeCacheService.Material.GetItem(text)?.Id ?? (-1);
		}
		PriceTypeCacheRow item = TradeSettings.TradeCacheService.PriceType.GetItem(_priceType, throwExceptionNotExits: true);
		_currCode = item.CurrCode.Trim();
		if (item.VAT)
		{
			_vat = ArmSoft.SME.Common.Resources.CostWithVAT;
		}
		else
		{
			_vat = ArmSoft.SME.Common.Resources.CostWithoutVAT;
		}
		if (!(_basePriceType == "S4") && !(_basePriceType == "S5") && !(_basePriceType == "S7") && !string.IsNullOrWhiteSpace(_basePriceType))
		{
			List<PriceTypesDataProvider.DataRow> rows = new PriceTypesDataProvider(TradeSettings.CreateTradeClient()).GetRows(new PriceTypesDataProvider.Param
			{
				Code = _basePriceType,
				ShowPurchasePriceRow = true,
				ShowSalePriceRow = true,
				ShowRetailAvailabilityPriceRow = true,
				ShowPrimeCostRows = true,
				ShowDiscountPriceRow = true
			});
			if (rows.Count != 0)
			{
				_basePriceCurrCode = rows[0].CurrCode.Trim();
				if (rows[0].VAT)
				{
					_basePriceVAT = ArmSoft.SME.Common.Resources.CostWithVAT;
				}
				else
				{
					_basePriceVAT = ArmSoft.SME.Common.Resources.CostWithoutVAT;
				}
			}
		}
		if (text2 == PriceFormationOpTypeSE.AutoForming.Value)
		{
			_forming = true;
		}
		else if (text2 == PriceFormationOpTypeSE.Delete.Value)
		{
			if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ConfRecordDel, (string.IsNullOrWhiteSpace(text) ? ArmSoft.SME.Trade.Common.Resources.PriceType : (ArmSoft.SME.Common.Resources.Code + " - " + text + ", " + ArmSoft.SME.Trade.Common.Resources.PriceType.ToLower())) + " - " + text3 + ", " + ArmSoft.SME.Common.Resources.DateShort + " - " + _date.FormatDDMMYY()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				_pricesProvider = new PricesDataProvider(TradeSettings.CreateTradeClient());
				List<PricesDataProvider.DataRow> rows2 = _pricesProvider.GetRows(new PricesDataProvider.Param
				{
					StartDate = _date,
					EndDate = _date,
					PriceType = _priceType,
					MTCode = text,
					MTGroup = _group
				}, withoutAccessCheck: true);
				if (_mtID == -1)
				{
					List<string> list = new List<string>();
					int num = 0;
					EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.DeleteErrors, "DeleteSelectedErrors");
					if (_selectedGoods.Count == 1)
					{
						foreach (MaterialCacheRow item2 in TradeSettings.TradeCacheService.Material.GetItems(_group))
						{
							PricesDataProvider.DataRow pricesRow = GetPricesRow(item2.Id, _forAltUnit ? item2.AltUnit : item2.Unit, rows2);
							if (pricesRow != null)
							{
								PricesDataProvider.DataRow.PricesRowKey value = new PricesDataProvider.DataRow.PricesRowKey
								{
									MTID = pricesRow.MTID,
									PriceType = pricesRow.PriceType,
									QntUnit = pricesRow.QntUnit,
									Date = pricesRow.Date
								};
								list.Add(JsonSerializer.Serialize(value));
								num++;
							}
						}
					}
					else
					{
						foreach (GoodsRow selectedGood in _selectedGoods)
						{
							PricesDataProvider.DataRow pricesRow = GetPricesRow(selectedGood.Id, _forAltUnit ? selectedGood.AltUnit : selectedGood.Unit, rows2);
							if (pricesRow != null)
							{
								PricesDataProvider.DataRow.PricesRowKey value = new PricesDataProvider.DataRow.PricesRowKey
								{
									MTID = pricesRow.MTID,
									PriceType = pricesRow.PriceType,
									QntUnit = pricesRow.QntUnit,
									Date = pricesRow.Date
								};
								list.Add(JsonSerializer.Serialize(value));
								num++;
							}
						}
					}
					if (list.Count == 0)
					{
						ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.PriceDeletetionSuccess);
						return false;
					}
					ArmSoft.SME.Common.Models.Directories.DirectoryDeleteSelectedRequestModel request = new ArmSoft.SME.Common.Models.Directories.DirectoryDeleteSelectedRequestModel
					{
						Type = DirectoryTypeSE.Price.Value,
						Keys = list
					};
					ArmSoft.SME.Client.Models.Directories.DirectoryDeleteSelectedResponseModel directoryDeleteSelectedResponseModel = DataProcessingRequestHelper.Execute<ArmSoft.SME.Common.Models.Directories.DirectoryDeleteSelectedRequestModel, ArmSoft.SME.Client.Models.Directories.DirectoryDeleteSelectedResponseModel>(DPRType.Other, "DeleteSelectedDirectories", request);
					if (directoryDeleteSelectedResponseModel.Exceptions != null && directoryDeleteSelectedResponseModel.Exceptions.Count != 0)
					{
						foreach (ArmSoft.SME.Client.Models.Directories.DirectoryDeleteResponseModel exception in directoryDeleteSelectedResponseModel.Exceptions)
						{
							enumerateErrorsReportViewer.AddError(exception.Exception.Message);
						}
					}
					if (enumerateErrorsReportViewer.HasErrors)
					{
						enumerateErrorsReportViewer.Show();
					}
				}
				else
				{
					MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(_mtID);
					if (itemById != null)
					{
						string unit = (_forAltUnit ? itemById.AltUnit : itemById.Unit);
						PricesDataProvider.DataRow pricesRow = GetPricesRow(_mtID, unit, rows2);
						if (pricesRow != null)
						{
							PricesDataProvider.DataRow.PricesRowKey value = new PricesDataProvider.DataRow.PricesRowKey
							{
								MTID = pricesRow.MTID,
								PriceType = pricesRow.PriceType,
								QntUnit = pricesRow.QntUnit,
								Date = pricesRow.Date
							};
							Settings.DirectoryService.Delete(DirectoryTypeSE.Price.Value, JsonSerializer.Serialize(value));
						}
					}
				}
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.PriceDeletetionSuccess);
			}
			return false;
		}
		if (new ConfirmPricesWindow(SelectedMaterials(_dateControl.EditValue), text2, _basePriceType)
		{
			Properties = 
			{
				{
					"PriceType",
					(object)_priceType
				},
				{
					"PriceTypeCaption",
					(object)text3
				},
				{
					"CurrCode",
					(object)_currCode
				},
				{
					"VAT",
					(object)_vat
				},
				{
					"BasePriceType",
					(object)_basePriceType
				},
				{
					"BasePriceVAT",
					(object)_basePriceVAT
				},
				{
					"BasePriceCurrCode",
					(object)_basePriceCurrCode
				},
				{
					"Date",
					(object)_date
				},
				{
					"AutoForming",
					(object)_forming
				},
				{
					"Group",
					(object)_group
				},
				{
					"MTID",
					(object)_mtID
				},
				{
					"Comment",
					(object)_comment
				}
			}
		}.Invoke())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.PricesFormationSuccess);
			return true;
		}
		return false;
	}

	private static PricesDataProvider.DataRow GetPricesRow(int mtID, string unit, List<PricesDataProvider.DataRow> pricesList)
	{
		return pricesList.FirstOrDefault((PricesDataProvider.DataRow p) => p.MTID == mtID && p.QntUnit == unit);
	}

	private void MTGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_mtGroup.Code) && _mtGroup.DoValidate())
		{
			_mtCode.DataView = new GoodsFilter(shortMode: true, "0", _mtGroup.Code);
		}
		else
		{
			_mtCode.DataView = new GoodsFilter();
		}
		if (!_mtCode.DoValidate())
		{
			_mtCode.Code = "";
		}
	}

	private void OperType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_operType.DoValidate();
		string text = Convert.ToString(e.NewValue) ?? "";
		bool flag = false;
		if (text == PriceFormationOpTypeSE.PricesInputing.Value || text == PriceFormationOpTypeSE.Delete.Value)
		{
			flag = true;
		}
		if (flag)
		{
			_basePriceTypeControl.Visibility = Visibility.Collapsed;
			_basePriceTypeControl.IsRequired = false;
			_operationControl.Visibility = Visibility.Collapsed;
			_operationControl.IsRequired = false;
			_prcentUnitControl.Visibility = Visibility.Collapsed;
			_prcentUnitControl.IsRequired = false;
			_value.Margin = new Thickness(-25.0, 0.0, 0.0, 0.0);
		}
		else
		{
			_basePriceTypeControl.Visibility = Visibility.Visible;
			_basePriceTypeControl.IsRequired = true;
			_operationControl.Visibility = Visibility.Visible;
			_operationControl.IsRequired = true;
			_prcentUnitControl.Visibility = Visibility.Visible;
			_prcentUnitControl.IsRequired = true;
			_value.Margin = new Thickness(0.0);
		}
		if (text == PriceFormationOpTypeSE.Delete.Value)
		{
			_value.Visibility = Visibility.Collapsed;
			_equal.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_commentControl.Parent).Visibility = Visibility.Collapsed;
		}
		else
		{
			_value.Visibility = Visibility.Visible;
			_equal.Visibility = Visibility.Visible;
			((LayoutItemWithRequired)(object)_commentControl.Parent).Visibility = Visibility.Visible;
		}
		_roundDegreeControl.IsReadOnly = text == PriceFormationOpTypeSE.Delete.Value;
		_roundDegreeControl.IsEnabled = !_roundDegreeControl.IsReadOnly;
		_roundToUpperControl.IsReadOnly = text == PriceFormationOpTypeSE.Delete.Value;
		_roundToUpperControl.IsEnabled = !_roundDegreeControl.IsReadOnly;
	}

	private List<SelectedMaterialsRow> SelectedMaterials(DateOnly? providerDate)
	{
		List<SelectedMaterialsRow> list = new List<SelectedMaterialsRow>();
		string mTCode = "";
		if (_selectedGoods != null)
		{
			mTCode = string.Join(",", _selectedGoods.Select((GoodsRow i) => i.Code));
		}
		_pricesProvider = new PricesDataProvider(TradeSettings.CreateTradeClient());
		_pricesRows = (from p in _pricesProvider.GetRows(new PricesDataProvider.Param
			{
				MTCode = mTCode,
				EndDate = _date
			}, withoutAccessCheck: true)
			where p.PriceType == _priceType || p.PriceType == _basePriceType
			select p).ToList();
		if (_basePriceType == "S6" && _docMTList != null)
		{
			FillRetailAvailabilityRows(providerDate);
		}
		if ((_basePriceType == "S4" || _basePriceType == "S5" || _basePriceType == "S6" || _basePriceType == "S7") && _docMTList != null)
		{
			decimal num = 0m;
			foreach (SelectedMaterialsRow docMT in _docMTList)
			{
				switch (_basePriceType ?? "")
				{
				case "S4":
					num = docMT.PurchasePrice;
					break;
				case "S5":
					num = docMT.SalePrice;
					break;
				case "S6":
					num = docMT.RetailAvailabilityPrice;
					break;
				case "S7":
					num = docMT.DiscountPrice;
					break;
				}
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(docMT.MTID);
				if (itemById == null)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInMaterialsByMTID, docMT.MTID));
				}
				if (_forAltUnit && itemById.AltUnit != itemById.Unit && docMT.QntUnit == itemById.Unit)
				{
					MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(itemById.Id, itemById.Unit.Trim(), throwExceptionNotExits: true);
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(itemById.Id, itemById.AltUnit.Trim(), throwExceptionNotExits: true);
					num = (num * qntUnit2.Coef / qntUnit.Coef).RoundAs(2);
				}
				list.Add(ProcessRow(itemById, num));
			}
		}
		else if (!_isAveragePrice)
		{
			if (_mtID != -1)
			{
				MaterialCacheRow itemById2 = TradeSettings.TradeCacheService.Material.GetItemById(_mtID);
				if (itemById2 == null)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInMaterialsByMTID, _mtID));
				}
				if (_forAltUnit)
				{
					if (itemById2.AltUnit != itemById2.Unit)
					{
						list.Add(ProcessRow(itemById2));
					}
				}
				else
				{
					list.Add(ProcessRow(itemById2));
				}
			}
			else if (_selectedGoods.Count == 1)
			{
				foreach (MaterialCacheRow item in TradeSettings.TradeCacheService.Material.GetList((MaterialCacheRow p) => p.GroupCode == _group))
				{
					if (_forAltUnit)
					{
						if (item.AltUnit == item.Unit)
						{
							list.Add(ProcessRow(item));
						}
					}
					else
					{
						list.Add(ProcessRow(item));
					}
				}
			}
			else
			{
				foreach (GoodsRow selectedGood in _selectedGoods)
				{
					if (_forAltUnit)
					{
						if (selectedGood.AltUnit != selectedGood.Unit)
						{
							list.Add(ProcessRow(selectedGood));
						}
					}
					else
					{
						list.Add(ProcessRow(selectedGood));
					}
				}
			}
		}
		else
		{
			foreach (GoodsRow selectedGood2 in _selectedGoods)
			{
				list.Add(ProcessRow(selectedGood2));
			}
		}
		return list;
	}

	private void FillRetailAvailabilityRows(DateOnly? providerDate)
	{
		GoodsRetailAvailabilityDataProvider goodsRetailAvailabilityDataProvider = new GoodsRetailAvailabilityDataProvider(TradeSettings.CreateTradeClient());
		foreach (SelectedMaterialsRow docMT in _docMTList)
		{
			List<GoodsRetailAvailabilityDataProvider.DataRow> rows = goodsRetailAvailabilityDataProvider.GetRows(new GoodsRetailAvailabilityDataProvider.Param
			{
				RemDate = providerDate.Value,
				GoodCode = docMT.MTCode
			});
			docMT.RetailAvailabilityPrice = 0m;
			if (rows.Count > 0)
			{
				docMT.RetailAvailabilityPrice = rows.Min((GoodsRetailAvailabilityDataProvider.DataRow value) => value.Price);
			}
		}
	}

	private SelectedMaterialsRow ProcessRow(MaterialCacheRow goodsRow, decimal docRowPrice = 0m)
	{
		string unit = "";
		string unitName = "";
		string caption = "";
		decimal basePrice = 0m;
		decimal oldPrice = 0m;
		decimal newPrice = 0m;
		if (!_forming)
		{
			GetValues(goodsRow, ref caption, ref unit, ref unitName, ref oldPrice, ref newPrice);
			basePrice = 0m;
			if (_value.Value != 0m)
			{
				newPrice = Methods.RoundPrice(_value.Value, _roundDegree, _roundToUpper);
			}
		}
		else if (_basePriceType == "S1" || _basePriceType == "S2" || _basePriceType == "S3")
		{
			FormingAvaragePrimeCost(goodsRow.Id, ref caption, ref unit, ref unitName, ref basePrice, ref oldPrice, ref newPrice);
		}
		else if (_basePriceType == "S4" || _basePriceType == "S5" || _basePriceType == "S6" || _basePriceType == "S7")
		{
			basePrice = docRowPrice;
			FormingValuesFromS4S5S6S7(goodsRow.Id, ref caption, ref unit, ref unitName, ref basePrice, ref oldPrice, ref newPrice);
		}
		else
		{
			FormingValues(goodsRow.Id, ref caption, ref unit, ref unitName, ref basePrice, ref oldPrice, ref newPrice);
		}
		return new SelectedMaterialsRow
		{
			MTID = goodsRow.Id,
			MTCode = goodsRow.MTCode,
			MTName = caption,
			QntUnit = unit,
			QntUnitName = unitName,
			BasePrice = basePrice,
			OldPrice = oldPrice,
			NewPrice = ((newPrice < 0m) ? 0m : newPrice),
			Comment = _comment
		};
	}

	private SelectedMaterialsRow ProcessRow(GoodsRow goodsRow, decimal docRowPrice = 0m)
	{
		string unit = "";
		string unitName = "";
		string caption = "";
		decimal basePrice = 0m;
		decimal oldPrice = 0m;
		decimal newPrice = 0m;
		if (!_forming)
		{
			GetValues(goodsRow, ref caption, ref unit, ref unitName, ref oldPrice, ref newPrice);
			basePrice = 0m;
			if (_value.Value != 0m)
			{
				newPrice = ((!(goodsRow.NewPrice != 0m)) ? _value.Value : goodsRow.NewPrice);
				newPrice = Methods.RoundPrice(newPrice, _roundDegree, _roundToUpper);
			}
		}
		else if (_basePriceType == "S1" || _basePriceType == "S2" || _basePriceType == "S3")
		{
			FormingAvaragePrimeCost(goodsRow.Id, ref caption, ref unit, ref unitName, ref basePrice, ref oldPrice, ref newPrice);
		}
		else if (_basePriceType == "S4" || _basePriceType == "S5" || _basePriceType == "S7")
		{
			basePrice = docRowPrice;
			FormingValuesFromS4S5S6S7(goodsRow.Id, ref caption, ref unit, ref unitName, ref basePrice, ref oldPrice, ref newPrice);
		}
		else
		{
			FormingValues(goodsRow.Id, ref caption, ref unit, ref unitName, ref basePrice, ref oldPrice, ref newPrice);
		}
		return new SelectedMaterialsRow
		{
			MTID = goodsRow.Id,
			MTCode = goodsRow.Code,
			MTName = caption,
			QntUnit = unit,
			QntUnitName = unitName,
			BasePrice = basePrice,
			OldPrice = oldPrice,
			NewPrice = ((newPrice < 0m) ? 0m : newPrice),
			Comment = _comment
		};
	}

	private void GetOldAndNewPrices(string goodsCode, string unit, string altUnit, ref decimal oldPrice, ref decimal newPrice)
	{
		string localUnit = (_forAltUnit ? altUnit : unit);
		PricesDataProvider.DataRow dataRow = (from p in _pricesRows
			where p.PriceType == _priceType && p.MTCode == goodsCode && p.QntUnit == localUnit
			orderby p.Date descending
			select p).ToList().FirstOrDefault();
		if (dataRow != null)
		{
			oldPrice = dataRow.Price;
			if (dataRow.Date == _date)
			{
				newPrice = oldPrice;
			}
		}
	}

	private void GetValues(MaterialCacheRow goodsRow, ref string caption, ref string unit, ref string unitName, ref decimal oldPrice, ref decimal newPrice)
	{
		if (goodsRow != null)
		{
			caption = goodsRow.Caption;
			unit = (_forAltUnit ? goodsRow.AltUnit.Trim() : goodsRow.Unit.Trim());
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unit);
			if (item != null)
			{
				unitName = item.Brief;
				GetOldAndNewPrices(goodsRow.MTCode, goodsRow.Unit, goodsRow.AltUnit, ref oldPrice, ref newPrice);
			}
		}
	}

	private void GetValues(GoodsRow goodsRow, ref string caption, ref string unit, ref string unitName, ref decimal oldPrice, ref decimal newPrice)
	{
		if (goodsRow != null)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(goodsRow.Code, throwExceptionNotExits: true);
			caption = item.Caption;
			unit = (_forAltUnit ? item.AltUnit.Trim() : goodsRow.Unit.Trim());
			QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(unit, throwExceptionNotExits: true);
			unitName = item2.Brief;
			GetOldAndNewPrices(goodsRow.Code, goodsRow.Unit, item.AltUnit, ref oldPrice, ref newPrice);
		}
	}

	private void FormingAvaragePrimeCost(int mtID, ref string caption, ref string unit, ref string unitName, ref decimal basePrice, ref decimal oldPrice, ref decimal newPrice)
	{
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mtID);
		if (itemById == null)
		{
			return;
		}
		ExchangeRate exchangeRate = new ExchangeRate(1m, 1m);
		ExchangeRate exchangeRate2 = new ExchangeRate(1m, 1m);
		if (_currCode != "AMD")
		{
			exchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode, _date);
		}
		if (_basePriceType == "S3")
		{
			exchangeRate2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(Settings.ParametersService.MTCurr(), _date);
		}
		MTStateForAvgPrimeCostForming mTCurrentStateForAvgPrimeCostForming = TradeSettings.CreateTradeClient().CommonServices.GetMTCurrentStateForAvgPrimeCostForming(mtID, _date);
		if (mTCurrentStateForAvgPrimeCostForming != null)
		{
			if (mTCurrentStateForAvgPrimeCostForming.Quantity == 0m)
			{
				basePrice = 0m;
			}
			else
			{
				decimal num = 1m;
				if (_forAltUnit && itemById.Unit.Trim() != itemById.AltUnit.Trim())
				{
					MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtID, itemById.Unit);
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtID, itemById.AltUnit);
					if (qntUnit2 != null && qntUnit != null)
					{
						num = (qntUnit2.Coef / qntUnit.Coef).RoundAs(2);
					}
				}
				switch (_basePriceType ?? "")
				{
				case "S1":
					basePrice = (mTCurrentStateForAvgPrimeCostForming.Sum * num / mTCurrentStateForAvgPrimeCostForming.Quantity).RoundAs(2);
					break;
				case "S2":
					basePrice = (mTCurrentStateForAvgPrimeCostForming.VatSum * num / mTCurrentStateForAvgPrimeCostForming.Quantity).RoundAs(2);
					break;
				case "S3":
					basePrice = (mTCurrentStateForAvgPrimeCostForming.CurSum * num / mTCurrentStateForAvgPrimeCostForming.Quantity).RoundAs(2);
					break;
				}
			}
		}
		newPrice = Methods.RoundPrice(CalculateNewPrice(basePrice, exchangeRate, exchangeRate2), _roundDegree, _roundToUpper);
		caption = itemById.Caption.Trim();
		unit = (_forAltUnit ? itemById.AltUnit.Trim() : itemById.Unit.Trim());
		oldPrice = 0m;
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unit);
		if (item != null)
		{
			unitName = item.Brief.Trim();
			decimal newPrice2 = 0m;
			GetOldAndNewPrices(itemById.MTCode, itemById.Unit, itemById.AltUnit, ref oldPrice, ref newPrice2);
		}
	}

	private void FormingValues(int mtID, ref string caption, ref string unit, ref string unitName, ref decimal basePrice, ref decimal oldPrice, ref decimal newPrice)
	{
		ExchangeRate exchangeRate = new ExchangeRate(1m, 1m);
		ExchangeRate exchangeRate2 = new ExchangeRate(1m, 1m);
		if (_currCode != "AMD")
		{
			exchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode, _date);
		}
		if (_basePriceCurrCode != "AMD")
		{
			exchangeRate2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_basePriceCurrCode, _date);
		}
		MaterialCacheRow goodRow = TradeSettings.TradeCacheService.Material.GetItemById(mtID);
		if (goodRow == null)
		{
			return;
		}
		caption = goodRow.Caption.Trim();
		string localUnit = (_forAltUnit ? goodRow.AltUnit : goodRow.Unit);
		unit = localUnit;
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unit);
		if (item == null)
		{
			return;
		}
		unitName = item.Brief.Trim();
		List<PricesDataProvider.DataRow> source = (from p in _pricesRows
			where p.MTCode == goodRow.MTCode && p.QntUnit == localUnit
			orderby p.Date descending
			select p).ToList();
		PricesDataProvider.DataRow dataRow = source.FirstOrDefault();
		if (dataRow != null)
		{
			basePrice = ((dataRow.PriceType.Trim() == _basePriceType) ? dataRow.Price : (source.FirstOrDefault((PricesDataProvider.DataRow p) => p.PriceType.Trim() == _basePriceType)?.Price ?? 0m));
			oldPrice = ((dataRow.PriceType.Trim() == _priceType) ? dataRow.Price : (source.FirstOrDefault((PricesDataProvider.DataRow p) => p.PriceType.Trim() == _priceType)?.Price ?? 0m));
		}
		else
		{
			basePrice = 0m;
			oldPrice = 0m;
		}
		newPrice = Methods.RoundPrice(CalculateNewPrice(basePrice, exchangeRate, exchangeRate2), _roundDegree, _roundToUpper);
	}

	private decimal CalculateNewPrice(decimal basePrice, ExchangeRate exchangeRate, ExchangeRate exchangeRate1)
	{
		decimal num;
		if (_operation == PriceFormationOpSE.Plus.Value)
		{
			num = ((!(_percentUnit == "%")) ? ((basePrice + _numValue) * exchangeRate1.Rate / exchangeRate1.Base / exchangeRate.Rate * exchangeRate.Base).RoundAs(2) : ((basePrice + basePrice * _numValue / 100m) * exchangeRate1.Rate / exchangeRate1.Base / exchangeRate.Rate * exchangeRate.Base).RoundAs(2));
		}
		else if (_operation == PriceFormationOpSE.Minus.Value)
		{
			num = ((!(_percentUnit == "%")) ? ((basePrice - _numValue) * exchangeRate1.Rate / exchangeRate1.Base / exchangeRate.Rate * exchangeRate.Base).RoundAs(2) : ((basePrice - basePrice * _numValue / 100m) * exchangeRate1.Rate / exchangeRate1.Base / exchangeRate.Rate * exchangeRate.Base).RoundAs(2));
		}
		else
		{
			num = ((!(_percentUnit == "%")) ? (basePrice * _numValue * exchangeRate1.Rate / exchangeRate1.Base / exchangeRate.Rate * exchangeRate.Base).RoundAs(2) : (basePrice * basePrice * _numValue / 100m * exchangeRate1.Rate / exchangeRate1.Base / exchangeRate.Rate * exchangeRate.Base).RoundAs(2));
		}
		Methods.RoundPrice(num, _roundDegree, _roundToUpper);
		return num;
	}

	private void FormingValuesFromS4S5S6S7(int mtID, ref string caption, ref string unit, ref string unitName, ref decimal basePrice, ref decimal oldPrice, ref decimal newPrice)
	{
		ExchangeRate exchangeRate = new ExchangeRate(1m, 1m);
		ExchangeRate exchangeRate2 = new ExchangeRate(1m, 1m);
		if (_currCode != "AMD")
		{
			exchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode, _date);
		}
		if (_basePriceCurrCode != "AMD")
		{
			exchangeRate2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_basePriceCurrCode, _date);
		}
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mtID);
		if (itemById != null)
		{
			caption = itemById.Caption.Trim();
			unit = (_forAltUnit ? itemById.AltUnit.Trim() : itemById.Unit.Trim());
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unit);
			if (item != null)
			{
				unitName = item.Brief.Trim();
				decimal newPrice2 = 0m;
				GetOldAndNewPrices(itemById.MTCode, itemById.Unit, itemById.AltUnit, ref oldPrice, ref newPrice2);
				newPrice = Methods.RoundPrice(CalculateNewPrice(basePrice, exchangeRate, exchangeRate2), _roundDegree, _roundToUpper);
			}
		}
	}
}
