using Igtampe.CDBFS.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Igtampe.CDBFS.Data {
    public class CdbfsSqliteContext : DbContext {

        public CdbfsSqliteContext(string Path) 
            : base(new DbContextOptionsBuilder<CdbfsSqliteContext>().UseSqlite($"Data Source={Path};Pooling=False;").Options) 
            => Database.EnsureCreated();

        /// <summary>Creates a CDBFS Sqlite File </summary>
        public static void CreateCdbfsSqliteFile(string Path) => throw new NotImplementedException("This isn't openable");

        /// <summary>Overrides on model creation to remove the plural convention</summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //This will singularize all table names
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()) { entityType.SetTableName(entityType.DisplayName()); }
        }

        public DbSet<CdbfsItem> CdbfsItem { get; set; }

        public DbSet<CdbfsFile> CdbfsFile { get; set; }

        public DbSet<CdbfsFolder> CdbfsFolder { get; set; }

        protected DbSet<CdbfsFileData> CdbfsFileData { get; set; }


        public async Task<List<CdbfsItem>> GetItems(string Path = "/") => await GetAt(CdbfsItem,Path);
        public async Task<List<CdbfsFile>> GetFiles(string Path = "/") => await GetAt(CdbfsFile, Path);
        public async Task<List<CdbfsFolder>> GetFolders(string Path = "/") => await GetAt(CdbfsFolder, Path);

        private static async Task<List<E>> GetAt<E>(IQueryable<E> Collection, string Path) where E : CdbfsItem {
            if (!Path.EndsWith('/')) { Path += "/"; }
            return await Collection.Where(A => A.FolderPath == Path).ToListAsync();
        }

        public async Task<byte[]> GetFileData(string Path) => (await GetFile(new(Path), true)).Data;
        
        public async Task CreateFile(string Path, byte[] Data) {

            if (await FileExists(Path)) { throw new InvalidOperationException("File Already Exists!"); }

            var P = new CdbfsPath(Path);

            var F = new CdbfsFile() {
                Name = P.ItemName,
                FolderPath = P.ItemPath,
                Data = Data,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            Add(F);
            await SaveChangesAsync();
        }

        public async Task UpdateFile(string Path, byte[] Data) {
            var F = await GetFile(new(Path));
            F.Data = Data;
            F.DateUpdated = DateTime.UtcNow;
            
            Update(F);
            await SaveChangesAsync();
        }

        public async Task DeleteFile(string Path) {
            var F = await GetFile(new(Path),true);
            Remove(F.DataHolder!);
            Remove(F);
            await SaveChangesAsync();
        }

        public async Task RenameFile(string Path, string NewFilename) {
            if (await FileExists(NewFilename)) { throw new InvalidOperationException("A file with that name already exists!"); }
            var F = await GetFile(new(Path));
            F.Name = NewFilename;
            
            CdbfsFile.Update(F);
            await SaveChangesAsync();
        }

        public async Task<bool> FileExists(string Filename) => await CdbfsFile.AnyAsync(A => A.Name == Filename);

        private async Task<CdbfsFile> GetFile(CdbfsPath Path, bool LoadData = false) {

            IQueryable<CdbfsFile> C = CdbfsFile;
            if (LoadData) { C = C.Include(A => A.DataHolder); }

            var F = await C.FirstOrDefaultAsync(A => A.Name == Path.ItemName && A.FolderPath==Path.ItemPath);
            return F is null ? throw new FileNotFoundException("File was not found!") : F;

        }
    }
}
