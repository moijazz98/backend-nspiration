namespace Nspiration.Response
{
    public class FolderSectionListResponse
    {
        public long ProjectId { get; set; }
        public long TypeId { get; set; }
        public long FolderId { get; set; }
        public string Name { get; set; }
        public List<SectionSubListResponse> SectionSubList { get; set; }
    }
}
