using Igtampe.CDBFS.Common;
using System.Data.SQLite;
using System.Data;

namespace Igtampe.CDBFS.Data {
    public class CdbfsSqliteDAO : ICdbfsDAO {

        public string SqliteFilename { get; set; }

        protected SQLiteConnection Conn { get; set; }

        public ConnectionState State => Conn.State;

        public CdbfsSqliteDAO(string SqliteFilename) {
            if (!SqliteFilename.ToLower().EndsWith(".sqlite")) { SqliteFilename += ".sqlite"; }
            this.SqliteFilename = SqliteFilename;
            Conn = CreateConn(SqliteFilename);
        }

        public async Task Open() {
            if (State != ConnectionState.Closed) { throw new InvalidOperationException("Connection is not openable at this time"); }
            await Conn.OpenAsync();
        }

        public static async Task<CdbfsSqliteDAO> CreateCdbfsSqliteFile(string SqliteFilename) {

            if (!SqliteFilename.ToLower().EndsWith(".sqlite")) { SqliteFilename += ".sqlite"; }

            Console.WriteLine($"Attempting to create Cdbfs Database at {SqliteFilename}");

            if (File.Exists(SqliteFilename)) { throw new ArgumentException($"File already exists at {SqliteFilename}"); }

            //Create a connection to this new file
            Console.WriteLine($"Creating Connection");
            SQLiteConnection Conn = CreateConn(SqliteFilename,true);

            Console.WriteLine($"Open sesame");
            await Conn.OpenAsync();

            Console.WriteLine($"Creating Tables");

            //Create the table(s)
            foreach (var T in CdbfsTablesSchema.Tables) {
                Console.WriteLine($"Creating Table {T.Name} with {T.Columns.Count} Column(s)");
                var Command = Conn.CreateCommand();
                Command.CommandText = T.ToSQLiteTableCreationScript(DbEngine.Sqlite);
                await Command.ExecuteNonQueryAsync();
                await Command.DisposeAsync();
            }

            Console.WriteLine($"Hasta la vista bebe");
            await Conn.CloseAsync();
            await Conn.DisposeAsync();

            return new(SqliteFilename);

        }

        private static SQLiteConnection CreateConn(string Filename, bool New = false) 
            => new($"Data Source={Filename}; Version=3;{(New ? "New=True;" : "")}Compress=True;");

        public async Task<byte[]> GetFile(string Path) {

            var P = CdbfsPath.ParsePath(Path);

            using var C = Conn.CreateCommand();
            C.CommandText = "SELECT Data FROM Files WHERE Name = @Name AND Path = @Path AND IsFolder=false";
            C.Parameters.Add("@Name", DbType.String).Value = P.ItemName;
            C.Parameters.Add("@Path", DbType.String).Value = P.ItemPath;

            using var R = await C.ExecuteReaderAsync();
            if (!R.HasRows) { throw new FileNotFoundException($"File {Path} was not found!"); }
            await R.ReadAsync();

            //Get the data
            using var M = new MemoryStream();
            await R.GetStream(0).CopyToAsync(M);

            return M.ToArray();
        }
        public async Task<CdbfsFolder> GetFiles(string Path="/") {

            //We don't have to parse the path. It should just exist.
            if (Path.EndsWith('/') && Path.Length!=1) { Path = Path[..^-1]; } //Trim an excess slash if there is one.
            if (Path != "/" && !await FolderExists(Path)) { throw new FileNotFoundException($"Directory {Path} was not found!"); }

            using var C = Conn.CreateCommand();
            C.CommandText = "Select Name, DateCreated, DateUpdated, IsFolder from Files where Path=@Path";
            C.Parameters.Add("@Path", DbType.String).Value = Path;

            using var R = await C.ExecuteReaderAsync();

            //For folder data, however, we do have to parse the path
            CdbfsPath P = CdbfsPath.ParsePath(Path);
            
            //Get the data
            CdbfsFolder F = new() { 
                Path=P.ItemPath,
                Name=string.IsNullOrWhiteSpace(P.ItemName) ? "ROOT" : P.ItemName,
            };

            while (await R.ReadAsync()) {
                CdbfsFile CF = new() {
                    Name = R.GetString(0),
                    DateCreated = R.GetDateTime(1),
                    DateUpdated = R.GetDateTime(2)
                };

                if (R.GetBoolean(3)) {
                    F.Subfolders.Add(CF);
                } else { F.Files.Add(CF);
                }
            }

            return F;
        }

        public async Task CreateFile(string Path, byte[] Data) => await AddItem(Path, false, Data);


        public async Task UpdateFile(string Path, byte[] Data) {
            if (!await FileExists(Path)) { throw new FileNotFoundException($"File {Path} was not found on this CDBFS and cannot be updated"); }

            var P = CdbfsPath.ParsePath(Path);

            using var C = Conn.CreateCommand();
            C.CommandText = "Update Files set DateUpdated = @DateUpdated, Data = @Data where Name = @Name and Path=@Path and IsFolder=false";

            C.Parameters.Add("@Name", DbType.String).Value = P.ItemName;
            C.Parameters.Add("@Path", DbType.String).Value = P.ItemPath;
            C.Parameters.Add("@DateUpdated", DbType.DateTime).Value = DateTime.UtcNow;
            C.Parameters.Add("@Data", DbType.Binary).Value = Data;

            await C.ExecuteNonQueryAsync();
        }


