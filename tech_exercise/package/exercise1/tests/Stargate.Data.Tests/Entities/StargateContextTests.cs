namespace Stargate.Data.Tests.Entities;

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Stargate.Data.Entities;
using Stargate.TestBase;

[TestFixture]
public class StargateContextTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializeDbSetsCorrectly()
	{
		// Arrange
		var options = new DbContextOptionsBuilder<StargateContext>()
			.UseInMemoryDatabase(databaseName: "TestDatabase")
			.Options;

		// Act
		using var context = new StargateContext(options);

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(context.People, Is.Not.Null);
			Assert.That(context.AstronautDetails, Is.Not.Null);
			Assert.That(context.AstronautDuties, Is.Not.Null);
		});
	}

	[Test]
	public void Constructor_ShouldCreateContextSuccessfully()
	{
		// Arrange
		var options = new DbContextOptionsBuilder<StargateContext>()
			.UseInMemoryDatabase(databaseName: "TestDatabase")
			.Options;

		// Act & Assert
		Assert.DoesNotThrow(() => new StargateContext(options));
	}
}

