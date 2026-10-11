namespace ArmSoft.Cloud.Client.AS7X;

public class UpdateResponse
{
	public bool isAvailalble { get; set; }

	public bool isCritical { get; set; }

	public bool forcedKeyUpdate { get; set; }

	public string Message { get; set; }

	public string RecommendVesion { get; set; }

	public string VersionURL { get; set; }

	public bool GenerateCertificate { get; set; }

	public bool UploadCertificate { get; set; }

	public string Token { get; set; }

	public string CertificateUploadId { get; set; }

	public bool InSupport { get; set; }

	public int RemainingSupportDaysCount { get; set; }
}
