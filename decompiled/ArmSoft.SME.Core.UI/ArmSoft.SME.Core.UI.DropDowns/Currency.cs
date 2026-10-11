using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(6)]
public class Currency : DropDownView<CurrencyDataProvider.DataRow, CurrencyDataProvider.Param>
{
	private readonly bool _withOutNCC;

	private readonly string _currCode;

	public override string HelpId => "Directory_Currencies";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override string Caption
	{
		get
		{
			if (_withOutNCC)
			{
				return Resources.Currencies;
			}
			return Resources.Curres;
		}
	}

	public Currency()
		: this(shortMode: false)
	{
	}

	public Currency(bool shortMode, bool withOutNCC = false, string currCode = "")
	{
		_withOutNCC = withOutNCC;
		_currCode = currCode;
		DataProvider = new CurrencyDataProvider();
		FillColumnsFromDataProvider();
		Columns["UnitName"].Visible = !shortMode;
		Columns["CentName"].Visible = !shortMode;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.WithOutNCC = _withOutNCC;
		ParametersProvider.Currency = _currCode;
	}

	public override short ValueMemberLength()
	{
		return 3;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Curr;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
