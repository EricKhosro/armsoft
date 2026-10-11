using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class DocsLogDataProvider : DataProvider<DocsLogDataProvider.DataRow, DocsLogDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
	{
		public DateTime Date { get; set; }

		public int UserID { get; set; }

		public string UserName { get; set; }

		public byte State { get; set; }

		public string StateName { get; set; }

		public string OpName { get; set; }

		public string Comment { get; set; }

		public string CompName { get; set; }

		public string AppTypeName { get; set; }

		public Guid ISN { get; set; }

		public DateOnly DocDate { get; set; }

		public int OrderNum { get; set; }

		public string DocNum { get; set; }

		public short DocTypeCode { get; set; }

		public string DocType { get; set; }

		public string CurrCode { get; set; }

		public decimal Summ { get; set; }

		public string DocComment { get; set; }

		public bool Matched(object key)
		{
			return (Guid)key == ISN;
		}

		public Guid GetDocISN()
		{
			return ISN;
		}

		public short GetDocType()
		{
			return DocTypeCode;
		}
	}

	public class Param : ExtendableParameter
	{
		public DateOnly? DocStartDate { get; set; }

		public DateOnly? DocEndDate { get; set; }

		public DateOnly? OpStartDate { get; set; }

		public DateOnly? OpEndDate { get; set; }

		public string DocType { get; set; }

		public int UserId { get; set; }
	}

	public override string Name => "DocsLog";

	public DocsLogDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
