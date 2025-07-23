namespace Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}

public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new List<string>();
    }

    public ValidationException(IEnumerable<string> failures) : this()
    {
        Errors = failures;
    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
