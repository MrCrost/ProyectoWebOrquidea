using MySql.Data.MySqlClient;
using Reloj_Control.ConexionDB;

namespace WebProyectoOrquidea.Models

// Modelo para el registro histórico de temperatura del ambiente

{
    public class RegistroHistoricoTA
    {
        public int IdRegistroHistoricoTA { get; set; }
        public int IdSensor { get; set; }
        public Sensor Sensor { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Periodo { get; set; }
        public bool Estado { get; set; }

        public async Task<List<RegistroHistoricoTA>> GetRegistroHistoricoTA()
        {
            var list = new List<RegistroHistoricoTA>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoTA, r.IdSensor, r.Fecha, r.Hora, r.Estado,
                       s.IdSensor AS SensorId, s.Nombre AS Sensor, s.Ubicacion AS Zona, s.Temperatura
                FROM RegistroHistoricoTA r
                LEFT JOIN Sensor s ON r.IdSensor = s.IdSensor
                ORDER BY r.IdRegistroHistoricoTA;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var registro = new RegistroHistoricoTA
                {
                    IdRegistroHistoricoTA = rd.GetInt32("IdRegistroHistoricoTA"),
                    IdSensor = rd.GetInt32("IdSensor"),
                    Fecha = rd.GetDateTime("Fecha"),
                    Hora = rd.GetTimeSpan("Hora"),
                    Estado = rd.GetBoolean("Estado"),
                    Sensor = !rd.IsDBNull(rd.GetOrdinal("SensorId")) ? new Sensor
                    {
                        IdSensor = rd.GetInt32("SensorId"),
                        Nombre = rd.IsDBNull(rd.GetOrdinal("Sensor")) ? null : rd.GetString("Sensor"),
                        Ubicacion = rd.IsDBNull(rd.GetOrdinal("Zona")) ? null : rd.GetString("Zona"),
                        Temperatura = rd.IsDBNull(rd.GetOrdinal("Temperatura")) ? 0 : rd.GetInt32("Temperatura")
                    } : null
                };
                list.Add(registro);
            }
            return list;
        }
    }
}
