using System.Security.Claims;
using Otp.Application.Common.Interfaces;

namespace Otp.Api.Services;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _contextAccessor;
	private readonly IServiceProvider _serviceProvider;

	public CurrentUserService(IHttpContextAccessor contextAccessor, IServiceProvider serviceProvider)
	{
		_contextAccessor = contextAccessor;
		_serviceProvider = serviceProvider;
	}

	public string Email => _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
	public string UserId =>  _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
	public string IpAddress => _contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
	public string Referrer => _contextAccessor.HttpContext?.Request.Headers["Referrer"].ToString() ?? string.Empty;
	public string UserAgent => _contextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty;


	public Guid PrincipalId
	{
		get
		{
			using var scope = _serviceProvider.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
			return dbContext.Principals.SingleOrDefault(c => c.UserId == UserId)?.Id ?? throw new ApplicationException();
		}
	}
}