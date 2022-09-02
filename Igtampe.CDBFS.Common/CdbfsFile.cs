namespace Igtampe.CDBFS.Common {
    public class CdbfsFile {

        public string Name { get; set; } = "";

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

    }
}