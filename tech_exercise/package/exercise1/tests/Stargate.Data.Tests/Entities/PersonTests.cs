namespace Stargate.Data.Tests.Entities;

using AutoFixture;
using NUnit.Framework;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Data.Entities;
using Stargate.TestBase;

[TestFixture]
public class PersonTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		var name = this.Fixture.Create<string>();

		// Act
		var person = new Person
		{
			Name = name
		};

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(person.Name, Is.EqualTo(name));
			Assert.That(person.AstronautDetail, Is.Not.Null);
			Assert.That(person.AstronautDuties, Is.Not.Null);
		});
		Assert.IsEmpty(person.AstronautDuties);
	}

	[Test]
	public void AstronautDetail_ShouldSetAndRetrieveCorrectly()
	{
		// Arrange
		var astronautDetail = this.Fixture.Create<IAstronautDetail>();
		var person = new Person();

		// Act
		person.AstronautDetail = astronautDetail;

		// Assert
		Assert.That(person.AstronautDetail, Is.EqualTo(astronautDetail));
	}

	[Test]
	public void AstronautDuties_ShouldSetAndRetrieveCorrectly()
	{
		// Arrange
		var astronautDuties = this.Fixture.CreateMany<IAstronautDuty>();
		var person = new Person();

		// Act
		person.AstronautDuties = astronautDuties;

		// Assert
		Assert.That(person.AstronautDuties, Is.EqualTo(astronautDuties));
	}
}
