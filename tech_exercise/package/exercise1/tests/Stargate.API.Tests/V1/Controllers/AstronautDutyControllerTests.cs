namespace Stargate.API.Tests.V1.Controllers;

using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Stargate.API.V1;
using Stargate.API.V1.Controllers;
using Stargate.Application.V1;
using Stargate.Application.V1.AstronautDuty.Commands;
using Stargate.Application.V1.Person.Queries;
using Stargate.Core.Dtos;
using Stargate.TestBase;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class AstronautDutyControllerTests : BaseTest
{
	private IMediator _mediator;
	private AstronautDutyController _controller;

	[SetUp]
	public override void Setup()
	{
		base.Setup();
		this._mediator = Substitute.For<IMediator>();
		this._controller = new AstronautDutyController(this._mediator);
	}

	[Test]
	public async Task GetAstronautDutiesByName_ShouldReturnGetPersonByNameResult_WhenPersonIsFound()
	{
		// Arrange
		var name = this.Fixture.Create<string>();
		var getPersonByNameQuery = new GetPersonByName { Name = name };
		var person = this.Fixture.Build<PersonAstronaut>()
			.With(p => p.Name, name)
			.Create();

		var response = new GetPersonByNameResult
		{
			Person = person
		};
		this._mediator.Send(getPersonByNameQuery).Returns(response);

		// Act
		var result = (ObjectResult)await this._controller.GetAstronautDutiesByName(name);

		// Assert
		Assert.IsNotNull(result);
		Assert.True(result.Value is GetPersonByNameResult);
		var actual = (GetPersonByNameResult)result.Value;
		Assert.AreEqual(person.Name, actual?.Person?.Name);
	}

	[Test]
	public async Task GetAstronautDutiesByName_ShouldReturnInternalServerError_WhenExceptionIsThrown()
	{
		// Arrange
		var name = this.Fixture.Create<string>();
		var exceptionMessage = this.Fixture.Create<string>();
		this._mediator.Send(Arg.Any<GetPersonByName>()).Throws(new Exception(exceptionMessage));

		// Act
		var result = await this._controller.GetAstronautDutiesByName(name);

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
	public async Task CreateAstronautDuty_ShouldGetPersonByNameResult_WhenRequestIsValid()
	{
		// Arrange
		var request = this.Fixture.Create<PersonAstronaut>();
		var name = this.Fixture.Create<string>();
		var createAstronautDutyRequest = request.ToCreateAstronautDuty();
		var response = this.Fixture
			.Build<CreateAstronautDutyResult>()
			.With(r => r.ResponseCode, (int)HttpStatusCode.OK)
			.With(r => r.Success, true)
			.With(r => r.Message, "Astronaut duty created successfully.")
			.Create();
		this._mediator.Send(Arg.Any<CreateAstronautDuty>()).Returns(response);
		var person = this.Fixture.Build<PersonAstronaut>()
			.With(p => p.Name, name)
			.Create();
		// Act
		var result = (ObjectResult)await this._controller.CreateAstronautDuty(request);

		// Assert
		Assert.IsNotNull(result);
		Assert.True(result.Value is CreateAstronautDutyResult);
		Assert.IsNotNull(result.Value);
		var actual = (CreateAstronautDutyResult)result.Value;
		Assert.IsNotNull(actual);
		Assert.AreEqual((int)HttpStatusCode.OK, actual.ResponseCode);
		Assert.IsTrue(actual.Success);
		Assert.AreEqual("Astronaut duty created successfully.", actual.Message);
	}

	[Test]
	public async Task CreateAstronautDuty_ShouldReturnInternalServerError_WhenExceptionIsThrown()
	{
		// Arrange
		var request = this.Fixture.Create<PersonAstronaut>();
		var exceptionMessage = this.Fixture.Create<string>();
		this._mediator.Send(Arg.Any<CreateAstronautDuty>()).Throws(new Exception(exceptionMessage));

		// Act
		var result = await this._controller.CreateAstronautDuty(request);

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

