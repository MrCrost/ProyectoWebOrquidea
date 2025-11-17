using MySql.Data.MySqlClient;
using Reloj_Control.ConexionDB;

namespace WebProyectoOrquidea.Models
{
    public class RegistroHistoricoHS
    {
        public int IdRegistroHistoricoHS { get; set; }
        public int IdSensor { get; set; }
        public Sensor Sensor { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string NombreOrquidea { get; set; }
        public bool Estado { get; set; }

        public async Task<List<RegistroHistoricoHS>> GetRegistroHistoricoHS()
        {
            var list = new List<RegistroHistoricoHS>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoHS, r.IdSensor, r.Fecha, r.Hora, r.Estado,
                       s.IdSensor AS SensorId, s.Nombre AS Sensor, s.Ubicacion AS Zona, s.Humedad
                FROM RegistroHistoricoHS r
                LEFT JOIN Sensor s ON r.IdSensor = s.IdSensor
                ORDER BY r.IdRegistroHistoricoHS;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var registro = new RegistroHistoricoHS
                {
                    IdRegistroHistoricoHS = rd.GetInt32("IdRegistroHistoricoHS"),
                    IdSensor = rd.GetInt32("IdSensor"),
                    Fecha = rd.GetDateTime("Fecha"),
                    Hora = rd.GetTimeSpan("Hora"),
                    Estado = rd.GetBoolean("Estado"),
                    Sensor = !rd.IsDBNull(rd.GetOrdinal("SensorId")) ? new Sensor
                    {
                        IdSensor = rd.GetInt32("SensorId"),
                        Nombre = rd.IsDBNull(rd.GetOrdinal("Sensor")) ? null : rd.GetString("Sensor"),                        
                        Ubicacion = rd.IsDBNull(rd.GetOrdinal("Zona")) ? null : rd.GetString("Zona"),
                        Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? null : rd.GetString("Humedad")
                    } : null
                };
                list.Add(registro);
            }
            return list;
        }
    }
}
