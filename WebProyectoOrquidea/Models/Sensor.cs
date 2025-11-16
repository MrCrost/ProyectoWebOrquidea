namespace WebProyectoOrquidea.Models
{
    public class Sensor
    {
        public int IdSensor { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Ubicacion { get; set; }
        public int Temperatura { get; set; }
        public string Humedad { get; set; }
    }
}
