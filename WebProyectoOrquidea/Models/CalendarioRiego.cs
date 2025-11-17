using MySql.Data.MySqlClient;
using Reloj_Control.ConexionDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebProyectoOrquidea.Models
{
    public class CalendarioRiego
    {
        public int IdOrquidea { get; set; }
        public string NombreOrquidea { get; set; }
        public string Zona { get; set; }
        public int DiaRiego { get; set; }
        public TimeSpan HoraRiego { get; set; }
        public int Frecuencia { get; set; }
        public string MetodoNotificacion { get; set; }
        public int EstadoNotificacion { get; set; }

        // LISTAR
        public async Task<List<CalendarioRiego>> GetCalendario()
        {
            var list = new List<CalendarioRiego>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT IdOrquidea, NombreOrquidea, Zona, DiaRiego, HoraRiego, Frecuencia, MetodoNotificacion, EstadoNotificacion
                FROM CalendarioRiego
                ORDER BY IdOrquidea;";
            using var cmd = new MySqlCommand(sql, cn);

            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new CalendarioRiego
                {
                    IdOrquidea = rd.GetInt32("IdOrquidea"),
                    NombreOrquidea = rd.GetString("NombreOrquidea"),
                    Zona = rd.GetString("Zona"),
                    DiaRiego = rd.GetInt32("DiaRiego"),
                    HoraRiego = rd.GetTimeSpan("HoraRiego"),
                    Frecuencia = rd.GetInt32("Frecuencia"),
                    MetodoNotificacion = rd.GetString("MetodoNotificacion"),
                    EstadoNotificacion = rd.GetInt32("EstadoNotificacion")
                });
            }
            return list;
        }

        // AGREGAR
        public async Task<int> AgregarCalendario(CalendarioRiego c)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                INSERT INTO CalendarioRiego
                (NombreOrquidea, Zona, DiaRiego, HoraRiego, Frecuencia, MetodoNotificacion, EstadoNotificacion)
                VALUES (@n,@z,@d,@h,@f,@m,@e);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", c.NombreOrquidea);
            cmd.Parameters.AddWithValue("@z", c.Zona);
            cmd.Parameters.AddWithValue("@d", c.DiaRiego);
            cmd.Parameters.AddWithValue("@h", c.HoraRiego);
            cmd.Parameters.AddWithValue("@f", c.Frecuencia);
            cmd.Parameters.AddWithValue("@m", c.MetodoNotificacion);
            cmd.Parameters.AddWithValue("@e", c.EstadoNotificacion);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
        // MODIFICAR
        public async Task ModificarCalendario(CalendarioRiego c)
        {
            using var cn = DB.GetConnection();

            const string sql = @"
                UPDATE CalendarioRiego
                SET EstadoNotificacion = @e
                WHERE IdOrquidea = @id;";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", c.IdOrquidea);
            cmd.Parameters.AddWithValue("@e", c.EstadoNotificacion);
            await cmd.ExecuteNonQueryAsync();
        }

        // ELIMINAR
        public async Task EliminarCalendario(int id)
        {
            using var cn = DB.GetConnection();

            const string sql = "DELETE FROM CalendarioRiego WHERE IdOrquidea = @id;"; 
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
