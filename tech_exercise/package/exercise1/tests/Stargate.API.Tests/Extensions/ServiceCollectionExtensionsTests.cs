namespace Stargate.API.Tests.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Stargate.API.Extensions;
using Stargate.Core.Exceptions;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;
using Stargate.TestBase;
using System.Collections.Generic;

[TestFixture]
public class ServiceCollectionExtensionsTests : BaseTest
{
	[Test]
	public void ConfigureDatabase_ShouldConfigureDbContext()
	{
		// Arrange
		var services = new ServiceCollection();
		var inMemorySettings = new Dictionary<string, string?> {
			{"ConnectionStrings:StarbaseApiDatabase", "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"},
			{"Logging:LogLevel:Default", "Information"},
			{"Logging:LogLevel:Microsoft", "Warning"},
			{"Logging:LogLevel:Microsoft.Hosting.Lifetime", "Information"}
		};

		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(inMemorySettings)
			.Build();

		// Act
		services.ConfigureDatabaseRepository(configuration);
		services.ConfigureLogging(configuration);
		services.RegisterRepositories();
		var serviceProvider = services.BuildServiceProvider();
		var context = serviceProvider.GetService<StargateContext>();

		// Assert
		Assert.IsNotNull(context);
		Assert.IsNotNull(serviceProvider.GetService<IPersonRepository>());
		Assert.IsNotNull(serviceProvider.GetService<IAstronautDetailRepository>());
		Assert.IsNotNull(serviceProvider.GetService<IAstronautDutyRepository>());
	}

	[Test]
	public void ConfigureDatabase_ShouldThrowConfigurationNotFoundException_WhenConnectionStringIsNull()
	{
		// Arrange
		var services = new ServiceCollection();
		var configuration = Substitute.For<IConfiguration>();
		configuration.GetConnectionString("StarbaseApiDatabase").Returns((string)null);

		// Act & Assert
		var ex = Assert.Throws<ConfigurationNotFoundException>(() => 
			services.ConfigureDatabaseRepository(configuration));
		Assert.AreEqual("Connection string 'StarbaseApiDatabase' is not configured.", ex.Message);
	}
}