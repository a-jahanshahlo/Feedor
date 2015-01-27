using System;

namespace RSSFeed.Service.Interface
{
    internal interface IUrlService
    {
        bool IsValidUri(string uri);
        string GetPort(Uri newUr);
        string GetAbsolutePath(Uri newUr);

    }
}