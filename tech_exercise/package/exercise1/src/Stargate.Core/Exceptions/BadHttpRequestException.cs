namespace Stargate.Core.Exceptions;

using System;

[Serializable]
public class BadHttpRequestException : Exception
{
	public BadHttpRequestException()
	{
	}

	public BadHttpRequestException(string? message) 
		: base($"Bad Request: {message}")
	{
	}

	public BadHttpRequestException(string? message, Exception? innerException) 
		: base($"Bad Request: {message}", innerException)
	{
	}
}