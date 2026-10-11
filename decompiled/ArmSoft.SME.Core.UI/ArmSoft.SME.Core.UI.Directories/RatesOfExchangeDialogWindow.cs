using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;

namespace ArmSoft.SME.Core.UI.Directories;

[DirectoryWindow(233)]
public class RatesOfExchangeDialogWindow : DirectoryDialogWindow<RatesOfExchange>
{
	private DateEditControl _date;

	private DropDownControl _curr;

	private PopupCalcEditExt _rate;

	private PopupCalcEditExt _exchangeBase;

	public RatesOfExchangeDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public RatesOfExchangeDialogWindow(RatesOfExchange directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_date = AddDateControl(Definition.Fields["Date"]);
		_curr = AddDropDownControl(Definition.Fields["Code"], new Currency(shortMode: true, withOutNCC: true));
		_rate = AddNumericControl(Definition.Fields["Rate"]);
		_exchangeBase = AddNumericControl(Definition.Fields["Base"]);
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _rate;
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_date.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_curr.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.NewMode)
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
			_exchangeBase.EditValue = 1m;
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		RatesOfExchangeDataProvider.DataRow.RatesOfExchangeRowKey key = new RatesOfExchangeDataProvider.DataRow.RatesOfExchangeRowKey
		{
			Currency = Directory.Code,
			Date = Directory.Date
		};
		dvp.Update(key);
	}
}
