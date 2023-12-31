﻿using System.Net;
using System.Net.Http.Json;

namespace PDSC.Common.HttpClasses;

/// <summary>
/// This class is a wrapper around the HttpClient class and is used to make Web API calls easier. All your RepositoryAPI classes should inherit from this class.
/// </summary>
public class HttpClientRepositoryBase : RepositoryBase
{
  #region Constructors
  public HttpClientRepositoryBase()
  {
    Init();
  }

  public HttpClientRepositoryBase(HttpClient client)
  {
    Init();
    HClient = client;
  }
  #endregion

  #region Public Properties
  /// <summary>
  /// Get/Set the instance of the HttpClient to use
  /// </summary>
  public HttpClient? HClient { get; set; }
  /// <summary>
  /// Get/Set the UrlPath such as /api/Customer
  /// </summary>
  public string UrlPath { get; set; } = string.Empty;
  /// <summary>
  /// Get/Set the UrlSearchRoute such as /Search
  /// </summary>
  public string UrlSearchRoute { get; set; } = "/Search";
  /// <summary>
  /// Get/Set the UrlCountRoute such as /Count
  /// </summary>
  public string UrlCountRoute { get; set; } = "/Count";
  /// <summary>
  /// Get/Set the UrlQuery such as /api/Customer?firstName=p&lastName=s
  /// </summary>
  public string UrlQuery { get; set; } = string.Empty;
  /// <summary>
  /// Get/Set the UserAgent such as the application name "Customer Maintenance"
  /// </summary>
  public string UserAgent { get; set; } = string.Empty;
  /// <summary>
  /// Get/Set the content type you want returned such as "application/json"
  /// </summary>
  public string ContentType { get; set; } = "application/json";
  /// <summary>
  /// Get/Set any additional headers you want to send to the Web API server
  /// </summary>
  public Dictionary<string, string> Headers { get; set; } = new();
  #endregion

  #region Init Method
  public override void Init()
  {
    base.Init();

    BaseWebAddress = string.Empty;
    UrlPath = string.Empty;
    UrlQuery = string.Empty;
    UrlSearchRoute = "/Search";
    UrlCountRoute = "/Count";
    UserAgent = string.Empty;
    BearerToken = string.Empty;
    Headers = new();
  }
  #endregion

  #region FixUrlParts Method
  protected virtual void FixUrlParts()
  {
    // Should the UrlPath starts with a forward slash?
    string addr = HClient?.BaseAddress?.AbsoluteUri ?? string.Empty;
    if (UrlPath.StartsWith('/') && addr.EndsWith("/")) {
      // Remove starting forward slash
      UrlPath = UrlPath[1..];
    }
    // Make sure the UrlPath does not end with a forward slash
    UrlPath = UrlPath.EndsWith('/') ? UrlPath[..^1] : UrlPath;
  }
  #endregion

  #region BuildUrlQueryParameters Method
  public string BuildUrlQueryParameters(object? search)
  {
    return BuildUrlQueryParameters(search, new());
  }

  public string BuildUrlQueryParameters(object? search, List<string> excludeProperties)
  {
    List<string> queryParams = new();
    // Ensure all properties to exclude are lower case
    if (excludeProperties.Count > 0) {
      for (int index = 0; index < excludeProperties.Count; index++) {
        excludeProperties[index] = excludeProperties[index].ToLower();
      }
    }

    string separator = "?";
    if (!string.IsNullOrEmpty(AdditionalUrlData)) {
      queryParams.Add($"{separator}{AdditionalUrlData}");
      separator = "&";
    }

    if (search != null) {
      // Do NOT add properties decorated with [JsonIgnore]
      // to the URI Query
      var props = search.GetType().GetProperties().ToList();
      foreach (var item in props) {
        if (!excludeProperties.Contains(item.Name.ToLower())) {
          // Only get those properties with no [JsonIgnore] attribute
          if (!item.GetCustomAttributes(true).ToList().Where(a => a.ToString() == "System.Text.Json.Serialization.JsonIgnoreAttribute").Any() &&
              !item.GetCustomAttributes(true).ToList().Where(a => a.ToString() == "System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute").Any()) {
            string? value = (string?)item.GetValue(search);
            if (!string.IsNullOrWhiteSpace(value)) {
              queryParams.Add($"{separator}{item.Name}={Uri.EscapeDataString(value)}");
              separator = "&";
            }
          }
        }
      }
    }

    return string.Join(string.Empty, queryParams.ToArray());
  }
  #endregion

