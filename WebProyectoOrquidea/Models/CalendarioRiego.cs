using MySql.Data.MySqlClient;
using WebProyectoOrquidea.ConexionDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebProyectoOrquidea.Models
{
    public class CalendarioRiego
    {     
        public int IdCalendarioRiego { get; set; }
        public string NombreOrquidea { get; set; }
        public string Zona { get; set; }
        public int DiaRiego { get; set; }
        public TimeSpan HoraRiego { get; set; }
        public int FrecuenciaRiego { get; set; }
        public string MetodoNotificacion { get; set; }
        public bool EstadoNotificacion { get; set; }

        // LISTAR
        public async Task<List<CalendarioRiego>> GetCalendario()
        {
            var list = new List<CalendarioRiego>();
            using var cn = DB.GetConnection();

            const string sql = @"
                SELECT IdCalendarioRiego, NombreOrquidea, Zona, DiaRiego, HoraRiego, FrecuenciaRiego, MetodoNotificacion, EstadoNotificacion
                FROM CalendarioRiego
                ORDER BY IdCalendarioRiego;";
            using var cmd = new MySqlCommand(sql, cn);

            using var rd = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new CalendarioRiego
                {       
                    IdCalendarioRiego = rd.GetInt32("IdCalendarioRiego"),
                    NombreOrquidea = rd.GetString("NombreOrquidea"),
                    Zona = rd.GetString("Zona"),
                    DiaRiego = rd.GetInt32("DiaRiego"),
                    HoraRiego = rd.GetTimeSpan("HoraRiego"),
                    FrecuenciaRiego = rd.GetInt32("FrecuenciaRiego"),
                    MetodoNotificacion = rd.GetString("MetodoNotificacion"),
                    EstadoNotificacion = rd.GetBoolean("EstadoNotificacion")
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
                (NombreOrquidea, Zona, DiaRiego, HoraRiego, FrecuenciaRiego, MetodoNotificacion, EstadoNotificacion)
                VALUES (@n,@z,@d,@h,@f,@m,@e);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", c.NombreOrquidea);
            cmd.Parameters.AddWithValue("@z", c.Zona);
            cmd.Parameters.AddWithValue("@d", c.DiaRiego);
            cmd.Parameters.AddWithValue("@h", c.HoraRiego);
            cmd.Parameters.AddWithValue("@f", c.FrecuenciaRiego);
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
                WHERE IdCalendarioRiego = @id;";
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", c.IdCalendarioRiego);
            cmd.Parameters.AddWithValue("@e", c.EstadoNotificacion);
            await cmd.ExecuteNonQueryAsync();
        }

        // ELIMINAR
        public async Task EliminarCalendario(int id)
        {
            using var cn = DB.GetConnection();

            const string sql = "DELETE FROM CalendarioRiego WHERE IdCalendarioRiego = @id;"; 
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
