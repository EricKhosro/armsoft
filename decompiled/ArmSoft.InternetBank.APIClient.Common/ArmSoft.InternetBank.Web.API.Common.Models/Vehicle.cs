namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Vehicle : VehicleInformation
{
	public byte[] Version { get; set; }

	public int Id { get; set; }

	public string LicensePlateNumber { get; set; }

	public string RegistrationCertificateNumber { get; set; }

	public string Note { get; set; }
}
