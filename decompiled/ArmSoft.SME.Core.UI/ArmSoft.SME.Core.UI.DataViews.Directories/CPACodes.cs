using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.DPR;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Report;
using DevExpress.Data;

namespace ArmSoft.SME.Core.UI.DataViews.Directories;

[DataView(14)]
public class CPACodes : DirectoryDataView<CPAClassifiersDataProvider.DataRow, CPAClassifiersDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.CPAClassifier.Value;

	public override string HelpId
	{
		get
		{
			if (Settings.SystemType != SystemTypes.Accountant)
			{
				return "Sales_Lists_CPAClassifications";
			}
			return "Warehouse_Lists_Classifier";
		}
	}

	public CPACodes()
	{
		DataProvider = new CPAClassifiersDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("Type", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("TypeName", ColumnSortOrder.Descending);
			dvp.Set_ColumnSortOrder("Code");
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (AllowAdd && AllowEdit)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "UpdateCPACodesFromB2B", Resources.ImportFromB2B, UpdateCPACodesFromB2B, "cloud_16.svg");
		}
	}

	private void UpdateCPACodesFromB2B()
	{
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(Resources.ASerrWindow, "UpdateCPACodesFromB2B");
		Mouse.OverrideCursor = Cursors.Wait;
		UpdateCPACodesFromB2BRequest request = new UpdateCPACodesFromB2BRequest
		{
			LastSyncedDate = null
		};
		UpdateCPACodesFromB2BResponse updateCPACodesFromB2BResponse = DataProcessingRequestHelper.Execute<UpdateCPACodesFromB2BRequest, UpdateCPACodesFromB2BResponse>(DPRType.Other, "UpdateCPACodesFromB2B", request);
		Settings.CoreCacheService.CPACode.Refresh();
		List<string> exceptions = updateCPACodesFromB2BResponse.Exceptions;
		string messageBoxText;
		if (exceptions != null && exceptions.Count > 0)
		{
			foreach (string exception in updateCPACodesFromB2BResponse.Exceptions)
			{
				enumerateErrorsReportViewer.AddError(exception);
			}
			messageBoxText = string.Format(Resources.ImportCompletedWithErrors, updateCPACodesFromB2BResponse.CreatedCount + updateCPACodesFromB2BResponse.EditedCount, updateCPACodesFromB2BResponse.Exceptions.Count);
		}
		else
		{
			messageBoxText = string.Format(Resources.ImportedOrUpdated, updateCPACodesFromB2BResponse.CreatedCount, updateCPACodesFromB2BResponse.EditedCount);
		}
		if (updateCPACodesFromB2BResponse.CreatedCount > 0 || updateCPACodesFromB2BResponse.EditedCount > 0)
		{
			Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		}
		Mouse.OverrideCursor = null;
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
		MessageBox.Show(messageBoxText, Resources.ImportFromB2B, MessageBoxButton.OK);
	}
}
