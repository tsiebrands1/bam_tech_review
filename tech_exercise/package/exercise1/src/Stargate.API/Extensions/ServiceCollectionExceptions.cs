namespace Stargate.API.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stargate.Core.Exceptions;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;
using Stargate.Data.Repositories;

public static class ServiceCollectionExceptions
{
	public static IServiceCollection ConfigureDatabase(this IServiceCollection services,
		IConfiguration configuration)
	{
		var options = new Action<DbContextOptionsBuilder>(optionsBuilder =>
		{
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("StarbaseApiDatabase"));

		});

		var connectionString = configuration.GetConnectionString("StarbaseApiDatabase")
			?? throw new ConfigurationNotFoundException("Connection string 'StarbaseApiDatabase' is not configured.");

		services.AddDbContext<StargateContext>(options);
		services.AddDbContextFactory<StargateContext>(options);

		return services;
	}

	public static IServiceCollection RegisterRepositories(this IServiceCollection services)
	{
		services.AddTransient<IPersonRepository, PersonRepository>();
		services.AddTransient<IAstronautDetailRepository, AstronautDetailRepository>();
		services.AddTransient<IAstronautDutyRepository, AstronautDutyRepository>();

		return services;
	}
}
