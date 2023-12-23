namespace PDSC.Common;

/// <summary>
/// All repository classes that target Tables in a database should inherit from this class.
/// </summary>
/// <typeparam name="TPK">Primary Key Data Type (int, string, guid, etc.)</typeparam>
/// <typeparam name="TEntity">An entity class</typeparam>
/// <typeparam name="TSearch">A search entity class</typeparam>
public interface IRepository<TPK, TEntity, TSearch>
{
  DataResponseBase? ResponseObject { get; set; }
  string BearerToken { get; set; }
  string BaseWebAddress { get; set; }
  string AdditionalUrlData { get; set; }

  Task<List<TEntity>> GetAsync();
  Task<TEntity?> GetAsync(TPK id);
  Task<List<TEntity>> SearchAsync(TSearch search);
  Task<int> CountAsync(TSearch search);

  Task<TEntity?> InsertAsync(TEntity? entity);
  Task<TEntity?> UpdateAsync(TEntity? entity);
  Task<bool?> DeleteAsync(TEntity? entity);
}