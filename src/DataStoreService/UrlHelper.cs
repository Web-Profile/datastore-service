using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;

namespace DataStoreService
{
    internal static class UrlHelperExtensions
    {
        public static string Profile(this UrlHelper url, string id)
        {
            return url.Link("DefaultApi", 
                new {controller = "Profile", @id = id});
        }
    }
}
