namespace PDSC.Common;

/// <summary>
/// Class that inherits from the Exception class and it used in a catch block when a validation exception is thrown.
/// </summary>
public class ValidationException : Exception
{
  public ValidationException() : base()
  {

  }

  public ValidationException(string message) : base(message)
  {

  }

  public ValidationException(ValidationError err) : base()
  {
    ValidationError = err;
  }

  public ValidationError ValidationError { get; set; } = new();
}
