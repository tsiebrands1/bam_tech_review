namespace Stargate.Data.Tests.Entities;

using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;
using Stargate.TestBase;

[TestFixture]
public class AstronautDetailTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		var personId = this.Fixture.Create<int>();
		var currentRank = this.Fixture.Create<string>();
		var currentDutyTitle = this.Fixture.Create<string>();
		var careerStartDate = this.Fixture.Create<DateTime>();
		var careerEndDate = this.Fixture.Create<DateTime?>();

		// Act
		var astronautDetail = new AstronautDetail
		{
			PersonId = personId,
			CurrentRank = currentRank,
			CurrentDutyTitle = currentDutyTitle,
			CareerStartDate = careerStartDate,
			CareerEndDate = careerEndDate
		};

		// Assert
		Assert.That(astronautDetail.PersonId, Is.EqualTo(personId));
		Assert.That(astronautDetail.CurrentRank, Is.EqualTo(currentRank));
		Assert.That(astronautDetail.CurrentDutyTitle, Is.EqualTo(currentDutyTitle));
		Assert.That(astronautDetail.CareerStartDate, Is.EqualTo(careerStartDate));
		Assert.That(astronautDetail.CareerEndDate, Is.EqualTo(careerEndDate));
	}

	[Test]
	public void SetPerson_ShouldSetPersonPropertyCorrectly()
	{
		// Arrange
		var person = Substitute.For<IPerson>();
		var astronautDetail = new AstronautDetail();

		// Act
		astronautDetail.SetPerson(person);

		// Assert
		Assert.That(astronautDetail.Person, Is.EqualTo(person));
	}
}

