namespace Igtampe.CDBFS.Common {
    public class CdbfsFolder : Nameable{
        public string Name { get; set; } = "";

        public string Path { get; set; } = "";

        public List<string> Subfolders { get; set; } = new();

        public List<string> Files { get; set; } = new();
    }
}
