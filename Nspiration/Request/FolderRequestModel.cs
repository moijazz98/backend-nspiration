namespace Nspiration.Request
{
    public class FolderRequestModel
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public long CreatedBy { get; set; }
    }
    public class DeleteFolderRequestModel
    {
        public List<long> FolderId { get; set; }
        public long ModifiedBy { get; set; }
    }
    public class RenameFolderRequestModel
    {
        public long FolderId { get; set; }
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public long ModifiedBy { get; set;}
    }
}
