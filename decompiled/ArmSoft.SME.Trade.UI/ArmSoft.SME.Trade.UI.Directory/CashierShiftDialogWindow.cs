using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Directories.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(33)]
public class CashierShiftDialogWindow : DirectoryDialogWindow<ArmSoft.SME.Trade.Common.Models.Directories.CashierShift>
{
	private DropDownControl _cashDesk;

	private ButtonEditExt _shiftNum;

	private PopupCalcEditExt _initRem;

	private DateEditControl _startDate;

	private TimeEditControl _startTime;

	private DateEditControl _endDate;

	private TimeEditControl _endTime;

	public bool ForClosingCashierShift { get; set; }

	protected override void ConstructUI()
	{
		base.ConstructUI();
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			_cashDesk = AddDropDownControl(Definition.Fields["CashDesk"], new Cashes());
			_cashDesk.IsReadOnly = Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode;
			_shiftNum = AddButtonEditControl(Definition.Fields["ShiftNumber"], 1);
			_shiftNum.IsReadOnly = Mode != DialogWindowMode.NewMode;
			_shiftNum.AllowMagicButton = true;
			_shiftNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
			_shiftNum.MagicClick += new SafeEventHandler<MagicClickEventArgs>(ShiftNum_MagicClick).Handler;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			_startDate = AddDateControl(container, "StartDateControl", TradeAllDirectoryDefinitions.CashierShift.Fields["StartDate"].Caption, useLongDate: false, TradeAllDirectoryDefinitions.CashierShift.Fields["StartDate"].IsRequired);
			_startDate.IsReadOnly = Mode == DialogWindowMode.ViewMode || ForClosingCashierShift;
			_startTime = AddTimeControl(container, "_startTime", "", showSeconds: false, isRequired: true);
			_startTime.IsReadOnly = Mode == DialogWindowMode.ViewMode || ForClosingCashierShift;
			if (Mode == DialogWindowMode.NewMode)
			{
				_startTime.EditValue = TradeSettings.CreateTradeClient().CommonServices.GetServerDate();
			}
			LayoutGroupExt container2 = AddHorizontalLayoutGroup();
			_endDate = AddDateControl(container2, "EndDateControl", TradeAllDirectoryDefinitions.CashierShift.Fields["EndDate"].Caption, useLongDate: false, TradeAllDirectoryDefinitions.CashierShift.Fields["EndDate"].IsRequired);
			_endDate.IsReadOnly = Mode == DialogWindowMode.ViewMode;
			_endTime = AddTimeControl(container2, "_endTime", "");
			_endTime.IsReadOnly = Mode == DialogWindowMode.ViewMode;
			_initRem = AddNumericControl(Definition.Fields["InitialRemain"]);
			_initRem.IsReadOnly = Mode == DialogWindowMode.ViewMode || ForClosingCashierShift;
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_startDate.EditValue = Directory.StartDate.ToDateOnly();
		_startTime.EditValue = Directory.StartDate.TimeOfDay.ToString();
		if (Directory.EndDate.HasValue)
		{
			_endDate.EditValue = Directory.EndDate.Value.ToDateOnly();
			_endTime.EditValue = Directory.EndDate.Value.TimeOfDay.ToString();
		}
		else if (ForClosingCashierShift)
		{
			DateTime serverDate = TradeSettings.CreateTradeClient().CommonServices.GetServerDate();
			_endDate.EditValue = serverDate.ToDateOnly();
			_endTime.EditValue = serverDate.TimeOfDay.ToString();
		}
		_cashDesk.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CashDesk_EditValueChanged).Handler;
		if (Mode == DialogWindowMode.NewMode)
		{
			CashDesk_EditValueChanged();
		}
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		Directory.StartDate = new DateTime(_startDate.EditValue.Value, TimeOnly.FromTimeSpan(_startTime.TimeSpanValue.Value));
		if (_endDate.EditValue.HasValue && _endTime.TimeSpanValue.HasValue)
		{
			Directory.EndDate = new DateTime(_endDate.EditValue.Value, TimeOnly.FromTimeSpan(_endTime.TimeSpanValue.Value));
		}
		else
		{
			Directory.EndDate = null;
		}
	}

	protected override void Validate(DialogWindowValidationArgs eventArg)
	{
		base.Validate(eventArg);
		if (!_endDate.EditValue.HasValue ^ (_endTime.EditValue.ToString() == "  :  "))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.EndDateIsRequired);
		}
	}

	public CashierShiftDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public CashierShiftDialogWindow(ArmSoft.SME.Trade.Common.Models.Directories.CashierShift directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	private void CashDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CashDesk_EditValueChanged();
	}

	private void CashDesk_EditValueChanged()
	{
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code);
		if (item != null)
		{
			_shiftNum.EditValue = item.NextCashierShiftNum;
		}
		if ((Mode == DialogWindowMode.NewMode) & _startDate.EditValue.HasValue)
		{
			SummCurrSummNccPair accRem = TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.Cash, _cashDesk.Code, -1, "AMD", _startDate.EditValue.Value);
			_initRem.EditValue = accRem.SummNcc;
		}
	}

	private void ShiftNum_MagicClick(object sender, MagicClickEventArgs e)
	{
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code);
		if (item != null)
		{
			_shiftNum.Text = item.NextCashierShiftNum;
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		CashierShiftCacheRow.RowKey key = new CashierShiftCacheRow.RowKey
		{
			CashDeskCode = Directory.CashDesk,
			ShiftNumber = Directory.ShiftNumber
		};
		dvp.Update(key);
	}
}
