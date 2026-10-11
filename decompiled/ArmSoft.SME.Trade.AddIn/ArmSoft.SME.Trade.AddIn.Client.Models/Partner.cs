using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("B15CF719-875E-4A6F-944E-F67C020CB8E6")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class Partner : ExchangeBase
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string LongName { get; set; }

	public string Type { get; set; }

	public string TaxCode { get; set; }

	public string Address { get; set; }

	public string BusinessAddress { get; set; }

	public string ManagerName { get; set; }

	public string ManagerPosition { get; set; }

	public string ChiefAccountantName { get; set; }

	public string ChiefAccountantPosition { get; set; }

	public string MainAim { get; set; }

	public string Phone { get; set; }

	public string Email { get; set; }

	public string StateRegister { get; set; }

	public string IDDocType { get; set; }

	public string Passport { get; set; }

	public string Gender { get; set; }

	public DateTime? Birthday { get; set; }

	public string NextContractNumber { get; set; }

	public bool? Supplier { get; set; }

	public bool? Customer { get; set; }

	public bool? Foreign { get; set; }

	public List<PartnerContract> PartnerContracts { get; set; }

	public List<PartnerSettlementAccount> SettlementAccounts { get; set; }

	public List<PartnerBusinessAddressRow> BusinessAddresses { get; set; }

	public int ActorID { get; set; }
}
