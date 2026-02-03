namespace MvcCorePracticaFinal.Models
{
    public class DetallesAlumno
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Imagen { get; set; }
        public DateTime Fecha { get; set; }
        public Boolean Capitan { get; set; }
        public string NombreActividad { get; set; }
        public string NombreCurso{ get; set; }
    }
}
