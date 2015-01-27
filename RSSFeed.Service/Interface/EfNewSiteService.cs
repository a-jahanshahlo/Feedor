using System;
using System.Globalization;

namespace RSSFeed.Service.Interface
{
    internal class UrlService : IUrlService
    {

        public bool IsValidUri(string uri)
        {
            Uri validatedUri;
            return Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out validatedUri);
        }

        public string GetPort(Uri newUri)
        {
            return newUri.Port == 80 ? "" :":"+ newUri.Port.ToString(CultureInfo.InvariantCulture);
        }

        public string GetAbsolutePath(Uri newUr)
        {

        return  newUr.Scheme + "://" + newUr.Host + GetPort(newUr);
        }
    }
}