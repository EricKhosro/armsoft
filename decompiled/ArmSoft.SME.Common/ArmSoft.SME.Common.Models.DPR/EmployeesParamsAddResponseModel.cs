using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class EmployeesParamsAddResponseModel
{
	public int SuccessCount { get; set; }

	public List<EmployeeParamAddResponseModel> Exceptions { get; set; }
}
