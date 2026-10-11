using System.Collections.Generic;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Models.Directories;

public class DirectoryStoreResponseModel<T>
{
	public short Type { get; set; }

	public List<ExceptionModel> Exceptions { get; set; }

	public T Directory { get; set; }
}
