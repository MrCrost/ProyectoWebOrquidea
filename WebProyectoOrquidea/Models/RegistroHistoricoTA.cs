using MySql.Data.MySqlClient;
using WebProyectoOrquidea.ConexionDB;

namespace WebProyectoOrquidea.Models

// Modelo para el registro histórico de temperatura del ambiente

{
    public class RegistroHistoricoTA
    {
        public int IdRegistroHistoricoTA { get; set; }
        public int IdValoresSensor { get; set; }
        public ValoresSensor valoresSensor { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Periodo { get; set; }
        public string Estado { get; set; }

        public async Task<int> AgregarRegistroHistoricoTA(RegistroHistoricoTA ta)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO RegistroHistoricoTA
                (IdValoresSensor, Fecha, Hora, Periodo, Estado)
                VALUES (@i,@f,@h,@p,@e);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@i", ta.IdValoresSensor);
            cmd.Parameters.AddWithValue("@f", ta.Fecha);
            cmd.Parameters.AddWithValue("@h", ta.Hora);
            cmd.Parameters.AddWithValue("@p", ta.Periodo);
            cmd.Parameters.AddWithValue("@e", ta.Estado);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<RegistroHistoricoTA>> GetRegistroHistoricoTA()
        {
            var list = new List<RegistroHistoricoTA>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoTA, r.IdValoresSensor, r.Fecha, r.Hora, r.Periodo, r.Estado,
                       v.IdSensor, v.Temperatura
                FROM RegistroHistoricoTA r
                LEFT JOIN ValoresSensor v ON r.IdValoresSensor = v.IdValoresSensor
                ORDER BY r.IdRegistroHistoricoTA;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var registro = new RegistroHistoricoTA
                {
                    IdRegistroHistoricoTA = rd.GetInt32("IdRegistroHistoricoTA"),
                    IdValoresSensor = rd.GetInt32("IdValoresSensor"),   
                    Fecha = rd.GetDateTime("Fecha"),
                    Hora = rd.GetTimeSpan("Hora"),
                    Periodo = rd.GetString("Periodo"),
                    Estado = rd.GetString("Estado"),
                    valoresSensor = !rd.IsDBNull(rd.GetOrdinal("IdValoresSensor")) ? new ValoresSensor
                    {
                        IdSensor = rd.GetInt32("IdSensor"),
                        Temperatura = rd.IsDBNull(rd.GetOrdinal("Temperatura")) ? (int?)null : rd.GetDouble("Temperatura")
                    } : null
                };
                list.Add(registro);
            }
            return list;
        }
    }
}
