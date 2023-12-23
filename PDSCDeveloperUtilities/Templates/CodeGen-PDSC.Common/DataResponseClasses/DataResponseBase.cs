using System.Net;

namespace PDSC.Common;

/// <summary>
/// A base class for the DataResponse class. Contains properties such as StatusCode, StatusMessage, RowsAffected, ResultMessage, LastException, and LastErrorMessage.
/// </summary>
public class DataResponseBase
{
  public HttpStatusCode StatusCode { get; set; }
  public string? StatusMessage { get; set; } = string.Empty;
  public int RowsAffected { get; set; }
  public string? ResultMessage { get; set; } = string.Empty;
  public Exception? LastException { get; set; }
  public string LastErrorMessage { get; set; } = string.Empty;
}
