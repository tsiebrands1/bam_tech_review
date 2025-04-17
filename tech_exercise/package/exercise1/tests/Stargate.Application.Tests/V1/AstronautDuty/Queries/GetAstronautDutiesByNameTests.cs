namespace Stargate.Application.Tests.V1.AstronautDuty.Queries;

using AutoFixture;
using FluentValidation;
using NUnit.Framework;
using Stargate.Application.V1.AstronautDuty.Queries;
using Stargate.TestBase;

[TestFixture]
public class GetAstronautDutiesByNameTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		var name = this.Fixture.Create<string>();

		// Act
		var query = new GetAstronautDutiesByName { Name = name };

		// Assert
		Assert.That(query.Name, Is.EqualTo(name));
	}

	[Test]
	public void Validate_ShouldThrowValidationException_WhenNameIsEmpty()
	{
		// Arrange
		var query = new GetAstronautDutiesByName { Name = string.Empty };

		// Act & Assert
		var ex = Assert.Throws<ValidationException>(() => query.Validate());
		Assert.That(ex, Is.Not.Null);
		Assert.That(ex.Errors.Any(e => e.PropertyName == nameof(GetAstronautDutiesByName.Name)), Is.True);
	}

	[Test]
	public void Equals_ShouldReturnTrue_WhenObjectsAreEqual()
	{
		// Arrange
		var name = this.Fixture.Create<string>();

		var query1 = new GetAstronautDutiesByName { Name = name };
		var query2 = new GetAstronautDutiesByName { Name = name };

		// Act
		var result = query1.Equals(query2);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void Equals_ShouldReturnFalse_WhenObjectsAreNotEqual()
	{
		// Arrange
		var query1 = this.Fixture.Create<GetAstronautDutiesByName>();
		var query2 = this.Fixture.Create<GetAstronautDutiesByName>();

		// Act
		var result = query1.Equals(query2);

		// Assert
		Assert.That(result, Is.False);
	}
}

