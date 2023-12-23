using PDSC.Common.JWT;

namespace PDSC.Common;

/// <summary>
/// This class holds the common properties that are normally 
/// read in from the appsettings.json file of your application.
/// </summary>
public class SettingsBase : CommonBase
{
  #region Private Variables
  private string _ApplicationName = "Your Application Name Here";
  private string _BaseWebAPIService = "https://localhost:3000";
  private string _InfoMessageDefault = "Problem Attempting to {Verb} using the {ClassName} API. Please Contact Your System Administrator.";
  private DateTime _LoginDateTime = DateTime.Now;
  private string _CORSPolicyName = "CORSYourApplicationName";
  private int _RecordsPerPage = 10;
  #endregion

  #region Public Properties
  public JwtSettings JWTSettings { get; set; } = new();

  /// <summary>
  /// Get/Set the Application Name
  /// </summary>
  public string ApplicationName
  {
    get { return _ApplicationName; }
    set
    {
      _ApplicationName = value;
      RaisePropertyChanged(nameof(ApplicationName));
    }
  }

  /// <summary>
  /// Get/Set the Base Web API Service
  /// </summary>
  public string BaseWebAPIService
  {
    get { return _BaseWebAPIService; }
    set
    {
      _BaseWebAPIService = value;
      RaisePropertyChanged(nameof(BaseWebAPIService));
    }
  }

  /// <summary>
  /// Get/Set the Info Message Default
  /// </summary>
  public string InfoMessageDefault
  {
    get { return _InfoMessageDefault; }
    set
    {
      _InfoMessageDefault = value;
      RaisePropertyChanged(nameof(InfoMessageDefault));
    }
  }

  /// <summary>
  /// Get/Set the date/time logged in
  /// </summary>
  public DateTime LoginDateTime
  {
    get { return _LoginDateTime; }
    set
    {
      _LoginDateTime = value;
      RaisePropertyChanged(nameof(LoginDateTime));
    }
  }

  /// <summary>
  /// Get/Set the CORS Policy Name
  /// </summary>
  public string CORSPolicyName
  {
    get { return _CORSPolicyName; }
    set
    {
      _CORSPolicyName = value;
      RaisePropertyChanged(nameof(CORSPolicyName));
    }
  }

  /// <summary>
  /// Get/Set RecordsPerPage
  /// </summary>
  public int RecordsPerPage
  {
    get { return _RecordsPerPage; }
    set
    {
      _RecordsPerPage = value;
      RaisePropertyChanged(nameof(RecordsPerPage));
    }
  }
  #endregion
}
