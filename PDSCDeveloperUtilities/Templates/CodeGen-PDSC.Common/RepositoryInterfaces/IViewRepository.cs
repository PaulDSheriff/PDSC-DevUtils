namespace PDSC.Common;

/// <summary>
/// All repository classes that target Views in a database should inherit from this class.
/// </summary>
/// <typeparam name="TEntity">An entity class</typeparam>
/// <typeparam name="TSearch">A search entity class</typeparam>
public interface IViewRepository<TEntity, TSearch>
{
  DataResponseBase? ResponseObject { get; set; }
  string BearerToken { get; set; }
  string BaseWebAddress { get; set; }
  string AdditionalUrlData { get; set; }

  Task<List<TEntity>> GetAsync();
  Task<List<TEntity>> SearchAsync(TSearch search);
  Task<TEntity?> GetAsync(TEntity entity);

  Task<int> CountAsync(TSearch search);
}