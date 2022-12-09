using System.Text.Json.Serialization;

namespace Otp.Application.Common.Interfaces;

public interface IChargebeeService
{
	Task<CreateCustomerResponse> CreateCustomerAsync(CancellationToken cancellationToken = default!);
	
	Task<UpdateCustomerResponse> UpdateCustomerAsync(string customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default!);

	Task<AddSubscriptionResponse> AddSubscriptionAsync(CustomerDto customer,
		AddSubscriptionRequest addSubscriptionRequest,
		CancellationToken cancellationToken = default);
}


public sealed record UpdateCustomerRequest
{
	[JsonPropertyName("first_name")]
	public string FirstName { get; init; }

	[JsonPropertyName("last_name")]
	public string LastName { get; init; }

	[JsonPropertyName("email")]
	public string Email { get; init; }

	[JsonPropertyName("created_at")]
	public long CreatedAt { get; set; }

	[JsonPropertyName("preferred_currency_code")]
	public long PreferredCurrencyCode { get; set; }
}

public sealed record UpdateCustomerResponse
{
	public CustomerDto Customer { get; init; }
}


public sealed record CreateCustomerResponse
{
	public CustomerDto Customer { get; init; }
}

public sealed record AddSubscriptionRequest
{
	[JsonPropertyName("trial_end")]
	public TimeSpan TrialEnd { get; } = TimeSpan.Zero;
}

public sealed record AddSubscriptionResponse
{
	public SubscriptionDto Subscription { get; init; }
	public CustomerDto Customer { get; init; }
}

public class SubscriptionDto
{
	[JsonPropertyName("Id")]
	public string Id { get; init; }

}

public sealed record CustomerDto
{
	[JsonPropertyName("Id")]
	public string Id { get; init; }

	[JsonPropertyName("first_name")]
	public string FirstName { get; init; }

	[JsonPropertyName("last_name")]
	public string LastName { get; init; }

	[JsonPropertyName("email")]
	public string Email { get; init; }
}