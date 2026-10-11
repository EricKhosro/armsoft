using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Core.UI.DataViews.MetaData;

[DataView(35)]
public class DocumentExtensions : POExtensions
{
	protected override POTypeSE POTypeDef => POTypeSE.Document;

	public override string Caption => Resources.DocumentsExtensions;
}
