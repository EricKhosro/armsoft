using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Data;

public class Schema
{
	public string Name { get; set; }

	public string Caption { get; set; }

	public List<Column> Columns { get; set; }

	public IEnumerable<Parameter> Parameters { get; set; }

	public IEnumerable<ExecutionPhase> ExecutionPhases { get; set; }
}
