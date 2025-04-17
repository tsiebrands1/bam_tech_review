namespace Stargate.API.Tests.V1;

using AutoFixture;
using NUnit.Framework;
using Stargate.API.V1;
using Stargate.Core.Dtos;
using Stargate.TestBase;
using System;

[TestFixture]
public class MappingExtentionsTests : BaseTest
{
	[Test]
	public void ToCreateAstronautDuty_ShouldMapCorrectly_WhenPersonAstronautIsValid()
	{
		// Arrange
		var personAstronaut = this.Fixture.Build<PersonAstronaut>()
			.With(p => p.CareerStartDate, DateTime.Now.AddYears(-10))
			.With(p => p.CareerEndDate, DateTime.Now)
			.Create();

		// Act
		var result = personAstronaut.ToCreateAstronautDuty();

		// Assert
		Assert.IsNotNull(result);
		Assert.AreEqual(personAstronaut.CareerStartDate, result.DutyStartDate);
		Assert.AreEqual(personAstronaut.CareerEndDate, result.DutyEndDate);
		Assert.AreEqual(personAstronaut.Name, result.Name);
		Assert.AreEqual(personAstronaut.CurrentRank, result.Rank);
		Assert.AreEqual(personAstronaut.CurrentDutyTitle, result.DutyTitle);
	}

	[Test]
	public void ToCreateAstronautDuty_ShouldThrowArgumentNullException_WhenPersonAstronautIsNull()
	{
		// Arrange
		PersonAstronaut personAstronaut = null;

		// Act & Assert
		var ex = Assert.Throws<ArgumentNullException>(() => personAstronaut.ToCreateAstronautDuty());
		Assert.AreEqual("Value cannot be null. (Parameter 'personAstronaut')", ex.Message);
	}

	[Test]
	public void ToCreateAstronautDuty_ShouldThrowArgumentException_WhenCareerStartDateIsNull()
	{
		// Arrange
		var personAstronaut = this.Fixture.Build<PersonAstronaut>()
			.With(p => p.CareerStartDate, (DateTime?)null)
			.Create();

		// Act & Assert
		var ex = Assert.Throws<ArgumentException>(() => personAstronaut.ToCreateAstronautDuty());
		Assert.AreEqual("CareerStartDate cannot be null (Parameter 'personAstronaut')", ex.Message);
	}
}

