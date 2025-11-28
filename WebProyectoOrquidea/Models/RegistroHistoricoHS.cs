using MySql.Data.MySqlClient;
using WebProyectoOrquidea.ConexionDB;

namespace WebProyectoOrquidea.Models
{
    public class RegistroHistoricoHS
    {
        public int IdRegistroHistoricoHS { get; set; }
        public int IdValoresSensor { get; set; }
        public ValoresSensor valoresSensor { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string NombreOrquidea { get; set; }
        public bool Estado { get; set; }

        public async Task<int> AgregarRegistroHistoricoHS(RegistroHistoricoHS hs)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO RegistroHistoricoHS
                (IdValoresSensor, Fecha, Hora, NombreOrquidea, Estado)
                VALUES (@i,@f,@h,@n,@e);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@i", hs.IdValoresSensor);
            cmd.Parameters.AddWithValue("@f", hs.Fecha);
            cmd.Parameters.AddWithValue("@h", hs.Hora);
            cmd.Parameters.AddWithValue("@n", hs.NombreOrquidea);
            cmd.Parameters.AddWithValue("@e", hs.Estado);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<RegistroHistoricoHS>> GetRegistroHistoricoHS()
        {
            var list = new List<RegistroHistoricoHS>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoHS, r.IdSensor, r.Fecha, r.Hora, r.NombreOrquidea, r.Estado,
                       s.IdSensor AS SensorId, s.IdValoresSensor, s.Nombre AS Sensor, s.Ubicacion AS Zona,
                       v.IdValoresSensor AS ValoresSensorId, v.Humedad
                FROM RegistroHistoricoHS r
                LEFT JOIN Sensor s ON r.IdSensor = s.IdSensor
                LEFT JOIN ValoresSensor v ON s.IdValoresSensor = v.IdValoresSensor
                ORDER BY r.IdRegistroHistoricoHS;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var registro = new RegistroHistoricoHS
                {
                    IdRegistroHistoricoHS = rd.GetInt32("IdRegistroHistoricoHS"),
                    IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                    Fecha = rd.GetDateTime("Fecha"),
                    Hora = rd.GetTimeSpan("Hora"),
                    NombreOrquidea = rd.GetString("NombreOrquidea"),
                    Estado = rd.GetBoolean("Estado"),
                    valoresSensor = !rd.IsDBNull(rd.GetOrdinal("IdValoresSensor")) ? new ValoresSensor
                    {
                        IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                        IdSensor = rd.GetInt32("SensorId"),
                        Temperatura = rd.IsDBNull(rd.GetOrdinal("Temperatura")) ? (int?)null : rd.GetDouble("Temperatura"),
                        Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? (double?)null : rd.GetDouble("Humedad"),
                        sensor = !rd.IsDBNull(rd.GetOrdinal("SensorId")) ? new Sensor
                        {
                            IdSensor = rd.GetInt32("ValoresSensorId"),
                            Ubicacion = rd.IsDBNull(rd.GetOrdinal("Ubicacion")) ? null : rd.GetString("Ubicacion")
                        } : null
                    } : null
                };
                list.Add(registro);
            }
            return list;
        }
    }
}
