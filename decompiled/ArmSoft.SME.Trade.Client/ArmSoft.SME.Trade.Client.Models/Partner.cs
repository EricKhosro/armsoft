using System;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Client.Data;

namespace ArmSoft.SME.Trade.Client.Models;

public class Partner : ExtendableRow, IMatchedFilterKey
{
	public int PartID { get; set; }

	public string PartCode { get; set; }

	public string Caption { get; set; }

	public string FullCaption { get; set; }

	public string Type { get; set; }

	public PartnerGroup PartnerGroup { get; set; }

	public bool Supplier { get; set; }

	public bool Customer { get; set; }

	public bool Foreign { get; set; }

	public string TaxCode { get; set; }

	public string SettlAcc { get; set; }

	public string Passport { get; set; }

	public string Email { get; set; }

	public string PartStateRegister { get; set; }

	public string Address { get; set; }

	public string BusinessAddress { get; set; }

	public string ManagerFIO { get; set; }

	public string ManagerPost { get; set; }

	public string ChiefaccFIO { get; set; }

	public string ChiefaccPost { get; set; }

	public string MainAIM { get; set; }

	public string Phone { get; set; }

	public string Contract { get; set; }

	public DateOnly? ContractDate { get; set; }

	public string NextContractNumber { get; set; }

	public int ActorId { get; set; }

	public DateOnly? Birthday { get; set; }

	public string Sex { get; private set; }

	public byte B2BStatusCode { get; set; }

	public string B2BStatusName { get; set; }

	public int B2BUserCount { get; set; }

	public int B2BProductCount { get; set; }

	public object FilterKey => PartCode;

	public bool FilterMatched(object key, object path)
	{
		if (((string)key).Trim() == Type.Trim())
		{
			return true;
		}
		return PartnerGroup.Path.StartsWith(((string)path).Trim() + "!");
	}
}
