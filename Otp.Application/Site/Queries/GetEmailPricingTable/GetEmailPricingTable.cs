using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Application.Site.Common.Models;

namespace Otp.Application.Site.Queries.GetEmailPricingTable;

public sealed record GetEmailPricingTable : IRequest<GetEmailPricingResponse>;

public class GetEmailPricingHandler : IRequestHandler<GetEmailPricingTable, GetEmailPricingResponse>
{
	private readonly IRequestMetadataContext _requestMetadataContext;
	private readonly IApplicationDbContext _applicationDbContext;

	public GetEmailPricingHandler(IRequestMetadataContext requestMetadataContext, IApplicationDbContext applicationDbContext)
	{
		_requestMetadataContext = requestMetadataContext ?? throw new ArgumentNullException(nameof(requestMetadataContext));
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
	}

	public async Task<GetEmailPricingResponse> Handle(GetEmailPricingTable request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(_requestMetadataContext.Country))
		{
			throw new InvalidRequestException(ExceptionConstants.RequiredHeaderMissing,
				"Unable to retrieve country information from header.");
		}
		
		var pricingTable =
			await _applicationDbContext.EmailPrices.FirstOrDefaultAsync(
				table => table.OriginCountry == _requestMetadataContext.Country,
				cancellationToken: cancellationToken);

		if (pricingTable is null)
		{
			throw new InvalidRequestException(ExceptionConstants.CountryNotSupported,
				$"The provided country {_requestMetadataContext.Country} is currently not supported.");
		}

		var regionInfo = new RegionInfo(pricingTable.OriginCountry);

		var price = pricingTable.Price.ToString(CultureInfo.CurrentCulture);
		return new GetEmailPricingResponse(new Currency(regionInfo.CurrencyNativeName,
				regionInfo.TwoLetterISORegionName,
				regionInfo.CurrencySymbol,
				regionInfo.DisplayName),
			new EmailPrice(price, $"{regionInfo.CurrencySymbol}{price}"));
	}
}

public sealed record GetEmailPricingResponse(Currency Currency, EmailPrice EmailPrice);
public sealed record EmailPrice(string RawPrice, string FormattedPrice);