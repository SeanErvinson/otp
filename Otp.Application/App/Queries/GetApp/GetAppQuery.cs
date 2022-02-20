using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Application.App.Queries.GetApp;

public record GetAppQuery(Guid Id) : IRequest<GetAppQueryDto>
{
	public class Handler : IRequestHandler<GetAppQuery, GetAppQueryDto>
	{
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<GetAppQueryDto> Handle(GetAppQuery request, CancellationToken cancellationToken)
		{
			var app = await _applicationDbContext.Apps.SingleOrDefaultAsync(app => app.Id == request.Id 
																					&& app.PrincipalId == _currentUserService.PrincipalId 
																					&& app.Status != AppStatus.Deleted,
																			cancellationToken);
			if (app is null) throw new NotFoundException(nameof(app));
			
			return new GetAppQueryDto
			{
				Id = app.Id,
				Name = app.Name,
				Tags = app.Tags,
				BackgroundUrl = app.Branding.BackgroundUrl,
				LogoUrl = app.Branding.LogoUrl,
				Description = app.Description,
				CallbackUrl = app.CallbackUrl,
				CreatedAt = app.CreatedAt,
				UpdatedAt = app.UpdatedAt
			};
		}
	}
}

public record GetAppQueryDto
{
	public Guid Id { get; set; }
	public string Name { get; init; } = default!;
	public string? Description { get; init; }
	public string? BackgroundUrl { get; init; }
	public string? LogoUrl { get; init; }
	public IEnumerable<string>? Tags { get; init; }
	public string? CallbackUrl { get; init; }
	public DateTime CreatedAt { get; init; }
	public DateTime? UpdatedAt { get; init; }
}