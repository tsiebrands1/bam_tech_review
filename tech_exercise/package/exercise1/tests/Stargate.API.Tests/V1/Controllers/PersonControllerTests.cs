namespace Stargate.API.Tests.V1.Controllers;

using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Stargate.API.V1.Controllers;
using Stargate.Application.V1;
using Stargate.Application.V1.Person.Commands;
using Stargate.Application.V1.Person.Queries;
using Stargate.TestBase;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class PersonControllerTests : BaseTest
{
	private IMediator _mediator;
	private PersonController _controller;

	[SetUp]
	public override void Setup()
	{
		base.Setup();
		this._mediator = Substitute.For<IMediator>();
		this._controller = new PersonController(this._mediator);
	}

	[Test]
	public async Task GetPeople_ShouldReturnOkResult_WhenPeopleAreFound()
	{
		// Arrange
		var response = this.Fixture.Build<GetPeopleResult>()
			.With(r => r.ResponseCode, (int)HttpStatusCode.OK)
			.With(r => r.Success, true)
			.With(r => r.Message, "People retrieved successfully.")
			.Create();
		this._mediator.Send(Arg.Any<GetPeople>()).Returns(response);

		// Act
		var result = (ObjectResult)await this._controller.GetPeople();

		// Assert
		Assert.IsInstanceOf<ObjectResult>(result);
		Assert.IsNotNull(result);
		var actual = result.Value as GetPeopleResult;
		Assert.IsNotNull(actual);
		Assert.AreEqual((int)HttpStatusCode.OK, actual.ResponseCode);
		Assert.AreEqual(response, actual);
	}

	[Test]
	public async Task GetPeople_ShouldReturnInternalServerError_WhenExceptionIsThrown()
	{
		// Arrange
		var exceptionMessage = this.Fixture.Create<string>();
		this._mediator.Send(Arg.Any<GetPeople>()).Throws(new Exception(exceptionMessage));

		// Act
		var result = (ObjectResult)await this._controller.GetPeople();

		// Assert
		Assert.IsInstanceOf<ObjectResult>(result);
		Assert.IsNotNull(result.Value);
		var actual = result.Value as BaseResponse;
		Assert.IsNotNull(actual);
		Assert.AreEqual((int)HttpStatusCode.InternalServerError, actual.ResponseCode);

		Assert.AreEqual(exceptionMessage, actual.Message);
		Assert.IsFalse(actual.Success);
	}

	[Test]
	public async Task GetPersonByName_ShouldReturnOkResult_WhenPersonIsFound()
	{
		// Arrange
		var name = this.Fixture.Create<string>();
		var response = this.Fixture.Build<GetPersonByNameResult>()
			.With(r => r.ResponseCode, (int)HttpStatusCode.OK)
			.With(r => r.Success, true)
			.With(r => r.Message, "Person retrieved successfully.")
			.Create();
		this._mediator.Send(Arg.Any<GetPersonByName>()).Returns(response);

		// Act
		var result = (ObjectResult)await this._controller.GetPersonByName(name);

		// Assert
		Assert.IsInstanceOf<ObjectResult>(result);
		Assert.IsNotNull(result);
		var actual = result.Value as GetPersonByNameResult;
		Assert.IsNotNull(actual);
		Assert.AreEqual((int)HttpStatusCode.OK, actual.ResponseCode);
		Assert.AreEqual(response, actual);
	}

	[Test]
	public async Task GetPersonByName_ShouldReturnInternalServerError_WhenExceptionIsThrown()
	{
		// Arrange
		var name = this.Fixture.Create<string>();
		var exceptionMessage = this.Fixture.Create<string>();
		this._mediator.Send(Arg.Any<GetPersonByName>()).Throws(new Exception(exceptionMessage));

		// Act
		var result = await this._controller.GetPersonByName(name);

		// Assert
		Assert.IsInstanceOf<ObjectResult>(result);
		var objectResult = result as ObjectResult;
		Assert.IsNotNull(objectResult);
		Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
		var response = objectResult.Value as BaseResponse;
		Assert.IsNotNull(response);
		Assert.AreEqual(exceptionMessage, response.Message);
		Assert.IsFalse(response.Success);
	}

	[Test]
	public async Task CreatePerson_ShouldReturnOkResult_WhenRequestIsValid()
	{
		// Arrange
		var name = this.Fixture.Create<string>();
		var response = this.Fixture.Build<CreatePersonResult>()
			.With(r => r.ResponseCode, (int)HttpStatusCode.OK)
			.With(r => r.Success, true)
			.With(r => r.Message, "Person created successfully.")
			.Create();
		this._mediator.Send(Arg.Any<CreatePerson>()).Returns(response);

		// Act
		var result = await this._controller.CreatePerson(name);

		// Assert
		Assert.IsInstanceOf<ObjectResult>(result);
		var okResult = result as ObjectResult;
		Assert.IsNotNull(okResult);
		var actual = okResult.Value as CreatePersonResult;
		Assert.IsNotNull(actual);
		Assert.AreEqual((int)HttpStatusCode.OK, actual.ResponseCode);
		Assert.AreEqual(response, okResult.Value);
	}

	[Test]
	public async Task CreatePerson_ShouldReturnInternalServerError_WhenExceptionIsThrown()
	{
		// Arrange
		var name = this.Fixture.Create<string>();
		var exceptionMessage = this.Fixture.Create<string>();
		this._mediator.Send(Arg.Any<CreatePerson>()).Throws(new Exception(exceptionMessage));

		// Act
		var result = await this._controller.CreatePerson(name);

		// Assert
		Assert.IsInstanceOf<ObjectResult>(result);
		var objectResult = result as ObjectResult;
		Assert.IsNotNull(objectResult);
		Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
		var response = objectResult.Value as BaseResponse;
		Assert.IsNotNull(response);
		Assert.IsTrue(response.Message.Contains(exceptionMessage));
		Assert.IsFalse(response.Success);
	}
}
