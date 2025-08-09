namespace AccountService.Utils.Result;

public class MbResponse<TResponse>
{
    public MbResponse(int statusCode, TResponse? result)
    {
        StatusCode = statusCode;
        Result = result;
    }


    public int StatusCode { get; set; }
    public TResponse? Result { get; set; }
}