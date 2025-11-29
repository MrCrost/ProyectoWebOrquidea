using MySql.Data.MySqlClient;
using WebProyectoOrquidea.ConexionDB;

namespace WebProyectoOrquidea.Models
{
    public class RegistroHistoricoHS
    {
        public int IdRegistroHistoricoHS { get; set; }
        public int IdValoresSensor { get; set; }
        public ValoresSensor valoresSensor { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Estado { get; set; }

        public async Task<int> AgregarRegistroHistoricoHS(RegistroHistoricoHS hs)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO RegistroHistoricoHS
                (IdValoresSensor, Fecha, Hora, Estado)
                VALUES (@i,@f,@h,@e);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@i", hs.IdValoresSensor);
            cmd.Parameters.AddWithValue("@f", hs.Fecha);
            cmd.Parameters.AddWithValue("@h", hs.Hora);
            cmd.Parameters.AddWithValue("@e", hs.Estado);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<RegistroHistoricoHS>> GetRegistroHistoricoHS()
        {
            var list = new List<RegistroHistoricoHS>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoHS, r.IdValoresSensor, r.Fecha, r.Hora, r.Estado,
                       v.IdSensor, v.Humedad, s.Ubicacion AS Zona
                FROM RegistroHistoricoHS r
                LEFT JOIN ValoresSensor v ON r.IdValoresSensor = v.IdValoresSensor
                LEFT JOIN Sensor s ON v.IdSensor = s.IdSensor
                ORDER BY r.IdRegistroHistoricoHS;";

            using var cmd = new MySqlCommand(sql, cn);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var registro = new RegistroHistoricoHS
                {
                    IdRegistroHistoricoHS = rd.GetInt32("IdRegistroHistoricoHS"),
                    IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                    Fecha = rd.GetDateTime("Fecha"),
                    Hora = rd.GetTimeSpan("Hora"),
                    Estado = rd.GetString("Estado"),
                    valoresSensor = !rd.IsDBNull(rd.GetOrdinal("IdValoresSensor")) ? new ValoresSensor
                    {
                        IdSensor = rd.GetInt32("IdSensor"),
                        Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? (double?)null : rd.GetDouble("Humedad"),
                        sensor = !rd.IsDBNull(rd.GetOrdinal("IdSensor")) ? new Sensor
                        {
                            Ubicacion = rd.IsDBNull(rd.GetOrdinal("Zona")) ? null : rd.GetString("Zona")
                        } : null
                    } : null
                };
                list.Add(registro);
            }
            return list;
        }

        // Filtros

        public async Task<List<RegistroHistoricoHS>> GetFiltroFechaHora(DateTime fechaInicial, DateTime fechaFinal, TimeSpan horaInicial, TimeSpan horaFinal)
        {
            var list = new List<RegistroHistoricoHS>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoHS, r.IdValoresSensor, r.Fecha, r.Hora, r.Estado, v.IdSensor, s.Ubicacion AS Zona, v.Humedad
                FROM RegistroHistoricoHS r
                LEFT JOIN ValoresSensor v ON r.IdValoresSensor = v.IdValoresSensor
                LEFT JOIN Sensor s ON v.IdSensor = s.IdSensor
                WHERE
                    r.Fecha BETWEEN @fi AND @ff AND
                    r.Hora BETWEEN @hi AND @hf  
                ORDER BY r.Fecha DESC, r.Hora DESC;";

            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@fi", fechaInicial);
            cmd.Parameters.AddWithValue("@ff", fechaFinal);
            cmd.Parameters.AddWithValue("@hi", horaInicial);
            cmd.Parameters.AddWithValue("@hf", horaFinal);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var registro = new RegistroHistoricoHS
                {
                    IdRegistroHistoricoHS = rd.GetInt32("IdRegistroHistoricoHS"),
                    IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                    Fecha = rd.GetDateTime("Fecha"),
                    Hora = rd.GetTimeSpan("Hora"),
                    Estado = rd.GetString("Estado"),
                    valoresSensor = !rd.IsDBNull(rd.GetOrdinal("IdValoresSensor")) ? new ValoresSensor
                    {
                        IdSensor = rd.GetInt32("IdSensor"),
                        Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? (double?)null : rd.GetDouble("Humedad"),
                        sensor = !rd.IsDBNull(rd.GetOrdinal("IdSensor")) ? new Sensor
                        {
                            Ubicacion = rd.IsDBNull(rd.GetOrdinal("Zona")) ? null : rd.GetString("Zona")
                        } : null
                    } : null
                };
                list.Add(registro);
            }
            return list;
        }

