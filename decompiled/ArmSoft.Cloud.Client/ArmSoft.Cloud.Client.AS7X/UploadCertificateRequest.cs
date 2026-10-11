using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class UploadCertificateRequest
{
	public string KeySerialNumber { get; set; }

	public short Localization { get; set; }

	public short Application { get; set; }

	public string ApplicationVersion { get; set; }

	public string ServerName { get; set; }

	public Guid msdbId { get; set; }

	public string CertificateUploadId { get; set; }

	public byte[] Certificate { get; set; }
}
