using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ClassManager.Utils
{
    public static class MyVisualTreeHelper
    {
        public static T SearchVisualTree<T>(DependencyObject tarElem) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(tarElem);
            if (count == 0)
                return null;

            for (int i = 0; i < count; ++i)
            {
                var child = VisualTreeHelper.GetChild(tarElem, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var res = SearchVisualTree<T>(child);
                    if (res != null)
                    {
                        return res;
                    }
                }
            }
            return null;
        }

    }
}
