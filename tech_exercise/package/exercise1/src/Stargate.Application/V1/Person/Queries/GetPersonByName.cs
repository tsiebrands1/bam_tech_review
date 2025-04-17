namespace Stargate.Application.V1.Person.Queries;

using MediatR;
using Stargate.Core.Dtos;

public record GetPersonByName : IRequest<GetPersonByNameResult>, IBaseRequest, 
	IValidatable, IEquatable<GetPersonByName>
{
	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(this.Name))
		{
			throw new ArgumentException("Name cannot be null or empty.", nameof(this.Name));
		}
	}

	/// <summary>
	/// The name of the person to retrieve.
	/// </summary>
	public string Name { get; set; } = string.Empty;
}

public class GetPersonByNameResult : BaseResponse
{
	public PersonAstronaut? Person { get; set; }
}
