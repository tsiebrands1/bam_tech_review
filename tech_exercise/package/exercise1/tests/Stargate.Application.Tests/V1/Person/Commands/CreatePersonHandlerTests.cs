namespace Stargate.Application.Tests.V1.Person.Commands;

using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Stargate.Application.V1.Person.Commands;
using Stargate.Core.V1;
using Stargate.Core.V1.Person;
using Stargate.TestBase;
using System;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class CreatePersonHandlerTests : BaseTest
{
	private CreatePersonHandler handler;
	private CreatePerson createPerson;
	private IPerson person;
	private IProcessWriteOperations<IPerson> personRepository;

	[SetUp]
	public override void Setup()
	{
		base.Setup();
		this.createPerson = this.Fixture.Create<CreatePerson>();
		this.person = this.Fixture.Create<IPerson>();

		this.personRepository = Substitute.For<IProcessWriteOperations<IPerson>>();
		var logger = Substitute.For<ILogger<CreatePersonHandler>>();

		this.handler = new CreatePersonHandler(logger, this.personRepository);
	}

	[Test]
	public async Task HandleShouldCreatePersonSuccessfully()
	{
		this.person.Id = 15; // Simulate a person with an ID of 15
		this.personRepository.AddOrUpdateEntityAsync(Arg.Any<IPerson>(), Arg.Any<CancellationToken>())
			.Returns(15);

		// Act
		var result = await this.handler.Handle(this.createPerson, CancellationToken.None);

		// Assert
		await this.personRepository.Received(1).AddOrUpdateEntityAsync(Arg.Any<IPerson>(), Arg.Any<CancellationToken>());
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(15));
	}

	[Test]
	public async Task HandleShouldHandleExceptionsCorrectly()
	{
		// Arrange
		this.personRepository.AddOrUpdateEntityAsync(Arg.Any<IPerson>(), Arg.Any<CancellationToken>())
			.ThrowsAsync(new Exception("Test exception"));

		// Act & Assert
		var ex = await this.handler.Handle(this.createPerson, CancellationToken.None);
		Assert.That(ex.Message, Is.EqualTo("Test exception"));
	}
}

