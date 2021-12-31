FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Otp.Api/Otp.Api.csproj", "Otp.Api/"]
RUN dotnet restore "Otp.Api/Otp.Api.csproj"
COPY . .
WORKDIR "/src/Otp.Api"
RUN dotnet build "Otp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Otp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Otp.Api.dll"]
