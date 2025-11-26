using MySql.Data.MySqlClient;

namespace WebProyectoOrquidea.ConexionDB
{
    public static class DB
    {
        private const string connectionString =
            "Server=localhost;Port=3306;Database=OrquideasDB;Uid=root;Pwd=root;";

        public static MySqlConnection GetConnection()
        {            
            var csOverride = Environment.GetEnvironmentVariable("ORQ_DB_CONN");
            var final = string.IsNullOrWhiteSpace(csOverride) ? connectionString : csOverride;

            var connection = new MySqlConnection(final);
            connection.Open();
            return connection;
        }
    }
}