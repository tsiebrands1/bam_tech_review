namespace Stargate.Application.Tests.V1.AstronautDuty;

using AutoFixture;
using NUnit.Framework;
using Stargate.Application.V1.AstronautDuty;
using Stargate.Application.V1.AstronautDuty.Commands;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.TestBase;

[TestFixture]
public class MappingExtensionsTests : BaseTest
{
	[Test]
	public void ToPersonDuty_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var astronautDuty = this.Fixture.Create<IAstronautDuty>();
		var person = this.Fixture.Create<IPerson>();

		// Act
		var result = astronautDuty.ToPersonDuty(person);

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.CareerEndDate, Is.EqualTo(astronautDuty.DutyEndDate));
			Assert.That(result.CareerStartDate, Is.EqualTo(astronautDuty.DutyStartDate));
			Assert.That(result.CurrentDutyTitle, Is.EqualTo(astronautDuty.DutyTitle));
			Assert.That(result.CurrentRank, Is.EqualTo(astronautDuty.Rank));
			Assert.That(result.PersonId, Is.EqualTo(person.Id));
			Assert.That(result.Name, Is.EqualTo(person.Name));
		});
	}

	[Test]
	public void ToPerson_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var person = this.Fixture.Create<IPerson>();

		// Act
		var result = person.ToPerson();

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.PersonId, Is.EqualTo(person.Id));
			Assert.That(result.CareerEndDate, Is.EqualTo(person.AstronautDetail.CareerEndDate));
			Assert.That(result.CareerStartDate, Is.EqualTo(person.AstronautDetail.CareerStartDate));
			Assert.That(result.CurrentDutyTitle, Is.EqualTo(person.AstronautDetail.CurrentDutyTitle));
			Assert.That(result.CurrentRank, Is.EqualTo(person.AstronautDetail.CurrentRank));
			Assert.That(result.Name, Is.EqualTo(person.Name));
		});
	}

	[Test]
	public void ToAstronautDuty_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var createAstronautDuty = this.Fixture.Create<CreateAstronautDuty>();

		// Act
		var result = createAstronautDuty.ToAstronautDuty();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.DutyTitle, Is.EqualTo(createAstronautDuty.DutyTitle));
			Assert.That(result.Rank, Is.EqualTo(createAstronautDuty.Rank));
			Assert.That(result.DutyStartDate, Is.EqualTo(createAstronautDuty.DutyStartDate));
			Assert.That(result.DutyEndDate, Is.Null);
		});
	}

	[Test]
	public void ToAstronautDetail_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var createAstronautDuty = this.Fixture.Create<CreateAstronautDuty>();
		var person = this.Fixture.Create<IPerson>();
		var astronautDetail = this.Fixture.Create<IAstronautDetail>();

		// Act
		var result = createAstronautDuty.ToAstronautDetail(person, astronautDetail);

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.CareerStartDate, Is.EqualTo(createAstronautDuty.DutyStartDate));
			Assert.That(result.CurrentRank, Is.EqualTo(createAstronautDuty.Rank));
			Assert.That(result.CurrentDutyTitle, Is.EqualTo(createAstronautDuty.DutyTitle));
			Assert.That(result.PersonId, Is.EqualTo(person.Id));
		});
	}
}
