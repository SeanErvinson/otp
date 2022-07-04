using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Otp.Infrastructure.Extensions;

public static class PropertyBuilderExtensions
{
	public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> builder) where T : class, new()
	{
		var valueConverter =
			new ValueConverter<T, string>(c => JsonSerializer.Serialize(c,
					new JsonSerializerOptions
					{
						DefaultIgnoreCondition =
							JsonIgnoreCondition.Always
					}),
				c => JsonSerializer.Deserialize<T>(c,
						new JsonSerializerOptions
						{
							DefaultIgnoreCondition =
								JsonIgnoreCondition.Always
						}) ??
					new T());
		var valueComparer = new ValueComparer<T>((l, r) => JsonSerializer.Serialize(l, new JsonSerializerOptions()) ==
				JsonSerializer.Serialize(r, new JsonSerializerOptions()),
			v => JsonSerializer.Serialize(v, new JsonSerializerOptions()).GetHashCode(),
			v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v,
						new JsonSerializerOptions()),
					new JsonSerializerOptions()) ??
				new T());
		builder.HasConversion(valueConverter, valueComparer);
		builder.Metadata.SetValueConverter(valueConverter);
		builder.Metadata.SetValueComparer(valueComparer);
		builder.HasColumnType("jsonb");
		return builder;
	}
}