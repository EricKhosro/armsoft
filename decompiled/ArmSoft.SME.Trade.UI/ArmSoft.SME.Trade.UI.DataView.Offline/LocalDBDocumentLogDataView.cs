using System;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataViewDataProvider.Offline;

namespace ArmSoft.SME.Trade.UI.DataView.Offline;

public class LocalDBDocumentLogDataView : DataView<LocalDBDocumentLogDataProvider.DataRow, LocalDBDocumentLogDataProvider.Param>
{
	private readonly string _caption;

	public override string Name => "LocalDocumentLog";

	public override string Caption => _caption;

	public Guid ISN => (Guid)Parameters["ISN"].Value;

	public LocalDBDocumentLogDataView(Guid isn)
	{
		DataProvider = new LocalDBDocumentLogDataProvider();
		AddParameter("ISN", Resources.DocumentISN, isn, null, includeCaptionToolTip: false);
		Document document = Settings.LocalDBDocumentService.Load(isn);
		string arg = DocumentHelpers.ConfigureDocumentDescription(ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum, SmartEnum<DocumentTypeSE, short>.FromValue(document.POType).Caption, document.DocNum, document.DocDate);
		_caption = string.Format(Resources.DocumentLogCaption, arg);
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ISN = ISN;
	}
}
