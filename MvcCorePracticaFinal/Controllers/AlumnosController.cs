using Microsoft.AspNetCore.Mvc;
using MvcCorePracticaFinal.Models;
using MvcCorePracticaFinal.Repositories;

namespace MvcCorePracticaFinal.Controllers
{
    public class AlumnosController : Controller
    {
        RepositoryAlumnos repo;

        public AlumnosController()
        {
            this.repo = new RepositoryAlumnos();
        }

        public IActionResult Index()
        {
            List<Alumno> alumnos = this.repo.GetAlumnos();
            return View(alumnos);
        }

        public IActionResult Details(int id)
        {
            DetallesAlumno model = this.repo.GetDetallesAlumno(id);
            return View(model);
        }
    }
}
