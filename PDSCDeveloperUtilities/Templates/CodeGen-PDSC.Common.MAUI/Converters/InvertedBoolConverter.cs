﻿using System.Globalization;

namespace PDSC.Common.MAUI.Converters;

public class InvertedBoolConverter : IValueConverter
{
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    if (value != null) {
      return !(bool)value;
    }
    else {
      return true;
    }
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    return new object();
  }
}
