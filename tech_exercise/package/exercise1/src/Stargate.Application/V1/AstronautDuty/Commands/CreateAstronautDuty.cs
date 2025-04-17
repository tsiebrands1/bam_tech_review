namespace Stargate.Application.V1.AstronautDuty.Commands;

using FluentValidation;
using MediatR;

public record CreateAstronautDuty : IRequest<CreateAstronautDutyResult>,
	IBaseRequest, IValidatable, IEquatable<CreateAstronautDuty>
{
	private static readonly Validator validator = new();
	public DateTime DutyStartDate { get; init; } = DateTime.MinValue;
	public DateTime? DutyEndDate { get; init; } = null;
	public string Name { get; init; } = string.Empty;
	public string Rank { get; init; } = string.Empty;
	public string DutyTitle { get; init; } = string.Empty;
	
	public CreateAstronautDuty()
	{
	}

	public CreateAstronautDuty(DateTime dutyStartDate, 
		DateTime? dutyEndDate, 
		string name, 
		string rank, 
		string dutyTitle)
	{
		this.DutyStartDate = dutyStartDate;
		this.DutyEndDate = dutyEndDate;
		this.Name = name;
		this.Rank = rank;
		this.DutyTitle = dutyTitle;
	}

	public void Validate()
	{
		validator.ValidateAndThrow(this);
	}

	internal class Validator : AbstractValidator<CreateAstronautDuty>
	{
		public Validator()
		{
			this.RuleFor(x => x.DutyStartDate)
				.NotEmpty()
				.WithMessage(DUTY_START_DATE_VALIDATION_MESSAGE);
			this.RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage(NAME_VALIDATION_MESSAGE);
			this.RuleFor(x => x.Rank)
				.NotEmpty()
				.WithMessage(RANK_VALIDATION_MESSAGE);
			this.RuleFor(x => x.DutyTitle)
				.NotEmpty()
				.WithMessage(DUTY_TITLE_VALIDATION_MESSAGE);
		}
	}
}
