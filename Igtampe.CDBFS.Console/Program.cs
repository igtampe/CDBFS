using Igtampe.CDBFS.Common;
using Igtampe.CDBFS.Data;
using System;

namespace Igtampe.CDBFS.CLI {
    public class Program {
        private async static Task Main(string[] args) {

            if (File.Exists("A:/cdbfs1.sqlite")) { File.Delete("A:/cdbfs1.sqlite"); }
            ICdbfsDAO D = await CdbfsSqliteDAO.CreateCdbfsSqliteFile("A:/cdbfs1.sqlite");

            await D.Open();

            await D.CreateFile("Dingus.txt", File.ReadAllBytes("A:/Dingus.txt"));
            await D.CreateFile("ChopoNoPauses.png", File.ReadAllBytes("A:/ChopoNoPauses.png"));

            foreach (CdbfsFile F in await D.GetFiles()) {
                Console.WriteLine($"{F.Name} : {F.DateCreated} : {F.DateUpdated}");
            }
        }
    }
}
