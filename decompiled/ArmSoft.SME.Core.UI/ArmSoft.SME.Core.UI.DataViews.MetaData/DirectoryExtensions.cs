using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Core.UI.DataViews.MetaData;

[DataView(41)]
public class DirectoryExtensions : POExtensions
{
	protected override POTypeSE POTypeDef => POTypeSE.Directory;

	public override string Caption => Resources.DirectoriesExtensions;
}
