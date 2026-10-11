using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViewsData;

namespace ArmSoft.SME.Core.UI.DataViews;

public class OnlineLicenseInstances : DataView<OnlineLicenseInstancesDataProvider.DataRow, OnlineLicenseInstancesDataProvider.Param>
{
	private readonly string _licenseId;

	public override string Caption => Resources.ProcessesAttachedToTheLicense;

	public OnlineLicenseInstances(string licenseId)
	{
		_licenseId = licenseId;
		DataProvider = new OnlineLicenseInstancesDataProvider();
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.LicenseId = Guid.Parse(_licenseId);
	}
}
