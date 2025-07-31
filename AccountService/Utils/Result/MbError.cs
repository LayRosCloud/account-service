namespace AccountService.Utils.Result; 

public class MbError : MbResponse<string>
{
    public MbError(int statusCode, string? result) : base(statusCode, result)
    {
    }
}