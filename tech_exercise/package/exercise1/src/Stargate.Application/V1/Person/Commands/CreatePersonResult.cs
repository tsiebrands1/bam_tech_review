namespace Stargate.Application.V1.Person.Commands;

public class CreatePersonResult : BaseResponse
{
	public int Id { get; set; }
	public string? Name { get; internal set; }
}
