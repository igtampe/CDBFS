namespace Igtampe.CDBFS.Common {

    /// <summary>A CDBFS File</summary>
    public class CdbfsFileData : AutomaticallyGeneratableIdentifiable{
        
        public CdbfsFile? Metadata { get; set; } = null;

        public byte[] Data { get; set; } = Array.Empty<byte>();

    }
}