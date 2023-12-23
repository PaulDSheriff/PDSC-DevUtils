using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PDSC.Common;

/// <summary>
/// Almost all classes in your application will eventually inherit from this class. It contains properties that are very useful such as LastException, LastErrorMessage, UserName and many others.
/// </summary>
public class CommonBase : INotifyPropertyChanged
{
  #region Constructor
  /// <summary>
  /// Constructor for CommonBase
  /// </summary>
  public CommonBase()
  {
    Init();
  }
  #endregion

  #region Init Method
  /// <summary>
  /// Initialize this class
  /// </summary>
  public virtual void Init()
  {
    UserName = "unknown";
  }
  #endregion

  #region Private Variables
  private string _UserName = "unknown";
  private string _EntityAsJson = string.Empty;
  private string _InfoMessage = string.Empty;
  private string _LastErrorMessage = string.Empty;
  private Exception? _LastException = null;
  private string _BearerToken = string.Empty;
  private string _BaseWebAddress = string.Empty;
  private string _AdditionalUrlData = string.Empty;
  #endregion

  #region Public Properties
  /// <summary>
  /// Get/Set the current user name for logging purposes
  /// </summary>
  [NotMapped]
  //[JsonIgnore]
  public string UserName
  {
    get { return _UserName; }
    set
    {
      _UserName = value;
      RaisePropertyChanged(nameof(UserName));
    }
  }

  /// <summary>
  /// Get/Set the current object as JSON
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  public string EntityAsJson
  {
    get { return _EntityAsJson; }
    set
    {
      _EntityAsJson = value;
      RaisePropertyChanged(nameof(EntityAsJson));
    }
  }

  /// <summary>
  /// Get/Set the last informational message to display
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  public string InfoMessage
  {
    get { return _InfoMessage; }
    set
    {
      _InfoMessage = value;
      RaisePropertyChanged(nameof(InfoMessage));
    }
  }

  /// <summary>
  /// Get/Set the class description as a singular word for messages
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  protected string ClassDescriptionSingular { get; set; } = string.Empty;

  /// <summary>
  /// Get/Set the class description as a plural word for messages
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  protected string ClassDescriptionPlural { get; set; } = string.Empty;

  /// <summary>
  /// Get/Set the message to display when the Repository object is not set
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  protected string RepositoryNotSetMessage { get; set; } = "The Repository Object is not Set.";

  /// <summary>
  /// Get/Set the last error message from the last operation
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  public string LastErrorMessage
  {
    get { return _LastErrorMessage; }
    set
    {
      _LastErrorMessage = value;
      RaisePropertyChanged(nameof(LastErrorMessage));
    }
  }

  /// <summary>
  /// Get/Set the last exception object from the last operation
  /// Sets the ErrorMessage to LastException.Message if ErrorMessage is blank
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  public Exception? LastException
  {
    get { return _LastException; }
    set
    {
      _LastException = value;
      if (_LastException != null) {
        if (string.IsNullOrEmpty(LastErrorMessage)) {
          LastErrorMessage = _LastException.Message;
        }
      }
      RaisePropertyChanged(nameof(LastException));
    }
  }


  /// <summary>
  /// Get/Set BearerToken
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  public string BearerToken
  {
    get { return _BearerToken; }
    set
    {
      _BearerToken = value;
      RaisePropertyChanged(nameof(BearerToken));
    }
  }

  /// <summary>
  /// Get/Set the base address such as https://www.pdsa.com
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  public string BaseWebAddress
  {
    get { return _BaseWebAddress; }
    set
    {
      _BaseWebAddress = value;
      RaisePropertyChanged(nameof(BaseWebAddress));
    }
  }

  /// <summary>
  /// Get/Set Additional Url Data to pass on the Web API calling line such as authId=12124. Commonly used when you need to search within a table where all customer data is located, and you only need a specific customers information. Set the customer id into this property.
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  public string AdditionalUrlData
  {
    get { return _AdditionalUrlData; }
    set
    {
      _AdditionalUrlData = value;
      RaisePropertyChanged(nameof(AdditionalUrlData));
    }
  }
  #endregion

  #region RaisePropertyChanged Method
  /// <summary>
  /// Event used to raise changes to any bound UI objects
  /// </summary>
  public event PropertyChangedEventHandler? PropertyChanged;

  public virtual void RaisePropertyChanged(string propertyName)
  {
    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
  #endregion

  #region ResetMessages Method
  protected virtual void ResetMessages()
  {
    EntityAsJson = string.Empty;
    InfoMessage = string.Empty;
    LastErrorMessage = string.Empty;
    LastException = null;
  }
  #endregion
}
