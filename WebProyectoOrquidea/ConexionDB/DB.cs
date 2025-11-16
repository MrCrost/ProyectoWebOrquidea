using MySql.Data.MySqlClient;

namespace Reloj_Control.ConexionDB
{
    public static class DB
    {
        // Deja tu cadena actual tal cual (PROD por defecto)
        private const string connectionString =
            "Server=100.126.25.114;Port=3306;Database=asistencia_db;Uid=remote;Pwd=perro123;";

        public static MySqlConnection GetConnection()
        {
            var csOverride = Environment.GetEnvironmentVariable("RELOJ_DB_CONN");
            var final = string.IsNullOrWhiteSpace(csOverride) ? connectionString : csOverride;

            var connection = new MySqlConnection(final);
            connection.Open();
            return connection;
        }
    }
}