using System;
using System.Windows;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(26)]
public class DocumentLog : DataView<DocumentLogDataProvider.DataRow, DocumentLogDataProvider.Param>
{
	private string _caption;

	public override string Name => "DocumentLog";

	public override string Caption => _caption;

	public Guid ISN => (Guid)Parameters["ISN"].Value;

	public DocumentLog(Guid isn)
	{
		DataProvider = new DocumentLogDataProvider(Settings.CreateApiClient());
		AddParameter("ISN", Resources.DocumentISN, isn, null, includeCaptionToolTip: false);
		_caption = string.Format(Resources.DocumentLogCaption, Settings.HostingEnvironment.GetDocumentDescription(ISN));
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ISN = ISN;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("UserID", value: false);
		dvp.Set_ColumnVisible("State", value: false);
		if (Settings.SystemType != SystemTypes.SalesManagement)
		{
			dvp.Set_ColumnVisible("ChangeDescription", value: false);
		}
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
		}
		if (dvp.IsActive)
		{
			try
			{
				_caption = string.Format(Resources.DocumentLogCaption, Settings.HostingEnvironment.GetDocumentDescription(ISN));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Resources.ErrorWord, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
	}
}
