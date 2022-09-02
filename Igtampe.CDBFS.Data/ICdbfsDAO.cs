using Igtampe.CDBFS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igtampe.CDBFS.Data {
    public interface ICdbfsDAO : IAsyncDisposable {

        public Task Open();

        public Task<List<CdbfsFile>> GetFiles();

        public Task<byte[]> GetFile(string Filename);

        public Task CreateFile(string Filename, byte[] Data);

        public Task UpdateFile(string Filename, byte[] Data);

        public Task DeleteFile(string Filename);

        public Task RenameFile(string Filename, string NewFilename);

        public Task<bool> FileExists(string Filename);

    }
}
