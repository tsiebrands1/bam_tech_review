namespace Stargate.Application.V1.AstronautDuty.Queries;

using FluentValidation;
using MediatR;

public record GetAstronautDutiesByName 
	: IRequest<GetAstronautDutiesByNameResult>, 
	IBaseRequest, IValidatable, IEquatable<GetAstronautDutiesByName>
{
	private static readonly Validator validator = new();
	public string Name { get; init; } = string.Empty;

	public void Validate()
	{
		validator.ValidateAndThrow(this);
	}

	public GetAstronautDutiesByName() { }

	internal class Validator : AbstractValidator<GetAstronautDutiesByName>
	{
		public Validator()
		{
			this.RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage(NAME_VALIDATION_MESSAGE);
		}
	}
}
