using MySql.Data.MySqlClient;
using WebProyectoOrquidea.ConexionDB;
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

        public async Task<int> AgregarRegistroHistoricoHA(RegistroHistoricoHA ha)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO RegistroHistoricoHA
                (IdSensor, Fecha, Hora, Estado)
                VALUES (@s,@f,@h,@e);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@s", ha.IdSensor);
            cmd.Parameters.AddWithValue("@f", ha.Fecha);
            cmd.Parameters.AddWithValue("@h", ha.Hora);
            cmd.Parameters.AddWithValue("@e", ha.Estado);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }        

        public async Task<List<RegistroHistoricoHA>> GetRegistroHistoricoHA()
        {
            var list = new List<RegistroHistoricoHA>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoHA, r.IdSensor, r.Fecha, r.Hora, r.Estado,
                       s.IdSensor AS SensorId, s.IdValoresSensor, s.Nombre AS Sensor, s.Ubicacion AS Zona,
                       v.IdValoresSensor AS ValoresSensorId, v.Humedad
                FROM RegistroHistoricoHS r
                LEFT JOIN Sensor s ON r.IdSensor = s.IdSensor
                LEFT JOIN ValoresSensor v ON s.IdValoresSensor = v.IdValoresSensor
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
                        IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                        Nombre = rd.IsDBNull(rd.GetOrdinal("Sensor")) ? null : rd.GetString("Sensor"),
                        Ubicacion = rd.IsDBNull(rd.GetOrdinal("Zona")) ? null : rd.GetString("Zona"),
                        valoresSensor = !rd.IsDBNull(rd.GetOrdinal("SensorId")) ? new ValoresSensor
                        {
                            IdValoresSensor = rd.GetInt32("ValoresSensorId"),
                            Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? null : rd.GetString("Humedad")
                        } : null
                    } : null
                };
                list.Add(registro);
            }
            return list;
        }
    } 
}
