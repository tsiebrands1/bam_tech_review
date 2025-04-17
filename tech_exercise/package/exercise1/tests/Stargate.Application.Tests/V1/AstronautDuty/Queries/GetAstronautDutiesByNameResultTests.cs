namespace Stargate.Application.Tests.V1.AstronautDuty.Queries;

using AutoFixture;
using NUnit.Framework;
using Stargate.Application.V1.AstronautDuty.Queries;
using Stargate.Core.Dtos;
using Stargate.TestBase;

[TestFixture]
public class GetAstronautDutiesByNameResultTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		var person = this.Fixture.Create<PersonAstronaut>();
		var duties = this.Fixture.CreateMany<PersonDuty>().ToList();
		var message = this.Fixture.Create<string>();
		var success = this.Fixture.Create<bool>();
		var responseCode = this.Fixture.Create<int>();

		// Act
		var result = new GetAstronautDutiesByNameResult
		{
			Person = person,
			AstronautDuties = duties,
			Message = message,
			Success = success,
			ResponseCode = responseCode
		};

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Person, Is.EqualTo(person));
			Assert.That(result.AstronautDuties, Is.EqualTo(duties));
			Assert.That(result.Message, Does.Contain(message));
			Assert.That(result.Success, Is.EqualTo(success));
			Assert.That(result.ResponseCode, Is.EqualTo(responseCode));
		});
	}

	[Test]
	public void ShouldInheritPropertiesFromBaseResponse()
	{
		// Arrange
		var message = this.Fixture.Create<string>();
		var success = this.Fixture.Create<bool>();
		var responseCode = this.Fixture.Create<int>();

		// Act
		var result = new GetAstronautDutiesByNameResult
		{
			Message = message,
			Success = success,
			ResponseCode = responseCode
		};

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Message, Does.Contain(message));
			Assert.That(result.Success, Is.EqualTo(success));
			Assert.That(result.ResponseCode, Is.EqualTo(responseCode));
		});
	}
}
