using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ContentDetailInfo
    {
        public ContentDetailInfo() { }

        public ContentDetailInfo(string appCode, Guid id, Guid userId, Guid contentTypeId, string title, string keyword, string descr, string contentText, byte openness, int sort, DateTime recordDate, DateTime lastUpdatedDate)
        {
            this.AppCode = appCode;
            this.Id = id;
            this.UserId = userId;
            this.ContentTypeId = contentTypeId;
            this.Title = title;
            this.Keyword = keyword;
            this.Descr = descr;
            this.ContentText = contentText;
            this.Openness = openness;
            this.Sort = sort;
            this.RecordDate = recordDate;
            this.LastUpdatedDate = lastUpdatedDate;
        }

        public string AppCode { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ContentTypeId { get; set; }
        public string Title { get; set; }
        public string Keyword { get; set; }
        public string Descr { get; set; }
        public string ContentText { get; set; }
        public byte Openness { get; set; }
        public int Sort { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
