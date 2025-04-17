namespace Stargate.Application.Tests.V1.Person.Commands;

using AutoFixture;
using NUnit.Framework;
using Stargate.Application.V1.Person.Commands;
using Stargate.TestBase;

[TestFixture]
public class CreatePersonResultTests : BaseTest
{
	[Test]
	public void Constructor_ShouldInitializePropertiesCorrectly()
	{
		// Arrange  
		var id = this.Fixture.Create<int>();
		var name = this.Fixture.Create<string>();
		var message = this.Fixture.Create<string>();
		var success = this.Fixture.Create<bool>();
		var responseCode = this.Fixture.Create<int>();

		// Act  
		var result = new CreatePersonResult
		{
			Id = id,
			Message = message,
			Success = success,
			ResponseCode = responseCode
		};

		// Set the Name property using reflection since its setter is internal  
		typeof(CreatePersonResult).GetProperty(nameof(CreatePersonResult.Name))
			?.SetValue(result, name);

		Assert.Multiple(() =>
		{
			// Assert  
			Assert.That(result.Id, Is.EqualTo(id));
			Assert.That(result.Name, Is.EqualTo(name));
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
		var result = new CreatePersonResult
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
