namespace AccountService.Utils.Result;

public static class ResultGenerator
{
    public static MbResponse<TResponse> Ok<TResponse>(TResponse response)
    {
        return new MbResponse<TResponse>(200, response);
    }

    public static MbResponse<TResponse> Create<TResponse>(TResponse response)
    {
        return new MbResponse<TResponse>(201, response);
    }
}