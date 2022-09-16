using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Igtampe.CDBFS.Common {
    public class CdbfsFile : CdbfsItem {
        
        /// <summary>Whether or not this file is loaded </summary>
        public bool IsLoaded => DataHolder != null;

        /// <summary>Data of this file</summary>
        [JsonIgnore]
        [NotMapped]
        public byte[] Data {
            get => DataHolder?.Data ?? Array.Empty<byte>();
            set {
                if (DataHolder != null) { DataHolder.Data = value; } 
                else DataHolder = new() { Data = value };
            }
        }

        /// <summary>Data holder of this file</summary>
        [ForeignKey("CdbfsFileData")]
        [JsonIgnore]
        public CdbfsFileData? DataHolder { get; set; } = null;

    }
}
