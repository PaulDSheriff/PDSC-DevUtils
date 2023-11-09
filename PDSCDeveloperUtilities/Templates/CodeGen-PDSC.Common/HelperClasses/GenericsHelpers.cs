namespace PDSC.Common;

public static class GenericsHelpers
{
  public static T ChangeType<T>(object? o)
  {
    Type conversionType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
    if (conversionType.Name == "Guid") {
      string? value = Convert.ToString(o);
      if (!string.IsNullOrEmpty(value) && Guid.TryParse(value, out Guid tmp)) {
        tmp = Guid.Parse(value);
        tmp = tmp.Equals(Guid.Empty) ? Guid.NewGuid() : tmp;
        return (T)Convert.ChangeType(tmp, conversionType);
      }
      else {
#pragma warning disable CS8603 // Possible null reference return.
        return (T)Convert.ChangeType(Guid.NewGuid(), conversionType);
#pragma warning restore CS8603 // Possible null reference return.
      }
    }
    else {
      if (o == null) {
#pragma warning disable CS8603 // Possible null reference return.
        return default;
#pragma warning restore CS8603 // Possible null reference return.
      }
      else {
        return (T)Convert.ChangeType(o, conversionType);
      }
    }
  }
}
