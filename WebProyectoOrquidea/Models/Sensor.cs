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
        public string Tipo { get; set; }
        public string Ubicacion { get; set; }

        public async Task<int> AgregarSensor(string tipo, string ubicacion)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO Sensor
                (Tipo, Ubicacion)
                VALUES (@t,@u);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@t", tipo);
            cmd.Parameters.AddWithValue("@u", ubicacion);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result); 
        }

        public async Task<List<Sensor>> GetSensor()
        {
            var list = new List<Sensor>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT * FROM orquideasdb.sensor;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var sensor = new Sensor
                {
                    IdSensor = rd.GetInt32("IdSensor"),
                    Tipo = rd.GetString("Tipo"),
                    Ubicacion = rd.GetString("Ubicacion")
                };
                list.Add(sensor);
            }
            return list;
        }
    }
}
