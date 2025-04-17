namespace Stargate.TestBase;

using AutoFixture;
using NUnit.Framework;

public abstract class BaseTest
{
	public IFixture Fixture { get; } = new Fixture();

	[SetUp]
	public virtual void Setup()
	{
		// Configure AutoFixture to use NSubstitute
		this.Fixture.Customize(new AutoFixture.AutoNSubstitute.AutoNSubstituteCustomization());

		// Replace ThrowingRecursionBehavior with OmitOnRecursionBehavior
		this.Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
			.ForEach(b => this.Fixture.Behaviors.Remove(b));
		this.Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

		//this.Fixture.Customizations.Add(new TypeRelay(typeof(IPersonAstronaut), typeof(PersonAstronaut)));
		//this.Fixture.Customizations.Add(new TypeRelay(typeof(IGetPeopleResult), typeof(GetPeopleResult)));
		//this.Fixture.Customizations.Add(new TypeRelay(typeof(IGetPersonByNameResult), typeof(GetPersonByNameResult)));
		//this.Fixture.Customizations.Add(new TypeRelay(typeof(IPersonDuty), typeof(BaseResponse)));
	}
}