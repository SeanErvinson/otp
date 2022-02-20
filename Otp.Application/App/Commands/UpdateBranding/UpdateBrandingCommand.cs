using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Exceptions;

namespace Otp.Application.App.Commands.UpdateBranding;

public record UpdateBrandingCommandRequest(IFormFile? BackgroundImage,
											IFormFile? LogoImage,
											string? SmsMessageTemplate);

public record UpdateBrandingCommand : IRequest
{
	public Guid Id { get; init; }
	public IFormFile? BackgroundImage { get; init; }
	public IFormFile? LogoImage { get; init; }
	public string? SmsMessageTemplate { get; init; }

	public class Handler : IRequestHandler<UpdateBrandingCommand>
	{
		private const string AppsContainerName = "apps";
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly IBlobStorageService _blobStorageService;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IBlobStorageService blobStorageService, IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
		{
			_blobStorageService = blobStorageService;
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<Unit> Handle(UpdateBrandingCommand request, CancellationToken cancellationToken)
		{
			var app = await _applicationDbContext.Apps.SingleOrDefaultAsync(app => app.Id == request.Id && app.PrincipalId == _currentUserService.PrincipalId,
																			cancellationToken);
			if (app is null) throw new NotFoundException(nameof(app));

			var prefix = $"{app.Id}/images";
			string? backgroundUri = default;
			if (request.BackgroundImage is not null)
			{
				var extension = Path.GetExtension(request.BackgroundImage.FileName);
				var resourcePath = Path.Combine(prefix, $"background{extension}");
				backgroundUri = (await UploadAppImage(request.BackgroundImage, resourcePath, cancellationToken)).ToString();
			}

			string? logoUrl = default;
			if (request.LogoImage is not null)
			{
				var extension = Path.GetExtension(request.LogoImage.FileName);
				var resourcePath = Path.Combine(prefix, $"logo{extension}");
				logoUrl = (await UploadAppImage(request.LogoImage, resourcePath, cancellationToken)).ToString();
			}

			string? smsMessageTemplate = default;
			if (!string.IsNullOrEmpty(request.SmsMessageTemplate))
			{
				smsMessageTemplate = request.SmsMessageTemplate;
			}

			try
			{
				app.Branding.UpdateBranding(logoUrl, backgroundUri, smsMessageTemplate);
			}
			catch (BrandingException ex)
			{
				throw new InvalidOperationException("Unable to update branding", ex);
			}

			await _applicationDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

		private async Task<Uri> UploadAppImage(IFormFile file, string resourcePath, CancellationToken cancellationToken)
		{
			return await _blobStorageService.UploadBlobAsync(AppsContainerName, resourcePath, file.OpenReadStream(),
															file.ContentType,
															true,
															new Dictionary<string, string>
															{
																{ "uploadedBy", _currentUserService.Email }
															},
															cancellationToken);
		}
	}
}