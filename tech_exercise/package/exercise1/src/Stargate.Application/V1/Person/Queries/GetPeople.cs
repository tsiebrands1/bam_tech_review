namespace Stargate.Application.V1.Person.Queries;

using MediatR;

public record GetPeople
	: IRequest<GetPeopleResult>, IBaseRequest, IValidatable
{
	public void Validate()
	{ 
	
	}
}
