using MySql.Data.MySqlClient;
using WebProyectoOrquidea.ConexionDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebProyectoOrquidea.Models
{
    public class ValoresSensor
    {
        public int IdValoresSensor { get; set; }
        public int IdSensor { get; set; }
        public Sensor sensor { get; set; }
        public double? Temperatura { get; set; }       
        public double? Humedad { get; set; }

        public async Task<int> AgregarValoresSensor(int idSensor, double? temperatura, double? humedad)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO ValoresSensor
                (IdSensor, Temperatura, Humedad)
                VALUES (@i,@t,@h);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@i", idSensor);
            cmd.Parameters.AddWithValue("@t", temperatura.HasValue ? (object)temperatura.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@h", humedad.HasValue ? (object)humedad.Value : DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        // Agregar valores e insertar registro en RegistroHistoricoTA
        public async Task<int> AgregarValoresEHistorialTA(int idSensor, double? temperatura, double? humedad, DateTime date, TimeSpan time, string periodo, string estado)
        {
            
            using var cn = DB.GetConnection();

            const string sql1 = @"
                INSERT INTO ValoresSensor
                (IdSensor, Temperatura, Humedad)
                VALUES (@i,@t,@h);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql1, cn);
            cmd.Parameters.AddWithValue("@i", idSensor);            
            cmd.Parameters.AddWithValue("@t", temperatura.HasValue ? (object)temperatura.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@h", humedad.HasValue ? (object)humedad.Value : DBNull.Value);

            var idValoresObj = await cmd.ExecuteScalarAsync();
            var idValores = Convert.ToInt32(idValoresObj);

            const string sql2 = @"
                INSERT INTO RegistroHistoricoTA
                (IdValoresSensor, Fecha, Hora, Periodo, Estado)
                VALUES (@i,@f,@h,@p,@e);
                SELECT LAST_INSERT_ID();";
            using var cmd2 = new MySqlCommand(sql2, cn);
            cmd2.Parameters.AddWithValue("@i", idValores);
            cmd2.Parameters.AddWithValue("@f", date);
            cmd2.Parameters.AddWithValue("@h", time);
            cmd2.Parameters.AddWithValue("@p", periodo);
            cmd2.Parameters.AddWithValue("@e", estado);

            var resultObj = await cmd2.ExecuteScalarAsync();
            return Convert.ToInt32(resultObj);
        }

        public async Task<List<ValoresSensor>> GetValoresSensor()
        {
            var list = new List<ValoresSensor>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT v.IdValoresSensor, v.IdSensor, v.Temperatura, v.Humedad, s.Tipo, s.Ubicacion
                FROM ValoresSensor v
                LEFT JOIN Sensor s ON v.IdSensor = s.IdSensor
                ORDER BY v.IdValoresSensor;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var valoresSensor = new ValoresSensor
                {
                    IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                    IdSensor = rd.GetInt32("IdSensor"),
                    Temperatura = rd.IsDBNull(rd.GetOrdinal("Temperatura")) ? (double?)null : rd.GetDouble("Temperatura"),                    
                    Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? (double?)null : rd.GetDouble("Humedad"),
                    sensor = !rd.IsDBNull(rd.GetOrdinal("IdSensor")) ? new Sensor
                    {
                        IdSensor = rd.GetInt32("IdSensor"),
                        Tipo = rd.IsDBNull(rd.GetOrdinal("Tipo")) ? null : rd.GetString("Tipo"),
                        Ubicacion = rd.IsDBNull(rd.GetOrdinal("Ubicacion")) ? null : rd.GetString("Ubicacion")
                    } : null
                };
                list.Add(valoresSensor);
            }
            return list;
        }
    }
}
