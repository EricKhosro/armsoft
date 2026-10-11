using System;
using System.Windows;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(24)]
public class PriceDialogWindow : DirectoryDialogWindow<Price>
{
	private DateEditControl _date;

	private DropDownControl _goodCode;

	private DropDownControl _priceType;

	private DropDownControl _qntUnit;

	private DropDownControl _curr;

	private PopupCalcEdit _price;

	private DropDownCommentControl _comment;

	private string _goodCodeOld = string.Empty;

	private string _priceTypeOld = string.Empty;

	private string _unitCodeOld = string.Empty;

	private DateOnly _dateOld;

	private PriceTypeCacheRow _priceTypeCacheRow;

	protected override bool SkipReadOnlyControlsValidation => false;

	protected override bool ViewModeIsWithNext => true;

	public PriceDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public PriceDialogWindow(Price directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_date = AddDateControl(Definition.Fields["PriceDate"]);
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_goodCode = AddDropDownControl(Definition.Fields["Code"], new GoodsFilter());
		_goodCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodCode_EditValueChanged).Handler;
		_priceType = AddDropDownControl(Definition.Fields["PriceType"], new PriceTypes());
		_priceType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PriceType_EditValueChanged).Handler;
		_qntUnit = AddDropDownControl(Definition.Fields["QntUnit"], new MTQntUnits());
		_qntUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(QntUnit_EditValueChanged).Handler;
		_curr = AddDropDownControl(Definition.Fields["Curr"], new ArmSoft.SME.Core.UI.DropDowns.Currency(shortMode: true));
		_curr.IsReadOnly = true;
		_curr.IsEnabled = false;
		_price = AddNumericControl(Definition.Fields["ItemPrice"]);
		_price.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Price_EditValueChanged).Handler;
		_comment = AddDropDownCommentControl(Definition.Fields["Comment"], new StandardComments());
		base.ConstructUI();
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_price.Value = 0m;
	}

	private void PriceType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace((string)e.NewValue) || !_priceType.DoValidate())
		{
			_priceTypeCacheRow = null;
			_curr.Code = string.Empty;
		}
		else
		{
			_priceTypeCacheRow = GetPriceTypeRow(_priceType.Code);
			_curr.Code = _priceTypeCacheRow.CurrCode;
			CalculatePrice();
		}
	}

	private void QntUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace((string)e.NewValue))
		{
			CalculatePrice();
		}
	}

	private void GoodCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace((string)e.NewValue) || !_goodCode.DoValidate())
		{
			_qntUnit.Code = string.Empty;
			_qntUnit.DataView = new MTQntUnits();
			return;
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_goodCode.Code);
		if (item != null)
		{
			_qntUnit.Code = item.Unit;
			_qntUnit.DataView = new MTQntUnits(item.MTCode);
			CalculatePrice();
			return;
		}
		throw new RESTException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _goodCode.Code));
	}

	private void Price_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_priceType.Code) && _priceType.DoValidate())
		{
			_price.EditValue = Methods.RoundPrice(_price.Value, _priceTypeCacheRow.Round);
		}
	}

	private PriceTypeCacheRow GetPriceTypeRow(string priceType)
	{
		return TradeSettings.TradeCacheService.PriceType.GetItem(priceType);
	}

	private void CalculatePrice()
	{
		if (!string.IsNullOrWhiteSpace(_qntUnit.Code) && _qntUnit.DoValidate() && !string.IsNullOrWhiteSpace(_priceType.Code) && _priceType.DoValidate() && !string.IsNullOrWhiteSpace(_goodCode.Code) && _goodCode.DoValidate() && _date.DoValidate())
		{
			int id = TradeSettings.TradeCacheService.Material.GetItem(_goodCode.Code, throwExceptionNotExits: true).Id;
			_price.EditValue = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceType.Code, id, _qntUnit.Code, _date.EditValue.Value, _curr.Code)?.Price;
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_goodCodeOld = _goodCode.Code;
		_priceTypeOld = _priceType.Code;
		_unitCodeOld = _qntUnit.Code;
		_dateOld = _date.EditValue.Value;
		if (Mode == DialogWindowMode.NewMode || Mode == DialogWindowMode.NewModeWithNext)
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		}
	}

	protected override void FillDocument()
	{
		Settings.CommonCoreHelper.CheckWorkingPeriod(_date.EditValue.Value, "PriceDate");
		Directory.GoodCodeOld = (string.IsNullOrWhiteSpace(_goodCodeOld) ? _goodCode.Code : _goodCodeOld);
		Directory.PriceTypeOld = (string.IsNullOrEmpty(_priceTypeOld) ? _priceType.Code : _priceTypeOld);
		Directory.UnitCodeOld = (string.IsNullOrWhiteSpace(_unitCodeOld) ? _qntUnit.Code : _unitCodeOld);
		Directory.PriceDateOld = _dateOld;
		base.FillDocument();
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(Directory.Code);
		if (item != null)
		{
			PricesDataProvider.DataRow.PricesRowKey key = new PricesDataProvider.DataRow.PricesRowKey
			{
				MTID = item.Id,
				PriceType = Directory.PriceType,
				QntUnit = Directory.QntUnit,
				Date = Directory.PriceDate
			};
			dvp.Update(key);
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(Directory.GoodCodeOld);
			if (item2 != null)
			{
				PricesDataProvider.DataRow.PricesRowKey key2 = new PricesDataProvider.DataRow.PricesRowKey
				{
					MTID = item2.Id,
					PriceType = _priceTypeOld,
					QntUnit = _unitCodeOld,
					Date = _dateOld
				};
				dvp.Update(key2);
			}
		}
	}
}
