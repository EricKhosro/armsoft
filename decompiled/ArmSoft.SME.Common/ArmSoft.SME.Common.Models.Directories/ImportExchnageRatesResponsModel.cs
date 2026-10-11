using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Directories;

public class ImportExchnageRatesResponsModel
{
	public int SuccessCount { get; set; }

	public List<ImportExchnageRateResponsModel> Exceptions { get; set; }
}
