using System.Text;

namespace Igtampe.CDBFS.Data {

    public enum DbEngine{
        Sqlite=0,
        Postgres=1
    }

    public static class CdbfsTablesSchema {

        public class Table {

            public string Name { get; set; }
            public List<Column> Columns { get; set; } = new();

            public Table(string Name, List<Column> Columns) { 
                this.Name = Name; this.Columns = Columns; 
            }

            public string ToSQLiteTableCreationScript(DbEngine Engine)
                => @$"
                    CREATE TABLE {Name} (
                        {string.Join(',', Columns.Select(A => $"{A.Name} {A.GetDataType(Engine)}" +
                                       $"{(A.IsPrimaryKey ? " PRIMARY KEY" : "")}"))}
                    )
                ";
        }

        public class Column {
            public string Name { get; set; } = "";
            public string SqliteDataType { get; set; } = "";
            public string PostgresDataType { get; set; } = "";
            public bool IsPrimaryKey { get; set; } = false;

            public Column(string name, string sqliteDataType, string postgresDataType, bool isPrimaryKey = false) {
                Name = name;
                SqliteDataType = sqliteDataType;
                PostgresDataType = postgresDataType;
                IsPrimaryKey = isPrimaryKey;
            }

            public string GetDataType(DbEngine Engine) =>
                Engine switch {
                    DbEngine.Sqlite => SqliteDataType,
                    DbEngine.Postgres => PostgresDataType,
                    _ => ""
                };

        }

        public static readonly List<Table> Tables = new() {
            new("Files", new(){
                new("Name","TEXT","TEXT", true),
                new("DateCreated","DATETIME","TIMESTAMP"),
                new("DateUpdated","DATETIME","TIMESTAMP"),
                new("Data","BLOB","BYTEA")
            })
        };
    }
}
