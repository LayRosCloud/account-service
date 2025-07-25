namespace AccountService.Utils.Middleware;

public class ExceptionDto
{
    public ExceptionDto(string message, int code)
    {
        Message = message;
        Code = code;
    }

    public string Message { get; }
    public int Code { get; }
}