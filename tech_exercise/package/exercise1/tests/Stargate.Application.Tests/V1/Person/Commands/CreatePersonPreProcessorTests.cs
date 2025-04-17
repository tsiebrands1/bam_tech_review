namespace Stargate.Application.Tests.V1.Person.Commands;

using AutoFixture;
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
public class CreatePersonPreProcessorTests : BaseTest
{
	private CreatePersonPreProcessor preProcessor;
	private CreatePerson createPerson;
	private IProcessWriteOperations<IPerson> personRepository;

	[SetUp]
	public override void Setup()
	{
		base.Setup();
		this.createPerson = this.Fixture.Create<CreatePerson>();
		this.personRepository = Substitute.For<IProcessWriteOperations<IPerson>>();
		this.preProcessor = new CreatePersonPreProcessor(this.personRepository);
	}

	[Test]
	public async Task Process_ShouldAddOrUpdatePersonSuccessfully()
	{
		// Arrange
		this.personRepository.AddOrUpdateEntityAsync(Arg.Any<IPerson>(), Arg.Any<CancellationToken>())
			.Returns(15);

		// Act
		await this.preProcessor.Process(this.createPerson, CancellationToken.None);

		// Assert
		await this.personRepository.Received(1).AddOrUpdateEntityAsync(Arg.Any<IPerson>(), Arg.Any<CancellationToken>());
	}

	[Test]
	public void Process_ShouldHandleExceptionsCorrectly()
	{
		// Arrange
		this.personRepository.AddOrUpdateEntityAsync(Arg.Any<IPerson>(), Arg.Any<CancellationToken>())
			.ThrowsAsync(new Exception("Test exception"));

		// Act & Assert
		var ex = Assert.ThrowsAsync<Exception>(() => this.preProcessor.Process(this.createPerson, CancellationToken.None));
		Assert.That(ex.Message, Is.EqualTo("Test exception"));
	}
}
