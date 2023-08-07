namespace Nspiration.Request
{
    public class FolderRequest
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public long CreatedBy { get; set; }
    }
    public class DeleteFolderRequest
    {
        public List<long> FolderId { get; set; }
        public long ModifiedBy { get; set; }
    }
    public class RenameFolderRequest
    {
        public long FolderId { get; set; }
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public long ModifiedBy { get; set;}
    }
}
