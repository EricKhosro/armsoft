using System;

namespace ArmSoft.EMarkReceiptClient.Models;

public class DatamatrixResponse
{
	public byte[] imagebytes { get; set; } = Array.Empty<byte>();

	public Guid datamatrix_file_id { get; set; }
}
