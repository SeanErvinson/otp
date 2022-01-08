using Otp.Core.Domains.Common;
using Otp.Core.Utils;

namespace Otp.Core.Domains;

public class App : AuditableEntity
{
	private App()
	{
	}

	public App(Guid principalId, string name, string apiKey, ICollection<string>? tags, string? description = null)
	{
		PrincipalId = principalId;
		Name = name;
		HashedApiKey = CryptoUtil.HashKey(apiKey);
		Status = AppStatus.Active;
		Description = description;
		if (tags?.Any() == true)
			_tags.AddRange(tags);
	}

	public Guid PrincipalId { get; set; }
	public string Name { get; }
	public string? Description { get; private set; }
	public string? CallbackUrl { get; private set; }
	public string HashedApiKey { get; private set; }

	private readonly List<string>? _tags = new();
	public string? EndpointSecret { get; private set; }

	public bool IsDeleted => Status == AppStatus.Deleted;

	public IReadOnlyCollection<string>? Tags => _tags?.AsReadOnly();

	public Principal Principal { get; set; }

	public AppStatus Status { get; private set; }

	public void UpdateDescription(string description)
	{
		Description = description;
	}

	public void UpdateCallbackUrl(string callbackUrl, string? endpointSecret)
	{
		CallbackUrl = callbackUrl;
		EndpointSecret = endpointSecret; //TODO: Probably needs to be salted since it is coming from the user
	}

	public void UpdateHashedApiKey(string value)
	{
		HashedApiKey = CryptoUtil.HashKey(value);
	}

	public void MarkAsDeleted()
	{
		Status = AppStatus.Deleted;
	}
}

public enum AppStatus
{
	Active = 1,
	Deleted = 2
}