        public async Task DeleteFile(string Path) => await DeleteItem(Path, false);

        public async Task<bool> Exists(string Path)
            => await CheckPathCol0Over0("select count(Name) from Files where Name = @Name and Path = @Path", Path);

        public async Task<bool> FolderExists(string Path)
            => await CheckPathCol0Over0("select count(Name) from Files where Name = @Name and Path = @Path AND IsFolder=true", Path);

        public async Task<bool> FileExists(string Path)
            => await CheckPathCol0Over0("select count(Name) from Files where Name = @Name and Path = @Path AND IsFolder=false", Path);

        private async Task<bool> CheckPathCol0Over0(string Query, string Path) {
            if (Path == "/" || string.IsNullOrWhiteSpace(Path)) { return true; }
            
            var P = CdbfsPath.ParsePath(Path);

            using var C = Conn.CreateCommand();
            C.CommandText = Query;
            C.Parameters.Add("@Name", DbType.String).Value = P.ItemName;
            C.Parameters.Add("@Path", DbType.String).Value = P.ItemPath;

            using var R = await C.ExecuteReaderAsync();
            if (!R.HasRows) { throw new InvalidOperationException("This isn't supposed to happen"); }
            await R.ReadAsync();

            return R.GetInt32(0) != 0;
        }


        public async Task RenameFile(string Path, string NewFilename) {
            if (!await FileExists(Path)) { throw new FileNotFoundException($"File {Path} was not found on this CDBFS and cannot be updated"); }

            var P = CdbfsPath.ParsePath(Path);

            using var C = Conn.CreateCommand();
            C.CommandText = "Update Files set Name = @NewName where Name = @Name and Path = @Path";

            C.Parameters.Add("@Name", DbType.String).Value = P.ItemName;
            C.Parameters.Add("@Path", DbType.String).Value = P.ItemPath;
            C.Parameters.Add("@NewName", DbType.String).Value = NewFilename;

            await C.ExecuteNonQueryAsync();
        }

        public async ValueTask DisposeAsync() {
            GC.SuppressFinalize(this);
            await Conn.DisposeAsync();
        }

        public async Task MoveFile(string Path, string NewPath) => throw new NotImplementedException();
        public async Task MoveFolder(string Path, string NewPath) => throw new NotImplementedException();
        public async Task CreateFolder(string Path) => await AddItem(Path, true);
        
        public async Task DeleteFolder(string Path) => await DeleteItem(Path, true);
        
        public Task RenameFolder(string Path, string NewFolderName) => throw new NotImplementedException();

        public async Task AddItem(string Path, bool Folder, byte[]? Data = null) {
            if (await Exists(Path)) { throw new ArgumentException($"Item at {Path} already exists on this CDBFS and cannot be created"); }

            var P = CdbfsPath.ParsePath(Path);

            using var C = Conn.CreateCommand();
            C.CommandText = "insert into Files (Name, Path, DateCreated, DateUpdated, Data, IsFolder)  values (@Name, @Path, @DateCreated, @DateUpdated, @Data, @IsFolder)";

            C.Parameters.Add("@Name", DbType.String).Value = P.ItemName;
            C.Parameters.Add("@Path", DbType.String).Value = P.ItemPath;
            C.Parameters.Add("@DateCreated", DbType.DateTime).Value = DateTime.UtcNow;
            C.Parameters.Add("@DateUpdated", DbType.DateTime).Value = DateTime.UtcNow;
            C.Parameters.Add("@Data", DbType.Binary).Value = Data;
            C.Parameters.Add("@IsFolder", DbType.Boolean).Value = Folder;

            await C.ExecuteNonQueryAsync();

        }

        public async Task DeleteItem(string Path, bool Folder) {
            var P = CdbfsPath.ParsePath(Path);

            using var C = Conn.CreateCommand();
            C.CommandText = "Delete from Files where Name = @Name AND Path=@Path AND IsFolder=@IsFolder";

            C.Parameters.Add("@Name", DbType.String).Value = P.ItemName;
            C.Parameters.Add("@Path", DbType.String).Value = P.ItemPath;
            C.Parameters.Add("@IsFolder", DbType.Boolean).Value = Folder;

            await C.ExecuteNonQueryAsync();

            if (Folder) {

                using var C2 = Conn.CreateCommand();
                C2.CommandText = "Delete from Files where Path like @Path ";

                //Unlike last one, this one has to be like this
                C2.Parameters.Add("@Path", DbType.String).Value = P.ItemPath + '%';
                await C2.ExecuteNonQueryAsync();
            }            
        }
    }
}
