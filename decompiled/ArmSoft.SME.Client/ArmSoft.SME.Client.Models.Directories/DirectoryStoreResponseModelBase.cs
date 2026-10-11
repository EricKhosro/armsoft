using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Directories;

namespace ArmSoft.SME.Client.Models.Directories;

public class DirectoryStoreResponseModelBase
{
	public short Type { get; set; }

	public List<ExceptionModel> Exceptions { get; set; }

	public Directory Directory { get; set; }
}
