using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class PartnersWithLogDataProvider : DataProvider<PartnersWithLogDataProvider.DataRow, PartnersWithLogDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public int PartID { get; set; }

		public string PartCode { get; set; } = string.Empty;

		public string Caption { get; set; } = string.Empty;

		public string FullCaption { get; set; } = string.Empty;

		public string Type { get; set; } = string.Empty;

		public string GroupCaption { get; set; } = string.Empty;

		public string GroupPath { get; set; } = string.Empty;

		public string SettlAcc { get; set; } = string.Empty;

		public string TaxCode { get; set; } = string.Empty;

		public string Address { get; set; } = string.Empty;

		public string BusinessAddress { get; set; } = string.Empty;

		public string Phone { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string ManagerFIO { get; set; } = string.Empty;

		public string ManagerPost { get; set; } = string.Empty;

		public string ChiefaccFIO { get; set; } = string.Empty;

		public string ChiefaccPost { get; set; } = string.Empty;

		public string PartStateRegister { get; set; } = string.Empty;

		public string IDDocTypeCode { get; set; } = string.Empty;

		public string IDDocType { get; set; } = string.Empty;

		public string Sex { get; set; } = string.Empty;

		public string SexCaption { get; set; } = string.Empty;

		public DateOnly? Birthday { get; set; }

		public string Passport { get; set; } = string.Empty;

		public string MainAIM { get; set; } = string.Empty;

		public string NextContractNumber { get; set; } = string.Empty;

		public bool Supplier { get; set; }

		public bool Customer { get; set; }

		public string LastModifierName { get; set; } = string.Empty;

		public DateTime? LastModifyDate { get; set; }

		public string CreatorName { get; set; } = string.Empty;

		public DateTime? CreationDate { get; set; }

		public byte B2BStatusCode { get; set; }

		public string B2BStatusName { get; set; }

		public PartnerCacheRow PartnerRow { get; set; }

		public object GetKey()
		{
			return PartID;
		}

		public string GetCode()
		{
			return PartCode;
		}

		public string GetName()
		{
			return Caption;
		}

		public bool Matched(object key)
		{
			return Convert.ToInt32(key) == PartID;
		}

		public override object ReferenceValue(string name, string source)
		{
			if (source.StartsWith("PartnerCache") && PartnerRow != null)
			{
				return PartnerRow.GetReferenceValue(source);
			}
			return null;
		}
	}

	public class Param
	{
		public string Group { get; set; } = string.Empty;

		public string PartnerCode { get; set; } = string.Empty;

		public bool ShowExtended { get; set; }

		public bool ShowOnlyB2BSupplierPartners { get; set; }

		public DateTime? LastModifiedDate { get; set; }
	}

	public override string Name => "PartnersWithLog";

	public PartnersWithLogDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
