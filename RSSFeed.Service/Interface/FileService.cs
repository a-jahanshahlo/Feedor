using System.Collections.Generic;
using System.IO;
using System.Web;
using RSSFeed.Domain.Poco;

namespace RSSFeed.Service.Interface
{
    public class FileService : IFile
    {
        private readonly IList<string> _validTypeList;
        private readonly IList<HttpPostedFileBase> _httpPostedFiles;
        private readonly IList<UploadFilesResult> _files;
        public FileService()
        {
            _files = new List<UploadFilesResult>();
            _httpPostedFiles = new List<HttpPostedFileBase>();
            _validTypeList = new List<string> { "image/jpeg", "images/jpeg", "application/octet-stream" };
        }

        public IList<UploadFilesResult> CreateList()
        {
            return new List<UploadFilesResult>();
        }


        public void ReadFiles(HttpFileCollectionBase files)
        {
            _httpPostedFiles.Clear();
            _files.Clear();
            foreach (string file in files)
            {
                var hpf = files[file] as HttpPostedFileBase;

                if (hpf.ContentLength == 0)
                    continue;
                if (!IsValidType(hpf.ContentType))
                    continue;

                _httpPostedFiles.Add(hpf);

                _files.Add(new UploadFilesResult()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }
        }


        public void CopyTo(IList<UploadFilesResult> filesResults, int count)
        {
            var filesresults = new UploadFilesResult[count];
            _files.CopyTo(filesresults,0);

            foreach (var item in filesresults)
            {
                filesResults.Add(item);
            }

        }


        public void SaveAll()
        {
            foreach (var hpf in _httpPostedFiles)
            {
                string savedFileName = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), Path.GetFileName(hpf.FileName));
                hpf.SaveAs(savedFileName);
            }
            _httpPostedFiles.Clear();
            _files.Clear();
        }


        public int UploadedFilesLength()
        {
            return this._httpPostedFiles.Count;
        }


        public bool IsValidType(string contentType)
        {
            return _validTypeList.Contains(contentType);
        }


        public IList<HttpPostedFileBase> GetHttpPostedFile()
        {
            return _httpPostedFiles;
        }
    }
}