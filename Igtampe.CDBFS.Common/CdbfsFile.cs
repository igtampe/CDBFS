using System.ComponentModel.DataAnnotations.Schema;

namespace Igtampe.CDBFS.Common {

    /// <summary>A CDBFS File</summary>
    public class CdbfsFile : AutomaticallyGeneratableIdentifiable {

        /// <summary>Name of this file</summary>
        public string Name { get; set; } = "";

        /// <summary>Date this file was created</summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        /// <summary>Date this file was updated</summary>
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>Whether or not this file is loaded </summary>
        public bool IsLoaded => DataHolder != null;

        /// <summary>Data of this file</summary>
        public byte[] Data {
            get => DataHolder?.Data ?? Array.Empty<byte>(); 
            set {
                if (DataHolder != null) { DataHolder.Data = value; } 
                else DataHolder = new() { Data = value };
            } 
        }

        /// <summary>Data holder of this file</summary>
        [ForeignKey("CdbfsFileData")]
        public CdbfsFileData? DataHolder { get; set; } = null;

    }
}