  #region SetBaseAddressAndHeaders Method
  protected void SetBaseAddressAndHeaders(HttpClient client)
  {
    // TODO: Check out this URL: https://code-maze.com/httpclient-example-aspnet-core-post-put-delete/
    // TODO: Use IHttpClientFactory? https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0

    // Set the base address such as https://www.pdsa.com
    client.BaseAddress = new Uri(BaseWebAddress);
    // Clear Accept Headers
    //client.DefaultRequestHeaders.Accept.Clear();
    // Add Content Type Default="application/json"
    //client.DefaultRequestHeaders.Accept.Add(new(ContentType));
    // Add the User-Agent
    //client.DefaultRequestHeaders.Add("User-Agent", UserAgent.Replace(" ", ""));
    // Add the Connection
    ///client.DefaultRequestHeaders.Add("Connection", "keep-alive");
    // Add any additional headers
    //foreach (var header in Headers) {
    //  client.DefaultRequestHeaders.Add(header.Key, header.Value);
    //}    
  }
  #endregion

  #region SetAuthTokenHeader Method
  protected void SetAuthTokenHeader(HttpClient? client)
  {
    if (client != null) {
      // If authorization is not set, and there is a bearer token
      if (client.DefaultRequestHeaders.Authorization == null && !string.IsNullOrEmpty(BearerToken)) {
        // Add bearer token header
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
      }
    }
  }
  #endregion

  #region GetAsync Method
  /// <summary>
  /// Get all records from the data store
  /// </summary>
  /// <typeparam name="T">The type to return</typeparam>
  /// <returns>An HttpDataResponse object</returns>
  public async Task<DataResponse<T>> GetAsync<T>(string urlPath) where T : class
  {
    UrlPath = urlPath;
    UrlQuery = string.Empty;
    if (!string.IsNullOrEmpty(AdditionalUrlData)) {
      UrlQuery += $"?{AdditionalUrlData}";
    }
    return await SubmitAsyncQuery<T>(HttpClientRequestTypeEnum.Get, null);
  }
  #endregion

  #region GetAsync(id) Method
  /// <summary>
  /// Get a single record from the data store
  /// </summary>
  /// <typeparam name="T">The type to return</typeparam>
  /// <param name="id"></param>
  /// <returns>An HttpDataResponse object</returns>
  public async Task<DataResponse<T>> GetAsync<T>(string urlPath, string id) where T : class
  {
    UrlPath = urlPath;
    UrlQuery = $"/{id}";
    if (!string.IsNullOrEmpty(AdditionalUrlData)) {
      UrlQuery += $"?{AdditionalUrlData}";
    }
    return await SubmitAsyncQuery<T>(HttpClientRequestTypeEnum.Get, null);
  }
  #endregion

  #region SearchAsync Method
  public async Task<DataResponse<TEntity>> SearchAsync<TEntity, TSearch>(string urlPath, TSearch? search) where TEntity : class
  {
    UrlPath = urlPath;
    if (!UrlPath.Contains(UrlSearchRoute)) {
      UrlPath += UrlSearchRoute;
    }
    UrlQuery = BuildUrlQueryParameters(search, new List<string>() { "SortExpression" });

    return await SubmitAsyncQuery<TEntity>(HttpClientRequestTypeEnum.Get, null);
  }
  #endregion

  #region CountAsync Method
  public async Task<DataResponse<T>> CountAsync<T, TSearch>(string urlPath, TSearch? search) where TSearch : class
  {
    UrlPath = urlPath;
    if (!UrlPath.Contains(UrlCountRoute)) {
      UrlPath += UrlCountRoute;
    }
    UrlQuery = BuildUrlQueryParameters(search, new List<string>() { "SortExpression" });

    return await SubmitAsyncQuery<T>(HttpClientRequestTypeEnum.Count, default);
  }
  #endregion

  #region PostAsync Method
  public async Task<DataResponse<T>> PostAsync<T>(string urlPath, T? entity) where T : class
  {
    UrlPath = urlPath;
    UrlQuery = string.Empty;
    if (!string.IsNullOrEmpty(AdditionalUrlData)) {
      UrlQuery += $"?{AdditionalUrlData}";
    }
    return await SubmitAsyncQuery(HttpClientRequestTypeEnum.Post, entity);
  }
  #endregion

  #region PutAsync Method
  public async Task<DataResponse<T>> PutAsync<T>(string urlPath, string id, T? entity) where T : class
  {
    UrlPath = urlPath;
    UrlQuery = $"/{id}";
    if (!string.IsNullOrEmpty(AdditionalUrlData)) {
      UrlQuery += $"?{AdditionalUrlData}";
    }
    return await SubmitAsyncQuery(HttpClientRequestTypeEnum.Put, entity);
  }
  #endregion

  #region DeleteAsync Method
  public async Task<DataResponse<T>> DeleteAsync<T>(string urlPath, string id) where T : class
  {
    UrlPath = urlPath;
    UrlQuery = $"/{id}";
    if (!string.IsNullOrEmpty(AdditionalUrlData)) {
      UrlQuery += $"?{AdditionalUrlData}";
    }
    return await SubmitAsyncQuery<T>(HttpClientRequestTypeEnum.Delete, null);
  }
  #endregion

