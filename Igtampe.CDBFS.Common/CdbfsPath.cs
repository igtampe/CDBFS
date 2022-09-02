namespace Igtampe.CDBFS.Common {
    public class CdbfsPath {

        public string FolderPath { get; set; } = "";

        public string Filename { get; set; } = "";

        public static CdbfsPath ParsePath(string Path) {
            //find the index of the last /
            int LastSlash = Path.LastIndexOf('/');
            if (LastSlash == -1) {
                //This must be in the root directory. Though this is a badly formed path, we will still love it
                return new() { FolderPath="/", Filename=Path};
            }

            return new() {
                FolderPath = Path[..LastSlash], //Anything before the index of is the folder path
                Filename = Path[(LastSlash+1)..] //Anything after it is the filename
            };
        }

        public override string ToString() => $"{FolderPath}/{Filename}";

        public override bool Equals(object? obj) => obj is not null && obj is CdbfsPath P && P.ToString()==ToString();

        public override int GetHashCode() => HashCode.Combine(FolderPath, Filename);
    }
}
