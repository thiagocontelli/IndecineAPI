namespace Indecine.Models;

public class Result
{
    public bool HasError { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;
    public string SuccessMessage { get; private set; } = string.Empty;

    public Result() { }

    private Result(bool hasError, string errorMessage, string successMessage)
    {
        HasError = hasError;
        ErrorMessage = errorMessage;
        SuccessMessage = successMessage;
    }

    public Result Fail(string errorMessage)
    {
        return new Result(true, errorMessage, string.Empty);
    }

    public Result Succeded(string successMessage)
    {
        return new Result(false, string.Empty, successMessage);
    }
}
