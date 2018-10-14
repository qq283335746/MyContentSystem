using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ContentFileInfo
    {
        public ContentFileInfo() { }

        public ContentFileInfo(Guid id, string appCode, Guid userId, Guid contentId, string fileName, int fileSize, string fileExt, string fileUrl, string viewUrl, DateTime lastUpdatedDate)
        {
            this.Id = id;
            this.AppCode = appCode;
            this.UserId = userId;
            this.ContentId = contentId;
            this.FileName = fileName;
            this.FileSize = fileSize;
            this.FileExt = fileExt;
            this.FileUrl = fileUrl;
            this.ViewUrl = viewUrl;
            this.LastUpdatedDate = lastUpdatedDate;
        }

        public Guid Id { get; set; }
        public string AppCode { get; set; }
        public Guid UserId { get; set; }
        public Guid ContentId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileExt { get; set; }
        public string FileUrl { get; set; }
        public string ViewUrl { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
