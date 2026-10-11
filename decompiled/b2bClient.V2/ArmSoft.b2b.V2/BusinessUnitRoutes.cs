using System;
using System.Collections.Generic;
using System.Text;

namespace ArmSoft.b2b.V2;

public class BusinessUnitRoutes
{
	private readonly B2BClient client;

	internal BusinessUnitRoutes(B2BClient client)
	{
		this.client = client;
	}

	public List<Actor> GetActors(ActorRequest request)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Actors");
		stringBuilder.AddQueryParameter(request);
		return client.Send<List<Actor>>("GET", stringBuilder.ToString());
	}

	public Actor GetActor(int id)
	{
		if (id == 0)
		{
			throw new ArgumentNullException("id");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Actors/{id}");
		stringBuilder.AddPathParameterValue("{id}", id);
		return client.Send<Actor>("GET", stringBuilder.ToString());
	}

	public List<ActorCombined> GetActorsCombined(ActorCombinedRequest request)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/ActorsCombined");
		stringBuilder.AddQueryParameter(request);
		return client.Send<List<ActorCombined>>("GET", stringBuilder.ToString());
	}

	public List<ActorCombined> GetActorsCombined(List<int> actorIds)
	{
		if (actorIds == null)
		{
			throw new ArgumentNullException("actorIds");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/ActorsCombined/FilteredByIds");
		return client.Send<List<ActorCombined>>("POST", stringBuilder.ToString(), new
		{
			ids = actorIds
		});
	}

	public ActorCombined GetActorCombined(int id)
	{
		if (id == 0)
		{
			throw new ArgumentNullException("id");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/ActorsCombined/{id}");
		stringBuilder.AddPathParameterValue("{id}", id);
		return client.Send<ActorCombined>("GET", stringBuilder.ToString());
	}

	public List<SalesPoint> GetSalesPoints(SalesPointRequest request)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/SalesPoints");
		stringBuilder.AddQueryParameter(request);
		return client.Send<List<SalesPoint>>("GET", stringBuilder.ToString());
	}

	public SalesPoint GetSalesPoint(int id)
	{
		if (id == 0)
		{
			throw new ArgumentNullException("id");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/SalesPoints/{id}");
		stringBuilder.AddPathParameterValue("{id}", id);
		return client.Send<SalesPoint>("GET", stringBuilder.ToString());
	}

	public List<SalesPoint> GetSalesPoints(List<int> salesPointIds)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/SalesPoints/FilteredByIds");
		return client.Send<List<SalesPoint>>("POST", stringBuilder.ToString(), new
		{
			ids = salesPointIds
		});
	}

	public Dictionary<int, List<Contact>> GetSalesPointContacts(List<int> salesPointIds, AccessLevel accessLevel)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/SalesPoints/Contacts/{accessLevel}");
		stringBuilder.AddPathParameterValue("{accessLevel}", accessLevel.ToString());
		return client.Send<Dictionary<int, List<Contact>>>("POST", stringBuilder.ToString(), new
		{
			ids = salesPointIds
		});
	}

	public List<Organization> GetOrganizations(OrganizationRequest request)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Organizations");
		stringBuilder.AddQueryParameter(request);
		return client.Send<List<Organization>>("GET", stringBuilder.ToString());
	}

	public List<OrganizationState> GetOrganizationStates()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Organizations/States");
		return client.Send<List<OrganizationState>>("GET", stringBuilder.ToString());
	}

	public Organization GetOrganization(string taxCode)
	{
		if (string.IsNullOrEmpty(taxCode))
		{
			throw new ArgumentNullException("taxCode");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Organizations/ByTaxCode/{taxCode}");
		stringBuilder.AddPathParameterValue("{taxCode}", taxCode);
		return client.Send<Organization>("GET", stringBuilder.ToString());
	}

	public List<Bank> GetBanks()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Banks");
		return client.Send<List<Bank>>("GET", stringBuilder.ToString());
	}

	public Bank GetBank(string code)
	{
		if (string.IsNullOrEmpty(code))
		{
			throw new ArgumentNullException("code");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Banks/{code}");
		stringBuilder.AddPathParameterValue("{code}", code);
		return client.Send<Bank>("GET", stringBuilder.ToString());
	}
}
