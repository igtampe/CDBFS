namespace Igtampe.CDBFS.Common {
    public class CdbfsFolder : Nameable{
        public string Name { get; set; } = "";

        public string Path { get; set; } = "";

        public List<CdbfsFile> Subfolders { get; set; } = new();

        public List<CdbfsFile> Files { get; set; } = new();
    }
}
