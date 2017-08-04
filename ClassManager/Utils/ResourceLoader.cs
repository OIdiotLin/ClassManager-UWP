using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.Utils
{
    public class ResourceLoader
    {
        private static Windows.ApplicationModel.Resources.ResourceLoader loader = 
            new Windows.ApplicationModel.Resources.ResourceLoader();

        public static string GetString(string resourceKey)
        {
            return loader.GetString(resourceKey);
        }
    }
}