  #region SubmitAsyncQuery Method
  protected async Task<DataResponse<T>> SubmitAsyncQuery<T>(HttpClientRequestTypeEnum requestType, T? payload)
  {
    DataResponse<T>? ret = new();
    HttpResponseMessage? resp = null;

    if (HClient == null) {
      ret.StatusMessage = "HttpClient is null.";
      ret.ResultMessage = "HttpClient is null.";
      ret.StatusCode = HttpStatusCode.BadRequest;
    }
    else {
      // Set the base address property, if not already
      if (string.IsNullOrEmpty(BaseWebAddress)) {
        BaseWebAddress = HClient?.BaseAddress?.AbsoluteUri ?? string.Empty;
      }
      // Make sure the BaseAddress does not end with a forward slash
      BaseWebAddress = BaseWebAddress.EndsWith('/') ? BaseWebAddress[..^1] : BaseWebAddress;

      if (HClient?.BaseAddress?.AbsoluteUri != BaseWebAddress + "/") {
        if (HClient != null) {
          // Put back the fixed up BaseWebAddress
          HClient.BaseAddress = new Uri(BaseWebAddress);
        }
      }

      // Set any Authorization Token
      SetAuthTokenHeader(HClient);

      // Make sure all URL parts are ready to submit
      FixUrlParts();

      // Reset Messages
      base.ResetMessages();

      try {
        // Open Path and Verify a Valid URI Address
        switch (requestType) {
          case HttpClientRequestTypeEnum.Get:
          case HttpClientRequestTypeEnum.Count:
            if (HClient != null) {
              resp = await HClient.GetAsync(UrlPath + UrlQuery);
            }
            break;
          case HttpClientRequestTypeEnum.Post:
            if (HClient != null) {
              resp = await HClient.PostAsJsonAsync(UrlPath + UrlQuery, payload);
            }
            break;
          case HttpClientRequestTypeEnum.Put:
            if (HClient != null) {
              resp = await HClient.PutAsJsonAsync(UrlPath + UrlQuery, payload);
            }
            break;
          case HttpClientRequestTypeEnum.Delete:
            if (HClient != null) {
              resp = await HClient.DeleteAsync(UrlPath + UrlQuery);
            }
            break;
        }
        if (resp != null) {
          // Store the response object into DataResponse object
          ret.StatusCode = resp.StatusCode;
          ret.StatusMessage = resp.ReasonPhrase;
          // Was the call successful?
          if (resp.IsSuccessStatusCode) {
            // Write out response information
            resp.EnsureSuccessStatusCode().WriteRequestToConsole();
            try {
              // Convert return value into a DataResponse object
              ret = await resp.Content.ReadFromJsonAsync<DataResponse<T>>();
              // See if a null was returned
              ret ??= new() {
                StatusCode = HttpStatusCode.InternalServerError,
                LastException = new("Object returned from Web API call is null"),
                ResultMessage = "Object returned from Web API call is null"
              };
            }
            catch (Exception ex) {
              ret = new() {
                StatusCode = HttpStatusCode.InternalServerError,
                LastException = ex,
                ResultMessage = "Error attempting to convert JSON into C# object"
              };
            }
          }
          else if (resp.StatusCode == HttpStatusCode.BadRequest) {
            // Validation Errors
            ValidationError? val = await resp.Content.ReadFromJsonAsync<ValidationError>();
            if (val != null) {
              val.DictionaryToValidationMessages();
              throw new ValidationException(val);
            }
          }
          else {
            // Store exception info into DataResponse object
            ret = new() {
              StatusCode = resp == null ? HttpStatusCode.BadRequest : resp.StatusCode,
              LastException = new ApplicationException("Response status code is NOT successful"),
              ResultMessage = "Response status code is NOT successful"
            };
          }
        }
        else {
          // Store exception info into DataResponse object
          ret = new() {
            StatusCode = HttpStatusCode.InternalServerError,
            LastException = new("HttpResponseMessage Object is null"),
            ResultMessage = "HttpResponseMessage object is null"
          };
        }
      }
      catch (ValidationException) {
        // Rethrow the Validation Exception
        throw;
      }
      catch (Exception ex) {
        // Store the last exception
        LastException = ex;
        // Store exception info into DataResponse object
        ret = new() {
          StatusCode = HttpStatusCode.InternalServerError,
          LastException = LastException,
          ResultMessage = ex.Message
        };
      }

      // Set DataResponse Message
      ret.StatusMessage = string.IsNullOrEmpty(ret.StatusMessage) ? Enum.GetName(ret.StatusCode) : ret.StatusMessage;
    }

    return ret;
  }
  #endregion
}
