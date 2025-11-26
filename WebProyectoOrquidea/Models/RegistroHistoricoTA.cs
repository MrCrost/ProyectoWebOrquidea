using MySql.Data.MySqlClient;
using WebProyectoOrquidea.ConexionDB;

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

        public async Task<int> AgregarRegistroHistoricoTA(RegistroHistoricoTA hs)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO RegistroHistoricoTA
                (IdSensor, Fecha, Hora, Periodo, Estado)
                VALUES (@s,@f,@h,@p,@e);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@s", hs.IdSensor);
            cmd.Parameters.AddWithValue("@f", hs.Fecha);
            cmd.Parameters.AddWithValue("@h", hs.Hora);
            cmd.Parameters.AddWithValue("@p", hs.Periodo);
            cmd.Parameters.AddWithValue("@e", hs.Estado);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<RegistroHistoricoTA>> GetRegistroHistoricoTA()
        {
            var list = new List<RegistroHistoricoTA>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoTA, r.IdSensor, r.Fecha, r.Hora, r.Periodo, r.Estado,
                       s.IdSensor AS SensorId, s.IdValoresSensor, s.Nombre AS Sensor, s.Ubicacion AS Zona,
                       v.IdValoresSensor AS ValoresSensorId, v.Temperatura
                FROM RegistroHistoricoTA r
                LEFT JOIN Sensor s ON r.IdSensor = s.IdSensor
                LEFT JOIN ValoresSensor v ON s.IdValoresSensor = v.IdValoresSensor
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
                    Periodo = rd.GetString("Periodo"),
                    Estado = rd.GetBoolean("Estado"),
                    Sensor = !rd.IsDBNull(rd.GetOrdinal("SensorId")) ? new Sensor
                    {
                        IdSensor = rd.GetInt32("SensorId"),
                        IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                        Nombre = rd.IsDBNull(rd.GetOrdinal("Sensor")) ? null : rd.GetString("Sensor"),
                        Ubicacion = rd.IsDBNull(rd.GetOrdinal("Zona")) ? null : rd.GetString("Zona"),
                        valoresSensor = !rd.IsDBNull(rd.GetOrdinal("SensorId")) ? new ValoresSensor
                        {
                            IdValoresSensor = rd.GetInt32("ValoresSensorId"),
                            Temperatura = rd.IsDBNull(rd.GetOrdinal("Temperatura")) ? 0 : rd.GetInt32("Temperatura"),
                        }: null
                    } : null
                };
                list.Add(registro);
            }
            return list;
        }
    }
}
