using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ClassManager.Utils
{
    /// <summary>
    /// <see cref="float"/>与<see cref="string"/>之间的转换器。
    /// 例如：5.8f -> "￥5.80"
    /// </summary>
    public class MoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return String.Format("￥{0}", Math.Round((float)value, 2).ToString("0.00"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value as string == "")
                return 0f;
            return float.Parse((value as string).Substring(1));
        }
    }

    /// <summary>
    /// <see cref="string"/>与<see cref="DateTimeOffset"/>之间的转换器
    /// 例如："2017-8-15" -> DateTimeOffset(2017,8,15)
    /// </summary>
    public class DateConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] date = (value as string).Split('-');
            DateTime datetime = new DateTime(Int16.Parse(date[0]), Int16.Parse(date[1]), Int16.Parse(date[2]));
            return new DateTimeOffset(datetime);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTimeOffset) value;
            return String.Format("{0}-{1}-{2}", date.Year, date.Month, date.Day);
        }
    }
}
