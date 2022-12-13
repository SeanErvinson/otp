using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Otp.Api.Extensions;
using Otp.Api.Middlewares;
using Otp.Api.Services;
using Otp.Api.Startup;
using Otp.Application;
using Otp.Application.Common.Interfaces;
using Otp.Infrastructure;
using Otp.Infrastructure.Options;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

void DbMigrate(IApplicationBuilder applicationBuilder)
{
    using var scope = applicationBuilder.ApplicationServices.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    dbInitializer.Migrate();
    dbInitializer.Seed();
}

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });
    builder.Services.AddHttpClient();
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new HeaderApiVersionReader("api-version");
    });
    builder.Services.AddOptions<StorageAccountOptions>().Bind(builder.Configuration.GetSection(StorageAccountOptions.Section));
    builder.Services.AddHealthChecks(builder.Configuration);
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddRouting(option =>
    {
        option.LowercaseUrls = true;
        option.LowercaseQueryStrings = true;
    });
    builder.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

    builder.Services.AddSwagger(builder.Configuration);

    builder.Services.AddHealthChecks(builder.Configuration);

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services);
    });

    builder.Services.AddOptions<StorageAccountOptions>().Bind(builder.Configuration.GetSection(StorageAccountOptions.Section));

    builder.Services.AddControllers()
        .AddJsonOptions(option =>
        {
            option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .AddFluentValidation();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger(builder.Configuration);
    builder.Services.AddJwtBearerAuthentication(builder.Configuration, builder.Environment);

    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication(builder.Configuration);

    builder.Services.AddScoped<IDbInitializer, DbInitializer>();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
    builder.Services.AddScoped<IAppContextService, AppContextService>();
    builder.Services.AddScoped<IOtpContextService, OtpContextService>();
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new HeaderApiVersionReader("api-version");
    });
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddRouting(option =>
    {
        option.LowercaseUrls = true;
        option.LowercaseQueryStrings = true;
    });
    builder.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerApi(builder.Configuration);
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
    }
    // DbMigrate(app);
    app.UseHttpsRedirection();
    app.UseCors(config => config.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<CustomExceptionMiddleware>();
    app.MapControllers();
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.Information("Host is shutting down");
    Log.CloseAndFlush();
}