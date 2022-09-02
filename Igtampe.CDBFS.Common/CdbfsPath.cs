namespace Igtampe.CDBFS.Common {
    public class CdbfsPath {

        public string ItemPath { get; set; } = "";

        public string ItemName { get; set; } = "";

        public static CdbfsPath ParsePath(string Path) {
            //find the index of the last /
            int LastSlash = Path.LastIndexOf('/');
            if (LastSlash == -1) {
                //This must be in the root directory. Though this is a badly formed path, we will still love it
                return new() { ItemPath="/", ItemName=Path};
            }

            return new() {
                ItemPath = string.IsNullOrWhiteSpace(Path[..LastSlash]) ? "/" : Path[..LastSlash], //Anything before the index of is the folder path
                ItemName = Path[(LastSlash+1)..] //Anything after it is the filename
            };
        }

        public override string ToString() => $"{ItemPath}/{ItemName}";

        public override bool Equals(object? obj) => obj is not null && obj is CdbfsPath P && P.ToString()==ToString();

        public override int GetHashCode() => HashCode.Combine(ItemPath, ItemName);
    }
}
