using System.Linq;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class EmployeeCache : CacheDefinition<EmployeeCacheRow, string>
{
	public override short CacheId => CacheIdSE.Employees.Value;

	public override bool StoreInLocalStorage => true;

	protected override bool CacheAlsoWithId => true;

	public EmployeeCacheRow GetEmployeeByCard(string card)
	{
		return GetList().FirstOrDefault((EmployeeCacheRow e) => e.EmployeeCard == card);
	}
}
