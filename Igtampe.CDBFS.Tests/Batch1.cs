using Igtampe.CDBFS.Common;
using Igtampe.CDBFS.Data;

namespace Igtampe.CDBFS.Tests {
    public class Tests {

        const string DbFile = "cdbfs1.sqlite";

        [SetUp]
        public async Task Setup() {
            if (File.Exists(DbFile)) { File.Delete(DbFile); }
            var D = await CdbfsSqliteDAO.CreateCdbfsSqliteFile(DbFile);
            await D.DisposeAsync();
        }

        [Test]
        public async Task OpeningWorks() {
            ICdbfsDAO D = new CdbfsSqliteDAO(DbFile);
            await D.Open();

            await D.DisposeAsync();
        }

        [Test]
        public async Task CreatedFileExists() {
            ICdbfsDAO D = new CdbfsSqliteDAO(DbFile);
            await D.Open();

            const string Filename = "I1";

            Assert.That(await D.FileExists(Filename), Is.False);

            //Add the file
            await D.CreateFile(Filename, Properties.Resources.I1);

            Assert.That(await D.FileExists(Filename), Is.True);
            Assert.That((await D.GetFiles()), Has.Count.EqualTo(1));
            Assert.That((await D.GetFiles())[0].Name, Is.EqualTo(Filename));

            await D.DisposeAsync();
        }

        [Test]
        public async Task CreatedFileMatchesContent() {
            ICdbfsDAO D = new CdbfsSqliteDAO(DbFile);
            await D.Open();

            const string Filename = "I1";

            Assert.That(await D.FileExists(Filename), Is.False);

            //Add the file and immediately retrieve it
            await D.CreateFile(Filename, Properties.Resources.I1);
            var Data = await D.GetFile(Filename);

            Assert.That(Enumerable.SequenceEqual(Data, Properties.Resources.I1), Is.True);
            
            await D.DisposeAsync();
        }

        [Test]
        public async Task UpdatedFileMatchesContent() {
            ICdbfsDAO D = new CdbfsSqliteDAO(DbFile);
            await D.Open();

            const string Filename = "I1";

            Assert.That(await D.FileExists(Filename), Is.False);

            //Add the file and immediately retrieve it
            await D.CreateFile(Filename, Properties.Resources.I1);
            var Data1 = await D.GetFile(Filename);
            
            //Ensure data saved correctly once
            Assert.That(Enumerable.SequenceEqual(Data1, Properties.Resources.I1), Is.True);

            await D.UpdateFile(Filename, Properties.Resources.I2);
            var Data2 = await D.GetFile(Filename);

            Assert.That(Enumerable.SequenceEqual(Data2, Properties.Resources.I2), Is.True);

            await D.DisposeAsync();
        }

        [Test]
        public async Task DeletedFileDoesNotExist() {
            ICdbfsDAO D = new CdbfsSqliteDAO(DbFile);
            await D.Open();

            const string Filename = "I1";

            Assert.That(await D.FileExists(Filename), Is.False);

            //Add the file
            await D.CreateFile(Filename, Properties.Resources.I1);

            Assert.That(await D.FileExists(Filename), Is.True);

            await D.DeleteFile(Filename);

            Assert.That(await D.FileExists(Filename), Is.False);

            await D.DisposeAsync();
        }

        [TearDown]
        public void Teardown() {
            if (File.Exists(DbFile)) { File.Delete(DbFile); }
        }
    }
}