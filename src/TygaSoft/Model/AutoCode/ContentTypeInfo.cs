using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ContentTypeInfo
    {
        public ContentTypeInfo() { }

        public ContentTypeInfo(string appCode, Guid id, Guid userId, string coded, string named, Guid parentId, string step, string flagName, byte openness, int sort, string remark, DateTime recordDate, DateTime lastUpdatedDate)
        {
            this.AppCode = appCode;
            this.Id = id;
            this.UserId = userId;
            this.Coded = coded;
            this.Named = named;
            this.ParentId = parentId;
            this.Step = step;
            this.FlagName = flagName;
            this.Openness = openness;
            this.Sort = sort;
            this.Remark = remark;
            this.RecordDate = recordDate;
            this.LastUpdatedDate = lastUpdatedDate;
        }

        public string AppCode { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Coded { get; set; }
        public string Named { get; set; }
        public Guid ParentId { get; set; }
        public string Step { get; set; }
        public string FlagName { get; set; }
        public byte Openness { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
