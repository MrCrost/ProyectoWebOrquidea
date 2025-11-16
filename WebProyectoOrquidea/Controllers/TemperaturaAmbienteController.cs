using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebProyectoOrquidea.Controllers
{
    public class TemperaturaAmbienteController : Controller
    {
        // GET: TemperaturaAmbienteController
        public ActionResult Temperature()
        {
            return View();
        }

        // GET: TemperaturaAmbienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TemperaturaAmbienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TemperaturaAmbienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Temperature));
            }
            catch
            {
                return View();
            }
        }

        // GET: TemperaturaAmbienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TemperaturaAmbienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Temperature));
            }
            catch
            {
                return View();
            }
        }

        // GET: TemperaturaAmbienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TemperaturaAmbienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Temperature));
            }
            catch
            {
                return View();
            }
        }

        // GET: Obtener historial de datos
        [HttpGet]
        public IActionResult GetHistoricalData(DateTime startDate, DateTime endDate, string sensorType)
        {
            // AQUÍ SE CONECTARÍA CON LA BASE DE DATOS
            // Consulta de ejemplo:
            /*
            var query = @"SELECT * FROM HistorialSensores 
                         WHERE FechaRegistro BETWEEN @StartDate AND @EndDate 
                         AND TipoSensor = @SensorType
                         ORDER BY FechaRegistro DESC";
            */

            return Json(new { success = true, data = new List<object>() });
        }
    }
}
