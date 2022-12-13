using System.Net.Http.Json;
using System.Text.Json;
using Otp.Application.Common.Interfaces;
using Serilog;

namespace Otp.Infrastructure.Services.Chargebee;

public sealed class ChargebeeService : IChargebeeService
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _defaultJsonSerializer;
	private readonly ILogger _logger;

	public ChargebeeService(HttpClient httpClient, ILogger logger, JsonSerializerOptions defaultJsonSerializer)
	{
		_defaultJsonSerializer = defaultJsonSerializer ?? throw new ArgumentNullException(nameof(defaultJsonSerializer));
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async Task<CreateCustomerResponse> CreateCustomerAsync(CancellationToken cancellationToken = default!)
	{
		var response = await _httpClient.PostAsJsonAsync("customers", new { }, cancellationToken);

		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<CustomerDto>(_defaultJsonSerializer,
			cancellationToken);

		if (result is null)
		{
			throw new JsonException("Unable to serialize content");
		}

		await AddSubscriptionAsync(result, new AddSubscriptionRequest(), cancellationToken);

		return new CreateCustomerResponse();
	}

	public async Task<UpdateCustomerResponse> UpdateCustomerAsync(string customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.PostAsJsonAsync($"customers/{customerId}",
			request,
			cancellationToken);
		
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<CustomerDto>(_defaultJsonSerializer, cancellationToken);

		if (result is null)
		{
			throw new JsonException("Unable to serialize content");
		}

		return new UpdateCustomerResponse();
	}

	public async Task<AddSubscriptionResponse> AddSubscriptionAsync(CustomerDto customer,
		AddSubscriptionRequest addSubscriptionRequest,
		CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.PostAsJsonAsync($"customers/{customer.Id}/subscription_for_items",
			addSubscriptionRequest,
			cancellationToken);

		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<CustomerDto>(_defaultJsonSerializer, cancellationToken);

		if (result is null)
		{
			throw new JsonException("Unable to serialize content");
		}

		return new AddSubscriptionResponse();
	}
}