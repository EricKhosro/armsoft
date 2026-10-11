using System;
using System.Windows;
using ArmSoft.EMarkReceiptClient.Configuration;
using ArmSoft.EMarkReceiptClient.Exceptions;
using ArmSoft.EMarkReceiptClient.Models;
using ArmSoft.EMarkReceiptClient.Services;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10127)]
public class EMarkReceiptApp : TradeDirectoryDataView<EMarkReceiptAppDataProvider.DataRow, EMarkReceiptAppDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.EMarkReceipt.Value;

	public override string Caption => ArmSoft.SME.Trade.Common.Resources.EMarkReceiptIntegration;

	public override string HelpId => "AdminTools_E-Mark_Receipt_Application";

	public override bool AllowCreateCopy => false;

	public EMarkReceiptApp()
	{
		DataProvider = new EMarkReceiptAppDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CheckStatus", ArmSoft.SME.Trade.Common.Resources.CheckStatus, CheckStatus);
	}

	private void CheckStatus()
	{
		try
		{
			EMarkReceiptAppDataProvider.DataRow dataRow = (EMarkReceiptAppDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				EMarkApiClient eMarkApiClient = new EMarkApiClient(new EMarkOptions
				{
					Datamatrix_file_id = dataRow.Id
				});
				ActivationStatusResponse activationStatusResponse = eMarkApiClient.CheckActivation();
				ArmSoft.SME.Trade.Common.Models.Directories.EMarkReceiptApp eMarkReceiptApp = Settings.DirectoryService.Load<ArmSoft.SME.Trade.Common.Models.Directories.EMarkReceiptApp>(DirectoryTypeSE.EMarkReceipt.Value, dataRow.Code, withoutAccessCheck: true);
				eMarkReceiptApp.Enabled = activationStatusResponse.activated;
				if (activationStatusResponse.activated)
				{
					RefreshTokenResponse refreshToken = eMarkApiClient.GetRefreshToken();
					eMarkReceiptApp.Token = refreshToken.token;
					eMarkReceiptApp.RefreshToken = refreshToken.refresh_token;
				}
				eMarkReceiptApp.CRN = activationStatusResponse.crn;
				Settings.DirectoryService.Store(eMarkReceiptApp, new DirStoreRequest
				{
					WithoutAccessCheck = true
				});
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Code);
				if (activationStatusResponse.activated)
				{
					eMarkApiClient.UploadMetadata(eMarkReceiptApp.DeviceId, eMarkReceiptApp.Name, EMarkReceiptDialogWindow.ImageSourceToByteArray(), "ASTrade_128.png");
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
				}
				else
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationNotSucceededTryAgain, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
				}
			}
		}
		catch (Exception ex)
		{
			if (ex is EMarkApiException ex2)
			{
				throw new ASException(ex2.Message);
			}
			Settings.ErrorHandler(ex);
		}
	}
}
