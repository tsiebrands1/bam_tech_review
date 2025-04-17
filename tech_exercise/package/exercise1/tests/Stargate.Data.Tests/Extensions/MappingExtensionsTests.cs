namespace Stargate.Data.Tests.Extensions;

using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;
using Stargate.Data.Extensions;
using Stargate.TestBase;

public class MappingExtensionsTests : BaseTest
{
	[Test]
	public void ToPerson_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var source = this.Fixture.Create<IPerson>();
		source.Id.Returns(1);
		source.Name.Returns("John Doe");

		// Act
		var result = source.ToPerson();

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Id, Is.EqualTo(source.Id));
			Assert.That(result.Name, Is.EqualTo(source.Name));
		});
	}

	[Test]
	public void ToPerson_WithDestination_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var source = this.Fixture.Create<IPerson>();
		var destination = this.Fixture.Create<Person>();
		source.Id.Returns(1);
		source.Name.Returns("John Doe");

		// Act
		var result = source.ToPerson(destination);

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Id, Is.EqualTo(source.Id));
			Assert.That(result.Name, Is.EqualTo(source.Name));
		});
	}

	[Test]
	public void ToAstronautDetail_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var source = this.Fixture.Create<IAstronautDetail>();
		source.Id.Returns(1);
		source.CurrentRank.Returns("Commander");
		source.CurrentDutyTitle.Returns("Mission Specialist");
		source.CareerStartDate.Returns(new DateTime(2000, 1, 1));
		source.CareerEndDate.Returns(new DateTime(2020, 1, 1));

		// Act
		var result = source.ToAstronautDetail();

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Id, Is.EqualTo(source.Id));
			Assert.That(result.CurrentRank, Is.EqualTo(source.CurrentRank));
			Assert.That(result.CurrentDutyTitle, Is.EqualTo(source.CurrentDutyTitle));
			Assert.That(result.CareerStartDate, Is.EqualTo(source.CareerStartDate));
			Assert.That(result.CareerEndDate, Is.EqualTo(source.CareerEndDate));
		});
	}

	[Test]
	public void ToAstronautDetail_WithDestination_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var source = this.Fixture.Create<IAstronautDetail>();
		var destination = this.Fixture.Create<AstronautDetail>();
		source.Id.Returns(1);
		source.CurrentRank.Returns("Commander");
		source.CurrentDutyTitle.Returns("Mission Specialist");
		source.CareerStartDate.Returns(new DateTime(2000, 1, 1));
		source.CareerEndDate.Returns(new DateTime(2020, 1, 1));

		// Act
		var result = source.ToAstronautDetail(destination);

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Id, Is.EqualTo(source.Id));
			Assert.That(result.CurrentRank, Is.EqualTo(source.CurrentRank));
			Assert.That(result.CurrentDutyTitle, Is.EqualTo(source.CurrentDutyTitle));
			Assert.That(result.CareerStartDate, Is.EqualTo(source.CareerStartDate));
			Assert.That(result.CareerEndDate, Is.EqualTo(source.CareerEndDate));
		});
	}

	[Test]
	public void FromEntity_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var source = this.Fixture.Create<AstronautDetail>();
		source.Id = 1;
		source.CurrentRank = "Commander";
		source.CurrentDutyTitle = "Mission Specialist";
		source.CareerStartDate = new DateTime(2000, 1, 1);
		source.CareerEndDate = new DateTime(2020, 1, 1);

		// Act
		var result = source.FromEntity();

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Id, Is.EqualTo(source.Id));
			Assert.That(result.CurrentRank, Is.EqualTo(source.CurrentRank));
			Assert.That(result.CurrentDutyTitle, Is.EqualTo(source.CurrentDutyTitle));
			Assert.That(result.CareerStartDate, Is.EqualTo(source.CareerStartDate));
			Assert.That(result.CareerEndDate, Is.EqualTo(source.CareerEndDate));
		});
	}

	[Test]
	public void ToAstronautDuty_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var source = this.Fixture.Create<IAstronautDuty>();
		source.Id.Returns(1);
		source.Rank.Returns("Lieutenant");
		source.DutyTitle.Returns("Engineer");
		source.DutyStartDate.Returns(new DateTime(2010, 1, 1));
		source.DutyEndDate.Returns(new DateTime(2015, 1, 1));

		// Act
		var result = source.ToAstronautDuty();

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Id, Is.EqualTo(source.Id));
			Assert.That(result.Rank, Is.EqualTo(source.Rank));
			Assert.That(result.DutyTitle, Is.EqualTo(source.DutyTitle));
			Assert.That(result.DutyStartDate, Is.EqualTo(source.DutyStartDate));
			Assert.That(result.DutyEndDate, Is.EqualTo(source.DutyEndDate));
		});
	}

	[Test]
	public void ToAstronautDuty_WithDestination_ShouldMapPropertiesCorrectly()
	{
		// Arrange
		var source = this.Fixture.Build<AstronautDuty>().Create();
		var destination = this.Fixture.Build<AstronautDuty>().Create();

		// Act
		var result = source.ToAstronautDuty(destination);

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Id, Is.EqualTo(source.Id));
			Assert.That(result.Rank, Is.EqualTo(source.Rank));
			Assert.That(result.DutyTitle, Is.EqualTo(source.DutyTitle));
			Assert.That(result.DutyStartDate, Is.EqualTo(source.DutyStartDate));
			Assert.That(result.DutyEndDate, Is.EqualTo(source.DutyEndDate));
		});
	}
}
