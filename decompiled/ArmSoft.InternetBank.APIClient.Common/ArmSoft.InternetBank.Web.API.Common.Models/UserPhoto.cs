namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserPhoto
{
	public byte[] Photo { get; set; }

	public int PhotoVersion { get; set; }

	public UserPhoto()
		: this(null, -1)
	{
	}

	public UserPhoto(byte[] photo, int photoVersion)
	{
		Photo = photo;
		PhotoVersion = photoVersion;
	}
}
