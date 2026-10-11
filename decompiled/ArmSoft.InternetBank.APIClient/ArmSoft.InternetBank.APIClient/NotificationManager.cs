using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class NotificationManager : BaseManager
{
	public NotificationManager(Client client)
		: base(client)
	{
	}

	public async Task<List<UserNotification>> GetNotificationsAsync()
	{
		return await client.GetAsync<List<UserNotification>>("notifications", requireAuthorization: true);
	}

	public async Task<UserNotification> GetPushNotificationDetailsAsync(string targetId)
	{
		return await client.GetAsync<UserNotification>("notifications/" + targetId + "/details", requireAuthorization: true);
	}

	public async Task MarkNotificationAsClickedAsync(int notificationId)
	{
		await client.PostAsync($"notifications/{notificationId}/markNotificationAsClicked", requireAuthorization: true);
	}

	public async Task MarkNotificationAsReadAsync(int notificationId)
	{
		await client.PostAsync($"notifications/{notificationId}/markNotificationAsRead", requireAuthorization: true);
	}

	public async Task MarkAllNotificationsAsReadAsync()
	{
		await client.PostAsync("notifications/markAllNotificationsAsRead", requireAuthorization: true);
	}

	public async Task MarkAllNotificationsAsSeenAsync()
	{
		await client.PostAsync("notifications/markAllNotificationsAsSeen", requireAuthorization: true);
	}

	public async Task<int> GetUnseenPushNotificationsCountAsync()
	{
		return await client.GetAsync<int>("notifications/unseencount", requireAuthorization: true);
	}
}
