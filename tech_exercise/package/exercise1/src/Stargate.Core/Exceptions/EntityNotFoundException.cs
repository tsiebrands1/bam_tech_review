namespace Stargate.Core.Exceptions;

using System;

public class EntityNotFoundException : Exception
{
	public EntityNotFoundException(string message) 
		: base($"Entity not found: {message}")
	{
	}
	public EntityNotFoundException(string message, Exception innerException) 
		: base($"Entity not found: {message}", innerException)
	{
	}
}
