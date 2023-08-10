namespace Nspiration.Request
{
    public class FolderRequest
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
    }
    public class DeleteFolderRequest
    {
        public List<long> FolderId { get; set; }
        public List<long>? SectionId { get; set; }
        public int ModifiedBy { get; set; }
    }
    public class RenameFolderRequest
    {
        public long FolderId { get; set; }
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public int ModifiedBy { get; set;}
    }
}
