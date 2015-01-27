using System.Collections.Generic;
using System.Web;
using RSSFeed.Domain.Poco;

namespace RSSFeed.Service.Interface
{
    public interface IFile
    {
        IList<UploadFilesResult> CreateList(); 
        void ReadFiles(HttpFileCollectionBase files);
        int UploadedFilesLength();
        void CopyTo(IList<UploadFilesResult> filesResults,int count);
        void SaveAll();        
           
        IList<HttpPostedFileBase> GetHttpPostedFile();    
        bool IsValidType(string contentType);
    }
}