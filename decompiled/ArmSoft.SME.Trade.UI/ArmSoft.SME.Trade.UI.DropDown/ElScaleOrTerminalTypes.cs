using System.Collections.Generic;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class ElScaleOrTerminalTypes : DropDownView<ElScaleOrTerminalTypesDataProvider.DataRow, ElScaleOrTerminalTypesDataProvider.Param>
{
	public Dictionary<string, string> _source;

	private readonly bool _isScale;

	public override string Caption
	{
		get
		{
			if (!_isScale)
			{
				return Resources.TerminalTypes;
			}
			return Resources.ScaleTypes;
		}
	}

	public ElScaleOrTerminalTypes(Dictionary<string, string> elScaleTypes, bool isScale)
	{
		_source = elScaleTypes;
		_isScale = isScale;
		DataProvider = new ElScaleOrTerminalTypesDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Source = _source;
	}

	public override short ValueMemberLength()
	{
		return 3;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
