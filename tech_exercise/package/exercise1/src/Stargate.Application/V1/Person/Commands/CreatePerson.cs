namespace Stargate.Application.V1.Person.Commands;

using FluentValidation;
using MediatR;

public record CreatePerson 
	: IRequest<CreatePersonResult>, IBaseRequest, 
	IValidatable, IEquatable<CreatePerson>
{
	private static readonly Validator validator = new();
	public string Name { get; set; } = string.Empty;

	public CreatePerson() { }

	public CreatePerson(string name)
	{
		this.Name = name;
	}

	public void Validate()
	{
		validator.ValidateAndThrow(this);
	}

	internal class Validator : AbstractValidator<CreatePerson>
	{
		public Validator()
		{
			this.RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage(NAME_VALIDATION_MESSAGE);
		}
	}
}
