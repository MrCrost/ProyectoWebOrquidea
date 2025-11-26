using MySql.Data.MySqlClient;
using WebProyectoOrquidea.ConexionDB;
using System;

namespace WebProyectoOrquidea.Models
{
    public class ValoresSensor
    {
        public int IdValoresSensor { get; set; }        
        public int? Temperatura { get; set; }       
        public string? Humedad { get; set; }

        public async Task<int> AgregarValoresSensor(ValoresSensor vs)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO ValoresSensor
                (Temperatura, Humedad)
                VALUES (@t,@h);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@t", vs.Temperatura);
            cmd.Parameters.AddWithValue("@h", vs.Humedad);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
    }
}
