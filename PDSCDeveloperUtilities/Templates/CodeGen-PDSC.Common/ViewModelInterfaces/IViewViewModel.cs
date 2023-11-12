using System.Collections.ObjectModel;

namespace PDSC.Common;

/// <summary>
/// Interface for all view model classes.
/// </summary>
/// <typeparam name="TEntity">An entity class</typeparam>
/// <typeparam name="TSearch">A search entity class</typeparam>
public interface IViewViewModel<TEntity, TSearch> where TEntity : class
{
  Task<DataResponse<ObservableCollection<TEntity>>> GetAsync();
  Task<DataResponse<TEntity>> GetAsync(TEntity entity);

  Task<DataResponse<ObservableCollection<TEntity>>> SearchAsync();
  Task<DataResponse<ObservableCollection<TEntity>>> SearchAsync(TSearch search);

  Task<DataResponse<int>> CountAsync(TSearch? search);
}
