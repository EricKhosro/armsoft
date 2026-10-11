namespace ArmSoft.InternetBank.APIClient;

public abstract class BaseManager
{
	protected Client client;

	protected BaseManager(Client client)
	{
		this.client = client;
	}
}
