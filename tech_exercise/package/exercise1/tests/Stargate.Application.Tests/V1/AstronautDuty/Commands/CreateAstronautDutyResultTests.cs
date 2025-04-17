namespace Stargate.Application.Tests.V1.AstronautDuty.Commands;

using AutoFixture;
using NUnit.Framework;
using Stargate.Application.V1.AstronautDuty.Commands;
using Stargate.TestBase;

[TestFixture]
public class CreateAstronautDutyResultTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		var id = this.Fixture.Create<int>();
		var message = this.Fixture.Create<string>();
		var success = this.Fixture.Create<bool>();
		var responseCode = this.Fixture.Create<int>();

		// Act
		var result = new CreateAstronautDutyResult
		{
			Id = id,
			Message = message,
			Success = success,
			ResponseCode = responseCode
		};

		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result.Id, Is.EqualTo(id));
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
		var result = new CreateAstronautDutyResult
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
