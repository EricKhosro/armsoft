using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BaseECRCheckDocumentsDataProvider : DataProvider<BaseECRCheckDocumentsDataProvider.DataRow, BaseECRCheckDocumentsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public Guid ISN { get; set; }

		public DateOnly DocDate { get; set; }

		public string DocNum { get; set; }

		public string ECRCheckNum { get; set; }

		public string CashCode { get; set; }

		public string CashCaption { get; set; }

		public decimal Amount { get; set; }
	}

	public class Param
	{
		public string EcrCheckNum { get; set; }

		public DateOnly EndDate { get; set; }

		public string DocType { get; set; }
	}

	public override string Name => "BaseECRCheckDocuments";

	public BaseECRCheckDocumentsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
