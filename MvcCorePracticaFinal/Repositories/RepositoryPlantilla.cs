using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcCorePracticaFinal.Models;
using System.Data;

#region PROCEDURES
//CREATE procedure SP_PLANTILLA_UPSERT
//(@hospitalCod int, @salaCod int, @empleadoNo int, @apellido nvarchar(50), @funcion nvarchar(50), @turno nvarchar(50), @salario int)
//as
//	IF EXISTS (SELECT 1 FROM PLANTILLA WHERE EMPLEADO_NO = @empleadoNo)
//    BEGIN
//        UPDATE PLANTILLA SET HOSPITAL_COD = @hospitalCod, SALA_COD = @salaCod, APELLIDO = @apellido, FUNCION = @funcion, T = @turno, SALARIO = @salario
//        WHERE EMPLEADO_NO = @empleadoNo;
//END
//ELSE
//    BEGIN
//        insert into PLANTILLA values (@hospitalCod, @salaCod, @empleadoNo, @apellido, @funcion, @turno, @salario)
//    END
//go

//exec SP_PLANTILLA_UPSERT 22, 6, 55556, 'Alonso', 'Interino', 'T', 1
#endregion

namespace MvcCorePracticaFinal.Repositories
{
    public class RepositoryPlantilla
    {
        private DataTable tablaPlantilla;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryPlantilla()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=Admin123;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "select * from PLANTILLA";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            this.tablaPlantilla = new DataTable();
            ad.Fill(this.tablaPlantilla);
        }

        public List<Plantilla> GetPlantilla()
        {
            var consulta = from datos in this.tablaPlantilla.AsEnumerable() select datos;
            List<Plantilla> plantillas = new List<Plantilla>();
            foreach (var row in consulta)
            {
                Plantilla plantilla = new Plantilla();
                plantilla.HospitalCod = row.Field<int>("HOSPITAL_COD");
                plantilla.SalaCod = row.Field<int>("SALA_COD");
                plantilla.EmpleadoNo = row.Field<int>("EMPLEADO_NO");
                plantilla.Apellido = row.Field<string>("APELLIDO");
                plantilla.Funcion = row.Field<string>("FUNCION");
                plantilla.Turno = row.Field<string>("T");
                plantilla.Salario = row.Field<int>("SALARIO");
                plantillas.Add(plantilla);
            }
            return plantillas;
        }

        public Plantilla FindPlantilla(int idEmpleado)
        {
            var consulta = from datos in this.tablaPlantilla.AsEnumerable() where datos.Field<int>("EMPLEADO_NO") == idEmpleado select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                var row = consulta.First();
                Plantilla plantilla = new Plantilla();
                plantilla.HospitalCod = row.Field<int>("HOSPITAL_COD");
                plantilla.SalaCod = row.Field<int>("SALA_COD");
                plantilla.EmpleadoNo = row.Field<int>("EMPLEADO_NO");
                plantilla.Apellido = row.Field<string>("APELLIDO");
                plantilla.Funcion = row.Field<string>("FUNCION");
                plantilla.Turno = row.Field<string>("T");
                plantilla.Salario = row.Field<int>("SALARIO");
                return plantilla;
            }
        }

        public async Task DeletePlantillaAsync(int idEmpleado)
        {
            string sql = "delete from PLANTILLA where EMPLEADO_NO=@id";
            this.com.Parameters.AddWithValue("@id", idEmpleado);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task UpsertPlantillaAsync(int hospitalCod, int salaCod, int empleadoNo, string apellido, string funcion, string turno, int salario)
        {
            string sql = "SP_PLANTILLA_UPSERT";
            this.com.Parameters.AddWithValue("@hospitalCod", hospitalCod);
            this.com.Parameters.AddWithValue("@salaCod", salaCod);
            this.com.Parameters.AddWithValue("@empleadoNo", empleadoNo);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@funcion", funcion);
            this.com.Parameters.AddWithValue("@turno", turno);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public List<string> GetFunciones()
        {
            var consulta = (from datos in this.tablaPlantilla.AsEnumerable() select datos.Field<string>("FUNCION")).Distinct();
            return consulta.ToList();
        }

        public ResumenPlantilla GetPlantillaFuncion(string funcion)
        {
            var consulta = from datos in this.tablaPlantilla.AsEnumerable() where datos.Field<string>("FUNCION") == funcion select datos;
            if (consulta.Count() == 0)
            {
                ResumenPlantilla model = new ResumenPlantilla();
                model.SumatorioSalario = 0;
                model.MaximoSalario = 0;
                model.MediaSalarial = 0;
                model.Plantillas = null;
                return model;
            }
            else
            {
                int sumatorio = consulta.Sum(z => z.Field<int>("SALARIO"));
                int maximoSala = consulta.Max(z => z.Field<int>("SALARIO"));
                double mediaSala = consulta.Average(z => z.Field<int>("SALARIO"));
                List<Plantilla> plantillas = new List<Plantilla>();
                foreach (var row in consulta)
                {
                    Plantilla plantilla = new Plantilla
                    {
                        HospitalCod = row.Field<int>("HOSPITAL_COD"),
                        SalaCod = row.Field<int>("SALA_COD"),
                        EmpleadoNo = row.Field<int>("EMPLEADO_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Funcion = row.Field<string>("FUNCION"),
                        Turno = row.Field<string>("T"),
                        Salario = row.Field<int>("SALARIO"),
                    };
                    plantillas.Add(plantilla);
                }

                ResumenPlantilla model = new ResumenPlantilla();
                model.SumatorioSalario = sumatorio;
                model.MaximoSalario = maximoSala;
                model.MediaSalarial = mediaSala;
                model.Plantillas = plantillas;
                return model;
            }
        }
    }
}
