namespace SharedKernel.Types;

public record Res<T>
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public T Value { get; set; }

    public static Err Error()
    {
        return new Err();
    }
}

public class Err
{
    public string ErrorMessage { get; set; }

    public static Err NotFound(string message = "Not Found")
    {
        return new Err { ErrorMessage = message };
    }

    public static Err Failure(string message = "Failure")
    {
        return new Err { ErrorMessage = message };
    }
}