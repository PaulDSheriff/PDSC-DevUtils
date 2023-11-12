using System.Text.Json.Serialization;

namespace PDSC.Common;

/// <summary>
/// All search classes should inherit from this class
/// </summary>
public class SearchBase : CommonBase
{
  public SearchBase()
  {
    NoFilterAppliedMessage = "(none)";
  }

  private string? _SortExpression = string.Empty;
  private string _NoFilterAppliedMessage = string.Empty;
  private int _RowIndex = 0;

  /// <summary>
  /// Get/Set SortExpression
  /// </summary>
  public string? SortExpression
  {
    get { return _SortExpression; }
    set
    {
      _SortExpression = value;
      RaisePropertyChanged(nameof(SortExpression));
    }
  }

  /// <summary>
  /// Get/Set NoFilterAppliedMessage
  /// </summary>
  [JsonIgnore]
  public string NoFilterAppliedMessage
  {
    get { return _NoFilterAppliedMessage; }
    set
    {
      _NoFilterAppliedMessage = value;
      RaisePropertyChanged(nameof(NoFilterAppliedMessage));
    }
  }

  /// <summary>
  /// Get/Set RowIndex
  /// </summary>
  [JsonIgnore]
  public int RowIndex
  {
    get { return _RowIndex; }
    set
    {
      _RowIndex = value;
      RaisePropertyChanged(nameof(RowIndex));
    }
  }
}
