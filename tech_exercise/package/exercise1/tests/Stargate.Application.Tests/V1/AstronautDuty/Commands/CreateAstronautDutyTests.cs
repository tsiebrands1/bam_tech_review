namespace Stargate.Application.Tests.V1.AstronautDuty.Commands;

using AutoFixture;
using NUnit.Framework;
using Stargate.Application.V1.AstronautDuty.Commands;
using Stargate.TestBase;
using System;
using ValidationException = FluentValidation.ValidationException;

[TestFixture]
public class CreateAstronautDutyTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		var dutyStartDate = DateTime.Now.AddYears(-1);
		var dutyEndDate = DateTime.Now;
		var name = this.Fixture.Create<string>();
		var rank = this.Fixture.Create<string>();
		var dutyTitle = this.Fixture.Create<string>();

		// Act
		var command = new CreateAstronautDuty(dutyStartDate, dutyEndDate, name, rank, dutyTitle);

		// Assert
		Assert.AreEqual(dutyStartDate, command.DutyStartDate);
		Assert.AreEqual(dutyEndDate, command.DutyEndDate);
		Assert.AreEqual(name, command.Name);
		Assert.AreEqual(rank, command.Rank);
		Assert.AreEqual(dutyTitle, command.DutyTitle);
	}

	[Test]
	public void Validate_ShouldThrowValidationException_WhenPropertiesAreInvalid()
	{
		// Arrange
		var command = new CreateAstronautDuty
		{
			DutyStartDate = DateTime.MinValue,
			Name = string.Empty,
			Rank = string.Empty,
			DutyTitle = string.Empty
		};

		// Act & Assert
		var ex = Assert.Throws<ValidationException>(() => command.Validate());
		Assert.IsNotNull(ex);
		Assert.IsTrue(ex.Errors.Any(e => e.PropertyName == nameof(CreateAstronautDuty.DutyStartDate)));
		Assert.IsTrue(ex.Errors.Any(e => e.PropertyName == nameof(CreateAstronautDuty.Name)));
		Assert.IsTrue(ex.Errors.Any(e => e.PropertyName == nameof(CreateAstronautDuty.Rank)));
		Assert.IsTrue(ex.Errors.Any(e => e.PropertyName == nameof(CreateAstronautDuty.DutyTitle)));
	}

	[Test]
	public void Equals_ShouldReturnTrue_WhenObjectsAreEqual()
	{
		// Arrange
		var dutyStartDate = DateTime.Now.AddYears(-1);
		var dutyEndDate = DateTime.Now;
		var name = this.Fixture.Create<string>();
		var rank = this.Fixture.Create<string>();
		var dutyTitle = this.Fixture.Create<string>();

		var command1 = new CreateAstronautDuty(dutyStartDate, dutyEndDate, name, rank, dutyTitle);
		var command2 = new CreateAstronautDuty(dutyStartDate, dutyEndDate, name, rank, dutyTitle);

		// Act
		var result = command1.Equals(command2);

		// Assert
		Assert.IsTrue(result);
	}

	[Test]
	public void Equals_ShouldReturnFalse_WhenObjectsAreNotEqual()
	{
		// Arrange
		var command1 = this.Fixture.Create<CreateAstronautDuty>();
		var command2 = this.Fixture.Create<CreateAstronautDuty>();

		// Act
		var result = command1.Equals(command2);

		// Assert
		Assert.IsFalse(result);
	}
}

