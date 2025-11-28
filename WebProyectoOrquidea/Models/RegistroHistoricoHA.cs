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
        public int IdValoresSensor { get; set; }
        public ValoresSensor valoresSensor { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }

        public async Task<int> AgregarRegistroHistoricoHA(RegistroHistoricoHA ha)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO RegistroHistoricoHA
                (IdValoresSensor, Fecha, Hora)
                VALUES (@i,@f,@h,@e);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@i", ha.IdValoresSensor);
            cmd.Parameters.AddWithValue("@f", ha.Fecha);
            cmd.Parameters.AddWithValue("@h", ha.Hora);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }        

        public async Task<List<RegistroHistoricoHA>> GetRegistroHistoricoHA()
        {
            var list = new List<RegistroHistoricoHA>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoHA, r.IdValoresSensor, r.Fecha, r.Hora, s.IdSensor as SensorId, s.Ubicacion, v.Humedad
                FROM RegistroHistoricoHA r
                LEFT JOIN ValoresSensor v ON r.IdValoresSensor = v.IdValoresSensor
                LEFT JOIN Sensor s ON v.IdSensor = s.IdSensor
                ORDER BY r.IdRegistroHistoricoHA;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var registro = new RegistroHistoricoHA
                {
                    IdRegistroHistoricoHA = rd.GetInt32("IdRegistroHistoricoHA"),
                    IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                    Fecha = rd.GetDateTime("Fecha"),
                    Hora = rd.GetTimeSpan("Hora"),
                    valoresSensor = !rd.IsDBNull(rd.GetOrdinal("IdValoresSensor")) ? new ValoresSensor
                    {
                        IdSensor = rd.GetInt32("SensorId"),
                        Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? (double?)null : rd.GetDouble("Humedad"),
                        sensor = !rd.IsDBNull(rd.GetOrdinal("SensorId")) ? new Sensor
                        {
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
