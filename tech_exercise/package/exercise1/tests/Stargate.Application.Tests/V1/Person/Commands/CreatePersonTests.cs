namespace Stargate.Application.Tests.V1.Person.Commands;

using AutoFixture;
using FluentValidation;
using NUnit.Framework;
using Stargate.Application.V1.Person.Commands;
using Stargate.TestBase;

[TestFixture]
public class CreatePersonTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		var name = this.Fixture.Create<string>();

		// Act
		var command = new CreatePerson(name);

		// Assert
		Assert.That(command.Name, Is.EqualTo(name));
	}

	[Test]
	public void Validate_ShouldThrowValidationException_WhenNameIsEmpty()
	{
		// Arrange
		var command = new CreatePerson { Name = string.Empty };

		// Act & Assert
		var ex = Assert.Throws<ValidationException>(() => command.Validate());
		Assert.That(ex, Is.Not.Null);
		Assert.That(ex.Errors.Any(e => e.PropertyName == nameof(CreatePerson.Name)), Is.True);
	}

	[Test]
	public void Equals_ShouldReturnTrue_WhenObjectsAreEqual()
	{
		// Arrange
		var name = this.Fixture.Create<string>();

		var command1 = new CreatePerson { Name = name };
		var command2 = new CreatePerson { Name = name };

		// Act
		var result = command1.Equals(command2);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void Equals_ShouldReturnFalse_WhenObjectsAreNotEqual()
	{
		// Arrange
		var command1 = this.Fixture.Create<CreatePerson>();
		var command2 = this.Fixture.Create<CreatePerson>();

		// Act
		var result = command1.Equals(command2);

		// Assert
		Assert.That(result, Is.False);
	}
}