        public async Task<List<RegistroHistoricoHS>> GetFiltroIdSensor(int idSensor, DateTime fechaInicial, DateTime fechaFinal, TimeSpan horaInicial, TimeSpan horaFinal)
        {
            var list = new List<RegistroHistoricoHS>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoHS, r.IdValoresSensor, r.Fecha, r.Hora, r.Estado, v.IdSensor, s.Ubicacion AS Zona, v.Humedad
                FROM RegistroHistoricoHS r
                LEFT JOIN ValoresSensor v ON r.IdValoresSensor = v.IdValoresSensor
                LEFT JOIN Sensor s ON v.IdSensor = s.IdSensor
                WHERE
                    r.Fecha BETWEEN @fi AND @ff AND
                    r.Hora BETWEEN @hi AND @hf AND
                    v.IdSensor =@i 
                ORDER BY r.Fecha ASC, r.Hora ASC;";

            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@i", idSensor);
            cmd.Parameters.AddWithValue("@fi", fechaInicial);
            cmd.Parameters.AddWithValue("@ff", fechaFinal);
            cmd.Parameters.AddWithValue("@hi", horaInicial);
            cmd.Parameters.AddWithValue("@hf", horaFinal);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var registro = new RegistroHistoricoHS
                {
                    IdRegistroHistoricoHS = rd.GetInt32("IdRegistroHistoricoHS"),
                    IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                    Fecha = rd.GetDateTime("Fecha"),
                    Hora = rd.GetTimeSpan("Hora"),
                    Estado = rd.GetString("Estado"),
                    valoresSensor = !rd.IsDBNull(rd.GetOrdinal("IdValoresSensor")) ? new ValoresSensor
                    {
                        IdSensor = rd.GetInt32("IdSensor"),
                        Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? (double?)null : rd.GetDouble("Humedad"),
                        sensor = !rd.IsDBNull(rd.GetOrdinal("IdSensor")) ? new Sensor
                        {
                            Ubicacion = rd.IsDBNull(rd.GetOrdinal("Zona")) ? null : rd.GetString("Zona")
                        } : null
                    } : null
                };
                list.Add(registro);
            }
            return list;
        }
        public async Task<List<RegistroHistoricoHS>> GetFiltroZona(string zona, DateTime fechaInicial, DateTime fechaFinal, TimeSpan horaInicial, TimeSpan horaFinal)
        {

            
            var list = new List<RegistroHistoricoHS>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT r.IdRegistroHistoricoHS, r.IdValoresSensor, r.Fecha, r.Hora, r.Estado, v.IdSensor, s.Ubicacion AS Zona, v.Humedad
                FROM RegistroHistoricoHS r
                LEFT JOIN ValoresSensor v ON r.IdValoresSensor = v.IdValoresSensor
                LEFT JOIN Sensor s ON v.IdSensor = s.IdSensor
                WHERE
                    r.Fecha BETWEEN @fi AND @ff AND
                    r.Hora BETWEEN @hi AND @hf AND
                    s.Ubicacion = @z    
                ORDER BY r.Fecha ASC, r.Hora ASC;";

            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@z", zona);
            cmd.Parameters.AddWithValue("@fi", fechaInicial);
            cmd.Parameters.AddWithValue("@ff", fechaFinal);
            cmd.Parameters.AddWithValue("@hi", horaInicial);
            cmd.Parameters.AddWithValue("@hf", horaFinal);
            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                Console.WriteLine("Estoy dentro");
                var registro = new RegistroHistoricoHS
                {
                    IdRegistroHistoricoHS = rd.GetInt32("IdRegistroHistoricoHS"),
                    IdValoresSensor = rd.GetInt32("IdValoresSensor"),
                    Fecha = rd.GetDateTime("Fecha"),
                    Hora = rd.GetTimeSpan("Hora"),
                    Estado = rd.GetString("Estado"),
                    valoresSensor = !rd.IsDBNull(rd.GetOrdinal("IdValoresSensor")) ? new ValoresSensor
                    {
                        IdSensor = rd.GetInt32("IdSensor"),
                        Humedad = rd.IsDBNull(rd.GetOrdinal("Humedad")) ? (double?)null : rd.GetDouble("Humedad"),
                        sensor = !rd.IsDBNull(rd.GetOrdinal("IdSensor")) ? new Sensor
                        {
                            Ubicacion = rd.IsDBNull(rd.GetOrdinal("Zona")) ? null : rd.GetString("Zona")
                        } : null
                    } : null                    
                };
                Console.WriteLine("hola");
                list.Add(registro);
            }
            return list;
        }
    }
}
