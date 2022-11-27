using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VolleyballScoreSheet.Converter
{
    class HiddenOrVissible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Visible")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
