using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class IPAddress : DropDownView<IPAddressDataProvider.DataRow, IPAddressDataProvider.Param>
{
	private readonly string _scaleCaption;

	public override string Caption => Resources.IPAddress + " (" + _scaleCaption + ")";

	public override string Name => "IPAddress";

	public override ReloadMode ReloadMode => ReloadMode.None;

	public List<IPAndMacAddress> ParamIPEndPoints { get; set; }

	public IPAddress(string scaleCaption)
	{
		_scaleCaption = scaleCaption;
		DataProvider = new IPAddressDataProvider();
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.IPAddress = ParamIPEndPoints;
	}

	public override short ValueMemberLength()
	{
		return 3;
	}

	public override string ValueMember(int index)
	{
		return Data[index].IP + ":" + Data[index].Port;
	}

	public override string CommentMember(int index)
	{
		return Data[index].MacAddress;
	}
}
