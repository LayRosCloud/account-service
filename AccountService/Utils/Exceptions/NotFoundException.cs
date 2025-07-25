namespace AccountService.Utils.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : this("Object not found")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}