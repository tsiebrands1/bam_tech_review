namespace Stargate.Application.Tests.V1.AstronautDuty.Queries;

using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Stargate.Application.V1.AstronautDuty.Queries;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.TestBase;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class GetAstronautDutiesByNameHandlerTests : BaseTest
{
	private GetAstronautDutiesByNameHandler handler;
	private GetAstronautDutiesByName query;
	private IPerson person;
	private IEnumerable<IAstronautDuty> duties;

	private IPersonRepository personRepository;
	private IAstronautDutyRepository astronautDutyRepository;

	[SetUp]
	public override void Setup()
	{
		base.Setup();
		this.query = this.Fixture.Create<GetAstronautDutiesByName>();
		this.person = this.Fixture.Create<IPerson>();
		this.duties = this.Fixture.CreateMany<IAstronautDuty>(3);

		this.personRepository = Substitute.For<IPersonRepository>();
		this.astronautDutyRepository = Substitute.For<IAstronautDutyRepository>();
		var logger = Substitute.For<ILogger<GetAstronautDutiesByNameHandler>>();

		this.handler = new GetAstronautDutiesByNameHandler(logger, this.astronautDutyRepository, this.personRepository);
	}

	[Test]
	public async Task HandleAsync_ShouldReturnNotFoundResult_WhenPersonNotFound()
	{
		// Arrange
		this.personRepository.GetPersonByNameAsync(this.query.Name, Arg.Any<CancellationToken>()).ReturnsNull();

		// Act
		var result = await this.handler.Handle(this.query, CancellationToken.None);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Success, Is.False);
		Assert.That(result.ResponseCode, Is.EqualTo((int)HttpStatusCode.NotFound));
		StringAssert.Contains($"Cannot find person with name of {this.query.Name}.", result.Message);
	}

	[Test]
	public async Task HandleAsync_ShouldReturnCorrectResult_WhenPersonAndDutiesFound()
	{
		this.astronautDutyRepository.GetAllAsync(this.query.Name, Arg.Any<CancellationToken>()).Returns(this.duties);
		this.person.AstronautDuties.Returns(this.duties.ToList());
		// Arrange
		this.personRepository.GetPersonByNameAsync(this.query.Name, Arg.Any<CancellationToken>()).Returns(this.person);


		// Act
		var result = await this.handler.Handle(this.query, CancellationToken.None);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(result.Success, Is.True);
			Assert.That(result.ResponseCode, Is.EqualTo((int)HttpStatusCode.OK));
			Assert.That(result.Person.Name, Is.EqualTo(this.person.Name));
			Assert.That(result.AstronautDuties.Count(), Is.EqualTo(this.duties.Count()));
		});
	}

	[Test]
	public async Task HandleShouldHandleExceptionsCorrectly()
	{
		// Arrange
		this.personRepository.GetPersonByNameAsync(this.query.Name, Arg.Any<CancellationToken>())
			.ThrowsForAnyArgs(new Exception("Test exception"));

		// Act & Assert
		var ex = await this.handler.Handle(this.query, CancellationToken.None);
		Assert.That(ex.Message, Does.Contain("Test exception"));
	}
}
