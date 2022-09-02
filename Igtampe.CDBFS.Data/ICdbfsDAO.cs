using Igtampe.CDBFS.Common;
namespace Igtampe.CDBFS.Data {
    public interface ICdbfsDAO : IAsyncDisposable {

        public Task Open();

        public Task<List<CdbfsFile>> GetFiles(string Path = "/");

        public Task<byte[]> GetFile(string Path);

        public Task CreateFile(string Path, byte[] Data);

        public Task UpdateFile(string Path, byte[] Data);

        public Task DeleteFile(string Path);

        public Task RenameFile(string Path, string NewFilename);

        public Task<bool> FileExists(string Path);
        public Task<bool> FolderExists(string Path);
        public Task<bool> Exists(string Path);

        public Task MoveFile(string Path, string NewPath);
        public Task MoveFolder(string Path, string NewPath);

        public Task CreateFolder(string Path);
        public Task DeleteFolder(string Path);

        public Task RenameFolder(string Path, string NewFolderName);


    }
}
