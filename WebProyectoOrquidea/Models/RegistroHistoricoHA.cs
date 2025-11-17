using MySql.Data.MySqlClient;
using Reloj_Control.ConexionDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebProyectoOrquidea.Models
{
    public class RegistroHistoricoHA
    {
        public int IdRegistroHistoricoHA { get; set; }
        public int IdSensor { get; set; }
        public Sensor Sensor { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }     
        public bool Estado { get; set; }

        public async Task<List<RegistroHistoricoHA>> GetRegistroHistoricoHA()
        {
            var list = new List<RegistroHistoricoHA>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoHA, r.IdSensor, r.Fecha, r.Hora, r.Estado,
                       s.IdSensor AS SensorId, s.Nombre AS Sensor, s.Ubicacion AS Zona, s.Humedad
                FROM RegistroHistoricoHA r
                LEFT JOIN Sensor s ON r.IdSensor = s.IdSensor
                ORDER BY r.IdRegistroHistoricoHA;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var registro = new RegistroHistoricoHA
                {
                    IdRegistroHistoricoHA = rd.GetInt32("IdRegistroHistoricoHA"),
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
