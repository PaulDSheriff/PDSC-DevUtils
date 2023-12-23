using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PDSC.Common.Web;

public abstract class RouterBase
{
  #region Constructor
  public RouterBase(ILogger logger)
  {
    _Logger = logger;
    _Cache = null;
  }

  public RouterBase(ILogger logger, IMemoryCache cache)
  {
    _Logger = logger;
    _Cache = cache;
  }
  #endregion

  #region Public Properties
  public string UrlFragment { get; set; } = string.Empty;
  public string TagName { get; set; } = string.Empty;
  public string EntityAsJson { get; set; } = string.Empty;
  public string InfoMessage { get; set; } = string.Empty;
  public string ErrorLogMessage { get; set; } = string.Empty;
  #endregion

  #region Protected Variables
  protected readonly ILogger _Logger;
  protected readonly IMemoryCache? _Cache;
  #endregion

  #region AddRoutes Abstract Method
  public abstract void AddRoutes(WebApplication app);
  #endregion

  #region SerializeEntity Method
  /// <summary>
  /// Serialize an object into a JSON string
  /// </summary>
  /// <typeparam name="T">The type to serialize</typeparam>
  /// <param name="entity">An instance of the type</param>
  /// <returns>A JSON string</returns>
  protected string SerializeEntity<T>(T? entity)
  {
    EntityAsJson = "Nothing serialized";

    try {
      // Attempt to serialize entity
      EntityAsJson = JsonSerializer.Serialize(entity);
    }
    catch {
      // Ignore the error
    }

    return EntityAsJson;
  }
  #endregion

  #region HandleException Methods
  /// <summary>
  /// Call this method to return a '500 Internal Server Error' and log an exception.
  /// </summary>
  /// <param name="ex">An Exception object</param>
  /// <param name="infoMsg">The info message to display to the user<param>
  /// <param name="errorMsg">The error message to log</param>
  /// <returns>A Status Code of 500</returns>
  protected IResult HandleException(Exception ex, string infoMsg, string errorMsg)
  {
    // Set properties from parameters passed in
    InfoMessage = infoMsg;
    ErrorLogMessage = errorMsg;

    return HandleException(ex);
  }

  /// <summary>
  /// Call this method to return a '500 Internal Server Error' and log an exception.
  /// Prior to calling this method...
  ///    Fill in the InfoMessage property with the value to display to the caller.
  ///    Fill in the ErrorLogMessage property with the value to place into the log file.
  /// </summary>
  /// <param name="ex">An Exception object</param>
  /// <returns>A Status Code of 500</returns>
  protected IResult HandleException(Exception ex)
  {
    IResult ret;

    // Create status code with generic message
    ret = Results.Problem(InfoMessage);

    // Add Message, Source, and Stack Trace
    ErrorLogMessage += $"{Environment.NewLine}Message: {ex.Message}";
    ErrorLogMessage += $"{Environment.NewLine}Source: {ex.Source}";
    ErrorLogMessage += $"{Environment.NewLine}Stack Trace: {ex.StackTrace}";

    // Log the exception
    _Logger.LogError(ex, "{ErrorLogMessage}", ErrorLogMessage);

    return ret;
  }
  #endregion

  #region AddToCache Method
  protected T? AddToCache<T>(object key, T value, int minutesToExpiration = 10)
  {
    T? ret = default;
    if (key != null && value != null) {
      if (_Cache != null) {
        ret = _Cache.Set(key, value, TimeSpan.FromMinutes(minutesToExpiration));
      }
    }

    return ret;
  }
  #endregion

  #region GetFromCache Method
  protected T? GetFromCache<T>(object key)
  {
    T? ret = default;

    if (_Cache != null) {
      var tmp = _Cache.Get(key);
      if (tmp != null) {
        ret = (T?)tmp;
      }
    }

    return ret;
  }
  #endregion
}
