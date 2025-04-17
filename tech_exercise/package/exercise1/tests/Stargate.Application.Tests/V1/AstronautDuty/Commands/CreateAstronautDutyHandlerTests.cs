namespace Stargate.Application.Tests.V1.AstronautDuty.Commands;

using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Stargate.Application.V1.AstronautDuty.Commands;
using Stargate.Core.V1;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;
using Stargate.TestBase;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PersonEntity = Stargate.Data.Entities.Person;

[TestFixture]
public class CreateAstronautDutyHandlerTests : BaseTest
{
	private CreateAstronautDutyHandler handler;
	private CreateAstronautDuty createAstronautDuty;
	private PersonEntity person;
	private AstronautDetail astronautDetail;
	private IEnumerable<AstronautDuty> duties;

	private IPersonRepository personRepository;
	private IAstronautDetailRepository astronautDetailRepository;
	private IProcessWriteOperations<IAstronautDetail> astronautDetailWriteOperations;
	private IProcessWriteOperations<IAstronautDuty> astronautDutyWriteOperations;

	[SetUp]
	public override void Setup()
	{
		base.Setup();
		var astronautDetailId = this.Fixture.Create<int>();
		var astronautDutyId = this.Fixture.Create<int>();
		var personName = this.Fixture.Create<string>();
		var rank = this.Fixture.Create<string>();
		var dutyTitle = this.Fixture.Create<string>();
		var dutyStartDate = this.Fixture.Create<DateTime>();
		var dutyEndDate = this.Fixture.Create<DateTime>();
		var personId = this.Fixture.Create<int>();
		this.person = this.Fixture.Build<PersonEntity>()
			.With(p => p.Id, personId)
			.With(p => p.Name, personName)
			.Create();
		this.astronautDetail = this.Fixture.Build<AstronautDetail>()
			.With(r => r.Id, astronautDetailId)
			.With(r => r.PersonId, personId)
			.With(r => r.CurrentRank, rank)
			.With(r => r.CurrentDutyTitle, dutyTitle)
			.With(r => r.CareerStartDate, dutyStartDate)
			.With(r => r.CareerEndDate, (DateTime?)null)
			.Create();
		this.duties = this.Fixture.Build<AstronautDuty>()
			.With(r => r.PersonId, personId)
			.CreateMany(3);
		this.createAstronautDuty = this.Fixture.Build<CreateAstronautDuty>()
			.With(r => r.Name, personName)
			.With(r => r.Rank, rank)
			.With(r => r.DutyTitle, dutyTitle)
			.With(r => r.DutyStartDate, dutyStartDate)
			.With(r => r.DutyEndDate, dutyEndDate)
			.Create();

		this.personRepository = Substitute.For<IPersonRepository>();
		this.astronautDetailRepository = Substitute.For<IAstronautDetailRepository>();
		this.astronautDetailWriteOperations = Substitute.For<IProcessWriteOperations<IAstronautDetail>>();
		this.astronautDutyWriteOperations = Substitute.For<IProcessWriteOperations<IAstronautDuty>>();
		var logger = Substitute.For<ILogger<CreateAstronautDutyHandler>>();

		this.personRepository.GetPersonByNameWithSqlAsync(this.createAstronautDuty.Name, Arg.Any<CancellationToken>())
			.Returns(this.person);

		this.handler = new CreateAstronautDutyHandler(logger, this.personRepository, this.astronautDetailRepository, 
			this.astronautDetailWriteOperations, this.astronautDutyWriteOperations);
	}

	[Test]
	public async Task Handle_ShouldThrowEntityNotFoundException_WhenPersonNotFound()
	{
		// Arrange
		this.personRepository.GetPersonByNameWithSqlAsync(this.createAstronautDuty.Name, Arg.Any<CancellationToken>())
			.ReturnsNull();

		// Act & Assert
		var ex = await this.handler.Handle(this.createAstronautDuty, CancellationToken.None);
		StringAssert.Contains("Entity not found: ", ex.Message);

		await this.personRepository.Received(1)
			.GetPersonByNameWithSqlAsync(this.createAstronautDuty.Name, Arg.Any<CancellationToken>());
	}

	[Test]
	public async Task HandleShouldCreateAstronautDutySuccessfully()
	{
		this.personRepository.GetPersonByNameWithSqlAsync(this.createAstronautDuty.Name, Arg.Any<CancellationToken>())
			.Returns(this.person);
		this.astronautDetailRepository.GetByPersonIdWithRawSqlAsync(this.person.Id, Arg.Any<CancellationToken>())
			.Returns(this.astronautDetail);
		this.astronautDetailWriteOperations
			.AddOrUpdateEntityAsync(Arg.Any<IAstronautDetail>(), Arg.Any<CancellationToken>())
			.Returns(this.astronautDetail.Id);
		this.astronautDutyWriteOperations.AddOrUpdateEntityAsync(Arg.Any<IAstronautDuty>(), Arg.Any<CancellationToken>())
			.Returns(this.duties.First().Id);

		// Act
		var result = await this.handler.Handle(this.createAstronautDuty, CancellationToken.None);

		await this.personRepository.Received(1).GetPersonByNameWithSqlAsync(this.createAstronautDuty.Name, Arg.Any<CancellationToken>());
		await this.astronautDetailWriteOperations.Received(1)
			.AddOrUpdateEntityAsync(Arg.Any<IAstronautDetail>(), Arg.Any<CancellationToken>());
		await this.astronautDetailRepository.Received(1)
			.GetByPersonIdWithRawSqlAsync(this.person.Id, Arg.Any<CancellationToken>());
		await this.astronautDutyWriteOperations.Received(1)
			.AddOrUpdateEntityAsync(Arg.Any<IAstronautDuty>(), Arg.Any<CancellationToken>());

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Success, Is.True);
		StringAssert.Contains("successfully", result.Message);
		Assert.That(result.ResponseCode, Is.EqualTo((int)HttpStatusCode.Created));
	}

	[Test]
	public async Task Handle_ShouldHandleExceptionsCorrectly()
	{
		// Arrange
		this.personRepository.GetPersonByNameWithSqlAsync(this.createAstronautDuty.Name, Arg.Any<CancellationToken>())
			.ThrowsForAnyArgs(new Exception("Test exception"));

		// Act & Assert
		var ex = await this.handler.Handle(this.createAstronautDuty, CancellationToken.None);
		Assert.That(ex.Message, Does.Contain("Test exception"));
	}
}
