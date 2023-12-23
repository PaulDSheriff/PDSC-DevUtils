using System.Collections.ObjectModel;

namespace PDSC.Common;

/// <summary>
/// Interface for all view model classes.
/// </summary>
/// <typeparam name="TPK">Primary Key Data Type (int, string, guid, etc.)</typeparam>
/// <typeparam name="TEntity">An entity class</typeparam>
/// <typeparam name="TSearch">A search entity class</typeparam>
public interface IViewModel<TPK, TEntity, TSearch> where TEntity : class
{
  string BearerToken { get; set; }
  string BaseWebAddress { get; set; }
  string AdditionalUrlData { get; set; }

  Task<DataResponse<ObservableCollection<TEntity>>> GetAsync();
  Task<DataResponse<TEntity>> GetAsync(TPK id);

  Task<DataResponse<ObservableCollection<TEntity>>> SearchAsync();
  Task<DataResponse<ObservableCollection<TEntity>>> SearchAsync(TSearch search);

  Task<DataResponse<TEntity>> SaveAsync();

  Task<DataResponse<TEntity>> InsertAsync();
  Task<DataResponse<TEntity>> InsertAsync(TEntity? entity);

  Task<DataResponse<TEntity>> UpdateAsync();
  Task<DataResponse<TEntity>> UpdateAsync(TPK id, TEntity? entity);

  Task<DataResponse<TEntity>> DeleteAsync(TPK id);

  Task<DataResponse<int>> CountAsync(TSearch? search);
}
