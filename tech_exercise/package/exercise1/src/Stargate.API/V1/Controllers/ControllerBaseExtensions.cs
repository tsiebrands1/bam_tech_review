namespace Stargate.API.V1.Controllers;

using Microsoft.AspNetCore.Mvc;
using Stargate.Application.V1;

public static class ControllerBaseExtensions
{

	public static IActionResult GetResponse(this ControllerBase controllerBase, BaseResponse response)
	{
		ArgumentNullException.ThrowIfNull(controllerBase, nameof(controllerBase));
		ArgumentNullException.ThrowIfNull(response, nameof(response));

		return new ObjectResult(response)
		{
			StatusCode = response.ResponseCode
		};
	}
}