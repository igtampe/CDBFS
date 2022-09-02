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

        public async Task<byte[]> GetFile(string Filename) {
            using var C = Conn.CreateCommand();
            C.CommandText = "SELECT Data FROM Files WHERE Name = @Name";
            C.Parameters.Add("@Name", DbType.String).Value = Filename;

            using var R = await C.ExecuteReaderAsync();
            if (!R.HasRows) { throw new FileNotFoundException("File was not found!"); }
            await R.ReadAsync();

            //Get the data
            using var M = new MemoryStream();
            await R.GetStream(0).CopyToAsync(M);

            return M.ToArray();
        }
        public async Task<List<CdbfsFile>> GetFiles() {

            using var C = Conn.CreateCommand();
            C.CommandText = "Select Name, DateCreated, DateUpdated from Files";

            using var R = await C.ExecuteReaderAsync();
            
            //Get the data
            List<CdbfsFile> Files = new();
            while (await R.ReadAsync()) {
                Files.Add(new() { 
                    Name=R.GetString(0),
                    DateCreated = R.GetDateTime(1),
                    DateUpdated = R.GetDateTime(2)
                });
            }

            return Files;
        }

        public async Task CreateFile(string Filename, byte[] Data) {

            if (await FileExists(Filename)) { throw new ArgumentException($"File {Filename} already exists on this CDBFS and cannot be created"); }

            using var C = Conn.CreateCommand();
            C.CommandText = "insert into Files (Name, DateCreated, DateUpdated, Data)  values (@Name, @DateCreated, @DateUpdated, @Data)";

            C.Parameters.Add("@Name", DbType.String).Value=Filename;
            C.Parameters.Add("@DateCreated", DbType.DateTime).Value = DateTime.UtcNow;
            C.Parameters.Add("@DateUpdated", DbType.DateTime).Value = DateTime.UtcNow;
            C.Parameters.Add("@Data", DbType.Binary).Value = Data;

            await C.ExecuteNonQueryAsync();
            
        }


        public async Task UpdateFile(string Filename, byte[] Data) {
            if (!await FileExists(Filename)) { throw new FileNotFoundException($"File {Filename} was not found on this CDBFS and cannot be updated"); }

            using var C = Conn.CreateCommand();
            C.CommandText = "Update Files set DateUpdated = @DateUpdated, Data = @Data where Name = @Name";

            C.Parameters.Add("@Name", DbType.String).Value = Filename;
            C.Parameters.Add("@DateUpdated", DbType.DateTime).Value = DateTime.UtcNow;
            C.Parameters.Add("@Data", DbType.Binary).Value = Data;

            await C.ExecuteNonQueryAsync();
        }


        public async Task DeleteFile(string Filename) {
            using var C = Conn.CreateCommand();
            C.CommandText = "Delete from Files where Name = @Name";
            
            C.Parameters.Add("@Name", DbType.String).Value = Filename;

            await C.ExecuteNonQueryAsync();

        }

        public async Task<bool> FileExists(string Filename) {

            using var C = Conn.CreateCommand();
            C.CommandText = "select count(Name) from Files where Name = @Name";

            C.Parameters.Add("@Name", DbType.String).Value = Filename;

            using var R = await C.ExecuteReaderAsync();
            if (!R.HasRows) { throw new InvalidOperationException("This isn't supposed to happen"); }
            await R.ReadAsync();

            return R.GetInt32(0) != 0;

        }
        public async Task RenameFile(string Filename, string NewFilename) {
            if (!await FileExists(Filename)) { throw new FileNotFoundException($"File {Filename} was not found on this CDBFS and cannot be updated"); }

            using var C = Conn.CreateCommand();
            C.CommandText = "Update Files set Name = @NewName where Name = @Name";

            C.Parameters.Add("@Name", DbType.String).Value = Filename;
            C.Parameters.Add("@NewName", DbType.String).Value = NewFilename;

            await C.ExecuteNonQueryAsync();
        }

        public async ValueTask DisposeAsync() {
            GC.SuppressFinalize(this);
            await Conn.DisposeAsync();
        }

    }
}
