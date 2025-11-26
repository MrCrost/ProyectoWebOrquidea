using MySql.Data.MySqlClient;
using WebProyectoOrquidea.ConexionDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebProyectoOrquidea.Models
{
    public class Sensor
    {
        public int IdSensor { get; set; }
        public int IdValoresSensor { get; set; }
        public ValoresSensor valoresSensor { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Ubicacion { get; set; }

        public async Task<int> AgregarSensor(Sensor hs)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO Sensor
                (IdValoresSensor, Nombre, Tipo, Ubicacion)
                VALUES (@i,@n,@t,@u);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@i", hs.IdValoresSensor);
            cmd.Parameters.AddWithValue("@n", hs.Nombre);
            cmd.Parameters.AddWithValue("@t", hs.Tipo);
            cmd.Parameters.AddWithValue("@u", hs.Ubicacion);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<Sensor>> GetSensor()
        {
            var list = new List<Sensor>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT s.IdSensor, s.IdValoresSensor, s.Nombre, s.Tipo, s.Ubicacion,
                       v.IdValoresSensor AS ValoresSensorId, v.Temperatura, v.Humedad
                FROM Sensor s
                LEFT JOIN ValoresSensor v ON s.IdValoresSensor = v.IdValoresSensor
                ORDER BY  s.IdSensor;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var sensor = new Sensor
                {
                    IdSensor = rd.GetInt32("IdSensor"),
                    IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                    Nombre = rd.GetString("Nombre"),
                    Tipo = rd.GetString("Tipo"),
                    Ubicacion = rd.GetString("Ubicacion"),
                    valoresSensor = !rd.IsDBNull(rd.GetOrdinal("ValoresSensorId")) ? new ValoresSensor
                    {
                        IdValoresSensor = rd.GetInt32("ValoresSensorId"),                        
                        Temperatura = rd.IsDBNull(rd.GetOrdinal("Temperatura")) ? 0 : rd.GetInt32("Temperatura"),
                        Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? null : rd.GetString("Humedad")
                        
                    } : null
                };
                list.Add(sensor);
            }
            return list;
        }
    }
}
