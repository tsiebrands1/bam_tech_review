namespace Stargate.API.Tests.V1.Controllers;

using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Stargate.API.V1.Controllers;
using Stargate.Application.V1;
using Stargate.TestBase;

[TestFixture]
public class ControllerBaseExtensionsTests : BaseTest
{
	[Test]
	public void GetResponse_ShouldReturnObjectResult_WithCorrectStatusCode()
	{
		// Arrange
		var controllerBase = Substitute.For<ControllerBase>();
		var response = this.Fixture.Create<BaseResponse>();

		// Act
		var result = controllerBase.GetResponse(response);

		// Assert
		Assert.IsInstanceOf<ObjectResult>(result);
		var objectResult = result as ObjectResult;
		Assert.IsNotNull(objectResult);
		Assert.AreEqual(response.ResponseCode, objectResult.StatusCode);
		Assert.AreEqual(response, objectResult.Value);
	}

	[Test]
	public void GetResponse_ShouldThrowArgumentNullException_WhenControllerBaseIsNull()
	{
		// Arrange
		ControllerBase controllerBase = null!;
		var response = this.Fixture.Create<BaseResponse>();

		// Act & Assert
		var ex = Assert.Throws<ArgumentNullException>(() => controllerBase.GetResponse(response));
		Assert.AreEqual("Value cannot be null. (Parameter 'controllerBase')", ex.Message);
	}

	[Test]
	public void GetResponse_ShouldThrowArgumentNullException_WhenResponseIsNull()
	{
		// Arrange
		var controllerBase = Substitute.For<ControllerBase>();
		BaseResponse response = null!;

		// Act & Assert
		var ex = Assert.Throws<ArgumentNullException>(() => controllerBase.GetResponse(response));
		Assert.AreEqual("Value cannot be null. (Parameter 'response')", ex.Message);
	}
}

