namespace AccountService.Utils.Exceptions;

public class ExceptionUtils
{
    public static NotFoundException GetNotFoundException(string objectName, object id)
    {
        return new NotFoundException($"'{objectName}' with id {id} is not found");
    }
}