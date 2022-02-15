using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BicycleRepairManagement
{
    public class String2ImagePath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return $"/Images/placeholder.png";
            } else if (File.Exists(value.ToString())) {
                return value;
            } else
            {
                return $"/Images/placeholder.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Int2String : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((uint) value)
            {
                case 0: return "Female";
                    case 1: return "Male";
                    case 2: return "Diverse";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString().First())
            {
                case 'F': return 0;
                case 'M': return 1;
                case 'D': return 2;
            }
            return null;
        }
    }

    public class Int2BicycleType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((uint) value)
            {
                case 0: return "City bike";
                case 1: return "Mountain bike";
                case 2: return "Road bike";
                case 3: return "Gravel bike";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "City bike": return 0;
                case "Mountain bike": return 1;
                case "Road bike": return 2;
                case "Gravel bike": return 3;
            }
            return null;
        }
    }

    public class DateTimeToDateOnlyString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime) value;
            return dateTime.ToString("dd'/'MM'/'yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (string) value;
            return DateTime.Parse(dateTime);
        }
    }
}
