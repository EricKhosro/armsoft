using System;
using System.Collections.Generic;
using System.Text;

namespace ArmSoft.Cloud.Client.SM;

public class SMRoutes
{
	private readonly CloudClient client;

	internal SMRoutes(CloudClient client)
	{
		this.client = client;
	}

	public SynchronizationMode GetSynchronizationMode()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/CustomerSynchronization");
		return client.Send<SynchronizationMode>("GET", stringBuilder.ToString());
	}

	public List<CustomerSuggestion> GetCustomerSuggestions(bool includeAll = false)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/Suggestions");
		stringBuilder.Append("?includeAll=");
		stringBuilder.Append(includeAll.ToString());
		return client.Send<List<CustomerSuggestion>>("GET", stringBuilder.ToString());
	}

	public void ChangeSuggestionState(int id, AppStateEnum state)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/Suggestions");
		stringBuilder.Append("/");
		stringBuilder.Append(id.ToString());
		stringBuilder.Append("/state");
		client.Send<List<CustomerSuggestion>>("PATCH", stringBuilder.ToString(), state);
	}

	public List<SMSDataRow> GetSMSList(SMSQuery query)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/SMS");
		return client.Send<List<SMSDataRow>>("POST", stringBuilder.ToString(), query);
	}

	public Dictionary<Guid, List<SMSDataRow>> GetSMSListByObject(SMSObjectsQuery query)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/SMS/ListByObjects");
		List<SMSDataRow> list = client.Send<List<SMSDataRow>>("POST", stringBuilder.ToString(), query);
		Dictionary<Guid, List<SMSDataRow>> dictionary = new Dictionary<Guid, List<SMSDataRow>>();
		foreach (SMSDataRow item in list)
		{
			if (dictionary.ContainsKey(item.ObjectId))
			{
				dictionary[item.ObjectId].Add(item);
				continue;
			}
			List<SMSDataRow> value = new List<SMSDataRow> { item };
			dictionary.Add(item.ObjectId, value);
		}
		return dictionary;
	}

	public void SendSMS(SMSBatch batch)
	{
		if (batch == null)
		{
			throw new ArgumentNullException("batch");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/SMS/Send");
		client.Send("POST", stringBuilder.ToString(), batch);
	}

	public List<CustomersStateSynchronizationError> CustomersStatesSynchronization(CustomersStateSynchronizationRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/CustomerStateSynchronization");
		return client.Send<List<CustomersStateSynchronizationError>>("POST", stringBuilder.ToString(), request);
	}

	public List<CustomerSynchronizationInfo> GetCustomersSynchronizationInfo()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/CustomerStateSynchronization/CustomersSynchronizationInfo");
		return client.Send<List<CustomerSynchronizationInfo>>("GET", stringBuilder.ToString());
	}

	public List<DatabaseRemoteParam> GetRemoteParams()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/RemoteParams");
		return client.Send<List<DatabaseRemoteParam>>("GET", stringBuilder.ToString());
	}

	public List<int> GetSalesPointIdsRegisteredInB2BMobileApp()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/B2BMobileApp/RegisteredSalesPointIds");
		return client.Send<List<int>>("GET", stringBuilder.ToString());
	}

	public List<SalesPointInfoRegisteredInB2BMobile> GetSalesPointInfoRegisteredInB2BMobileApp(int? salesPointId = null)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/B2BMobileApp/SalesPointsInfoRegisteredInB2BMobileApp");
		if (salesPointId.HasValue)
		{
			stringBuilder.Append("?salesPointId=");
			stringBuilder.Append(salesPointId.ToString());
		}
		return client.Send<List<SalesPointInfoRegisteredInB2BMobile>>("GET", stringBuilder.ToString());
	}

	public List<B2BMobileApplicationForCooperation> GetApplicationForCooperationInB2BMobileApp(int? salesPointId = null, MobileUserApplicationState? state = null)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/B2BMobileApp/ApplicationForCooperation");
		string text = "?";
		if (salesPointId.HasValue)
		{
			stringBuilder.Append(text + "salesPointId=");
			stringBuilder.Append(salesPointId.ToString());
			text = "&";
		}
		if (state.HasValue)
		{
			stringBuilder.Append(text + "state=");
			stringBuilder.Append(state.ToString());
		}
		return client.Send<List<B2BMobileApplicationForCooperation>>("GET", stringBuilder.ToString());
	}

	public bool IsSalesPointRegisteredInB2BMobileApp(int salesPointId)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/B2BMobileApp/IsSalesPointRegistered?salesPointId=");
		stringBuilder.Append(salesPointId.ToString());
		return client.Send<bool>("GET", stringBuilder.ToString());
	}

	public void SendNotificationToB2BMobileApp(List<MobileNotificationAddRequest> notificationsRequest)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/B2BMobileApp/SendNotification");
		client.Send("POST", stringBuilder.ToString(), notificationsRequest);
	}

	public void SendNotificationByPhoneNumberToB2BMobileApp(List<MobileNotificationAddRequestByPhone> notificationsRequest)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/B2BMobileApp/SendNotificationByPhoneNumber");
		client.Send("POST", stringBuilder.ToString(), notificationsRequest);
	}

	public void UpdateApplicationForCooperationState(int cooperationId, MobileUserApplicationState state)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/B2BMobileApp/UpdateApplicationForCooperationState?cooperationId=");
		stringBuilder.Append(cooperationId.ToString());
		client.Send("POST", stringBuilder.ToString(), state);
	}

	public CustomerSyncState GetCustomersSynchronizationState()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/CustomerSynchronization/GetLastSyncState");
		return client.Send<CustomerSyncState>("GET", stringBuilder.ToString());
	}

	public void UpdateLastSyncDate()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/CustomerSynchronization/UpdateLastSyncDate");
		client.Send("POST", stringBuilder.ToString());
	}

	public List<int> PrepareCustomersSynchronization(List<CustomerSyncRow> TimeStamps)
	{
		if (TimeStamps == null)
		{
			throw new ArgumentNullException("TimeStamps");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/CustomerSynchronization/PrepareSynchronization");
		return client.Send<List<int>>("POST", stringBuilder.ToString(), TimeStamps);
	}

	public void CustomersSynchronization(List<Customer> customers, ulong? TS)
	{
		if (customers == null)
		{
			throw new ArgumentNullException("customers");
		}
		CustomersSynchronizationRequest requestObject = new CustomersSynchronizationRequest
		{
			Entities = customers,
			DBTS = TS
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/CustomerSynchronization/Synchronization");
		client.Send("POST", stringBuilder.ToString(), requestObject);
	}

	public DateTime? GetActualRoutesLastSyncDate()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/Route");
		return client.Send<DateTime?>("GET", stringBuilder.ToString());
	}

	public void ActualRoutesSynchronization(List<ActualRoute> Routes)
	{
		if (Routes == null)
		{
			throw new ArgumentNullException("Routes");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/Route");
		client.Send("POST", stringBuilder.ToString(), Routes);
	}

	public SMMobileVersionResponse GetAvailableSMMobileVersion(string keySerialNumber)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/SM/SMMobile/GetVersionInfo?keySerialNumber=");
		stringBuilder.Append(keySerialNumber);
		return client.Send<SMMobileVersionResponse>("GET", stringBuilder.ToString());
	}
}
