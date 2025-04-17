namespace Stargate.Data.Tests.Entities;

using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;
using Stargate.TestBase;

[TestFixture]
public class AstronautDutyTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		var personId = this.Fixture.Create<int>();
		var rank = this.Fixture.Create<string>();
		var dutyTitle = this.Fixture.Create<string>();
		var dutyStartDate = this.Fixture.Create<DateTime>();
		var dutyEndDate = this.Fixture.Create<DateTime?>();

		// Act
		var astronautDuty = new AstronautDuty
		{
			PersonId = personId,
			Rank = rank,
			DutyTitle = dutyTitle,
			DutyStartDate = dutyStartDate,
			DutyEndDate = dutyEndDate
		};

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(astronautDuty.PersonId, Is.EqualTo(personId));
			Assert.That(astronautDuty.Rank, Is.EqualTo(rank));
			Assert.That(astronautDuty.DutyTitle, Is.EqualTo(dutyTitle));
			Assert.That(astronautDuty.DutyStartDate, Is.EqualTo(dutyStartDate));
			Assert.That(astronautDuty.DutyEndDate, Is.EqualTo(dutyEndDate));
		});
	}

	[Test]
	public void SetPerson_ShouldSetPersonPropertyCorrectly()
	{
		// Arrange
		var person = Substitute.For<IPerson>();
		var astronautDuty = new AstronautDuty();

		// Act
		astronautDuty.SetPerson(person);

		// Assert
		Assert.That(astronautDuty.Person1, Is.EqualTo(person));
	}
}
