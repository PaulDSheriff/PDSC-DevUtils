namespace PDSC.Common;

/// <summary>
/// This class inherits from the DataResponseBase class and adds an additional property, Data. This Data property is where the payload is placed into when returning data from a Web API call. This Data, plus the other properties from the DataResponseBase class provide all the information needed to determine if a call was successful or not.
/// </summary>
/// <typeparam name="T">A data type</typeparam>
public class DataResponse<T> : DataResponseBase
{
  public T? Data { get; set; } = default;
}
