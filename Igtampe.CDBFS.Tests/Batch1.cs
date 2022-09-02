using Igtampe.CDBFS.Data;

namespace Igtampe.CDBFS.Tests {
    public class Tests {

        const string DbFile = "cdbfs1.sqlite";

        ICdbfsDAO D;

        [SetUp]
        public async Task Setup() {
            if (File.Exists(DbFile)) { File.Delete(DbFile); }
            D = await CdbfsSqliteDAO.CreateCdbfsSqliteFile(DbFile);
            await D.Open();
        }

        [Test]
        public async Task CreatedFileExists() {
            const string Filename = "/I1";

            Assert.That(await D.FileExists(Filename), Is.False);

            //Add the file
            await D.CreateFile(Filename, Properties.Resources.I1);

            Assert.That(await D.FileExists(Filename), Is.True);
            Assert.That((await D.GetFiles()).Files, Has.Count.EqualTo(1));
            Assert.That('/'+(await D.GetFiles()).Files[0].Name, Is.EqualTo(Filename));
        }

        [Test]
        public async Task RenamedFileExists() {
            const string Filename = "/I1";
            const string NewFilename = "/I2";
            const string NewFilenameName = "I2";

            Assert.That(await D.FileExists(Filename), Is.False);
            Assert.That(await D.FileExists(NewFilename), Is.False);

            //Add the file
            await D.CreateFile(Filename, Properties.Resources.I1);

            Assert.That(await D.FileExists(Filename), Is.True);
            Assert.That(await D.FileExists(NewFilename), Is.False);

            await D.RenameFile(Filename, NewFilenameName);

            Assert.That(await D.FileExists(Filename), Is.False);
            Assert.That(await D.FileExists(NewFilename), Is.True);
        }

        [Test]
        public async Task CreatedFileMatchesContent() {
            const string Filename = "/I1";

            Assert.That(await D.FileExists(Filename), Is.False);

            //Add the file and immediately retrieve it
            await D.CreateFile(Filename, Properties.Resources.I1);
            var Data = await D.GetFile(Filename);

            Assert.That(Enumerable.SequenceEqual(Data, Properties.Resources.I1), Is.True);
        }

        [Test]
        public async Task UpdatedFileMatchesContent() {
            const string Filename = "/I1";

            Assert.That(await D.FileExists(Filename), Is.False);

            //Add the file and immediately retrieve it
            await D.CreateFile(Filename, Properties.Resources.I1);
            var Data1 = await D.GetFile(Filename);
            
            //Ensure data saved correctly once
            Assert.That(Enumerable.SequenceEqual(Data1, Properties.Resources.I1), Is.True);

            await D.UpdateFile(Filename, Properties.Resources.I2);
            var Data2 = await D.GetFile(Filename);

            Assert.That(Enumerable.SequenceEqual(Data2, Properties.Resources.I2), Is.True);

        }

        [Test]
        public async Task DeletedFileDoesNotExist() {
            const string Filename = "/I1";

            Assert.That(await D.FileExists(Filename), Is.False);

            //Add the file
            await D.CreateFile(Filename, Properties.Resources.I1);

            Assert.That(await D.FileExists(Filename), Is.True);

            await D.DeleteFile(Filename);

            Assert.That(await D.FileExists(Filename), Is.False);
        }

        [TearDown]
        public async Task Teardown() {
            await D.DisposeAsync();
            if (File.Exists(DbFile)) { File.Delete(DbFile); }
        }
    }
}