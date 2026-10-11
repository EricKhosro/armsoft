using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class ECRChecksJournalInsidePOS : DropDownView<ECRChecksListInsidePOSDataProvider.DataRow, ECRChecksListInsidePOSDataProvider.Param>
{
	public override string HelpId => "ECRChecksJournalInsidePOS";

	public override string Caption => Resources.ECRChecksJournal + " " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY();

	public override bool KeyMemberSupported => true;

	public DateOnly? StartDate { get; set; } = Settings.HostingEnvironment.DateBegin;

	public DateOnly? EndDate { get; set; } = Settings.HostingEnvironment.DateEnd;

	public string CurrCode { get; set; } = string.Empty;

	public string CashCode { get; set; } = string.Empty;

	public string ShiftNumber { get; set; } = string.Empty;

	public string DocState { get; set; } = string.Empty;

	public ECRChecksJournalInsidePOS(Window owner)
	{
		DataProvider = new ECRChecksListInsidePOSDataProvider();
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.CashCode = CashCode;
		ParametersProvider.ShiftNumber = ShiftNumber;
		ParametersProvider.DocState = DocState;
	}

	public override object KeyMember(int index)
	{
		return Data[index].ISN;
	}
}
