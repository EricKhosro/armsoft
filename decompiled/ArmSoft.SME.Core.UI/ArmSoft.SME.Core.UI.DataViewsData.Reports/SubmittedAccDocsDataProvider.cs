using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.SME.Core.UI.DataViewsData.Reports;

public class SubmittedAccDocsDataProvider : TaxBaseDataProvider
{
	public override string Name => "SubmittedAccDocs";

	public override string Caption => DataProviderTypeSE.TaxServiceAccDocsSubmitted.Caption;

	public override TaxDocType TaxDocType => TaxDocType.AccDoc;

	public override Status TaxDocStatus => Status.Issued;

	public override ReceivedOrSubmittedType ReceivedOrSubmittedType => ReceivedOrSubmittedType.Submitted;

	public override IEnumerable<TaxParameter> GetParameters()
	{
		return new _003C_003Ez__ReadOnlyArray<TaxParameter>(new TaxParameter[9]
		{
			new TaxParameter("SubmitStartDate", Resources.SubmissionDatePeriodBegin, SupportedType.DateTime, ArmSoft.SME.Core.UI.Settings.HostingEnvironment.DateBegin, "dd/MM/yy"),
			new TaxParameter("SubmitEndDate", Resources.SubmissionDatePeriodEnd, SupportedType.DateTime, ArmSoft.SME.Core.UI.Settings.HostingEnvironment.DateEnd, "dd/MM/yy"),
			new TaxParameter("SupplyStartDate", Resources.SupplyDatePeriodBegin, SupportedType.DateTime, ArmSoft.SME.Core.UI.Settings.HostingEnvironment.DateBegin, "dd/MM/yy"),
			new TaxParameter("SupplyEndDate", Resources.SupplyDatePeriodEnd, SupportedType.DateTime, ArmSoft.SME.Core.UI.Settings.HostingEnvironment.DateEnd, "dd/MM/yy"),
			new TaxParameter("BuyerTin", Resources.SupplierTaxCode, SupportedType.String, ""),
			new TaxParameter("BuyerName", Resources.SupplierName, SupportedType.String, ""),
			new TaxParameter("TaxSerialNumber", Resources.TaxSerialNumber, SupportedType.String, ""),
			new TaxParameter("TaxDocTypes", Resources.TypeWord, SupportedType.List, null),
			new TaxParameter("ShowOutSumsAsNegatives", Resources.ShowCorrectionDocSummsAsNegatives, SupportedType.Boolean, false)
		});
	}
}
