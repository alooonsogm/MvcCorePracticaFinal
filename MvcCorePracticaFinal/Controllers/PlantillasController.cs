using Microsoft.AspNetCore.Mvc;
using MvcCorePracticaFinal.Models;
using MvcCorePracticaFinal.Repositories;

namespace MvcCorePracticaFinal.Controllers
{
    public class PlantillasController : Controller
    {
        private RepositoryPlantilla repo;

        public PlantillasController()
        {
            this.repo = new RepositoryPlantilla();
        }

        public IActionResult Index()
        {
            List<Plantilla> plantillas = this.repo.GetPlantilla();
            return View(plantillas);
        }

        public IActionResult Details(int id)
        {
            Plantilla plantilla = this.repo.FindPlantilla(id);
            return View(plantilla);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.repo.DeletePlantillaAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult Upsert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Plantilla plantilla)
        {
            await this.repo.UpsertPlantillaAsync(plantilla.HospitalCod, plantilla.SalaCod, plantilla.EmpleadoNo, plantilla.Apellido, plantilla.Funcion, plantilla.Turno, plantilla.Salario);
            return RedirectToAction("Index");
        }

        public IActionResult DatosPlantilla()
        {
            List<string> funciones = this.repo.GetFunciones();
            ViewData["FUNCIONES"] = funciones;
            return View();
        }

        [HttpPost]
        public IActionResult DatosPlantilla(string funcion)
        {
            List<string> funciones = this.repo.GetFunciones();
            ViewData["FUNCIONES"] = funciones;
            ResumenPlantilla model = this.repo.GetPlantillaFuncion(funcion);
            return View(model);
        }
    }
}
