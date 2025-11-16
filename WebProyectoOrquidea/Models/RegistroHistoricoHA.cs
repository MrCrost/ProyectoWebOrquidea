namespace WebProyectoOrquidea.Models
{
    public class RegistroHistoricoHA
    {
        public int IdRegistroHistoricoHA { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public string Zona { get; set; }
        public string Sensor { get; set; }        
        public double Humedad { get; set; }        
        public string Estado { get; set; }
    }
}
