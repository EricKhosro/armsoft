using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.SME.Core.UI.DataViewsData.Reports;

public class DraftAccDocsDataProvider : TaxBaseDataProvider
{
	public override string Name => "DraftAccDocs";

	public override string Caption => DataProviderTypeSE.TaxServiceAccDocsDraft.Caption;

	public override TaxDocType TaxDocType => TaxDocType.AccDoc;

	public override Status TaxDocStatus => Status.Draft;

	public override ReceivedOrSubmittedType ReceivedOrSubmittedType => ReceivedOrSubmittedType.Draft;

	public override IEnumerable<TaxParameter> GetParameters()
	{
		return new _003C_003Ez__ReadOnlyArray<TaxParameter>(new TaxParameter[7]
		{
			new TaxParameter("CreationStartDate", Resources.CreationDatePeriodBegin, SupportedType.DateTime, ArmSoft.SME.Core.UI.Settings.HostingEnvironment.DateBegin, "dd/MM/yy"),
			new TaxParameter("CreationEndDate", Resources.CreationDatePeriodEnd, SupportedType.DateTime, ArmSoft.SME.Core.UI.Settings.HostingEnvironment.DateEnd, "dd/MM/yy"),
			new TaxParameter("SupplyStartDate", Resources.SupplyDatePeriodBegin, SupportedType.DateTime, ArmSoft.SME.Core.UI.Settings.HostingEnvironment.DateBegin, "dd/MM/yy"),
			new TaxParameter("SupplyEndDate", Resources.SupplyDatePeriodEnd, SupportedType.DateTime, ArmSoft.SME.Core.UI.Settings.HostingEnvironment.DateEnd, "dd/MM/yy"),
			new TaxParameter("BuyerTin", Resources.SupplierTaxCode, SupportedType.String, ""),
			new TaxParameter("BuyerName", Resources.SupplierName, SupportedType.String, ""),
			new TaxParameter("TaxDocTypes", Resources.TypeWord, SupportedType.List, null)
		});
	}
}
