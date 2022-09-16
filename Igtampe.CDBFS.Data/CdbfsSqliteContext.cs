using Igtampe.CDBFS.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Igtampe.CDBFS.Data {
    public class CdbfsSqliteContext : DbContext, ICdbfsDAO {

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

        public DbSet<CdbfsFile> CdbfsFile { get; set; }

        private DbSet<CdbfsFileData> CdbfsFileData { get; set; }

        public Task Open() => throw new NotImplementedException("This isn't openable");

        public async Task<List<CdbfsFile>> GetFiles() => await CdbfsFile.ToListAsync();

        public async Task<byte[]> GetFile(string Filename) => (await GetFileObject(Filename, true)).Data;

        public async Task CreateFile(string Filename, byte[] Data) {

            if (await FileExists(Filename)) { throw new InvalidOperationException("File Already Exists!"); }

            var F = new CdbfsFile() {
                Name = Filename,
                Data = Data,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            CdbfsFile.Add(F);
            await SaveChangesAsync();
        }

        public async Task UpdateFile(string Filename, byte[] Data) {
            var F = await GetFileObject(Filename);
            F.Data = Data;
            F.DateUpdated = DateTime.UtcNow;
            
            CdbfsFile.Update(F);
            await SaveChangesAsync();
        }

        public async Task DeleteFile(string Filename) {
            var F = await GetFileObject(Filename,true);
            Remove(F.DataHolder);
            Remove(F);
            await SaveChangesAsync();
        }

        public async Task RenameFile(string Filename, string NewFilename) {
            if (await FileExists(NewFilename)) { throw new InvalidOperationException("A file with that name already exists!"); }
            var F = await GetFileObject(Filename);
            F.Name = NewFilename;
            
            CdbfsFile.Update(F);
            await SaveChangesAsync();
        }

        public async Task<bool> FileExists(string Filename) => await CdbfsFile.AnyAsync(A => A.Name == Filename);

        private async Task<CdbfsFile> GetFileObject(string Filename, bool LoadData = false) {

            IQueryable<CdbfsFile> C = CdbfsFile;
            if (LoadData) { C = C.Include(A => A.DataHolder); }

            var F = await C.FirstOrDefaultAsync(A => A.Name == Filename);
            return F is null ? throw new FileNotFoundException("File was not found!") : F;

        }
    }
}
