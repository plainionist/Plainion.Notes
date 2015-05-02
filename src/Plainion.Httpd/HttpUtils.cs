using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Plainion.Httpd
{
    public static class HttpUtils
    {
        public static IDictionary<string, string> ParseQueryString( this Uri url )
        {
            return HttpUtility.ParseQueryString( url.Query ).ToDictionary();
        }

        public static IDictionary<string, string> ToDictionary( this NameValueCollection collection )
        {
            var dict = new Dictionary<string, string>();

            foreach( var key in collection.Keys.OfType<string>() )
            {
                dict[ key ] = collection[ key ];
            }

            return dict;
        }
    }
}
