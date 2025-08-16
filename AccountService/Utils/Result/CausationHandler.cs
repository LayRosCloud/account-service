namespace AccountService.Utils.Result;

public static class CausationHandler
{
    public static void ChangeCautionHeader(HttpContext context, Guid action)
    {
        context.Response.Headers["X-Causation-Id"] = action.ToString();
    }
}