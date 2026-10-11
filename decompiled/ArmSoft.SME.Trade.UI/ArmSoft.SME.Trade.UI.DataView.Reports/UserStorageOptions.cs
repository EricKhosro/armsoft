using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

[DataView(40)]
public class UserStorageOptions : TradeDirectoryDataView<UserStorageOptionsDataProvider.DataRow, UserStorageOptionsDataProvider.Param>
{
	private readonly int _userId;

	private readonly string _userName;

	public override string Caption => string.Format(Resources.UserStorageAvailability, _userName);

	public override bool AllowAdd => false;

	public override bool AllowDelete => false;

	public override bool AllowCreateCopy => false;

	public override bool AllowDeleteSelected => false;

	public override short DirectoryType => DirectoryTypeSE.UserStorageOptions.Value;

	public UserStorageOptions(int userId, string userName)
	{
		_userId = userId;
		_userName = userName;
		DataProvider = new UserStorageOptionsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("UserId", value: false);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.UserId = _userId;
	}
}
