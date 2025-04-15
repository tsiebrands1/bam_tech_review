namespace Stargate.Application.V1;

using System.Net;

public class BaseResponse
{
	public BaseResponse()
	{
	}

	public BaseResponse(string message, bool success, int responseCode)
	{
		this.Message = message;
		this.Success = success;
		this.ResponseCode = responseCode;
	}

	public bool Success { get; set; } = true;
	public string Message { get; set; } = "Successful";
	public int ResponseCode { get; set; } = (int)HttpStatusCode.OK;
}