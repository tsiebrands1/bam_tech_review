namespace Stargate.API.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stargate.Core.Exceptions;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;
using Stargate.Data.Repositories;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection ConfigureDatabaseRepository(this IServiceCollection services,
		IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("StarbaseApiDatabase")
			?? throw new ConfigurationNotFoundException("Connection string 'StarbaseApiDatabase' is not configured.");
		var builder =
			new DbContextOptionsBuilder<StargateContext>();
		var options = CreateOptions(builder, connectionString);

		services.AddDbContext<StargateContext>(options => options.UseSqlite(connectionString));
		services.AddDbContextFactory<StargateContext>(options => options.UseSqlite(connectionString));

		services.AddLogging(loggingBuilder =>
		{
			loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
			loggingBuilder.AddConsole();
			loggingBuilder.AddDebug();
		});

		return services;
	}

	public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddLogging(loggingBuilder =>
		{
			loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
			loggingBuilder.AddConsole();
			loggingBuilder.AddDebug();
			// Add other logging providers as needed
		});

		return services;
	}

	public static IServiceCollection RegisterRepositories(this IServiceCollection services)
	{
		services.AddScoped<IPersonRepository, PersonRepository>();
		services.AddScoped<IAstronautDetailRepository, AstronautDetailRepository>();
		services.AddScoped<IAstronautDutyRepository, AstronautDutyRepository>();
		return services;
	}

	private static DbContextOptions<StargateContext> CreateOptions
		(DbContextOptionsBuilder<StargateContext> builder, string connectionString)
	{
		builder.UseSqlite(connectionString);
		var options = builder.Options;

		return options;
	}
}
