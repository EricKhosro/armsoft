namespace ArmSoft.Cloud.Client.SM;

public class SynchronizationMode
{
	public bool Allow { get; set; }

	public bool UseB2BForCustomers { get; set; }

	public bool UseCloudForCustomers { get; set; }

	public bool UseB2BForSuggestions { get; set; }

	public bool UseCloudForSuggestions { get; set; }

	public bool UseB2BForActualRoutes { get; set; }

	public bool UseCloudForActualRoutes { get; set; }
}
