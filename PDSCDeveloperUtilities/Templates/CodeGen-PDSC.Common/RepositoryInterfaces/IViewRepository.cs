namespace PDSC.Common;

/// <summary>
/// Interface for all view repository classes
/// </summary>
/// <typeparam name="TEntity">An entity class</typeparam>
/// <typeparam name="TSearch">A search entity class</typeparam>
public interface IViewRepository<TEntity, TSearch>
{
  DataResponseBase? ResponseObject { get; set; }

  Task<List<TEntity>> GetAsync();
  Task<List<TEntity>> SearchAsync(TSearch search);
  Task<TEntity?> GetAsync(TEntity entity);

  Task<int> CountAsync(TSearch search);
}