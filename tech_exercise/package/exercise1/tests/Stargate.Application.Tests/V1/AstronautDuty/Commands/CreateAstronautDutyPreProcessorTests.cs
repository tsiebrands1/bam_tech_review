namespace Stargate.Application.Tests.V1.AstronautDuty.Commands;

using AutoFixture;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Stargate.Application.V1.AstronautDuty.Commands;
using Stargate.Core.Exceptions;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;
using Stargate.TestBase;
using System;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class CreateAstronautDutyPreProcessorTests : BaseTest
{
	private CreateAstronautDutyPreProcessor preProcessor;
	private IAstronautDutyRepository astronautDutyRepository;
	private IPersonRepository personRepository;

	private Person person;
	private AstronautDuty astronautDuty;
	private CreateAstronautDuty createAstronautDuty;

	[SetUp]
	public override void Setup()
	{
		base.Setup();
		var personId = this.Fixture.Create<int>();
		var personName = this.Fixture.Create<string>();
		var dutyTitle = this.Fixture.Create<string>();
		var dutyStartDate = this.Fixture.Create<DateTime>();
		this.createAstronautDuty = this.Fixture.Build<CreateAstronautDuty>()
			.With(c => c.Name, personName)
			.With(c => c.DutyTitle, dutyTitle)
			.With(c => c.DutyStartDate, dutyStartDate)
			.Create();
		this.astronautDuty = this.Fixture.Build<AstronautDuty>()
			.With(d => d.Id, personId)
			.Create();
		this.person = this.Fixture.Build<Person>()
			.With(p => p.Id, personId)
			.Create();
		this.astronautDutyRepository = Substitute.For<IAstronautDutyRepository>();
		this.personRepository = Substitute.For<IPersonRepository>();
		this.preProcessor = new CreateAstronautDutyPreProcessor(this.astronautDutyRepository, this.personRepository);
	}

	[Test]
	public void Process_ShouldThrowEntityNotFoundException_WhenPersonNotFound()
	{
		// Arrange
		var request = this.Fixture.Create<CreateAstronautDuty>();
		this.personRepository.GetPersonByNameAsync(this.createAstronautDuty.Name, Arg.Any<CancellationToken>())
			.ReturnsNull();

		// Act & Assert
		var ex = Assert.ThrowsAsync<EntityNotFoundException>(() => this.preProcessor.Process(request, 
			CancellationToken.None));
		StringAssert.Contains("Entity not found: ", ex.Message);
	}

	[Test]
	public void Process_ShouldThrowBadHttpRequestException_WhenPreviousDutyExists()
	{
		this.personRepository.GetPersonByNameAsync(this.createAstronautDuty.Name, Arg.Any<CancellationToken>()).Returns(this.person);
		this.astronautDutyRepository.GetByTitleAndStartDateAsync(this.createAstronautDuty.DutyTitle, 
			this.createAstronautDuty.DutyStartDate, Arg.Any<CancellationToken>()).Returns(this.astronautDuty);

		// Act & Assert
		var ex = Assert.ThrowsAsync<BadHttpRequestException>(() => this.preProcessor.Process(this.createAstronautDuty, CancellationToken.None));
		StringAssert.Contains("Previous duty exists", ex.Message);
	}

	[Test]
	public async Task Process_ShouldCompleteSuccessfully_WhenNoExceptionsThrown()
	{
		this.personRepository.GetPersonByNameAsync(this.createAstronautDuty.Name, Arg.Any<CancellationToken>()).Returns(this.person);
		this.astronautDutyRepository.GetByTitleAndStartDateAsync(this.createAstronautDuty.DutyTitle, 
			this.createAstronautDuty.DutyStartDate, Arg.Any<CancellationToken>())
			.ReturnsNull();

		// Act
		await this.preProcessor.Process(this.createAstronautDuty, CancellationToken.None);

		// Assert
		Assert.Pass();
	}
}

