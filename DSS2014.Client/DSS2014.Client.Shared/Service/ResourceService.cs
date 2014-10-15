using DSS2014.Client.Portable.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;

namespace DSS2014.Client.Service
{
    public class ResourceService : IResourceService
    {
        private ResourceLoader _resLoader = new ResourceLoader();

        public string GetString(string key)
        {
            return _resLoader.GetString(key);
        }
    }
}
