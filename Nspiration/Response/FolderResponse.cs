namespace Nspiration.Response
{
    public class FolderResponse
    {
        public long? Id { get; set; }
        public long? ProjectId { get; set; }
        public string? Name { get; set; }
        public List<SectionResponseList>? SectionResponseList { get; set; }
    }
    public class SectionResponseList
    {
        public long SectionId { get; set; }
        public long? FolderId { get; set; }
        public string? PathName { get; set; }
        public string? ShadeName { get; set; }
        public string? ShadeCode { get; set; }
    }
    public class FolderResponseWithSection
    {
        public List<FolderResponse>? FolderResponse { get; set; }
        public List<SectionResponseList>? SectionResponse { get; set; }
    }
}
