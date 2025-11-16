namespace WebProyectoOrquidea.Models
{
    public class CalendarioRiego
    {
        public string NombreOrquidea { get; set; }
        public string Zone { get; set; }
        public string DiaRiego { get; set; }
        public TimeSpan HoraRiego { get; set; }
        public string Frecuencia { get; set; }
        public string MetodoNotificacion { get; set; }
        public int EstadoNotificacion { get; set; }

    }
}
