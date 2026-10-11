using System;
using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AA : Application
{
	public bool ForceInstitutionsAffiliation { get; set; }

	public bool BankSalaryClient { get; set; }

	public decimal? Income { get; set; }

	public IEnumerable<AmioLoanFile> Documents { get; set; }

	public OpenCreditTransitionType Status { get; set; }

	public string OfferId { get; set; }

	public int MinTerm { get; set; }

	public int MaxTerm { get; set; }

	public decimal? MinAmount { get; set; }

	public decimal? MaxAmount { get; set; }

	public decimal? InterestRate { get; set; }

	public DateTime? OfferValidDate { get; set; }

	public DateTime DefaultFirstPaymentDate { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public string ProductTitle { get; set; }

	public string ProductLoanEngine { get; set; }

	public string ProcuringType { get; set; }

	public List<AmioLoanAccountInformation> Accounts { get; set; }

	public List<PayerAccountData> AccountsDatas { get; set; }

	public AmioLoanDetailsInfo Information { get; set; }

	public string BranchName { get; set; }

	[Obsolete("Use BranchName instead.")]
	public string BranchShortName { get; set; }

	public string BranchAddress { get; set; }

	public string BranchCode { get; set; }

	public int Duration { get; set; }

	public short RepaymentDay { get; set; }

	public string AccountNumber { get; set; }

	public decimal ActualPercent { get; set; }

	public bool HasArbitration { get; set; }
}
