namespace Stargate.Application.Tests.V1;

using AutoFixture;
using NUnit.Framework;
using Stargate.Application.V1.Person;
using Stargate.Application.V1.Person.Commands;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.Person;
using Stargate.TestBase;

[TestFixture]
public class MapperExtensionsTests : BaseTest
{
	[Test]
	public void ToPerson_ShouldMapPropertiesCorrectly_FromCreatePerson()
	{
		// Arrange
		var createPerson = this.Fixture.Create<CreatePerson>();

		// Act
		var result = createPerson.ToPerson();

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Name, Is.EqualTo(createPerson.Name));
	}

	[Test]
	public void ToPerson_ShouldMapPropertiesCorrectly_FromIPersonAndIAstronautDetail()
	{
		// Arrange
		var person = this.Fixture.Create<IPerson>();
		var astronautDetail = this.Fixture.Create<IAstronautDetail>();

		// Act
		var result = person.ToPerson(astronautDetail);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(result.CareerEndDate, Is.EqualTo(astronautDetail.CareerEndDate));
			Assert.That(result.CareerStartDate, Is.EqualTo(astronautDetail.CareerStartDate));
			Assert.That(result.CurrentDutyTitle, Is.EqualTo(astronautDetail.CurrentDutyTitle));
			Assert.That(result.CurrentRank, Is.EqualTo(astronautDetail.CurrentRank));
			Assert.That(result.Name, Is.EqualTo(person.Name));
		});
	}
}
