namespace WebProyectoOrquidea.Models

// Modelo para el registro histórico de temperatura del ambiente

{
    public class RegistroHistoricoTA
    {
        public int IdRegistroHistoricoTA { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public string Sensor { get; set; }
        public string Ubicacion { get; set; }
        public double Temperatura { get; set; }
        public string Periodo { get; set; }
        public string Estado { get; set; }
    }
}
