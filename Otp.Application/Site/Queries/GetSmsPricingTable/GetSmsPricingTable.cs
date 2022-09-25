using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Application.Site.Common.Models;

namespace Otp.Application.Site.Queries.GetSmsPricingTable;

public record GetSmsPricingTable : IRequest<GetSmsPricingResponse>;

public class GetSmsPricingHandler : IRequestHandler<GetSmsPricingTable, GetSmsPricingResponse>
{
	private readonly IRequestMetadataContext _requestMetadataContext;
	private readonly IApplicationDbContext _applicationDbContext;

	public GetSmsPricingHandler(IRequestMetadataContext requestMetadataContext, IApplicationDbContext applicationDbContext)
	{
		_requestMetadataContext = requestMetadataContext;
		_applicationDbContext = applicationDbContext;
	}

	public async Task<GetSmsPricingResponse> Handle(GetSmsPricingTable request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(_requestMetadataContext.Country))
		{
			throw new InvalidRequestException(ExceptionConstants.RequiredHeaderMissing,
				"Unable to retrieve country information from header.");
		}

		var pricingTable =
			await _applicationDbContext.SmsPrices.Where(table => table.OriginCountry == _requestMetadataContext.Country)
				.ToListAsync(cancellationToken: cancellationToken);

		if (pricingTable is null)
		{
			throw new InvalidRequestException(ExceptionConstants.CountryNotSupported,
				$"The provided country {_requestMetadataContext.Country} is currently not supported.");
		}

		var regionInfo = new RegionInfo(_requestMetadataContext.Country);

		var prices = pricingTable.Select(p =>
		{
			var price = p.Price.ToString(CultureInfo.CurrentCulture);

			return new SmsPrice(p.RecipientCountry, price, $"{regionInfo.CurrencySymbol}{price}");
		});

		return new GetSmsPricingResponse(new Currency(regionInfo.CurrencyNativeName,
				regionInfo.TwoLetterISORegionName,
				regionInfo.CurrencySymbol,
				regionInfo.DisplayName),
			prices);
	}
}

public record GetSmsPricingResponse(Currency Currency, IEnumerable<SmsPrice> SmsPrices);
public record SmsPrice(string To, string RawPrice, string FormattedPrice);