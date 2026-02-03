using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcCorePracticaFinal.Models;
using System.Data;

#region VISTA
//CREATE VIEW VISTA_DETALLE_ALUMNO AS
//select USUARIOSTAJAMAR.IDUSUARIO as IdUsuario, USUARIOSTAJAMAR.Nombre as NombreUsuario, Apellidos as ApellidosUsuario, EMAIL, IMAGEN,
//fecha_inscripcion as FechaInscripcion, quiere_ser_capitan as Capitan,
//ACTIVIDADES.nombre as NombreActividad, CURSOSTAJAMAR.NOMBRE as NombreCurso FROM USUARIOSTAJAMAR 
//INNER JOIN inscripciones ON USUARIOSTAJAMAR.IDUSUARIO = inscripciones.ID_USUARIO INNER JOIN evento_actividades ON 
//evento_actividades.IdEventoActividad = inscripciones.IdEventoActividad INNER JOIN ACTIVIDADES ON ACTIVIDADES.id_actividad = evento_actividades.IdActividad 
//INNER JOIN CURSOSTAJAMAR ON CURSOSTAJAMAR.IDCURSO = USUARIOSTAJAMAR.IDCURSO;

//select* from VISTA_DETALLE_ALUMNO WHERE IdUsuario = 87
#endregion

namespace MvcCorePracticaFinal.Repositories
{
    public class RepositoryAlumnos
    {
        private DataTable tablaAlumnos;
        private DataTable tablaDetallesAlumno;

        public RepositoryAlumnos()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=PRUEBAMARTA;Persist Security Info=True;User ID=SA;Password=Admin123;Trust Server Certificate=True";
            string sql = "select * from USUARIOSTAJAMAR";
            string sql2 = "select * from VISTA_DETALLE_ALUMNO";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            SqlDataAdapter ad2 = new SqlDataAdapter(sql2, connectionString);
            this.tablaAlumnos = new DataTable();
            this.tablaDetallesAlumno = new DataTable();
            ad.Fill(this.tablaAlumnos);
            ad2.Fill(this.tablaDetallesAlumno);
        }

        public List<Alumno> GetAlumnos()
        {
            var consulta = from datos in this.tablaAlumnos.AsEnumerable() select datos;
            List<Alumno> alumnos = new List<Alumno>();
            foreach (var row in consulta)
            {
                Alumno alumno = new Alumno();
                alumno.IdUsuario = row.Field<int>("IDUSUARIO");
                alumno.Nombre = row.Field<string>("NOMBRE");
                alumno.Apellidos = row.Field<string>("APELLIDOS");
                alumno.Email = row.Field<string>("EMAIL");
                alumno.Imagen = row.Field<string>("IMAGEN");
                alumno.IdCurso = row.Field<int>("IDCURSO");
                alumnos.Add(alumno);
            }
            return alumnos;
        }
        public DetallesAlumno GetDetallesAlumno(int idUsuario)
        {
            var consulta = from datos in this.tablaDetallesAlumno.AsEnumerable() where datos.Field<int>("IdUsuario") == idUsuario select datos;
            var row = consulta.First();

            DetallesAlumno detalleAlumno = new DetallesAlumno();
            detalleAlumno.IdUsuario = row.Field<int>("IdUsuario");
            detalleAlumno.NombreUsuario = row.Field<string>("NombreUsuario");
            detalleAlumno.Apellido = row.Field<string>("ApellidosUsuario");
            detalleAlumno.Email = row.Field<string>("EMAIL");
            detalleAlumno.Imagen = row.Field<string>("IMAGEN");
            detalleAlumno.Fecha = row.Field<DateTime>("FechaInscripcion");
            detalleAlumno.Capitan = row.Field<Boolean>("Capitan");
            detalleAlumno.NombreActividad = row.Field<string>("NombreActividad");
            detalleAlumno.NombreCurso = row.Field<string>("NombreCurso");
            return detalleAlumno;
        }
    }
}
