namespace MvcCorePracticaFinal.Models
{
    public class ResumenPlantilla
    {
        public int SumatorioSalario { get; set; }
        public int MaximoSalario { get; set; }
        public double MediaSalarial { get; set; }
        public List<Plantilla> Plantillas { get; set; }
    }
}
