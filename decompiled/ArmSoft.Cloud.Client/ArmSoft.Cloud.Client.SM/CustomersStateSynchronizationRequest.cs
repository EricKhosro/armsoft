using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArmSoft.Cloud.Client.SM;

public class CustomersStateSynchronizationRequest
{
	public class ContainerDebt
	{
		public string Name { get; set; }

		public decimal Quantity { get; set; }
	}

	public class DebtAperture
	{
		public Guid Id { get; set; }

		public string SaleNum { get; set; }

		[JsonConverter(typeof(OnlyDateConverter))]
		public DateTime Date { get; set; }

		public decimal SaleSum { get; set; }

		public decimal Debt { get; set; }

		public int DelayDays { get; set; }
	}

	public class DataAccessState
	{
		public bool CustomerRems { get; set; }

		public bool DepositProducts { get; set; }

		public bool SalesOrders { get; set; }

		public bool Sales { get; set; }

		public bool SalesReturns { get; set; }

		public bool Payments { get; set; }

		public bool CustomerPayments { get; set; }
	}

	public class DocumentInfo
	{
		public Guid Id { get; set; }

		public DocumentType Type { get; set; }

		[JsonConverter(typeof(OnlyDateConverter))]
		public DateTime Date { get; set; }

		public decimal Amount { get; set; }

		public string DocNum { get; set; }

		public Guid? BaseId { get; set; }

		public JObject Data { get; set; }
	}

	public class CustomerState
	{
		public int SalesPointId { get; set; }

		[JsonConverter(typeof(OnlyDateConverter))]
		public DateTime Date { get; set; }

		public decimal? Debt { get; set; }

		public decimal? PrepaymentSum { get; set; }

		public decimal? SalesReturnSum { get; set; }

		public long Hash { get; set; }

		public bool TrustB2BPhones { get; set; }

		public List<ContainerDebt> ContainersDebts { get; set; }

		public List<DebtAperture> DebtAperture { get; set; }

		public List<DocumentInfo> Documents { get; set; }

		public List<string> PhoneNumbers { get; set; }

		public DataAccessState DataAccessState { get; set; }

		public List<Guid> NotChangedDocsISN { get; set; }
	}

	public List<CustomerState> CustomersStates { get; set; }

	public List<int> Delete { get; set; }
}
