namespace Stargate.Application.V1;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Stargate.Core.Exceptions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public abstract class BaseHandler<TRequest, TResponse>(ILogger logger) 
	: IRequestHandler<TRequest, TResponse>
	where TRequest : IValidatable, IRequest<TResponse>, new()
	where TResponse : BaseResponse, new()
{
	private readonly ILogger logger = logger;

	public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
	{
		try
		{
			request.Validate();
			await this.HandleAsync(request, cancellationToken);
			return new TResponse
			{
				Success = true,
				Message = "Operation completed successfully.",
				ResponseCode = (int)HttpStatusCode.OK
			};
		}
		// Catching specific exceptions to provide more meaningful error messages
		catch (EntityNotFoundException ex)
		{
			this.logger.LogError(ex, "Entity not found: {Exception}", ex);
			return new TResponse
			{
				Success = false,
				Message = ex.Message,
				ResponseCode = (int)HttpStatusCode.NotFound
			};
		}
		catch (ArgumentNullException ex)
		{
			this.logger.LogError(ex, "Argument null: {Exception}", ex);
			return new TResponse
			{
				Success = false,
				Message = ex.Message,
				ResponseCode = (int)HttpStatusCode.BadRequest
			};
		}
		catch (ValidationException ex)
		{
			this.logger.LogError(ex, "Validation error: {Exception}", ex);
			return new TResponse
			{
				Success = false,
				Message = ex.Message,
				ResponseCode = (int)HttpStatusCode.BadRequest
			};
		}
		catch (Exception ex)
		{
			this.logger.LogError(ex, "Unhandled exception: {Exception}", ex);
			return new TResponse
			{
				Success = false,
				Message = ex.Message,
				ResponseCode = (int)HttpStatusCode.InternalServerError
			};
		}
	}

	protected abstract ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);

	protected virtual void Validate(TRequest request)
	{
		if (request is IValidatable validatable)
		{
			validatable.Validate();
		}
		else
		{
			throw new InvalidOperationException("Request does not implement IValidatable");
		}
	}
}
