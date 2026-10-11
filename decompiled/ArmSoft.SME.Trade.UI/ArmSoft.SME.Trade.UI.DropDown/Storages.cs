using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

[DropDown(10004)]
public class Storages : DropDownView<StoragesDataProvider.DataRow, StoragesDataProvider.Param>
{
	public bool? IsRetail { get; set; }

	public bool? CheckViewOrOperationAccess { get; set; }

	public override string Caption
	{
		get
		{
			if (IsRetail.HasValue)
			{
				return string.Format(ArmSoft.SME.Trade.Common.Resources.WholeSaleOrRetailStorages, (IsRetail == true) ? ArmSoft.SME.Trade.Common.Resources.RetailWord : ArmSoft.SME.Trade.Common.Resources.WholeSaleWord);
			}
			return ArmSoft.SME.Common.Resources.StoragesAndShops;
		}
	}

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public Storages(bool? isRetail = null, bool? checkViewOrOperationAccess = null)
	{
		IsRetail = isRetail;
		CheckViewOrOperationAccess = checkViewOrOperationAccess;
		DataProvider = new StoragesDataProvider();
		FillColumnsFromDataProvider();
		Columns["IsDefault"].Visible = false;
		Columns.AddSorting("Code");
	}

	public override short ValueMemberLength()
	{
		return 5;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.IsRetail = IsRetail;
		ParametersProvider.CheckViewOrOperationAccess = CheckViewOrOperationAccess;
	}
}
