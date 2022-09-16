using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Igtampe.CDBFS.Common {

    /// <summary>A CDBFS File</summary>
    public abstract class CdbfsItem : AutomaticallyGeneratableIdentifiable {

        /// <summary>Name of this item</summary>
        public string Name { get; set; } = "";

        /// <summary>Path to the folder where this item is located in</summary>
        public string FolderPath { get; set; } = "/";

        /// <summary>Date this file was created</summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        /// <summary>Date this file was updated</summary>
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
        
    }
}