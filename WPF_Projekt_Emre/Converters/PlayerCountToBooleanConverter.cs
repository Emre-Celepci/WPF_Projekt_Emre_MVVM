using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF_Projekt_Emre.Converters
{
    public class PlayerCountToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int selectedCount && parameter != null)
            {
                return selectedCount.ToString() == parameter.ToString();
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter != null)
            {
                return int.Parse(parameter.ToString()!);
            }

            return Binding.DoNothing;
        }
    }
}